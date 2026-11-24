import { useEffect } from 'react';
import { createBrowserRouter, Link, Navigate, NavLink, Outlet, useLocation, useNavigate } from 'react-router';
import { clearToken, isLoggedIn } from './api/auth';
import { setUnauthorizedHandler } from './api/http';
import { Toasts } from './components/toast';
import {
  ConfigHomePage,
  DbConnectionEditPage,
  DbConnectionListPage,
  FilterEditPage,
  FilterListPage,
  UserEditPage,
  UserListPage,
} from './pages/config';
import { AddDbContentPage, DbContentPage } from './pages/db';
import { LoginPage, NotFoundPage, WelcomePage } from './pages/misc';
import { EditContentPage, ListContentPage, PreviewPage, StructurePage } from './pages/repository';
import {
  ContentListPage,
  DbRepositoryListPage,
  NewContentPage,
  NewDbRepositoryPage,
  NewPagePage,
  PageListPage,
} from './pages/sections';

const logo = `${import.meta.env.BASE_URL}assets/fcms-logo.png`;

const NAV: [label: string, to: string, extraClass?: string][] = [
  ['Pages', '/pages'],
  ['Content', '/content'],
  ['Database', '/db'],
  ['Config', '/config', 'config-link'],
];

function loginUrl(location: { pathname: string; search: string }) {
  return `/login?returnUrl=${encodeURIComponent(location.pathname + location.search)}`;
}

function Layout() {
  const navigate = useNavigate();
  const location = useLocation();

  // Any 401/403 from the API (e.g. an expired token) sends the user to the login page.
  useEffect(() => {
    setUnauthorizedHandler(() => {
      if (window.location.pathname.endsWith('/login')) return;
      navigate(loginUrl(location), { replace: true });
    });
  }, [navigate, location]);

  const signedIn = isLoggedIn();
  const logout = () => {
    clearToken();
    navigate('/login', { replace: true });
  };

  return (
    <>
      <header className="header">
        <div className="container">
          <div className="cms-logo">
            <Link to="/">
              <img src={logo} alt="FCms" />
            </Link>
          </div>
          {signedIn && (
            <nav className="cms-menu">
              <ul className="nav nav-pills">
                {NAV.map(([label, to, extra]) => (
                  <li key={to}>
                    <NavLink to={to} className={`nav-link${extra ? ` ${extra}` : ''}`}>
                      {label}
                    </NavLink>
                  </li>
                ))}
              </ul>
            </nav>
          )}
          {signedIn && (
            <div className="cms-user">
              <button type="button" className="cms-button cms-button-secondary cms-button-sm" onClick={logout}>
                <span className="fa fa-sign-out" /> Sign out
              </button>
            </div>
          )}
        </div>
      </header>
      <div className="page">
        <div className="container">
          <Outlet />
        </div>
      </div>
      <footer className="footer">
        <div className="container">
          <p>
            fCms is an Open Source .NET Smart CMS · Created by Mikhail Flenov from{' '}
            <a href="http://www.profwebdev.com">ProfWebDev</a>
          </p>
        </div>
      </footer>
      <Toasts />
    </>
  );
}

/** Every API call needs a JWT, so screens other than login require one up front. */
function RequireAuth() {
  const location = useLocation();
  return isLoggedIn() ? <Outlet /> : <Navigate to={loginUrl(location)} replace />;
}

export const routes = [
  {
    element: <Layout />,
    children: [
      { path: '/login', element: <LoginPage /> },
      {
        element: <RequireAuth />,
        children: [
          { path: '/', element: <Navigate to="/welcome" replace /> },
          { path: '/welcome', element: <WelcomePage /> },

          { path: '/pages', element: <PageListPage /> },
          { path: '/pages/add', element: <NewPagePage /> },
          { path: '/pages/structure/:id', element: <StructurePage section="pages" /> },
          { path: '/pages/edit/:id', element: <EditContentPage section="pages" /> },
          { path: '/pages/preview/:id', element: <PreviewPage /> },
          { path: '/pages/list/:repo/:id', element: <ListContentPage section="pages" /> },

          { path: '/content', element: <ContentListPage /> },
          { path: '/content/add', element: <NewContentPage /> },
          { path: '/content/structure/:id', element: <StructurePage section="content" /> },
          { path: '/content/edit/:id', element: <EditContentPage section="content" /> },
          { path: '/content/preview/:id', element: <PreviewPage /> },
          { path: '/content/list/:repo/:id', element: <ListContentPage section="content" /> },

          { path: '/db', element: <DbRepositoryListPage /> },
          { path: '/db/add', element: <NewDbRepositoryPage /> },
          { path: '/db/content/:id', element: <DbContentPage /> },
          { path: '/db/content/add/:id', element: <AddDbContentPage /> },
          { path: '/db/structure/:id', element: <StructurePage section="db" /> },

          { path: '/config', element: <ConfigHomePage /> },
          { path: '/config/filters', element: <FilterListPage /> },
          { path: '/config/filter', element: <FilterEditPage /> },
          { path: '/config/filter/:id', element: <FilterEditPage /> },
          { path: '/config/dbconnections', element: <DbConnectionListPage /> },
          { path: '/config/dbconnection', element: <DbConnectionEditPage /> },
          { path: '/config/dbconnection/:id', element: <DbConnectionEditPage /> },
          { path: '/config/users', element: <UserListPage /> },
          { path: '/config/user', element: <UserEditPage /> },
          { path: '/config/user/:id', element: <UserEditPage /> },

          { path: '*', element: <NotFoundPage /> },
        ],
      },
    ],
  },
];

export function createRouter() {
  return createBrowserRouter(routes, { basename: import.meta.env.BASE_URL.replace(/\/$/, '') || '/' });
}
