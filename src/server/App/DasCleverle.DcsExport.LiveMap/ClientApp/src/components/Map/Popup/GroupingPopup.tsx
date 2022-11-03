import _ from 'lodash';
import { GroupingPopupConfig } from '../../../api/client';

interface GroupingPopupProps {
  input: GeoJSON.Feature[];
  config: GroupingPopupConfig;
}

export default function GroupingPopup({ input, config }: GroupingPopupProps) {
  let result = _.chain(input)
    .map((feature) => feature.properties)
    .groupBy((properties) => config.groupBy.evalSync(properties ?? undefined))
    .mapValues((value, key) => ({ key, value, count: value.length }))
    .values();

  if (config.orderBy) {
    result = result.orderBy((item) => config.orderBy!.evalSync(item));
  }

  const grouped = result
    .map((item) => {
      return {
        key: item.key,
        rendered: config.render.evalSync(item),
      };
    })
    .value();

  return (
    <ul className="list-unstyled">
      {grouped.map(({ key, rendered }) => (
        <li key={key} className="mt-1">
          {rendered}
        </li>
      ))}
    </ul>
  );
}
