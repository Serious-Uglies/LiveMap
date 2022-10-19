import { Routes } from 'react-router-dom';
import { Route } from 'react-router';

import Layout from './components/Layout/Layout';
import Map from './components/Map/Map';

import './App.css';
import './i18n';

export default function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Map />} />
      </Routes>
    </Layout>
  );
}
