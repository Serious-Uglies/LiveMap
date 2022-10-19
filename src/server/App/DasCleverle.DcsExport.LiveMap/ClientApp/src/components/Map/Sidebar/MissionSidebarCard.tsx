import { useTranslation } from 'react-i18next';
import { useAppSelector } from '../../../store';

import SidebarCard from './SidebarCard';

export default function MissionSidebarCard() {
  const { t } = useTranslation();
  const missionProperties = useAppSelector((state) => [
    { title: t('sidebar.mission.name'), value: state.liveState.missionName },
    { title: t('sidebar.mission.theatre'), value: state.liveState.theatre },
    {
      title: t('sidebar.mission.time'),
      value: t('sidebar.mission.timeValue', {
        time: state.liveState.time ? new Date(state.liveState.time) : undefined,
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
