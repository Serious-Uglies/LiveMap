import i18next from 'i18next';

import datetime from './datetime';
import frequency from './frequency';
import join from './join';

export type Formatter = (
  value: any,
  lng: string,
  nestedFormatter?: Formatter
) => string;

const formatters: { [name: string]: Formatter } = {
  datetime,
  frequency,
  join,
};
const formatterRe =
  /^([A-Za-z0-9]+)(?:\((?:'([A-Za-z0-9.]+)'|([A-Za-z0-9.]+))\))?$/;

export default function format(
  value: any,
  format?: string,
  lng?: string
): string {
  if (!lng || !format) {
    return String(value);
  }

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

  let argument: Formatter | undefined = undefined;
  if (translationKey) {
    argument = (value: any) => i18next.t(translationKey, { value });
  } else if (nestedFormatter) {
    argument = formatters[nestedFormatter];
  }

  return formatter(value, lng, argument);
}
