import React, { useEffect, useRef } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import useAsyncEffect from 'use-async-effect';
import mapboxgl from 'mapbox-gl';
import { getMapboxConfig } from '../../api/config';
import { connect } from '../../api/liveState';

import {
  airbaseToFeature,
  objectToFeature,
  updateObjectFeature,
} from './features';
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
        addLayer(layersRef.current, map.current, name, options);
      }

      dispatch(connect());
    });
  }, []);

  useEffect(() => {
    updateMap(layersRef.current['airbases'], airbases, airbaseToFeature);
  }, [airbases]);

  useEffect(() => {
    updateMap(
      layersRef.current['objects'],
      objects,
      objectToFeature,
      updateObjectFeature
    );
  }, [objects]);

  useEffect(() => {
    if (!map.current) {
      return;
    }

    if (theatre) {
      map.current.setCenter(theatre.center);
      map.current.setZoom(theatre.zoom);
    }
  }, [theatre]);

  return <div ref={mapContainer} className="map"></div>;
}
