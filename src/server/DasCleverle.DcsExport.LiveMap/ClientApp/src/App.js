import React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout/Layout';
import { Mapbox } from './components/Map/Mapbox';

import './App.css';

export default function App() {
  return (
    <Layout>
      <Route exact path="/" component={Mapbox} />
    </Layout>
  );
}
