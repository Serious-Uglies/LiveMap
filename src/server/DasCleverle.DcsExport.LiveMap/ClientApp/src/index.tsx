import 'bootstrap/dist/css/bootstrap.min.css';
import 'mapbox-gl/dist/mapbox-gl.css';
import 'flag-icons/css/flag-icons.min.css';

import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';

import { Provider } from 'react-redux';
import store from './store';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const container = document.getElementById('root');
const root = createRoot(container!);

root.render(
  <Provider store={store}>
    <BrowserRouter basename={baseUrl!}>
      <App />
    </BrowserRouter>
  </Provider>
);
