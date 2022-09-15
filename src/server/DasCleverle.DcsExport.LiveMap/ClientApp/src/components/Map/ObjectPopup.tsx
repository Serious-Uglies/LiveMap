import { useTranslation } from 'react-i18next';
import { MapObject } from '../../api/types';

import './ObjectPopup.css';

interface ObjectPopupProps {
  objects?: MapObject[];
}

interface ObjectPopupAggreated {
  [key: string]: {
    count: number;
    name: string;
    player?: string;
  };
}

export default function ObjectPopup({ objects }: ObjectPopupProps) {
  const { t } = useTranslation();

  if (!objects) {
    return null;
  }

  const counts = objects.reduce((map: ObjectPopupAggreated, object) => {
    const name = object.displayName || object.typeName || '';
    const player = object.player;
    const key = object.player ? `${name}-${object.player}` : name;

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
        {sorted.map(([key, object]) => (
          <li key={key} className="mt-1">
            {t(
              object.player ? 'objectPopup.player' : 'objectPopup.unit',
              object
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}
