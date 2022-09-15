import React from 'react';

import Card from 'react-bootstrap/Card';
import { BsX } from 'react-icons/bs';

import './SidebarCard.css';

interface SidebarCardProperty {
  title: string;
  value: React.ReactNode | null;
}

interface SidebarCardProps {
  title: string;
  properties?: SidebarCardProperty[];
  visible?: boolean;
  dismissable?: boolean;
  onDismiss?: () => void;
}

export default function SidebarCard({
  title,
  properties,
  visible = true,
  dismissable = false,
  onDismiss,
}: SidebarCardProps) {
  if (!visible || !properties) {
    return null;
  }

  return (
    <Card className="property-card">
      <Card.Header className="d-flex align-items-center">
        <div className="flex-grow-1">{title}</div>
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
