import React from 'react';

import './Backdrop.css';

export default function Backdrop({ children, show }) {
  return (
    show && (
      <div className="backdrop-wrapper">
        <div className="backdrop"></div>
        <div className="backdrop-content">{children}</div>
      </div>
    )
  );
}
