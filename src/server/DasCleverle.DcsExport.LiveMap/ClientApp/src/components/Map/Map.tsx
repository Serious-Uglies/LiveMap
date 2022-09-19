import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { MapLayerMouseEvent, Popup } from 'react-map-gl';

import Alert from 'react-bootstrap/Alert';
import Spinner from 'react-bootstrap/Spinner';

import Mapbox from './Mapbox/Mapbox';
import Sidebar from './Sidebar/Sidebar';
import MissionSidebarCard from './Sidebar/MissionSidebarCard';
import AirbaseSidebarCard from './Sidebar/AirbaseSidebarCard';
import Backdrop from './Backdrop';
import ObjectPopup from './ObjectPopup';

import { Airbase, MapObject } from '../../api/types';
import { connect } from '../../api/liveState';
import { useViewState } from './hooks';

import './Map.css';
import { useAppDispatch, useAppSelector } from '../../store';
import { getLayers } from '../../api/config';
import { AnyLayer } from 'mapbox-gl';

interface ObjectPopupState {
  objects?: MapObject[];
  props: { latitude: number; longitude: number };
  show: boolean;
}

export default function Map() {
  const dispatch = useAppDispatch();
  const [viewState, setViewState] = useViewState();
  const [layers, setLayers] = useState<AnyLayer[]>([]);
  const [objectPopup, setObjectPopup] = useState<ObjectPopupState>({
    show: false,
    props: { latitude: 0, longitude: 0 },
  });
  const [airbase, setAirbase] = useState<Airbase | undefined>();
  const { ready: translationsReady, t } = useTranslation();

  useEffect(() => {
    dispatch(connect());
    getLayers().then((layers) => setLayers(layers ?? []));
  }, [dispatch]);

  const phase = useAppSelector((state) => state.liveState.phase);
  const isRunning = useAppSelector((state) => state.liveState.isRunning);
  const mapFeatures = useAppSelector((state) => state.liveState.mapFeatures);

  const handleMapClick = (event: MapLayerMouseEvent) => {
    // const objectFeatures = event.features?.filter(
    //   (f) => f.layer.id === 'objects'
    // );
    // if (objectFeatures?.length) {
    //   const selectedObjects = objectFeatures.map(
    //     (f) => objects[f.properties?.id]
    //   );
    //   const longitude =
    //     objectFeatures.reduce(
    //       (s, f) => s + (f.geometry as GeoJSON.Point).coordinates[0],
    //       0
    //     ) / objectFeatures.length;
    //   const latitude =
    //     objectFeatures.reduce(
    //       (s, f) => s + (f.geometry as GeoJSON.Point).coordinates[1],
    //       0
    //     ) / objectFeatures.length;
    //   const props = { longitude, latitude };
    //   setObjectPopup({ objects: selectedObjects, props, show: true });
    // }
    // const selectedAirbase = event.features?.find(
    //   (f) => f.layer.id === 'airbases'
    // );
    // if (selectedAirbase?.properties) {
    //   setAirbase(airbases[selectedAirbase.properties.id]);
    // }
  };

  const handleObjectPopupDismiss = () =>
    setObjectPopup({ show: false, props: { latitude: 0, longitude: 0 } });
  const handleAirbaseCardDismiss = () => setAirbase(undefined);

  const showBackdrop = !translationsReady || phase !== 'loaded' || !isRunning;

  return (
    <>
      <Mapbox
        onClick={handleMapClick}
        viewState={viewState}
        setViewState={setViewState}
        interactiveLayerIds={layers.map((l) => l.id)}
      >
        {objectPopup.show && (
          <Popup {...objectPopup.props} onClose={handleObjectPopupDismiss}>
            <ObjectPopup objects={objectPopup.objects} />
          </Popup>
        )}

        {layers.map((layer) => (
          <Mapbox.Layer
            key={layer.id}
            layer={layer}
            source={mapFeatures[layer.id]}
          />
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
            {t('phase.reconnecting')}
          </Alert>
        )}
        {phase === 'error' && (
          <Alert variant="danger">{t('phase.error')}</Alert>
        )}
        {phase === 'loaded' && !isRunning && (
          <Alert variant="info">{t('phase.notRunning')}</Alert>
        )}
      </Backdrop>
    </>
  );
}
