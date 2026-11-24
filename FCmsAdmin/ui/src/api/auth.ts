// JWT storage. Keys match the previous Angular admin so existing sessions survive the upgrade.
const TOKEN_KEY = 'cms_jwt_token';

function storage(): Storage | null {
  try {
    return window.localStorage;
  } catch {
    return null;
  }
}

export function getToken(): string | null {
  return storage()?.getItem(TOKEN_KEY) ?? null;
}

export function setToken(token: string): void {
  storage()?.setItem(TOKEN_KEY, token);
}

export function clearToken(): void {
  storage()?.removeItem(TOKEN_KEY);
}

/** True when a token is stored and its `exp` claim is still in the future. */
export function isLoggedIn(): boolean {
  const token = getToken();
  if (!token) return false;
  try {
    const payload = JSON.parse(atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')));
    return typeof payload.exp === 'number' && Date.now() < payload.exp * 1000;
  } catch {
    return false;
  }
}
