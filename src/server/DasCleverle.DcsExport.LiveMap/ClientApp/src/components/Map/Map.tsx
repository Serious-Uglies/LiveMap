import mapboxgl, { AnyLayer } from 'mapbox-gl';
import { useCallback, useEffect, useState } from 'react';
import Alert from 'react-bootstrap/Alert';
import Spinner from 'react-bootstrap/Spinner';
import { useTranslation } from 'react-i18next';
import { MapLayerMouseEvent } from 'react-map-gl';
import { getLayers } from '../../api/config';
import { connect } from '../../api/liveState';
import { useAppDispatch, useAppSelector } from '../../store';
import Backdrop from './Backdrop';
import { useViewState } from './hooks';
import './Map.css';
import Mapbox from './Mapbox/Mapbox';
import Popup from './Popup/Popup';
import MissionSidebarCard from './Sidebar/MissionSidebarCard';
import Sidebar from './Sidebar/Sidebar';

export default function Map() {
  const dispatch = useAppDispatch();
  const [viewState, setViewState] = useViewState();
  const [layers, setLayers] = useState<AnyLayer[]>([]);
  const [popup, setPopup] = useState<mapboxgl.MapboxGeoJSONFeature[]>([]);
  const { ready: translationsReady, t } = useTranslation();

  useEffect(() => {
    dispatch(connect());
    getLayers().then((layers) => setLayers(layers ?? []));
  }, [dispatch]);

  const phase = useAppSelector((state) => state.liveState.phase);
  const isRunning = useAppSelector((state) => state.liveState.isRunning);
  const mapFeatures = useAppSelector((state) => state.liveState.mapFeatures);
  const showBackdrop = !translationsReady || phase !== 'loaded' || !isRunning;

  const handleMapClick = useCallback((event: MapLayerMouseEvent) => {
    setPopup(event.features ?? []);
  }, []);

  const handlePopupClose = useCallback(() => {
    setPopup([]);
  }, []);

  return (
    <>
      <Mapbox
        onClick={handleMapClick}
        viewState={viewState}
        setViewState={setViewState}
        interactiveLayerIds={layers.map((l) => l.id)}
      >
        {popup.length ? (
          <Popup features={popup} onClose={handlePopupClose} />
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
