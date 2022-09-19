export type Map<T> = { [id: string]: T };

export type Phase =
  | 'uninitialized'
  | 'loading'
  | 'loaded'
  | 'reconnecting'
  | 'error';

export type ObjectType = 'Unknown' | 'Unit' | 'Static';

export type Coalition = 'Neutral' | 'Red' | 'Blue';

export type ObjectAttribute =
  | 'Fixed'
  | 'Awacs'
  | 'Tanker'
  | 'Rotary'
  | 'Ground'
  | 'Water';

export type AirbaseCategory = 'airdrome' | 'farp';

export interface Position {
  lat: number;
  long: number;
}

export interface MapObject {
  id: number;
  type: ObjectType;
  int?: number;
  name?: string;
  displayName?: string;
  coalition: Coalition;
  country?: string;
  typeName?: string;
  player?: string;
  attributes: ObjectAttribute[];
  position: Position;
}

export interface AirbaseRunway {
  name: string;
  edge1: string;
  edge2: string;
  course: number;
}

export interface AirbaseBeacon {
  runway?: string;
  callsign?: string;
  frequency: string;
  position?: Position;
}

export interface ChannelBeacon extends AirbaseBeacon {
  channel: number;
}

export interface TacanBeacon extends ChannelBeacon {
  mode?: string;
}

export interface AirbaseBeacons {
  tacan: TacanBeacon[];
  ils: AirbaseBeacon[];
  vor: ChannelBeacon[];
  ndb: AirbaseBeacon[];
  rsbn: ChannelBeacon[];
  prmg: AirbaseBeacon[];
}

export interface Airbase {
  id: string;
  name?: string;
  coalition: Coalition;
  category: AirbaseCategory;
  runways: AirbaseRunway[];
  frequencies: number[];
  beacons: AirbaseBeacons;
  position?: Position;
}

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
