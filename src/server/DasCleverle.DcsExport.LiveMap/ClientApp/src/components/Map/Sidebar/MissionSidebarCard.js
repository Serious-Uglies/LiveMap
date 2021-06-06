import React from 'react';
import { useSelector } from 'react-redux';

import SidebarCard from './SidebarCard';

export default function MissionSidebarCard() {
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

  return (
    <SidebarCard title="Aktuelle Mission" properties={missionProperties} />
  );
}
