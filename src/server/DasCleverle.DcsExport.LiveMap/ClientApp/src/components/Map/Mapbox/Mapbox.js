import React, { useState } from 'react';
import useAsyncEffect from 'use-async-effect';
import MapGL from 'react-map-gl';
import { getMapboxConfig } from '../../../api/config';

import MapboxLayer from './MapboxLayer';

import './Mapbox.css';

const mapOptions = {
  scrollZoom: {
    speed: 0.03,
    smooth: true,
  },
  dragRotate: false,
  touchZoomRotate: false,
  touchPitch: false,
};

function Mapbox({
  children,
  onClick,
  onMouseMove,
  interactiveLayerIds,
  viewport,
  onViewportChange,
}) {
  const [config, setConfig] = useState();

  useAsyncEffect(async () => {
    setConfig(await getMapboxConfig());
  }, []);

  return config ? (
    <MapGL
      {...viewport}
      asyncRender={true}
      width="100%"
      height="100%"
      onViewportChange={onViewportChange}
      onNativeClick={onClick}
      onMouseMove={onMouseMove}
      interactiveLayerIds={interactiveLayerIds}
      mapboxApiAccessToken={config.mapboxToken}
      mapStyle={config.mapboxStyle}
      {...mapOptions}
    >
      {children}
    </MapGL>
  ) : (
    <div>Loading ...</div>
  );
}

Mapbox.Layer = MapboxLayer;

export default Mapbox;
