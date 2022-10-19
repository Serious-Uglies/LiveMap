import _ from 'lodash';
import mapboxgl from 'mapbox-gl';
import React from 'react';
import { useEffect, useState } from 'react';
import { Popup as MapboxPopup } from 'react-map-gl';
import { getPopups, PopupConfig } from '../../../api/client';
import { useAppSelector } from '../../../store';
import GroupingPopup from './GroupingPopup';

import './Popup.css';
import PropertyListPopup from './PropertyListPopup';

interface PopupProps {
  features?: mapboxgl.MapboxGeoJSONFeature[];
  onClose: () => void;
}

export default function Popup({ features, onClose }: PopupProps) {
  const [popups, setPopups] = useState<{ [layer: string]: PopupConfig } | null>(
    null
  );
  const [config, setConfig] = useState<PopupConfig | null>(null);
  const [popupFeatures, setPopupFeatures] = useState<
    GeoJSON.Feature<GeoJSON.Point>[]
  >([]);
  const [longitude, setLongitude] = useState<number>(0);
  const [latitude, setLatitude] = useState<number>(0);

  const mapFeatures = useAppSelector((state) => state.liveState.mapFeatures);

  useEffect(() => {
    getPopups().then((popups) => setPopups(popups));
  }, []);

  useEffect(() => {
    if (!features?.length || !popups) {
      setConfig(null);
      return;
    }

    const { config, points } = _.chain(features)
      .groupBy((f) => f.layer.id)
      .map((group, layer) => ({
        config: popups[layer],
        points: _.chain(group)
          .map((f) => mapFeatures[layer].features.find((f1) => f.id === f1.id))
          .compact()
          .filter(isPointFeature)
          .value(),
      }))
      .maxBy((features) => features.config.priority)
      .value();

    setConfig(config);

    if (config.allowClustering) {
      const latitude = _.meanBy(points, 'geometry.coordinates[1]');
      const longitude = _.meanBy(points, 'geometry.coordinates[0]');

      setPopupFeatures(points);
      setLatitude(latitude);
      setLongitude(longitude);
    } else {
      setPopupFeatures([points[0]]);
      setLatitude(points[0].geometry.coordinates[1]);
      setLongitude(points[0].geometry.coordinates[0]);
    }
  }, [features, mapFeatures, popups]);

  if (!config) {
    return null;
  }

  return (
    <MapboxPopup
      className="popup"
      longitude={longitude}
      latitude={latitude}
      closeOnClick={false}
      onClose={onClose}
    >
      <PopupErrorBoundary>
        {config.type === 'grouping' ? (
          <GroupingPopup config={config} input={popupFeatures} />
        ) : null}
        {config.type === 'property-list' ? (
          <PropertyListPopup config={config} input={popupFeatures[0]} />
        ) : null}
      </PopupErrorBoundary>
    </MapboxPopup>
  );
}

class PopupErrorBoundary extends React.Component<any, { hasError: boolean }> {
  constructor(props: any) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error: Error) {
    return { hasError: true };
  }

  render() {
    if (this.state.hasError) {
      return <em>Fehler beim Anzeigen des Popups</em>;
    }

    return this.props.children;
  }
}

function isPointFeature(
  feature: GeoJSON.Feature
): feature is GeoJSON.Feature<GeoJSON.Point> {
  return feature.geometry.type === 'Point';
}
