import { Layer, Source } from 'react-map-gl';
import { AnyLayer } from 'mapbox-gl';

interface MapboxLayerProps {
  source: GeoJSON.FeatureCollection;
  layer: AnyLayer;
}

export default function MapboxLayer({ layer, source }: MapboxLayerProps) {
  return (
    <Source id={layer.id} type="geojson" data={source}>
      <Layer {...layer} />
    </Source>
  );
}
