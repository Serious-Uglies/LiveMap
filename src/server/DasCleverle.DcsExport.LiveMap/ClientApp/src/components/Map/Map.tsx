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

import { connect } from '../../api/liveState';
import { useViewState } from './hooks';

import './Map.css';
import { useAppDispatch, useAppSelector } from '../../store';
import { getLayers } from '../../api/config';
import { AnyLayer } from 'mapbox-gl';

interface ObjectPopupState {
  features?: GeoJSON.Feature[];
  latitude?: number;
  longitude?: number;
}

export default function Map() {
  const dispatch = useAppDispatch();
  const [viewState, setViewState] = useViewState();
  const [layers, setLayers] = useState<AnyLayer[]>([]);
  const [objectPopup, setObjectPopup] = useState<ObjectPopupState>({});
  const [airbase, setAirbase] = useState<GeoJSON.Feature | null>(null);
  const { ready: translationsReady, t } = useTranslation();

  useEffect(() => {
    dispatch(connect());
    getLayers().then((layers) => setLayers(layers ?? []));
  }, [dispatch]);

  const phase = useAppSelector((state) => state.liveState.phase);
  const isRunning = useAppSelector((state) => state.liveState.isRunning);
  const mapFeatures = useAppSelector((state) => state.liveState.mapFeatures);

  const handleMapClick = (event: MapLayerMouseEvent) => {
    const features = event.features?.filter((f) => f.layer.id === 'objects');

    if (features?.length) {
      const longitude =
        features.reduce(
          (s, f) => s + (f.geometry as GeoJSON.Point).coordinates[0],
          0
        ) / features.length;
      const latitude =
        features.reduce(
          (s, f) => s + (f.geometry as GeoJSON.Point).coordinates[1],
          0
        ) / features.length;

      setObjectPopup({ features, longitude, latitude });
    }

    const airbase = event.features?.find((f) => f.layer.id === 'airbases');
    setAirbase(airbase ?? null);
  };

  const handleObjectPopupDismiss = () => setObjectPopup({ features: [] });
  const handleAirbaseCardDismiss = () => setAirbase(null);

  const showBackdrop = !translationsReady || phase !== 'loaded' || !isRunning;

  return (
    <>
      <Mapbox
        onClick={handleMapClick}
        viewState={viewState}
        setViewState={setViewState}
        interactiveLayerIds={layers.map((l) => l.id)}
      >
        {objectPopup.features?.length ? (
          <Popup
            latitude={objectPopup.latitude ?? 0}
            longitude={objectPopup.longitude ?? 0}
            closeOnClick={false}
            onClose={handleObjectPopupDismiss}
          >
            <ObjectPopup features={objectPopup.features} />
          </Popup>
        ) : null}

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
            feature={airbase}
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
