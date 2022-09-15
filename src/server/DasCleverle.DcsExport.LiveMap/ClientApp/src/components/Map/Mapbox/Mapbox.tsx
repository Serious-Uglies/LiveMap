import React, { useEffect, useState } from 'react';
import { MapLayerMouseEvent, Map, ViewState } from 'react-map-gl';
import { getMapboxConfig, MapboxConfiguration } from '../../../api/config';

import MapboxLayer from './MapboxLayer';

import './Mapbox.css';

// const mapOptions = {
//   scrollZoom: {
//     speed: 0.03,
//     smooth: true,
//   },
//   dragRotate: false,
//   touchZoomRotate: false,
//   touchPitch: false,
// };

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
  const [config, setConfig] = useState<MapboxConfiguration | null>();
  const [cursor, setCursor] = useState('auto');

  useEffect(() => {
    getMapboxConfig().then((config) => setConfig(config));
  }, []);

  if (!config) {
    return null;
  }

  const onMouseEnter = (e: MapLayerMouseEvent) => {
    if (!e.features) {
      return;
    }

    setCursor('pointer');
  };

  const onMouseLeave = (e: MapLayerMouseEvent) => {
    if (!e.features) {
      return;
    }

    setCursor('auto');
  };

  return (
    <Map
      {...viewState}
      onMove={(e) => setViewState(e.viewState)}
      cursor={cursor}
      interactiveLayerIds={interactiveLayerIds}
      mapboxAccessToken={config.mapboxToken}
      mapStyle={config.mapboxStyle}
      onClick={onClick}
      onMouseEnter={onMouseEnter}
      onMouseLeave={onMouseLeave}
    >
      {children}
    </Map>
  );
}

Mapbox.Layer = MapboxLayer;

export default Mapbox;
