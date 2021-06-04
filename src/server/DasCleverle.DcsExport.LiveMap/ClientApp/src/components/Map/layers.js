export const layers = {
  objects: {
    layout: {
      'icon-size': 0.7,
      'symbol-sort-key': ['get', 'sortKey'],
    },
  },

  airbases: {
    layout: {
      'icon-size': 0.7,
      'icon-rotate': ['get', 'rotation'],
      'text-field': '{name}',
      'text-anchor': 'left',
      'text-offset': [0.73, 0],
      'text-font': ['DIN Pro Regular'],
      'text-size': [
        'interpolate',
        ['cubic-bezier', 0.2, 0, 0.9, 1],
        ['zoom'],
        3,
        12,
        13,
        19,
      ],
    },
    paint: {
      'text-halo-color': 'white',
      'text-halo-width': 1,
      'text-halo-blur': 1,
    },
  },
};

export const addLayer = (layers, map, name, options) => {
  const layer = {
    name: name,
    features: {
      type: 'FeatureCollection',
      features: [],
    },
    featuresById: {},
  };

  map.addSource(name, {
    type: 'geojson',
    data: layer.features,
  });

  map.addLayer({
    ...options,
    id: name,
    source: name,
    type: 'symbol',
    layout: {
      'icon-image': '{icon}',
      'icon-allow-overlap': true,
      ...options.layout,
    },
  });

  layer.source = map.getSource(name);
  layers[name] = layer;

  map.on('mouseenter', name, (event) => {
    event.target.getCanvas().style.cursor = 'pointer';
  });

  map.on('mouseleave', name, (event) => {
    event.target.getCanvas().style.cursor = '';
  });
};

export const updateMap = (layer, data, createFeature, updateFeature) => {
  if (!layer) {
    return;
  }

  const featuresToKeep = [];

  for (let feature of layer.features.features) {
    if (!data[feature.id]) {
      delete layer.featuresById[feature.id];
      continue;
    }

    featuresToKeep.push(feature);
  }

  layer.features.features = featuresToKeep;

  for (let item of Object.values(data)) {
    let feature = layer.featuresById[item.id];

    if (!feature) {
      feature = createFeature(item);
      layer.featuresById[item.id] = feature;
      layer.features.features.push(feature);
    } else if (updateFeature) {
      updateFeature(feature, item);
    }
  }

  layer.source.setData(layer.features);
};
