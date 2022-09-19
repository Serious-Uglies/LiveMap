import { useTranslation } from 'react-i18next';

import './ObjectPopup.css';

interface ObjectPopupProps {
  features?: GeoJSON.Feature[];
}

interface FeatureList {
  [key: string]: {
    count: number;
    name: string;
    player?: string;
  };
}

export default function ObjectPopup({ features }: ObjectPopupProps) {
  const { t } = useTranslation();

  if (!features) {
    return null;
  }

  const counts = features.reduce((map: FeatureList, feature) => {
    if (!feature.properties) {
      return map;
    }

    const name = feature.properties.name;
    const player = feature.properties.player;
    const key = player ? `${name}-${player}` : name;

    if (map[key]) {
      map[key].count++;
    } else {
      map[key] = { count: 1, name, player };
    }

    return map;
  }, {});

  const sorted = Object.entries(counts).sort(([a], [b]) => a.localeCompare(b));

  return (
    <div className="object-popup">
      <ul className="list-unstyled">
        {sorted.map(([key, feature]) => (
          <li key={key} className="mt-1">
            {t(
              feature.player ? 'objectPopup.player' : 'objectPopup.unit',
              feature
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}
