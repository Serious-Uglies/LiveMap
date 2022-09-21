export type Map<T> = { [id: string]: T };

export type Phase =
  | 'uninitialized'
  | 'loading'
  | 'loaded'
  | 'reconnecting'
  | 'error';

export interface MapFeatures {
  [source: string]: GeoJSON.FeatureCollection;
}

export interface LiveState {
  phase: Phase;
  isRunning: boolean;
  mapFeatures: MapFeatures;
  missionName?: string;
  theatre?: string;
  time?: string;
}
