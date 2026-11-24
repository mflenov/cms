import type { ReactNode } from 'react';

interface Props {
  isVisible: boolean;
  closeCaption?: string;
  onSave: () => void;
  onCancel: () => void;
  children: ReactNode;
}

export function SlidePanel({ isVisible, closeCaption = 'Save', onSave, onCancel, children }: Props) {
  if (!isVisible) return null;
  return (
    <div className="fcms-modal-screen">
      <div className="fcms-slideout">
        <div className="fcms-fullscreen">{children}</div>
        <div className="cms-controls cms-buttons cms-right">
          <button type="button" className="cms-button" onClick={onSave}>
            {closeCaption}
          </button>
          <button type="button" className="cms-button" onClick={onCancel}>
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
}
