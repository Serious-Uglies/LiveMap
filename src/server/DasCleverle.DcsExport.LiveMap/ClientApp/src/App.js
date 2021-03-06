import React from 'react';
import { Route } from 'react-router';

import Layout from './components/Layout/Layout';
import Map from './components/Map/Map';

import './App.css';
import './i18n';

export default function App() {
  return (
    <Layout>
      <Route exact path="/" component={Map} />
    </Layout>
  );
}
