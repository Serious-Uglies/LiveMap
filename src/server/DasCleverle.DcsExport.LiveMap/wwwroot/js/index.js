class IndexPage {
  constructor() {
    this.$map = $('#map');
    this.$loading = $('#loading');
    this.$spinner = $('#spinner');
    this.$error = $('#error');
    this.$missionProperties = $('#sidebar-mission-properties');
    this.$airbaseProperties = $('#sidebar-airbase-properties');

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hub/livemap')
      .withAutomaticReconnect([1000, 2000, 5000, 5000])
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection.on('Event', this.handleEvent.bind(this));
    this.connection.onclose(this.handleConnectionClose.bind(this));
    this.connection.onreconnected(this.handleReconnected.bind(this));
    this.connection.onreconnecting(this.handleReconnecting.bind(this));

    this.objects = {};
    this.airbases = {};

    this.objectLayers = ['objects-air', 'objects-earthbound'];
    this.theatre = null;
    this.theatreProperties = {
      ['Caucasus']: {
        center: [41.6748132691836, 43.729303011322685],
        zoom: 6.15,
        timezone: 'Asia/Tbilisi',
      },
      ['Nevada']: {
        center: [-115.3220498166852, 37.959029700616],
        zoom: 6.66,
        timezone: 'America/Los_Angeles',
      },
      ['PersianGulf']: {
        center: [55.9127513841311, 26.872559655101398],
        zoom: 6.04,
        timezone: 'Asia/Dubai',
      },
      ['Syria']: {
        center: [37.71425723879233, 34.890062119982204],
        zoom: 6.37,
        timezone: 'Asia/Damascus',
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

    const started = await this.startConnection();

    if (!started) {
      return;
    }

    if (this.isRunning) {
      this.setLoading(false);
    }
  }

  loadState(state) {
    this.setRunning(state.isRunning);

    if (!state.isRunning) {
      return;
    }

    this.initMap(state);
    state.objects.forEach((obj) => this.addObject(obj));
    state.airbases.forEach((airbase) => this.addAirbase(airbase));

    this.map.update();
  }

  initMap(init) {
    if (init.isRunning !== false) {
      this.setRunning(true);
    }

    if (init.theatre) {
      this.theatre = this.theatreProperties[init.theatre];

      this.map.setZoom(this.theatre.zoom || 6);
      this.map.setCenter(
        this.theatre.center || [init.mapCenter.long, init.mapCenter.lat]
      );
    }

    this.$missionProperties.template('mission-properties', init);
  }

  handleMapClick(event) {
    this.$airbaseProperties.hide();

    const features = event.target
      .queryRenderedFeatures(event.point)
      .filter((feature) => !!this.map.layers[feature.layer.id]);

    if (features.length == 0) {
      return;
    }

    const i = features.findIndex((f) => f.layer.id == 'airbases');
    const airbaseFeature = features[i];

    if (airbaseFeature) {
      const airbase = this.airbases[airbaseFeature.properties.id];
      const handlers = {
        close: () => this.$airbaseProperties.hide(),
      };

      this.$airbaseProperties.template('airbase-properties', airbase, handlers);
      this.$airbaseProperties.show();

      features.splice(i, 1);
    }

    if (features.length == 0) {
      return;
    }

    const long =
      features.reduce((s, f) => s + f.geometry.coordinates[0], 0) /
      features.length;

    const lat =
      features.reduce((s, f) => s + f.geometry.coordinates[1], 0) /
      features.length;

    const popup = $('<div />').template('feature-popup', {
      objects: features.map((f) => this.objects[f.properties.id]),
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

      case 'MissionEnd':
        this.endMission();
        break;

      case 'Time':
        this.updateTime(payload);
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

      case 'AddAirbase':
        this.addAirbase(payload);
        break;

      default:
        console.log(`Unhandled event: ${event}, payload: `, payload);
        break;
    }

    this.map.update();
  }

  updateTime({ time }) {
    time = new Date(time);

    const format = new Intl.DateTimeFormat('de-DE', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      timeZone: this.theatre.timezone,
    });

    $('#mission-time').template('mission-time', { time: format.format(time) });
  }

  addObject(obj) {
    const coalition = obj.coalition.toLowerCase();
    const iconType = this.getIconType(obj);
    const pilot = obj.player ? 'player' : 'ai';
    const layer = this.determineLayer(obj);

    this.objects[obj.id] = obj;

    this.map.addFeature(obj.id, layer, obj.position, {
      icon: `${coalition}-${iconType}-${pilot}`,
    });
  }

  updateObject(obj) {
    this.map.updateFeature(obj.id, obj.position, this.objectLayers);
  }

  removeObject(obj) {
    this.map.removeFeature(obj.id, this.objectLayers);
  }

  addAirbase(airbase) {
    const ilss = airbase.beacons.ils;
    airbase.ilsByRwy = {};

    for (let ils of ilss) {
      airbase.ilsByRwy[ils.runway] = ils.frequency;
    }

    const coalition = airbase.coalition.toLowerCase();
    const category = airbase.category.toLowerCase();
    const rotation = airbase.runways[0] ? airbase.runways[0].course : 0;

    this.airbases[airbase.id] = airbase;
    this.map.addFeature(airbase.id, 'airbases', airbase.position, {
      icon: `${coalition}-${category}`,
      rotation: rotation,
      name: airbase.name,
    });
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

  endMission() {
    this.map.clear();
    this.map.setCenter([0, 0]);
    this.map.setZoom(20);

    this.$missionProperties.html('');
    this.$airbaseProperties.html('');

    this.setRunning(false);
  }

  setRunning(running) {
    this.isRunning = running;

    if (running) {
      this.setLoading(false);
      this.$spinner.show();
      this.$error.hide();
    } else {
      this.setLoading(true);
      this.$spinner.hide();

      this.$error.show();
      this.$error.template('no-mission-running');
    }
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
