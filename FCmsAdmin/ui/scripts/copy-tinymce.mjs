// Copies the self-hosted TinyMCE distribution into public/tinymce so Vite
// serves it in dev and copies it into the build output (../wwwroot/fcms/tinymce).
import { cpSync, existsSync, rmSync } from 'node:fs';
import { basename } from 'node:path';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const root = resolve(dirname(fileURLToPath(import.meta.url)), '..');
const src = resolve(root, 'node_modules/tinymce');
const dest = resolve(root, 'public/tinymce');

if (!existsSync(src)) {
  console.error('tinymce is not installed; run npm install first');
  process.exit(1);
}
rmSync(dest, { recursive: true, force: true });
const SKIP = new Set(['package.json', 'composer.json', 'bower.json', 'README.md', 'CHANGELOG.md', 'license.md']);

// tinymce.min.js loads only minified plugins/themes/skins, so drop the unminified duplicates.
function wanted(path) {
  if (SKIP.has(basename(path)) || path.endsWith('.d.ts')) return false;
  const unminified = path.match(/^(.*)\.(js|css)$/);
  if (unminified && !path.endsWith('.min.' + unminified[2]) && existsSync(`${unminified[1]}.min.${unminified[2]}`)) {
    return false;
  }
  return true;
}

cpSync(src, dest, { recursive: true, filter: wanted });
