import { Layer, Source } from 'react-map-gl';
import { AnyLayer } from 'mapbox-gl';

interface MapboxLayerProps {
  source: GeoJSON.FeatureCollection;
  layer: AnyLayer;
}

const emptyFeatureCollection: GeoJSON.FeatureCollection = {
  type: 'FeatureCollection',
  features: [],
};

export default function MapboxLayer({
  layer,
  source = emptyFeatureCollection,
}: MapboxLayerProps) {
  return (
    <Source id={layer.id} type="geojson" data={source}>
      <Layer {...layer} />
    </Source>
  );
}
