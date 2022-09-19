import { AnyLayer } from 'mapbox-gl';

const mapboxConfigEndpoint = '/api/config/mapbox';
const layersConfigEndpoint = '/api/config/layers';

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

export async function getLayers(): Promise<AnyLayer[] | null> {
  try {
    return await fetch(layersConfigEndpoint).then((res) => res.json());
  } catch {
    return null;
  }
}
