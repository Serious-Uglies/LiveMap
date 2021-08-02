const localesEndpoint = '/api/locales';

export function getAvailableLocales() {
  return fetch(localesEndpoint)
    .then((res) => res.json())
    .catch(() => []);
}
