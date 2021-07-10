import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import Nav from 'react-bootstrap/Nav';
import NavDropdown from 'react-bootstrap/NavDropdown';
import Spinner from 'react-bootstrap/Spinner';

import { getAvailableLocales } from '../../api/locales';

import './LocalePicker.css';

function getCurrentLocale(locales, currentLocale) {
  const getLocale = (id) => {
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

  return null;
}

function LocaleLabel({ locale: { flag, label } }) {
  return (
    <>
      {flag && <span className={`flag-icon flag-icon-${flag}`}></span>}
      <span class="locale-label">{label}</span>
    </>
  );
}

export default function LocalePicker() {
  const [locales, setLocales] = useState(null);
  const { i18n } = useTranslation();

  useEffect(
    () => getAvailableLocales().then((locales) => setLocales(locales)),
    []
  );

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

  const handleSelect = (locale) => i18n.changeLanguage(locale);

  return (
    <NavDropdown
      onSelect={handleSelect}
      title={<LocaleLabel locale={getCurrentLocale(locales, i18n.language)} />}
      alignRight
    >
      {locales.map((locale) => (
        <NavDropdown.Item key={locale.id} eventKey={locale.id}>
          <LocaleLabel locale={locale} />
        </NavDropdown.Item>
      ))}
    </NavDropdown>
  );
}
