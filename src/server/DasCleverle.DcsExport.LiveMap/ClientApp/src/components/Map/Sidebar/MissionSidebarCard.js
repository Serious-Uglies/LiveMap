import React from 'react';
import { useSelector } from 'react-redux';
import { useTranslation } from 'react-i18next';

import SidebarCard from './SidebarCard';

export default function MissionSidebarCard() {
  const { t } = useTranslation();
  const missionProperties = useSelector((state) => [
    { title: t('sidebar.mission.name'), value: state.liveState.missionName },
    { title: t('sidebar.mission.theatre'), value: state.liveState.theatre },
    {
      title: t('sidebar.mission.time'),
      value: t('sidebar.mission.timeValue', {
        time: new Date(state.liveState.time),
      }),
    },
  ]);

  return (
    <SidebarCard
      title={t('sidebar.mission.title')}
      properties={missionProperties}
    />
  );
}
