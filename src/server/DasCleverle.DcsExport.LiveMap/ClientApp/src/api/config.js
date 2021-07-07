const mapboxConfigEndpoint = '/api/config/mapbox';

export function getMapboxConfig() {
  return fetch(mapboxConfigEndpoint)
    .then((res) => res.json() || {})
    .catch(() => {});
}
