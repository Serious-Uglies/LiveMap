import React from 'react';
import Card from 'react-bootstrap/Card';

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
      <Card.Header>
        {title}
        {dismissable && (
          <div className="float-end">
            <button className="btn-close" onClick={onDismiss}></button>
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
