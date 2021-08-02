import React from 'react';

import Card from 'react-bootstrap/Card';
import { BsX } from 'react-icons/bs';

import './SidebarCard.css';

export default function SidebarCard({
  title,
  properties,
  visible = true,
  dismissable,
  onDismiss,
}) {
  if (!visible) {
    return null;
  }

  return (
    <Card className="property-card">
      <Card.Header className="d-flex align-items-center pr-2">
        <div className="mr-auto">{title}</div>
        {dismissable && (
          <div>
            <button className="btn-dismiss" onClick={onDismiss}>
              <BsX />
            </button>
          </div>
        )}
      </Card.Header>
      <Card.Body>
        {properties.map(
          ({ title, value }) =>
            value && (
              <React.Fragment key={title}>
                <div className="property-title">{title}</div>
                {value}
              </React.Fragment>
            )
        )}
      </Card.Body>
    </Card>
  );
}
