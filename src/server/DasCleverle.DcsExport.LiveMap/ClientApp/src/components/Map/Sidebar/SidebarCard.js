export function SidebarCard({ title, properties }) {
  return (
    <div className="card property-card">
      <div className="card-header">{title}</div>
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
