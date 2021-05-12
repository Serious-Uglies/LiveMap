class LiveMap {
  constructor($target, options) {
    mapboxgl.accessToken = $target.data('token');

    this.options = options;
    this.map = new mapboxgl.Map({
      container: $target[0],
      style: $target.data('style'),
      center: [0, 0],
      zoom: 20,

      dragRotate: false,
      touchZoomRotate: false,
      touchPitch: false,
    });

    this.featuresById = {};
    this.layers = {};

    this.map.on('load', this.handleMapLoad.bind(this));
  }

  addFeature(id, layerName, position, properties) {
    if (this.featuresById[layerName][id]) {
      return;
    }

    const layer = this.layers[layerName];

    if (!layer) {
      return;
    }

    const feature = {
      type: 'Feature',
      properties: Object.assign({}, properties, {
        id: id,
        layer: layerName,
      }),
      geometry: {
        type: 'Point',
        coordinates: [position.long, position.lat],
      },
    };

    layer.features.features.push(feature);
    this.featuresById[layerName][id] = feature;
  }

  updateFeature(id, position, layers) {
    const current = this.findFeature(id, layers);

    if (!current) {
      return;
    }

    if (position) {
      current.geometry.coordinates = [position.long, position.lat];
    }
  }

  removeFeature(id, layers) {
    const feature = this.findFeature(id, layers);

    if (!feature) {
      return;
    }

    const layer = this.layers[feature.properties.layer];
    const features = layer.features.features;
    const index = features.findIndex((f) => f.properties.id == id);

    features.splice(index, 1);
    delete this.featuresById[layer.name][id];
  }

  clear() {
    for (const layer of Object.values(this.layers)) {
      this.featuresById[layer.name] = {};
      layer.features.features = [];
    }

    this.update();
  }

  update() {
    for (const layer of Object.values(this.layers)) {
      layer.source.setData(layer.features);
    }
  }

  on(event, handler) {
    this.map.on(event, handler);
  }

  setCenter(center) {
    this.map.setCenter(center);
  }

  setZoom(zoom) {
    this.map.setZoom(zoom);
  }

  handleMapLoad() {
    if (this.options.layers) {
      for (let [name, layer] of Object.entries(this.options.layers)) {
        this.addLayer(name, layer);
      }
    }
  }

  addLayer(name, options) {
    const layer = {
      name: name,
      features: {
        type: 'FeatureCollection',
        features: [],
      },
    };

    this.map.addSource(name, {
      type: 'geojson',
      data: layer.features,
    });

    this.map.addLayer(
      Object.assign({}, options, {
        id: name,
        type: 'symbol',
        source: name,
        layout: Object.assign({}, options.layout || {}, {
          'icon-image': '{icon}',
          'icon-allow-overlap': true,
        }),
      })
    );

    this.map.on('mouseenter', name, this.handleMapMouseEnter.bind(this));
    this.map.on('mouseleave', name, this.handleMapMouseLeave.bind(this));

    layer.source = this.map.getSource(name);
    this.layers[name] = layer;
    this.featuresById[name] = {};
  }

  handleMapMouseEnter(event) {
    event.target.getCanvas().style.cursor = 'pointer';
  }

  handleMapMouseLeave(event) {
    event.target.getCanvas().style.cursor = '';
  }

  findFeature(id, layers) {
    for (let layer of Array.from(layers)) {
      const feature = this.featuresById[layer][id];

      if (feature) {
        return feature;
      }
    }

    return null;
  }
}
