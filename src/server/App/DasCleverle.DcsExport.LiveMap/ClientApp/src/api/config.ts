const mapboxConfigEndpoint = '/api/config/mapbox';

export interface MapboxConfiguration {
  mapboxToken: string;
  mapboxStyle: string;
}

export async function getMapboxConfig(): Promise<MapboxConfiguration | null> {
  try {
    return await fetch(mapboxConfigEndpoint).then((res) => res.json());
  } catch {
    return null;
  }
}
