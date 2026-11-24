import { useEffect, type ReactNode } from 'react';

interface Props {
  isVisible: boolean;
  closeCaption?: string;
  onSave: () => void;
  onCancel: () => void;
  children: ReactNode;
}

export function SlidePanel({ isVisible, closeCaption = 'Save', onSave, onCancel, children }: Props) {
  // Escape cancels, like clicking the backdrop.
  useEffect(() => {
    if (!isVisible) return;
    const onKey = (e: KeyboardEvent) => e.key === 'Escape' && onCancel();
    document.addEventListener('keydown', onKey);
    return () => document.removeEventListener('keydown', onKey);
  }, [isVisible, onCancel]);

  if (!isVisible) return null;
  return (
    <div className="fcms-modal-screen" onClick={(e) => e.target === e.currentTarget && onCancel()}>
      <div className="fcms-slideout" role="dialog" aria-modal="true">
        <div className="fcms-fullscreen">{children}</div>
        <div className="fcms-slideout-footer">
          <button type="button" className="cms-button cms-button-secondary" onClick={onCancel}>
            Cancel
          </button>
          <button type="button" className="cms-button" onClick={onSave}>
            {closeCaption}
          </button>
        </div>
      </div>
    </div>
  );
}
