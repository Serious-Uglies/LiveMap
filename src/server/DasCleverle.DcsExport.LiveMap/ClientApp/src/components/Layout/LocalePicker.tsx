import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { getAvailableLocales, Locale } from '../../api/locales';

import Nav from 'react-bootstrap/Nav';
import NavDropdown from 'react-bootstrap/NavDropdown';
import Spinner from 'react-bootstrap/Spinner';

import './LocalePicker.css';

function getCurrentLocale(
  locales: Locale[],
  currentLocale: string
): Locale | undefined {
  const getLocale = (id: string) => {
    return locales.find((l) => l.id === id);
  };

  const locale = getLocale(currentLocale);

  if (locale) {
    return locale;
  }

  const split = currentLocale.split('-');

  if (split.length > 0) {
    return getLocale(split[0]);
  }

  return undefined;
}

type LocaleLabelProps = {
  locale?: Locale;
};

function LocaleLabel({ locale }: LocaleLabelProps) {
  if (!locale) {
    return null;
  }

  return (
    <>
      {locale.flag && <span className={`fi fi-${locale.flag}`}></span>}
      <span className="locale-label">{locale.label}</span>
    </>
  );
}

export default function LocalePicker() {
  const [locales, setLocales] = useState<Locale[]>([]);
  const { i18n } = useTranslation();

  useEffect(() => {
    getAvailableLocales().then((locales) => setLocales(locales));
  }, []);

  if (locales === null) {
    return (
      <Nav.Item>
        <Spinner animation="border" />
      </Nav.Item>
    );
  }

  if (!locales.length) {
    return null;
  }

  const handleSelect = (locale: string | null) => {
    i18n.changeLanguage(locale ?? undefined);
  };

  return (
    <NavDropdown
      onSelect={handleSelect}
      title={<LocaleLabel locale={getCurrentLocale(locales, i18n.language)} />}
      align="end"
    >
      {locales.map((locale) => (
        <NavDropdown.Item key={locale.id} eventKey={locale.id}>
          <LocaleLabel locale={locale} />
        </NavDropdown.Item>
      ))}
    </NavDropdown>
  );
}
