import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import { useTranslation } from 'react-i18next';
import SidebarCard from './SidebarCard';

interface AirbaseSidebarCardProps {
  feature: GeoJSON.Feature | null;
  onDismiss: () => void;
}

interface AirbaseRunway {
  name: string;
  edge1: string;
  edge2: string;
  course: number;
}

interface AirbaseBeacon {
  runway?: string;
  callsign?: string;
  frequency: string;
}

export default function AirbaseSidebarCard({
  feature,
  onDismiss,
}: AirbaseSidebarCardProps) {
  const { t } = useTranslation();

  if (!feature?.properties) {
    return null;
  }

  // Unfortunately mapbox encodes nested properties as JSON
  // so we need to handle it here somehow ...
  const name = feature.properties.name;
  const frequencies = JSON.parse(feature.properties.frequencies);
  const runways = JSON.parse(feature.properties.runways);
  const beacons = JSON.parse(feature.properties.beacons);
  const { tacan, vor, ndb, ils } = beacons ?? {};

  const airbaseProperties = feature
    ? [
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
          value: runways?.length ? (
            <Runways runways={runways} ils={ils} />
          ) : null,
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
            vor?.length
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
            ndb?.length
              ? 'sidebar.airbase.ndbList'
              : 'sidebar.airbase.ndbListNone',
            {
              ndb,
            }
          ),
        },
      ]
    : undefined;

  return (
    <SidebarCard
      title={t('sidebar.airbase.title')}
      properties={airbaseProperties}
      dismissable
      visible={!!feature}
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
