export default function frequency(value: any) {
  const unit = value > 1000000 ? 'MHz' : 'kHz';
  const converted = unit === 'MHz' ? value / 1000000 : value / 1000;

  const rounded = (
    Math.round((converted + Number.EPSILON) * 1000) / 1000
  ).toFixed(3);

  return `${rounded} ${unit}`;
}
