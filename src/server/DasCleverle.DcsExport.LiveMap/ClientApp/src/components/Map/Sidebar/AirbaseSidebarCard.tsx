import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import { useTranslation } from 'react-i18next';
import { Airbase, AirbaseBeacon, AirbaseRunway } from '../../../api/types';
import SidebarCard from './SidebarCard';

interface AirbaseSidebarCardProps {
  airbase?: Airbase;
  onDismiss: () => void;
}

export default function AirbaseSidebarCard({
  airbase,
  onDismiss,
}: AirbaseSidebarCardProps) {
  const { t } = useTranslation();
  const { name, frequencies, runways, beacons } = airbase || {};

  const { tacan, vor, ndb, ils } = beacons || {};

  const airbaseProperties = airbase && [
    { title: t('sidebar.airbase.name'), value: name },
    {
      title: t('sidebar.airbase.frequency'),
      value: t(
        frequencies?.length
          ? 'sidebar.airbase.frequencyList'
          : 'sidebar.airbase.frequencyListNone',
        {
          frequencies,
        }
      ),
    },
    {
      title: t('sidebar.airbase.runways'),
      value: runways?.length ? <Runways runways={runways} ils={ils} /> : null,
    },
    {
      title: t('sidebar.airbase.tacan'),
      value: t(
        tacan?.length
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
        vor?.length ? 'sidebar.airbase.vorList' : 'sidebar.airbase.vorListNone',
        {
          vor,
        }
      ),
    },
    {
      title: t('sidebar.airbase.ndb'),
      value: t(
        ndb?.length ? 'sidebar.airbase.ndbList' : 'sidebar.airbase.ndbListNone',
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

interface RunwaysProps {
  runways: AirbaseRunway[];
  ils?: AirbaseBeacon[];
}

function Runways({ runways, ils }: RunwaysProps) {
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

interface RunwayEdgeProps {
  edge: string;
  ils?: AirbaseBeacon[];
}

function RunwayEdge({ edge, ils }: RunwayEdgeProps) {
  const { t } = useTranslation();
  const runwayILS = ils?.find((i) => i.runway === edge);

  return (
    <div key={edge}>
      <strong>{edge}</strong>
      {runwayILS && t('sidebar.airbase.runwayILS', { ils: runwayILS })}
    </div>
  );
}
