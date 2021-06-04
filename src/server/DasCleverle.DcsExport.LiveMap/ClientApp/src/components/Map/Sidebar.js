import React from 'react';
import { useSelector } from 'react-redux';
import { theatres } from './theatres';

import './Sidebar.css';

export function Sidebar() {
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
      value: new Date(state.liveState.time),
      format: formatTime,
    },
  ]);

  return (
    <div className="sidebar">
      <div className="card property-card">
        <div className="card-header">Aktuelle Mission</div>
        <div className="card-body">
          {missionProperties.map(({ title, value, format }) => (
            <>
              <div className="property-title">{title}</div>
              {format ? format(value) : value}
            </>
          ))}
        </div>
      </div>
    </div>
  );
}
