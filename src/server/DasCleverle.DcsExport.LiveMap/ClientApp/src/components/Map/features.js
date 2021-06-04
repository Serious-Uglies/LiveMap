const createFeature = (id, position, properties) => {
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

const getIconType = (object) => {
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

const getSortKey = (object) => {
  if (
    object.type === 'Unit' &&
    (object.attributes.includes('Fixed') ||
      object.attributes.includes('Rotary'))
  ) {
    return 1;
  }

  return 0;
};

export const createObjectFeature = (object) => {
  const coalition = object.coalition.toLowerCase();
  const iconType = getIconType(object);
  const pilot = object.player ? 'player' : 'ai';

  return createFeature(object.id, object.position, {
    icon: `${coalition}-${iconType}-${pilot}`,
    sortKey: getSortKey(object),
  });
};

export const updateObjectFeature = (feature, { position }) => {
  feature.geometry.coordinates = [position.long, position.lat];
};

export const createAirbaseFeature = (airbase) => {
  const coalition = airbase.coalition.toLowerCase();
  const category = airbase.category.toLowerCase();
  const rotation = airbase.runways[0] ? airbase.runways[0].course : 0;

  return createFeature(airbase.id, airbase.position, {
    name: airbase.name,
    icon: `${coalition}-${category}`,
    rotation,
  });
};
