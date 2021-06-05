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
          <div class="float-end">
            <button class="btn-close" onClick={onDismiss}></button>
          </div>
        )}
      </Card.Header>
      <Card.Body>
        {properties.map(
          ({ title, value, format }) =>
            value && (
              <div key={title}>
                <div className="property-title">{title}</div>
                {value}
              </div>
            )
        )}
      </Card.Body>
    </Card>
  );
}
