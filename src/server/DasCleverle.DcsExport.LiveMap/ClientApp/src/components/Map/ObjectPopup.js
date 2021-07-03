import React from 'react';
import { useTranslation } from 'react-i18next';

import './ObjectPopup.css';

export default function ObjectPopup({ objects }) {
  const { t } = useTranslation();
  const counts = objects.reduce((map, object) => {
    const name = object.displayName || object.typeName;
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
