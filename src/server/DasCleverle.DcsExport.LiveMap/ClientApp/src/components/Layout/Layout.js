import React, { Component } from 'react';
import { Container } from 'reactstrap';
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
