const formatOptions: Intl.DateTimeFormatOptions = {
  year: 'numeric',
  month: '2-digit',
  day: '2-digit',
  hour: '2-digit',
  minute: '2-digit',
  second: '2-digit',
};
const formats: { [lng: string]: Intl.DateTimeFormat } = {};

const getFormat = (lng: string) =>
  formats[lng] || (formats[lng] = new Intl.DateTimeFormat(lng, formatOptions));

export default function datetime(value: any, lng: string) {
  return getFormat(lng).format(value);
}
