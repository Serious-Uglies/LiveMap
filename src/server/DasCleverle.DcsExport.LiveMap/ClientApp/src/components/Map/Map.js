import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Popup } from 'react-map-gl';

import Alert from 'react-bootstrap/Alert';
import Spinner from 'react-bootstrap/Spinner';

import Mapbox from './Mapbox/Mapbox';
import Sidebar from './Sidebar/Sidebar';
import MissionSidebarCard from './Sidebar/MissionSidebarCard';
import AirbaseSidebarCard from './Sidebar/AirbaseSidebarCard';
import Backdrop from './Backdrop';

import { connect } from '../../api/liveState';
import { useViewport } from './hooks';
import layers from './layers';

import './Map.css';
import { createSelector } from 'reselect';
import liveState from '../../store/liveState';

const ObjectPopup = ({ objects }) => {
  return (
    <div className="mt-2">
      {objects.map((o) => (
        <div className="mt-1" key={o.id}>
          <strong>{o.typeName}</strong>
          <div>{o.name}</div>
        </div>
      ))}
    </div>
  );
};

const layersSelector = createSelector(
  (state) => state.liveState,
  (liveState) =>
    layers.map((layer) => ({
      ...layer,
      data: layer.getData(liveState),
    }))
);

export default function Map() {
  const dispatch = useDispatch();

  useEffect(() => dispatch(connect()), [dispatch]);

  const [viewport, setViewport] = useViewport();
  const layers = useSelector(layersSelector);

  const phase = useSelector((state) => state.liveState.phase);
  const isRunning = useSelector((state) => state.liveState.isRunning);
  const objects = useSelector((state) => state.liveState.objects);
  const airbases = useSelector((state) => state.liveState.airbases);

  const [objectPopup, setObjectPopup] = useState({ show: false });
  const [airbase, setAirbase] = useState();

  const handleMapClick = (event) => {
    const objectFeatures = event.features.filter(
      (f) => f.layer.id === 'objects'
    );

    if (objectFeatures.length) {
      const selectedObjects = objectFeatures.map(
        (f) => objects[f.properties.id]
      );

      const longitude =
        objectFeatures.reduce((s, f) => s + f.geometry.coordinates[0], 0) /
        objectFeatures.length;

      const latitude =
        objectFeatures.reduce((s, f) => s + f.geometry.coordinates[1], 0) /
        objectFeatures.length;

      const props = { longitude, latitude };

      setObjectPopup({ objects: selectedObjects, props, show: true });
    }

    const selectedAirbase = event.features.find(
      (f) => f.layer.id === 'airbases'
    );

    if (selectedAirbase) {
      setAirbase(airbases[selectedAirbase.properties.id]);
    }
  };

  const handleObjectPopupDismiss = () => setObjectPopup({ show: false });
  const handleAirbaseCardDismiss = () => setAirbase(null);

  const showBackdrop = phase !== 'loaded' || !isRunning;

  return (
    <>
      <Mapbox
        onClick={handleMapClick}
        viewport={viewport}
        onViewportChange={setViewport}
        interactiveLayerIds={layers.map((l) => l.id)}
      >
        {objectPopup.show && (
          <Popup {...objectPopup.props} onClose={handleObjectPopupDismiss}>
            <ObjectPopup objects={objectPopup.objects} />
          </Popup>
        )}

        {layers.map((layer) => (
          <Mapbox.Layer key={layer.id} {...layer} />
        ))}
      </Mapbox>
      {!showBackdrop && (
        <Sidebar>
          <MissionSidebarCard />
          <AirbaseSidebarCard
            airbase={airbase}
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
