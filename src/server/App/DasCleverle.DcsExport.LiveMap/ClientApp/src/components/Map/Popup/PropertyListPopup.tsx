import {
  ListPropertyDisplay,
  PropertyDisplay,
  PropertyListPopupConfig,
  ScalarPropertyDisplay,
} from '../../../api/client';

interface PropertyListPopupProps {
  input: GeoJSON.Feature;
  config: PropertyListPopupConfig;
}

function getScalar(display: ScalarPropertyDisplay, input: any) {
  const value = display.value.evalSync(input);
  return value ? value : null;
}

function getList(display: ListPropertyDisplay, input: any) {
  const list = display.selector.evalSync(input);

  if (!Array.isArray(list) || list.length === 0) {
    return null;
  }

  return (
    <ul>
      {list.map((item, index) =>
        item ? <li key={index}>{display.value.evalSync({ item })}</li> : null
      )}
    </ul>
  );
}

function getDisplay(display: PropertyDisplay, input: any) {
  switch (display.type) {
    case 'scalar':
      return getScalar(display, input);
    case 'list':
      return getList(display, input);
  }
}

export default function PropertyListPopup({
  input,
  config,
}: PropertyListPopupProps) {
  return (
    <ul className="list-unstyled">
      {config.properties.map((property) => {
        const label = property.label.evalSync(input.properties ?? undefined);
        const display = getDisplay(property.display, input.properties);

        if (!display) {
          return null;
        }

        return (
          <li key={property.id} className="mt-1">
            <strong>{label}:&nbsp;</strong>
            {display}
          </li>
        );
      })}
    </ul>
  );
}
