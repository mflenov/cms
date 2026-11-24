import { clearToken, getToken, isLoggedIn } from './auth';

export const API_ENDPOINT: string = import.meta.env.VITE_API_ENDPOINT ?? '/';
export const API_BASE = API_ENDPOINT + 'cms/api/';

export class ApiError extends Error {
  readonly status: number;

  constructor(status: number, message: string) {
    super(message);
    this.status = status;
  }
}

type UnauthorizedHandler = () => void;
let onUnauthorized: UnauthorizedHandler = () => {};

/** Registered by the app shell so a 401/403 from any request sends the user to the login page. */
export function setUnauthorizedHandler(handler: UnauthorizedHandler): void {
  onUnauthorized = handler;
}

export async function request<T>(method: string, path: string, body?: unknown): Promise<T> {
  const headers: Record<string, string> = { Accept: 'application/json' };
  if (body !== undefined) headers['Content-Type'] = 'application/json';
  const token = getToken();
  if (token && isLoggedIn()) headers.Authorization = `Bearer ${token}`;

  let response: Response;
  try {
    response = await fetch(API_BASE + path, {
      method,
      headers,
      body: body === undefined ? undefined : JSON.stringify(body),
    });
  } catch (e) {
    throw new ApiError(0, `Network error: ${(e as Error).message}`);
  }

  const text = await response.text();
  let payload: any = undefined;
  if (text) {
    try {
      payload = JSON.parse(text);
    } catch {
      payload = text;
    }
  }

  if (!response.ok) {
    const isLoginCall = path.startsWith('v1/auth/');
    if ((response.status === 401 || response.status === 403) && !isLoginCall) {
      clearToken();
      onUnauthorized();
    }
    const description = payload && typeof payload === 'object' ? payload.description : undefined;
    throw new ApiError(
      response.status,
      description || `${method} ${API_BASE + path}: ${response.status} ${response.statusText}`,
    );
  }
  return payload as T;
}

export const http = {
  get: <T>(path: string) => request<T>('GET', path),
  post: <T>(path: string, body: unknown) => request<T>('POST', path, body),
  put: <T>(path: string, body: unknown) => request<T>('PUT', path, body),
  patch: <T>(path: string, body: unknown) => request<T>('PATCH', path, body),
  delete: <T>(path: string) => request<T>('DELETE', path),
};
