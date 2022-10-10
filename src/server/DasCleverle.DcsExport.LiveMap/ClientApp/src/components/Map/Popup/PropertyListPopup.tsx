import { PropertyListPopupConfig } from '../../../api/client';

interface PropertyListPopupProps {
  input: GeoJSON.Feature;
  config: PropertyListPopupConfig;
}

export default function PropertyListPopup({
  input,
  config,
}: PropertyListPopupProps) {
  return (
    <ul className="list-unstyled">
      {config.properties.map((property) => {
        const label = property.label.evalSync(input.properties ?? undefined);
        const value = property.value.evalSync(input.properties ?? undefined);

        if (!value) {
          return null;
        }

        return (
          <li key={label} className="mt-1">
            <strong>{label}:&nbsp;</strong>
            {value}
          </li>
        );
      })}
    </ul>
  );
}
