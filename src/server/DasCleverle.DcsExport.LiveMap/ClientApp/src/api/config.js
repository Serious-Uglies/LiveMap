const mapboxConfigEndpoint = '/api/config/mapbox';

export async function getMapboxConfig() {
  try {
    return (await fetch(mapboxConfigEndpoint).then((res) => res.json())) || {};
  } catch {
    return {};
  }
}
