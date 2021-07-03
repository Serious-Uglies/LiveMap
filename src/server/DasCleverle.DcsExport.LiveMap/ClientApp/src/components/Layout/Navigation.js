import React from 'react';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';
import Navbar from 'react-bootstrap/Navbar';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';

import './Navigation.css';

export default function Navigation() {
  const { t } = useTranslation();

  return (
    <header>
      <Navbar bg="light" expand="lg">
        <Container fluid>
          <Navbar.Brand as={Link} to="/">
            {t('navbar.brand')}
          </Navbar.Brand>
          <Navbar.Toggle />
          <Navbar.Collapse>
            <Nav>
              <Nav.Link as={Link} to="/">
                {t('navbar.home')}
              </Nav.Link>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </header>
  );
}
