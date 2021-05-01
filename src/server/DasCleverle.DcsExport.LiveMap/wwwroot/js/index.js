class IndexPage {
  constructor() {
    this.$loading = $('#loading');
    this.$spinner = $('#spinner');
    this.$error = $('#error');

    this.featuresById = {};
    this.featureCollection = {
      type: 'FeatureCollection',
      features: [],
    };

    mapboxgl.accessToken = $('#map').data('token');
    this.map = new mapboxgl.Map({
      container: 'map',
      style: 'mapbox://styles/dascleverle/cko47dgy70ymg17qbegroiuue',
      // TODO: move to server state
      center: [36.18279, 34.83058],
      zoom: 6.6,
    });

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hub/livemap')
      .withAutomaticReconnect([1000, 2000, 5000, 5000])
      .build();

    this.map.on('load', this.handleMapLoad.bind(this));

    this.connection.on('Event', this.handleEvent.bind(this));
    this.connection.onclose(this.handleConnectionClose.bind(this));
    this.connection.onreconnected(this.handleReconnected.bind(this));
    this.connection.onreconnecting(this.handleReconnecting.bind(this));
  }

  async initialize() {
    await this.startConnection();
  }

  handleMapLoad() {
    this.map.addSource('units', {
      type: 'geojson',
      data: this.featureCollection,
    });

    this.map.addLayer({
      id: 'units',
      type: 'symbol',
      source: 'units',
      layout: {
        'icon-image': '{icon}',
        'icon-allow-overlap': true,
        'icon-size': 0.85,
      },
    });

    this.source = this.map.getSource('units');
  }

  handleEvent({ event: { event, payload } }) {
    switch (event) {
      case 'AddUnit':
        this.addUnit(payload);
        break;

      case 'UpdatePosition':
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

    const controller = unit.player ? 'player' : 'ai';

    const feature = {
      type: 'Feature',
      properties: {
        id: unit.id,
        icon: `${unit.coalition.toLowerCase()}-fixed-${controller}`,
        description: unit.name,
      },
      geometry: {
        type: 'Point',
        coordinates: [unit.position.long, unit.position.lat],
      },
    };

    this.featuresById[unit.id] = feature;
    this.featureCollection.features.push(feature);

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

    delete this.featuresById[unit.id];
    const features = this.featureCollection.features;
    const index = features.findIndex((f) => f.properties.id == unit.id);

    features.splice(index, 1);

    this.updateMap();
  }

  clear() {
    this.featuresById = {};
    this.featureCollection.features = [];
    this.updateMap();
  }

  updateMap() {
    this.source.setData(this.featureCollection);
  }

  setLoading(loading) {
    if (loading) {
      this.$loading.show();
    } else {
      this.$loading.hide();
    }
  }

  async startConnection() {
    try {
      this.setLoading(true);

      await this.connection.start();

      this.$error.hide();
    } catch (err) {
      console.error(err);
      this.$error.show();
    }

    this.setLoading(false);
  }

  handleConnectionClose() {
    this.$spinner.hide();
    this.$error.html('Die Verbindung konnte nicht hergestellt werden.');
  }

  handleReconnecting() {
    this.setLoading(true);
    this.$error.show();
    this.$error.html(
      'Die Verbindung zum Server ist abgebrochen. Versuche erneute Verbindung ...'
    );
  }

  handleReconnected() {
    this.setLoading(false);
  }
}

window.indexPage = new IndexPage();
window.indexPage.initialize();
