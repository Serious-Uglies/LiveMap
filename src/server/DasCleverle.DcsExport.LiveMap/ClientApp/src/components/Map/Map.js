import React, { useCallback, useState } from 'react';
import { useStore } from 'react-redux';

import { Mapbox } from './Mapbox';
import { Sidebar } from './Sidebar/Sidebar';
import { MissionSidebarCard } from './Sidebar/MissionSidebarCard';
import { AirbaseSidebarCard } from './Sidebar/AirbaseSidebarCard';

const ObjectPopup = ({ selectedObjects }) => {
  return (
    <div className="mt-2">
      {selectedObjects.map((o) => (
        <div className="mt-1" key={o.id}>
          <strong>{o.typeName}</strong>
          <div>{o.name}</div>
        </div>
      ))}
    </div>
  );
};

export function Map() {
  const store = useStore();
  const [selectedAirbase, setAirbase] = useState(null);

  const handleMapClick = useCallback(
    (map, layer, features) => {
      const {
        liveState: { airbases, objects },
      } = store.getState();

      switch (layer) {
        case 'airbases':
          if (features.length === 0) {
            setAirbase(null);
          } else {
            setAirbase(airbases[features[0].properties.id]);
          }
          break;

        case 'objects':
          const selectedObjects = features.map((f) => objects[f.properties.id]);
          if (selectedObjects.length === 0) {
            return null;
          }

          return <ObjectPopup selectedObjects={selectedObjects} />;

        default:
          break;
      }
    },
    [store]
  );

  return (
    <>
      <Mapbox onClick={handleMapClick} />
      <Sidebar>
        <MissionSidebarCard />
        <AirbaseSidebarCard airbase={selectedAirbase} />
      </Sidebar>
    </>
  );
}
