// Small shared building blocks for the admin screens.
import type { ReactNode } from 'react';
import { Link } from 'react-router';

/** A clickable Font Awesome icon rendered as a real button, so it is focusable and keyboard-operable. */
export function IconButton({
  icon,
  title,
  onClick,
  danger = false,
  className = '',
}: {
  icon: string;
  title: string;
  onClick: () => void;
  danger?: boolean;
  className?: string;
}) {
  return (
    <button
      type="button"
      className={`cms-icon-btn fa fa-${icon}${danger ? ' cms-icon-danger' : ''} ${className}`.trim()}
      title={title}
      aria-label={title}
      onClick={onClick}
    />
  );
}

/** Page title with an optional "back" link above it and actions on the right. */
export function PageHeader({
  title,
  back,
  actions,
}: {
  title: ReactNode;
  back?: { to: string; label: string };
  actions?: ReactNode;
}) {
  return (
    <div className="cms-page-header">
      <div>
        {back && (
          <Link to={back.to} className="cms-back-link">
            <span className="fa fa-angle-left" /> {back.label}
          </Link>
        )}
        <h1>{title}</h1>
      </div>
      {actions && <div className="cms-buttons">{actions}</div>}
    </div>
  );
}

export function Loading() {
  return (
    <div className="cms-loading" role="status">
      <span className="fa fa-circle-o-notch fa-spin" /> Loading…
    </div>
  );
}

export function EmptyState({ children }: { children: ReactNode }) {
  return <div className="cms-empty">{children}</div>;
}

/** Asks before a destructive action. */
export function confirmDelete(what: string): boolean {
  return window.confirm(`Delete ${what}? This cannot be undone.`);
}
