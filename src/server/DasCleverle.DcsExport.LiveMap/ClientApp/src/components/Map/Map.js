import React, { useCallback, useState } from 'react';
import { useSelector, useStore } from 'react-redux';

import Alert from 'react-bootstrap/Alert';
import Spinner from 'react-bootstrap/Spinner';

import Mapbox from './Mapbox';
import Sidebar from './Sidebar/Sidebar';
import MissionSidebarCard from './Sidebar/MissionSidebarCard';
import AirbaseSidebarCard from './Sidebar/AirbaseSidebarCard';
import Backdrop from './Backdrop';

import './Map.css';

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

export default function Map() {
  const store = useStore();
  const [selectedAirbase, setAirbase] = useState(null);
  const phase = useSelector((state) => state.liveState.phase);
  const isRunning = useSelector((state) => state.liveState.isRunning);

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

  const handleAirbaseCardDismiss = () => setAirbase(null);

  const showBackdrop = phase !== 'loaded' || !isRunning;

  return (
    <>
      <Mapbox onClick={handleMapClick} />
      {!showBackdrop && (
        <Sidebar>
          <MissionSidebarCard />
          <AirbaseSidebarCard
            airbase={selectedAirbase}
            onDismiss={handleAirbaseCardDismiss}
          />
        </Sidebar>
      )}
      <Backdrop show={showBackdrop}>
        {(phase === 'loading' || phase === 'reconnecting') && (
          <Spinner animation="border" className="backdrop-spinner" />
        )}
        {phase === 'reconnecting' && (
          <Alert variant="danger" className="mt-2">
            Die Verbindung zum Server ist abgebrochen. Versuche erneute
            Verbindung ...
          </Alert>
        )}
        {phase === 'error' && (
          <Alert variant="danger">
            Die Verbindung konnte nicht hergestellt werden.
          </Alert>
        )}
        {phase === 'loaded' && !isRunning && (
          <Alert variant="info">
            Aktuell läuft keine Mission. Schau später noch einmal vorbei, oder
            bitte jemanden eine Mission zu starten.
          </Alert>
        )}
      </Backdrop>
    </>
  );
}
