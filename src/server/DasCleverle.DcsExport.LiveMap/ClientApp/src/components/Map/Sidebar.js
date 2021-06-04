import React from 'react';
import { useSelector } from 'react-redux';
import { theatres } from './theatres';

import './Sidebar.css';

const formatFrequency = (frequency) => {
  const unit = frequency > 1000000 ? 'MHz' : 'kHz';
  const converted = unit === 'MHz' ? frequency / 1000000 : frequency / 1000;

  const rounded = (
    Math.round((converted + Number.EPSILON) * 1000) / 1000
  ).toFixed(3);

  return `${rounded} ${unit}`;
};

const Runways = ({ runways, ilss }) => {
  if (runways.length === 0) {
    return null;
  }

  const getILS = (runway) => ilss.find((i) => i.runway === runway);

  const renderEdge = (edge) => {
    const ils = getILS(edge);
    return (
      <div key={edge}>
        <strong>{edge}</strong>
        {ils && <span> (ILS: {formatFrequency(ils.frequency)})</span>}
      </div>
    );
  };

  return (
    <div className="row">
      <div className="col">{runways.map((r) => renderEdge(r.edge1))}</div>
      <div className="col">{runways.map((r) => renderEdge(r.edge2))}</div>
    </div>
  );
};

const SidebarCard = ({ title, properties }) => {
  return (
    <div className="card property-card">
      <div className="card-header">{title}</div>
      <div className="card-body">
        {properties.map(
          ({ title, value, format }) =>
            value && (
              <div key={title}>
                <div className="property-title">{title}</div>
                {value}
              </div>
            )
        )}
      </div>
    </div>
  );
};

export function Sidebar({ selectedAirbase }) {
  const timeFormat = useSelector((state) => {
    const theatre = state.liveState.theatre;
    if (!theatre) {
      return null;
    }

    return new Intl.DateTimeFormat('de-DE', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      timeZone: theatres[theatre].timeZone,
    });
  });

  const formatTime = (time) => (timeFormat ? timeFormat.format(time) : '');

  const missionProperties = useSelector((state) => [
    { title: 'Mission', value: state.liveState.missionName },
    { title: 'Schauplatz', value: state.liveState.theatre },
    {
      title: 'Datum',
      value: formatTime(new Date(state.liveState.time)),
    },
  ]);

  const airbaseProperties = selectedAirbase && [
    { title: 'Name', value: selectedAirbase.name },
    {
      title: 'Tower',
      value:
        selectedAirbase.frequencies.length > 0
          ? selectedAirbase.frequencies.map(formatFrequency).join(', ')
          : 'Keine Frequenz bekannt',
    },
    {
      title: 'Runways',
      value: (
        <Runways
          runways={selectedAirbase.runways}
          ilss={selectedAirbase.beacons.ils}
        />
      ),
    },
    {
      title: 'TACAN',
      value:
        selectedAirbase.beacons.tacan.length > 0
          ? selectedAirbase.beacons.tacan
              .map((t) => `${t.channel}${t.mode} (${t.callsign})`)
              .join(', ')
          : null,
    },
    {
      title: 'VOR',
      value:
        selectedAirbase.beacons.vor.length > 0
          ? selectedAirbase.beacons.vor
              .map((v) => `${formatFrequency(v.frequency)} (${v.callsign})`)
              .join(', ')
          : null,
    },
    {
      title: 'NDB',
      value:
        selectedAirbase.beacons.ndb.length > 0
          ? selectedAirbase.beacons.ndb
              .map((v) => `${formatFrequency(v.frequency)} (${v.callsign})`)
              .join(', ')
          : null,
    },
  ];

  return (
    <div className="sidebar">
      <SidebarCard title="Aktuelle Mission" properties={missionProperties} />
      {selectedAirbase && (
        <SidebarCard
          title="Ausgewähltes Airfield"
          properties={airbaseProperties}
        />
      )}
    </div>
  );
}
