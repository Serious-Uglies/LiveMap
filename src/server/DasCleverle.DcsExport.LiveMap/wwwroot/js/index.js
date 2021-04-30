class IndexPage {
  constructor() {
    mapboxgl.accessToken = $('#map').data('token');

    this.map = new mapboxgl.Map({
      container: 'map',
      style: 'mapbox://styles/mapbox/light-v10',
      // TODO: move to server state
      center: [36.18279, 34.83058],
      zoom: 6.6,
    });

    this.map.on('load', this.handleLoad.bind(this));
  }

  handleLoad() {
    console.log('Map loaded!');
  }
}

window.indexPage = new IndexPage();
