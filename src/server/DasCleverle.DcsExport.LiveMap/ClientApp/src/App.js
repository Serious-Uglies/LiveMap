import React from 'react';
import { Route } from 'react-router';

import Layout from './components/Layout/Layout';
import Map from './components/Map/Map';

import './App.css';

export default function App() {
  return (
    <React.Suspense fallback={<p>Loading ...</p>}>
      <Layout>
        <Route exact path="/" component={Map} />
      </Layout>
    </React.Suspense>
  );
}
