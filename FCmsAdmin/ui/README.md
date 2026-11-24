# FCms admin UI

React 19 + TypeScript single-page app built with Vite. It talks to the FCmsAdmin API (`cms/api/v1/...`).

## Development

```bash
npm install
npm start        # dev server on http://localhost:4200, API expected at http://localhost:56710/
npm test         # unit tests (Vitest)
npm run typecheck
```

Run the API alongside it with `dotnet run --project ../FCmsManagerAngular.csproj` (Development profile, port 56710).
The default login is `test` / `password`.

The API base URL comes from `VITE_API_ENDPOINT` in `.env.development` / `.env.production`.

## Build

`npm run build` type-checks and writes the production bundle to `../wwwroot/fcms`. That folder is packed into
the FCmsAdmin NuGet package as static web assets and served by the host app at `/_content/FCmsAdmin/fcms/`
(the Vite `base` in production). Release builds of the csproj run this automatically.

TinyMCE is self-hosted: `scripts/copy-tinymce.mjs` copies it from `node_modules` into `public/tinymce`
(git-ignored) before `start` and `build`.

## Layout

- `src/api` — fetch wrapper (JWT header, 401/403 → login), endpoint functions, token storage
- `src/components` — content editor, filter editors, slide panel, toasts, rich text/date inputs
- `src/pages` — route screens; `App.tsx` holds the layout and route table
