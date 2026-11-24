import { useState, type FormEvent } from 'react';
import { Navigate, useNavigate, useSearchParams } from 'react-router';
import { isLoggedIn } from '../api/auth';
import { ApiError } from '../api/http';
import { authApi } from '../api/services';
import './login.css';

export function WelcomePage() {
  return (
    <>
      <h1>Welcome to FCMS Manager</h1>
      <p>
        FCMS Manager is a Smart CMS content manager. CMS manager can be smart. It can return data based on the various
        filters you can configure in the manager.
      </p>
    </>
  );
}

export function NotFoundPage() {
  return (
    <>
      <h1>Page not found</h1>
      <p>The page you are looking for does not exist.</p>
    </>
  );
}

/** Only allows in-app paths as a post-login redirect target. */
function safeReturnUrl(value: string | null): string {
  return value && value.startsWith('/') && !value.startsWith('//') ? value : '/welcome';
}

export function LoginPage() {
  const navigate = useNavigate();
  const [params] = useSearchParams();
  const returnUrl = safeReturnUrl(params.get('returnUrl'));
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  if (isLoggedIn() && !loading) return <Navigate to={returnUrl} replace />;

  const onSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    if (!username || !password) {
      setError('Please enter both username and password.');
      return;
    }
    setLoading(true);
    try {
      await authApi.login(username, password);
      navigate(returnUrl, { replace: true });
    } catch (err) {
      setLoading(false);
      setError(
        err instanceof ApiError && err.status === 401
          ? 'Invalid username or password.'
          : (err as Error).message || 'Login failed. Please try again.',
      );
    }
  };

  return (
    <div className="login-wrapper">
      <div className="login-card">
        <div className="login-header">
          <h2>Sign In</h2>
          <p>Default username/password is test/password</p>
        </div>
        {error && <div className="error">{error}</div>}
        <form onSubmit={onSubmit}>
          <div className="form-group">
            <label htmlFor="username">Username</label>
            <input
              type="text"
              id="username"
              className="form-control"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Enter your username"
              required
              autoFocus
            />
          </div>
          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              className="form-control"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter your password"
              required
            />
          </div>
          <button type="submit" className="cms-button login-button" disabled={loading}>
            {loading ? 'Signing in...' : 'Sign In'}
          </button>
        </form>
      </div>
    </div>
  );
}
