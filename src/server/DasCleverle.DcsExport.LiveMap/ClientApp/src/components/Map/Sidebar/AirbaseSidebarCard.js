import React from 'react';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import { useTranslation, Trans } from 'react-i18next';
import SidebarCard from './SidebarCard';

export default function AirbaseSidebarCard({ airbase, onDismiss }) {
  const { t } = useTranslation();
  const { name, frequencies, runways, beacons } = airbase || {};

  const { tacan, vor, ndb, ils } = beacons || {};

  const airbaseProperties = airbase && [
    { title: t('sidebar.airbase.name'), value: name },
    {
      title: t('sidebar.airbase.frequency'),
      value: t(
        frequencies.length > 0
          ? 'sidebar.airbase.frequencyList'
          : 'sidebar.airbase.frequencyListNone',
        {
          frequencies,
        }
      ),
    },
    {
      title: t('sidebar.airbase.runways'),
      value: runways.length ? <Runways runways={runways} ils={ils} /> : null,
    },
    {
      title: t('sidebar.airbase.tacan'),
      value: t(
        tacan.length > 0
          ? 'sidebar.airbase.tacanList'
          : 'sidebar.airbase.tacanListNone',
        {
          tacan,
        }
      ),
    },
    {
      title: t('sidebar.airbase.vor'),
      value: t(
        vor.length > 0
          ? 'sidebar.airbase.vorList'
          : 'sidebar.airbase.vorListNone',
        {
          vor,
        }
      ),
    },
    {
      title: t('sidebar.airbase.ndb'),
      value: t(
        ndb.length > 0
          ? 'sidebar.airbase.ndbList'
          : 'sidebar.airbase.ndbListNone',
        {
          ndb,
        }
      ),
    },
  ];

  return (
    <SidebarCard
      title={t('sidebar.airbase.title')}
      properties={airbaseProperties}
      dismissable
      visible={!!airbase}
      onDismiss={onDismiss}
    />
  );
}

function Runways({ runways, ils }) {
  return (
    <Row>
      <Col>
        {runways.map((r) => (
          <RunwayEdge key={r.edge1} edge={r.edge1} ils={ils} />
        ))}
      </Col>
      <Col>
        {runways.map((r) => (
          <RunwayEdge key={r.edge1} edge={r.edge2} ils={ils} />
        ))}
      </Col>
    </Row>
  );
}

function RunwayEdge({ edge, ils }) {
  const { t } = useTranslation();
  const runwayILS = ils.find((i) => i.runway === edge);

  return (
    <div key={edge}>
      <Trans i18nKey="sidebar.airbase.runwayEdge">
        <strong>{{ edge }}</strong>
      </Trans>
      {runwayILS && t('sidebar.airbase.runwayILS', { ils: runwayILS })}
    </div>
  );
}
