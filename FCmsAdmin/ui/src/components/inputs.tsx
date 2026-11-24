import { Editor } from '@tinymce/tinymce-react';

/** Self-hosted TinyMCE, copied into public/tinymce by scripts/copy-tinymce.mjs. */
const TINYMCE_SRC = `${import.meta.env.BASE_URL}tinymce/tinymce.min.js`;

export function RichTextEditor({ value, onChange }: { value: string | null | undefined; onChange: (html: string) => void }) {
  return (
    <Editor
      tinymceScriptSrc={TINYMCE_SRC}
      licenseKey="gpl"
      value={value ?? ''}
      onEditorChange={onChange}
      init={{ menubar: false, promotion: false, branding: false, height: 300 }}
    />
  );
}

/** ISO timestamp -> yyyy-mm-dd (local date), as used by <input type="date">. */
export function isoToDateInput(iso: string | null | undefined): string {
  if (!iso) return '';
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return '';
  const pad = (n: number) => String(n).padStart(2, '0');
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`;
}

/** yyyy-mm-dd -> ISO timestamp of local midnight on that day; empty input -> ''. */
export function dateInputToIso(value: string): string {
  if (!value) return '';
  const [y, m, d] = value.split('-').map(Number);
  return new Date(y, m - 1, d).toISOString();
}

interface DateInputProps {
  value: string | null | undefined;
  onChange: (iso: string) => void;
  placeholder?: string;
}

/** Date picker bound to an ISO timestamp string. */
export function DateInput({ value, onChange, placeholder }: DateInputProps) {
  return (
    <input
      type="date"
      className="form-control"
      placeholder={placeholder}
      value={isoToDateInput(value)}
      onChange={(e) => onChange(dateInputToIso(e.target.value))}
    />
  );
}
