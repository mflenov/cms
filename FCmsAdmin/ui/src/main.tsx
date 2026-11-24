import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { RouterProvider } from 'react-router/dom';
import { createRouter } from './App';
import './styles.css';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={createRouter()} />
  </StrictMode>,
);
