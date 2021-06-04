import { formatFrequency } from './util';
import { SidebarCard } from './SidebarCard';

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
    <div className="row">
      <div className="col">{runways.map((r) => renderEdge(r.edge1, ils))}</div>
      <div className="col">{runways.map((r) => renderEdge(r.edge2, ils))}</div>
    </div>
  );
}

export function AirbaseSidebarCard({ airbase }) {
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
    airbase && (
      <SidebarCard
        title="Ausgewähltes Airfield"
        properties={airbaseProperties}
      />
    )
  );
}
