import { AnyLayer } from 'mapbox-gl';
import { LiveState } from '../../api/types';
import {
  createAirbaseFeature,
  createObjectFeature,
  FeatureFactory,
  FeatureUpdater,
  updateObjectFeature,
} from './features';

export interface LayerConfig {
  getData: (liveState: LiveState) => any;
  createFeature: FeatureFactory;
  updateFeature?: FeatureUpdater;
  config: AnyLayer;
}

const layers: LayerConfig[] = [
  {
    getData: (liveState) => liveState.objects,
    createFeature: createObjectFeature,
    updateFeature: updateObjectFeature,
    config: {
      id: 'objects',
      type: 'symbol',
      layout: {
        'icon-image': '{icon}',
        'icon-allow-overlap': true,
        'icon-size': 0.7,
        'symbol-sort-key': ['get', 'sortKey'],
      },
    },
  },
  {
    getData: (liveState) => liveState.airbases,
    createFeature: createAirbaseFeature,
    config: {
      id: 'airbases',
      type: 'symbol',
      layout: {
        'icon-image': '{icon}',
        'icon-allow-overlap': true,
        'icon-size': 0.7,
        'icon-rotate': ['get', 'rotation'],
        'text-field': '{name}',
        'text-anchor': 'left',
        'text-offset': [0.73, 0],
        'text-font': ['DIN Pro Regular'],
        'text-size': [
          'interpolate',
          ['cubic-bezier', 0.2, 0, 0.9, 1],
          ['zoom'],
          3,
          12,
          13,
          19,
        ],
      },
      paint: {
        'text-halo-color': 'white',
        'text-halo-width': 1,
        'text-halo-blur': 1,
      },
    },
  },
];

export default layers;
