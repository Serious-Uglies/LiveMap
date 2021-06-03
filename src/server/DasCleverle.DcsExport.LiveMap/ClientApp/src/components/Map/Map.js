import React, { useEffect, useRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import useAsyncEffect from 'use-async-effect';
import mapboxgl from 'mapbox-gl';
import { getMapboxConfig } from '../../api/config';
import { getLiveState } from '../../api/liveState';

import { load } from '../../store/liveState';
import { airbaseToFeature, getObjectLayer, objectToFeature } from './features';
import { addLayer, updateMap, layers } from './layers';
import { theatres } from './theatres';

import './Map.css';

export function Map() {
  const dispatch = useDispatch();
  const airbases = useSelector((state) => state.liveState.airbases);
  const objects = useSelector((state) => state.liveState.objects);
  const theatre = useSelector((state) => theatres[state.liveState.theatre]);

  const mapContainer = useRef(null);
  const map = useRef(null);
  const layersRef = useRef({});

  useAsyncEffect(async () => {
    const { mapboxToken, mapboxStyle } = await getMapboxConfig();

    if (!mapboxToken) {
      return;
    }

    mapboxgl.accessToken = mapboxToken;

    if (map.current) {
      return;
    }

    map.current = new mapboxgl.Map({
      container: mapContainer.current,
      style: mapboxStyle,
      center: [0, 0],
      zoom: 20,

      dragRotate: false,
      touchZoomRotate: false,
      touchPitch: false,
    });

    map.current.on('load', async () => {
      for (let [name, options] of Object.entries(layers)) {
        addLayer(layersRef, map.current, name, options);
      }

      const liveState = await getLiveState();

      if (liveState) {
        dispatch(load(liveState));
      }
    });
  }, []);

  useEffect(() => {
    updateMap(layersRef, airbases, () => 'airbases', airbaseToFeature);
  }, [airbases]);

  useEffect(() => {
    updateMap(layersRef, objects, getObjectLayer, objectToFeature);
  }, [objects]);

  useEffect(() => {
    if (!map.current) {
      return;
    }

    map.current.setCenter(theatre.center);
    map.current.setZoom(theatre.zoom);
  }, [theatre]);

  return <div ref={mapContainer} className="map"></div>;
}
