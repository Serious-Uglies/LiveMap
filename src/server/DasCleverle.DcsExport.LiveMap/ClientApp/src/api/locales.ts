const localesEndpoint = '/api/locales';

export interface Locale {
  id: string;
  flag: string;
  label: string;
}

export function getAvailableLocales(): Promise<Locale[]> {
  return fetch(localesEndpoint)
    .then((res) => res.json())
    .catch(() => []);
}
