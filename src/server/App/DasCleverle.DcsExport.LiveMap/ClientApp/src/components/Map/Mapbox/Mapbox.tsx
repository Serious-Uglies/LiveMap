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
  const [cursor, setCursor] = useState('auto');
  const [loaded, setLoaded] = useState(false);

  useEffect(() => {
    getMapboxConfig().then((config) => setConfig(config));
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
    const map = mapRef.current;

    if (!map) {
      return;
    }

    const loading = new Set<string>();
    const fallbackImage = {
      width: 24,
      height: 24,
      data: new Uint8Array(24 * 24 * 4),
    };

    map.on('styleimagemissing', ({ id }: { id: string }) => {
      if (loading.has(id)) {
        return;
      }

      loading.add(id);
      map.addImage(id, fallbackImage);

      map.loadImage('/api/client/icon/' + id, (error, image) => {
        if (error) {
          console.error(error);
          return;
        }

        if (!image) {
          return;
        }

        map.updateImage(id, image);
        loading.delete(id);

        if (loading.size === 0) {
          map.triggerRepaint();
        }
      });
    });

    setLoaded(true);
  }, []);

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
