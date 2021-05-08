class IndexPage {
  constructor() {
    this.$loading = $('#loading');
    this.$spinner = $('#spinner');
    this.$error = $('#error');
    this.$sidebar = $('#sidebar');

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
      zoom: 6.6,

      dragRotate: false,
      touchZoomRotate: false,
      touchPitch: false,
    });

    this.map.on('load', this.handleMapLoad.bind(this));
  }

  async handleMapLoad() {
    this.addLayer('objects-earthbound', {
      layout: {
        'icon-size': 0.85,
      },
    });

    this.addLayer('objects-air', {
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
    this.initMap(state);
    state.objects.forEach((obj) => this.addObject(obj));
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
      case 'Init':
        this.initMap(payload);
        break;

      case 'AddObject':
        this.addObject(payload);
        break;

      case 'UpdateObject':
        this.updateObject(payload);
        break;

      case 'RemoveObject':
        this.removeObject(payload);
        break;

      case 'MissionEnd':
        this.clear();
        break;

      default:
        console.log(`Unhandled event: ${event}, payload: `, payload);
    }
  }

  initMap(init) {
    this.map.setCenter(
      {
        ['Caucasus']: { lng: 41.6748132691836, lat: 43.729303011322685 },
        ['Nevada']: { lng: -115.3220498166852, lat: 37.959029700616 },
        ['PersianGulf']: { lng: 55.9127513841311, lat: 26.872559655101398 },
        ['Syria']: { lng: 37.71425723879233, lat: 34.890062119982204 },
      }[init.theatre] || [init.mapCenter.long, init.mapCenter.lat]
    );
    this.map.setZoom(
      {
        ['Caucasus']: 6.15,
        ['Nevada']: 6.66,
        ['PersianGulf']: 6.04,
        ['Syria']: 6.37,
      }[init.theatre] || 6
    );

    this.$sidebar.template('sidebar', this, init);
  }

  addObject(obj) {
    if (this.featuresById[obj.id]) {
      return;
    }

    const coalition = obj.coalition.toLowerCase();
    const iconType = this.getIconType(obj);
    const pilot = obj.player ? 'player' : 'ai';
    const layer = this.determineLayer(obj);

    const feature = {
      type: 'Feature',
      properties: {
        id: obj.id,
        icon: `${coalition}-${iconType}-${pilot}`,
        layer: layer.name,
        name: obj.name,
        typeName: obj.typeName,
      },
      geometry: {
        type: 'Point',
        coordinates: [obj.position.long, obj.position.lat],
      },
    };

    layer.features.features.push(feature);

    this.featuresById[obj.id] = feature;
    this.updateMap();
  }

  updateObject(obj) {
    const feature = this.featuresById[obj.id];

    if (!feature) {
      return;
    }

    feature.geometry.coordinates = [obj.position.long, obj.position.lat];
    this.updateMap();
  }

  removeObject(obj) {
    const feature = this.featuresById[obj.id];

    if (!feature) {
      return;
    }

    const layer = this.layers[feature.properties.layer];
    const features = layer.features.features;
    const index = features.findIndex((f) => f.properties.id == obj.id);

    features.splice(index, 1);
    delete this.featuresById[obj.id];

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

  determineLayer(obj) {
    if (
      obj.type === 'Unit' &&
      (obj.attributes.includes('Fixed') || obj.attributes.includes('Rotary'))
    ) {
      return this.layers['objects-air'];
    }

    return this.layers['objects-earthbound'];
  }

  getIconType(obj) {
    if (obj.type === 'Static') {
      return 'ground';
    }

    if (obj.attributes.includes('Water')) {
      return 'water';
    } else if (obj.attributes.includes('Ground')) {
      return 'ground';
    } else if (obj.attributes.includes('Rotary')) {
      return 'rotary';
    } else if (obj.attributes.includes('Tanker')) {
      return 'tanker';
    } else if (obj.attributes.includes('Awacs')) {
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
