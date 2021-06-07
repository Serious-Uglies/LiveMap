import React from 'react';

import './ObjectPopup.css';

export default function ObjectPopup({ objects }) {
  const counts = objects.reduce((map, object) => {
    const unitName = object.displayName || object.typeName;
    const displayName = object.player
      ? `${unitName} (${object.player})`
      : unitName;

    if (map[displayName]) {
      map[displayName]++;
    } else {
      map[displayName] = 1;
    }

    return map;
  }, {});

  const sorted = Object.entries(counts).sort(([a], [b]) => a.localeCompare(b));

  return (
    <div className="object-popup">
      <ul className="list-unstyled">
        {sorted.map(([displayName, count]) => (
          <li className="mt-1" key={displayName}>
            {count}x {displayName}
          </li>
        ))}
      </ul>
    </div>
  );
}
