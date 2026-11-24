/// <reference types="vitest/config" />
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// In production the admin UI is packed into the FCmsAdmin NuGet package as static web
// assets and served by the host app at /_content/FCmsAdmin/fcms/.
export default defineConfig(({ mode }) => ({
  base: mode === 'production' ? '/_content/FCmsAdmin/fcms/' : '/',
  plugins: [react()],
  server: { port: 4200 },
  build: {
    outDir: '../wwwroot/fcms',
    emptyOutDir: true,
    chunkSizeWarningLimit: 2048,
  },
  test: {
    environment: 'jsdom',
    css: false,
  },
}));
