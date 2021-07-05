import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

import HttpBacked from 'i18next-http-backend';
import LanguageDetector from 'i18next-browser-languagedetector';

import format from './formatters';

i18n
  .use(LanguageDetector)
  .use(HttpBacked)
  .use(initReactI18next)
  .init({
    backend: {
      loadPath: '/api/locales/{{lng}}/{{ns}}',
    },

    fallbackLng: 'en',
    debug: true,

    interpolation: {
      // Not required as React escapes by itself
      escapeValue: false,

      format: format,
    },

    react: {
      useSuspense: false,
    },
  });

export default i18n;
