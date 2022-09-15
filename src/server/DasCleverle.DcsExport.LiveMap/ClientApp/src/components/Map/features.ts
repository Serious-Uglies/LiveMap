import { Airbase, MapObject, Position } from '../../api/types';
import { UpdateObjectPayload } from '../../store/types';

export type FeatureFactory = (value: any) => GeoJSON.Feature | null;
export type FeatureUpdater = (feature: GeoJSON.Feature, value: any) => void;

const createFeature = (
  id: string | number,
  position: Position,
  properties: any
): GeoJSON.Feature<GeoJSON.Point> => {
  return {
    type: 'Feature',
    id,
    properties: {
      ...properties,
      id,
    },
    geometry: {
      type: 'Point',
      coordinates: [position.long, position.lat],
    },
  };
};

const getIconType = (object: MapObject) => {
  if (object.type === 'Static') {
    return 'ground';
  }

  if (object.attributes.includes('Water')) {
    return 'water';
  } else if (object.attributes.includes('Ground')) {
    return 'ground';
  } else if (object.attributes.includes('Rotary')) {
    return 'rotary';
  } else if (object.attributes.includes('Tanker')) {
    return 'tanker';
  } else if (object.attributes.includes('Awacs')) {
    return 'awacs';
  }

  return 'fixed';
};

const getSortKey = (object: MapObject) => {
  if (
    object.type === 'Unit' &&
    (object.attributes.includes('Fixed') ||
      object.attributes.includes('Rotary'))
  ) {
    return 1;
  }

  return 0;
};

export const createObjectFeature = (object: MapObject) => {
  const coalition = object.coalition.toLowerCase();
  const iconType = getIconType(object);
  const pilot = object.player ? 'player' : 'ai';

  return createFeature(object.id, object.position, {
    icon: `${coalition}-${iconType}-${pilot}`,
    sortKey: getSortKey(object),
  });
};

export const updateObjectFeature = (
  feature: GeoJSON.Feature,
  { position }: UpdateObjectPayload
) => {
  if (!position) {
    return;
  }

  if (feature.geometry.type !== 'Point') {
    return;
  }

  feature.geometry.coordinates = [position.long, position.lat];
};

export const createAirbaseFeature = (airbase: Airbase) => {
  if (!airbase.position) {
    return null;
  }

  const coalition = airbase.coalition.toLowerCase();
  const category = airbase.category.toLowerCase();
  const rotation = airbase.runways[0] ? airbase.runways[0].course : 0;

  return createFeature(airbase.id, airbase.position, {
    name: airbase.name,
    icon: `${coalition}-${category}`,
    rotation,
  });
};
