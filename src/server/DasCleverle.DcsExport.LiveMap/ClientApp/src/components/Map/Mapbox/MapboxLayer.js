import React, { useEffect, useState } from 'react';
import { Layer, Source } from 'react-map-gl';
import produce from 'immer';

const updateLayer = (layer, data, createFeature, updateFeature) => {
  const {
    data: { features },
    ids,
  } = layer;

  const updated = [];

  for (let feature of features) {
    const id = feature.properties.id;

    if (!data[id]) {
      delete ids[id];
      continue;
    }

    if (updateFeature) {
      updateFeature(feature, data[id]);
    }

    updated.push(feature);
  }

  for (let item of Object.values(data)) {
    const id = item.id;

    if (!ids[id]) {
      ids[id] = true;
      updated.push(createFeature(item));

      continue;
    }
  }

  layer.data.features = updated;
};

export default function MapboxLayer({
  id,
  data = {},
  createFeature,
  updateFeature,
  layer: layerProps,
}) {
  const [layer, setLayer] = useState({
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
    <Source id={id} type="geojson" data={layer.data}>
      <Layer {...layerProps} id={id} type="symbol" />
    </Source>
  );
}
