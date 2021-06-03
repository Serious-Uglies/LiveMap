import React from 'react';
import { Navigation } from './Navigation';

import './Layout.css';

export function Layout({ children }) {
  return (
    <>
      <Navigation />
      <div className="content">{children}</div>
    </>
  );
}
