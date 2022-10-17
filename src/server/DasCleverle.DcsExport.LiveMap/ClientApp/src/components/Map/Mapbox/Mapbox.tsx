import React, { useCallback, useEffect, useRef, useState } from 'react';
import {
  MapLayerMouseEvent,
  Map,
  ViewState,
  MapRef,
  ViewStateChangeEvent,
} from 'react-map-gl';
import { getMapboxConfig, MapboxConfiguration } from '../../../api/config';

import MapboxLayer from './MapboxLayer';

import './Mapbox.css';
import { getIcons, IconInfo } from '../../../api/client';

type MapboxProps = {
  children: React.ReactNode;
  interactiveLayerIds?: string[];
  viewState: ViewState;
  setViewState: (viewport: ViewState) => void;
  onClick: (e: MapLayerMouseEvent) => void;
};

function Mapbox({
  children,
  interactiveLayerIds,
  viewState,
  setViewState,
  onClick,
}: MapboxProps) {
  const mapRef = useRef<MapRef | null>(null);
  const [config, setConfig] = useState<MapboxConfiguration | null>();
  const [icons, setIcons] = useState<IconInfo[]>([]);
  const [cursor, setCursor] = useState('auto');
  const [loaded, setLoaded] = useState(false);

  useEffect(() => {
    getMapboxConfig().then((config) => setConfig(config));
    getIcons().then((icons) => setIcons(icons));
  }, []);

  const handleMouseEnter = useCallback(
    (e: MapLayerMouseEvent) => {
      if (!loaded || !e.features) {
        return;
      }

      setCursor('pointer');
    },
    [loaded]
  );

  const handleMouseLeave = useCallback(
    (e: MapLayerMouseEvent) => {
      if (!loaded || !e.features) {
        return;
      }

      setCursor('auto');
    },
    [loaded]
  );

  const handleMapMove = useCallback(
    (e: ViewStateChangeEvent) => {
      if (!loaded) {
        return;
      }

      setViewState(e.viewState);
    },
    [loaded, setViewState]
  );

  const handleMapLoad = useCallback(() => {
    if (!mapRef.current) {
      return;
    }

    const map = mapRef.current;

    for (let icon of icons) {
      map.loadImage(icon.url, (error, image) => {
        if (error) {
          console.error(error);
          return;
        }

        if (!image) {
          return;
        }

        map.addImage(icon.id, image);

        const images = map.listImages();
        let loadedCount = 0;

        for (let { id } of icons) {
          if (images.includes(id)) {
            loadedCount++;
          }
        }

        if (loadedCount === icons.length) {
          setLoaded(true);
        }
      });
    }
  }, [icons]);

  if (!config) {
    return null;
  }

  return (
    <Map
      {...viewState}
      ref={mapRef}
      onLoad={handleMapLoad}
      onMove={handleMapMove}
      cursor={cursor}
      interactiveLayerIds={loaded ? interactiveLayerIds : []}
      mapboxAccessToken={config.mapboxToken}
      mapStyle={config.mapboxStyle}
      onClick={onClick}
      onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave}
      dragRotate={false}
      touchZoomRotate={false}
      touchPitch={false}
    >
      {loaded ? children : null}
    </Map>
  );
}

Mapbox.Layer = MapboxLayer;

export default Mapbox;
