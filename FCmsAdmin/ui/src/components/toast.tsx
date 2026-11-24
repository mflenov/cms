import { useSyncExternalStore } from 'react';
import { ApiError } from '../api/http';

export interface Toast {
  id: number;
  title: string;
  message: string;
  classname: string;
  errorCode?: number;
}

let toasts: Toast[] = [];
let nextId = 1;
const listeners = new Set<() => void>();

function emit() {
  listeners.forEach((l) => l());
}

function remove(id: number) {
  toasts = toasts.filter((t) => t.id !== id);
  emit();
}

function show(message: string, options: Partial<Omit<Toast, 'id'>> & { delay?: number } = {}) {
  const { delay = 3000, ...rest } = options;
  const toast: Toast = { id: nextId++, title: 'Warning', classname: 'bg-info text-light', message, ...rest };
  toasts = [...toasts, toast];
  emit();
  if (delay > 0) setTimeout(() => remove(toast.id), delay);
}

export const toast = {
  show,
  success: (message: string) => show(message, { title: 'Success', classname: 'bg-success text-light' }),
  warning: (message: string) => show(message, { classname: 'bg-warning text-dark' }),
  error: (message: string, errorCode?: number) =>
    show(message, { title: 'Error', classname: 'bg-danger text-light', errorCode }),
  /** Shows any thrown value as an error toast. Auth failures are skipped: they already redirect to login. */
  fromError: (e: unknown) => {
    if (e instanceof ApiError) {
      if (e.status === 401 || e.status === 403) return;
      show(e.message, { title: 'Error', classname: 'bg-danger text-light', errorCode: e.status || undefined });
    } else {
      show(e instanceof Error ? e.message : String(e), { title: 'Error', classname: 'bg-danger text-light' });
    }
  },
};

function subscribe(listener: () => void) {
  listeners.add(listener);
  return () => listeners.delete(listener);
}

export function Toasts() {
  const items = useSyncExternalStore(subscribe, () => toasts);
  return (
    <div className="toast-container position-fixed top-0 end-0 p-3" style={{ zIndex: 1200 }}>
      {items.map((t) => (
        <div key={t.id} className={`toast show ${t.classname}`} role="alert" aria-live="assertive" aria-atomic="true">
          <div className="toast-header">
            <strong className="me-auto">{t.title}</strong>
            <button type="button" className="btn-close" aria-label="Close" onClick={() => remove(t.id)} />
          </div>
          <div className="toast-body">
            {t.errorCode ? (
              <p>
                Error code: <i>{t.errorCode}</i>
              </p>
            ) : null}
            <div>{t.message}</div>
          </div>
        </div>
      ))}
    </div>
  );
}
