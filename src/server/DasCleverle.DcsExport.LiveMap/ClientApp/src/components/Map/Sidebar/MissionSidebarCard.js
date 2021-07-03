import React from 'react';
import { useSelector } from 'react-redux';

import SidebarCard from './SidebarCard';

const format = new Intl.DateTimeFormat('de-DE', {
  year: 'numeric',
  month: '2-digit',
  day: '2-digit',
  hour: '2-digit',
  minute: '2-digit',
  second: '2-digit',
});

export default function MissionSidebarCard() {
  const formatTime = (time) => format.format(time);

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
