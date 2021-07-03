import i18next from 'i18next';

import datetime from './datetime';
import frequency from './frequency';
import join from './join';

const formatters = { datetime, frequency, join };
const formatterRe = /^([A-Za-z0-9]+)(\(('?([A-Za-z0-9.]+)'?)\))?$/;

export default function format(value, format, lng) {
  const match = formatterRe.exec(format);

  if (!match) {
    return value;
  }

  const formatter = formatters[match[1]];
  const argument = match[3];

  const formatterArgument =
    argument && argument.startsWith("'") && argument.endsWith("'")
      ? (value) => i18next.t(match[4], { value })
      : formatters[argument];

  return formatter(value, formatterArgument, lng);
}
