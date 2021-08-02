import i18next from 'i18next';

import datetime from './datetime';
import frequency from './frequency';
import join from './join';

const formatters = { datetime, frequency, join };
const formatterRe =
  /^([A-Za-z0-9]+)(?:\((?:'([A-Za-z0-9.]+)'|([A-Za-z0-9.]+))\))?$/;

export default function format(value, format, lng) {
  const match = formatterRe.exec(format);

  if (!match) {
    return value;
  }

  // Capturing groups:
  // 1: formatter
  // 2: translation key
  // 3: nested formatter
  // (2 and 3 are mutually exclusive)

  const formatter = formatters[match[1]];

  if (!formatter) {
    return `Format error: Could not find formatter (format: ${format})`;
  }

  const translationKey = match[2];
  const nestedFormatter = match[3];

  let argument;
  if (translationKey) {
    argument = (value) => i18next.t(translationKey, { value });
  } else if (nestedFormatter) {
    argument = formatters[nestedFormatter];
  }

  return formatter(value, argument, lng);
}
