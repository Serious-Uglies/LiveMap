const formatOptions = {
  year: 'numeric',
  month: '2-digit',
  day: '2-digit',
  hour: '2-digit',
  minute: '2-digit',
  second: '2-digit',
};
const formats = {};

const getFormat = (lng) =>
  formats[lng] || (formats[lng] = new Intl.DateTimeFormat(lng, formatOptions));

export default function datetime(value, _, lng) {
  return getFormat(lng).format(value);
}
