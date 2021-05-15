$.template.addFunction('formatFrequency', (frequency) => {
  const unit = frequency > 1000000 ? 'MHz' : 'kHz';
  const converted = unit === 'MHz' ? frequency / 1000000 : frequency / 1000;

  const rounded = (
    Math.round((converted + Number.EPSILON) * 1000) / 1000
  ).toFixed(3);

  return `${rounded} ${unit}`;
});
