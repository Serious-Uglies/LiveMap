import './Backdrop.css';

type BackdropProps = {
  children: React.ReactNode;
  show: boolean;
};

export default function Backdrop({ children, show }: BackdropProps) {
  if (!show) {
    return null;
  }

  return (
    <div className="backdrop-wrapper">
      <div className="backdrop"></div>
      <div className="backdrop-content">{children}</div>
    </div>
  );
}
