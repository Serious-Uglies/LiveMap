import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import Nav from 'react-bootstrap/Nav';
import NavDropdown from 'react-bootstrap/NavDropdown';
import Spinner from 'react-bootstrap/Spinner';

import { getAvailableLocales } from '../../api/locales';

function getCurrentLocaleLabel(locales, currentLocale) {
  const getLabel = (id) => {
    const locale = locales.find((l) => l.id === id);
    return locale && locale.label;
  };

  const label = getLabel(currentLocale);

  if (label) {
    return label;
  }

  const split = currentLocale.split('-');

  if (split.length > 0) {
    return getLabel(split[0]);
  }

  return null;
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
      title={getCurrentLocaleLabel(locales, i18n.language)}
      alignRight
    >
      {locales.map(({ id, label }) => (
        <NavDropdown.Item key={id} eventKey={id}>
          {label}
        </NavDropdown.Item>
      ))}
    </NavDropdown>
  );
}
