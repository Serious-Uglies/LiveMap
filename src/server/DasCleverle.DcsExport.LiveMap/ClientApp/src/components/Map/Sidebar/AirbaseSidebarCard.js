import React from 'react';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import { SidebarCard } from './SidebarCard';

import { formatFrequency } from './util';

const getILS = (runway, ilss) => ilss.find((i) => i.runway === runway);

const renderEdge = (edge, ilss) => {
  const ils = getILS(edge, ilss);
  return (
    <div key={edge}>
      <strong>{edge}</strong>
      {ils && <span> (ILS: {formatFrequency(ils.frequency)})</span>}
    </div>
  );
};

function Runways({ airbase }) {
  const {
    runways,
    beacons: { ils },
  } = airbase;

  if (runways.length === 0) {
    return null;
  }

  return (
    <Row>
      <Col>{runways.map((r) => renderEdge(r.edge1, ils))}</Col>
      <Col>{runways.map((r) => renderEdge(r.edge2, ils))}</Col>
    </Row>
  );
}

export function AirbaseSidebarCard({ airbase, onDismiss }) {
  const airbaseProperties = airbase && [
    { title: 'Name', value: airbase.name },
    {
      title: 'Tower',
      value:
        airbase.frequencies.length > 0
          ? airbase.frequencies.map(formatFrequency).join(', ')
          : 'Keine Frequenz bekannt',
    },
    {
      title: 'Runways',
      value: <Runways airbase={airbase} />,
    },
    {
      title: 'TACAN',
      value:
        airbase.beacons.tacan.length > 0
          ? airbase.beacons.tacan
              .map((t) => `${t.channel}${t.mode} (${t.callsign})`)
              .join(', ')
          : null,
    },
    {
      title: 'VOR',
      value:
        airbase.beacons.vor.length > 0
          ? airbase.beacons.vor
              .map((v) => `${formatFrequency(v.frequency)} (${v.callsign})`)
              .join(', ')
          : null,
    },
    {
      title: 'NDB',
      value:
        airbase.beacons.ndb.length > 0
          ? airbase.beacons.ndb
              .map((v) => `${formatFrequency(v.frequency)} (${v.callsign})`)
              .join(', ')
          : null,
    },
  ];

  return (
    <SidebarCard
      title="Ausgewähltes Airfield"
      properties={airbaseProperties}
      dismissable
      visible={!!airbase}
      onDismiss={onDismiss}
    />
  );
}
