import React from 'react';
import Navigation from './Navigation';

import './Layout.css';

type LayoutProps = {
  children: React.ReactNode;
};

export default function Layout({ children }: LayoutProps) {
  return (
    <>
      <Navigation />
      <div className="content">{children}</div>
    </>
  );
}
