const createFeature = (id, position, properties) => {
  return {
    type: 'Feature',
    id,
    properties,
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

export const objectToFeature = (object) => {
  const coalition = object.coalition.toLowerCase();
  const iconType = getIconType(object);
  const pilot = object.player ? 'player' : 'ai';

  return createFeature(object.id, object.position, {
    icon: `${coalition}-${iconType}-${pilot}`,
  });
};

export const airbaseToFeature = (airbase) => {
  const coalition = airbase.coalition.toLowerCase();
  const category = airbase.category.toLowerCase();
  const rotation = airbase.runways[0] ? airbase.runways[0].course : 0;

  return createFeature(airbase.id, airbase.position, {
    name: airbase.name,
    icon: `${coalition}-${category}`,
    rotation,
  });
};

export const getObjectLayer = (object) => {
  if (
    object.type === 'Unit' &&
    (object.attributes.includes('Fixed') ||
      object.attributes.includes('Rotary'))
  ) {
    return 'objects-air';
  }

  return 'objects-earthbound';
};
