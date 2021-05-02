class IndexPage {
  constructor() {
    this.$loading = $('#loading');
    this.$spinner = $('#spinner');
    this.$error = $('#error');

    this.featuresById = {};
    this.layers = {};

    mapboxgl.accessToken = $('#map').data('token');

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hub/livemap')
      .withAutomaticReconnect([1000, 2000, 5000, 5000])
      .build();

    this.connection.on('Event', this.handleEvent.bind(this));
    this.connection.onclose(this.handleConnectionClose.bind(this));
    this.connection.onreconnected(this.handleReconnected.bind(this));
    this.connection.onreconnecting(this.handleReconnecting.bind(this));
  }

  async initialize() {
    this.setLoading(true);

    this.map = new mapboxgl.Map({
      container: 'map',
      style: 'mapbox://styles/dascleverle/cko5q98k62fvv18lj5jln6inl',
      // TODO: move to server state
      center: [36.18279, 34.83058],
      zoom: 6.6,

      dragRotate: false,
      touchZoomRotate: false,
      touchPitch: false,
    });

    this.map.on('load', this.handleMapLoad.bind(this));
  }

  async handleMapLoad() {
    this.addLayer('units-earthbound', {
      layout: {
        'icon-size': 0.85,
      },
    });

    this.addLayer('units-air', {
      layout: {
        'icon-size': 0.7,
      },
    });

    const state = await this.getLiveState();

    if (!state) {
      return;
    }

    this.loadState(state);

    if (await this.startConnection()) {
      this.setLoading(false);
    }
  }

  loadState(state) {
    state.units.forEach((unit) => this.addUnit(unit));
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

    this.map.on('click', name, this.handleMapClick.bind(this));
    this.map.on('mouseenter', name, this.handleMapMouseEnter.bind(this));
    this.map.on('mouseleave', name, this.handleMapMouseLeave.bind(this));

    layer.source = this.map.getSource(name);
    this.layers[name] = layer;
  }

  handleMapClick(event) {
    const properties = event.features.map((f) => f.properties);

    const popup = $('<div />').template('feature-popup', this, {
      features: properties,
    });

    new mapboxgl.Popup()
      .setLngLat(event.lngLat)
      .setHTML(popup.html())
      .addTo(this.map);
  }

  handleMapMouseEnter(event) {
    event.target.getCanvas().style.cursor = 'pointer';
  }

  handleMapMouseLeave(event) {
    event.target.getCanvas().style.cursor = '';
  }

  handleEvent({ event: { event, payload } }) {
    switch (event) {
      case 'AddUnit':
        this.addUnit(payload);
        break;

      case 'UpdateUnit':
        this.updateUnit(payload);
        break;

      case 'RemoveUnit':
        this.removeUnit(payload);
        break;

      case 'MissionEnd':
        this.clear();
        break;

      default:
        console.log(`Unhandled event: ${event}, payload: `, payload);
    }
  }

  addUnit(unit) {
    if (this.featuresById[unit.id]) {
      return;
    }

    const coalition = unit.coalition.toLowerCase();
    const iconType = this.getUnitIconType(unit);
    const pilot = unit.player ? 'player' : 'ai';
    const layer = this.determineLayer(unit);

    const feature = {
      type: 'Feature',
      properties: {
        id: unit.id,
        icon: `${coalition}-${iconType}-${pilot}`,
        layer: layer.name,
        name: unit.name,
        typeName: unit.typeName,
      },
      geometry: {
        type: 'Point',
        coordinates: [unit.position.long, unit.position.lat],
      },
    };

    layer.features.features.push(feature);

    this.featuresById[unit.id] = feature;
    this.updateMap();
  }

  updateUnit(unit) {
    const feature = this.featuresById[unit.id];

    if (!feature) {
      return;
    }

    feature.geometry.coordinates = [unit.position.long, unit.position.lat];
    this.updateMap();
  }

  removeUnit(unit) {
    const feature = this.featuresById[unit.id];

    if (!feature) {
      return;
    }

    const layer = this.layers[feature.properties.layer];
    const features = layer.features.features;
    const index = features.findIndex((f) => f.properties.id == unit.id);

    features.splice(index, 1);
    delete this.featuresById[unit.id];

    this.updateMap();
  }

  clear() {
    this.featuresById = {};

    for (const layer of Object.values(this.layers)) {
      layer.features.features = [];
    }

    this.updateMap();
  }

  updateMap() {
    for (const layer of Object.values(this.layers)) {
      layer.source.setData(layer.features);
    }
  }

  determineLayer(unit) {
    if (
      unit.attributes.includes('Fixed') ||
      unit.attributes.includes('Rotary')
    ) {
      return this.layers['units-air'];
    }

    return this.layers['units-earthbound'];
  }

  getUnitIconType(unit) {
    if (unit.attributes.includes('Water')) {
      return 'water';
    } else if (unit.attributes.includes('Ground')) {
      return 'ground';
    } else if (unit.attributes.includes('Rotary')) {
      return 'rotary';
    } else if (unit.attributes.includes('Tanker')) {
      return 'tanker';
    } else if (unit.attributes.includes('Awacs')) {
      return 'awacs';
    }

    return 'fixed';
  }

  setLoading(loading) {
    if (loading) {
      this.$loading.show();
    } else {
      this.$loading.hide();
    }
  }

  async getLiveState() {
    try {
      return await fetch('/api/state').then((res) => res.json());
    } catch (err) {
      console.error(err);

      this.$error.show();
      this.handleConnectionClose();

      return null;
    }
  }

  async startConnection() {
    try {
      await this.connection.start();

      return true;
    } catch (err) {
      console.error(err);

      this.$error.show();
      this.handleConnectionClose();

      return false;
    }
  }

  handleConnectionClose() {
    this.$spinner.hide();
    this.$error.template('connection-error');
  }

  handleReconnecting() {
    this.setLoading(true);
    this.$error.show();
    this.$error.template('connection-reconnecting');
  }

  handleReconnected() {
    this.setLoading(false);
  }
}

window.indexPage = new IndexPage();
window.indexPage.initialize();
