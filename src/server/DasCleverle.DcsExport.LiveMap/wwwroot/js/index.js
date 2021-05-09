class IndexPage {
  constructor() {
    this.$map = $('#map');
    this.$loading = $('#loading');
    this.$spinner = $('#spinner');
    this.$error = $('#error');
    this.$sidebar = $('#sidebar');

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hub/livemap')
      .withAutomaticReconnect([1000, 2000, 5000, 5000])
      .build();

    this.connection.on('Event', this.handleEvent.bind(this));
    this.connection.onclose(this.handleConnectionClose.bind(this));
    this.connection.onreconnected(this.handleReconnected.bind(this));
    this.connection.onreconnecting(this.handleReconnecting.bind(this));

    this.theatreProperties = {
      ['Caucasus']: {
        center: [41.6748132691836, 43.729303011322685],
        zoom: 6.15,
      },
      ['Nevada']: {
        center: [-115.3220498166852, 37.959029700616],
        zoom: 6.66,
      },
      ['PersianGulf']: {
        center: [55.9127513841311, 26.872559655101398],
        zoom: 6.04,
      },
      ['Syria']: {
        center: [37.71425723879233, 34.890062119982204],
        zoom: 6.37,
      },
    };
  }

  async initialize() {
    this.setLoading(true);

    this.map = new LiveMap(this.$map, {
      layers: {
        'objects-earthbound': {
          layout: {
            'icon-size': 0.85,
          },
        },
        'objects-air': {
          layout: {
            'icon-size': 0.7,
          },
        },
      },
    });

    this.map.on('load', this.handleMapLoad.bind(this));
    this.map.on('click', this.handleMapClick.bind(this));
  }

  async handleMapLoad() {
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
    this.map.update();
  }

  initMap(init) {
    if (init.theatre) {
      const theatre = this.theatreProperties[init.theatre];

      this.map.setZoom(theatre.zoom || 6);
      this.map.setCenter(
        theatre.center || [init.mapCenter.long, init.mapCenter.lat]
      );
    }

    this.$sidebar.template('sidebar', this, init);
  }

  handleMapClick(event) {
    const features = event.target
      .queryRenderedFeatures(event.point)
      .filter((feature) => !!this.map.layers[feature.layer.id]);

    if (features.length == 0) {
      return;
    }

    const long =
      features.reduce((s, f) => s + f.geometry.coordinates[0], 0) /
      features.length;

    const lat =
      features.reduce((s, f) => s + f.geometry.coordinates[1], 0) /
      features.length;

    const popup = $('<div />').template('feature-popup', this, {
      features: features.map((f) => f.properties),
    });

    new mapboxgl.Popup()
      .setLngLat([long, lat])
      .setHTML(popup.html())
      .addTo(event.target);
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
        this.map.clear();
        break;

      default:
        console.log(`Unhandled event: ${event}, payload: `, payload);
        break;
    }

    this.map.update();
  }

  addObject(obj) {
    const coalition = obj.coalition.toLowerCase();
    const iconType = this.getIconType(obj);
    const pilot = obj.player ? 'player' : 'ai';
    const layer = this.determineLayer(obj);

    this.map.addFeature(obj.id, layer, obj.position, {
      icon: `${coalition}-${iconType}-${pilot}`,
      name: obj.name,
      typeName: obj.typeName,
    });
  }

  updateObject(obj) {
    this.map.updateFeature(obj.id, obj.position);
  }

  removeObject(obj) {
    this.map.removeFeature(obj.id);
  }

  determineLayer(obj) {
    if (
      obj.type === 'Unit' &&
      (obj.attributes.includes('Fixed') || obj.attributes.includes('Rotary'))
    ) {
      return 'objects-air';
    }

    return 'objects-earthbound';
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
