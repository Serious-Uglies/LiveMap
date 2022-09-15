import { useEffect, useState } from 'react';
import { Layer, Source } from 'react-map-gl';
import produce from 'immer';
import { AnyLayer } from 'mapbox-gl';
import { FeatureFactory, FeatureUpdater } from '../features';

interface Data {
  [id: string | number]: any;
}

interface MapboxLayerProps {
  data: Data;
  createFeature: FeatureFactory;
  updateFeature?: FeatureUpdater;
  config: Partial<AnyLayer>;
}

interface MapboxLayerState {
  data: GeoJSON.FeatureCollection;
  ids: { [id: string | number]: boolean };
}

const updateLayer = (
  layer: MapboxLayerState,
  data: Data,
  createFeature: FeatureFactory,
  updateFeature?: FeatureUpdater
) => {
  const {
    data: { features },
    ids,
  } = layer;

  const updated = [];

  for (let feature of features) {
    const id = feature.id;

    if (!id) {
      continue;
    }

    if (!data[id]) {
      delete ids[id];
      continue;
    }

    if (updateFeature) {
      updateFeature(feature, data[id]);
    }

    updated.push(feature);
  }

  for (let [id, item] of Object.entries(data)) {
    if (!ids[id]) {
      const feature = createFeature(item);

      if (!feature) {
        continue;
      }

      ids[id] = true;
      updated.push(feature);

      continue;
    }
  }

  layer.data.features = updated;
};

export default function MapboxLayer({
  data,
  createFeature,
  updateFeature,
  config,
}: MapboxLayerProps) {
  const [layer, setLayer] = useState<MapboxLayerState>({
    data: {
      type: 'FeatureCollection',
      features: [],
    },
    ids: {},
  });

  useEffect(
    () =>
      setLayer((l) =>
        produce(l, (draft) =>
          updateLayer(draft, data, createFeature, updateFeature)
        )
      ),
    [data, createFeature, updateFeature]
  );

  return (
    <Source id={config.id} type="geojson" data={layer.data}>
      <Layer {...(config as any)} />
    </Source>
  );
}
