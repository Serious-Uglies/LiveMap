export function SidebarCard({
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
    <div className="card property-card">
      <div className="card-header">
        {title}
        {dismissable && (
          <div class="float-end">
            <button class="btn-close" onClick={onDismiss}></button>
          </div>
        )}
      </div>
      <div className="card-body">
        {properties.map(
          ({ title, value, format }) =>
            value && (
              <div key={title}>
                <div className="property-title">{title}</div>
                {value}
              </div>
            )
        )}
      </div>
    </div>
  );
}
