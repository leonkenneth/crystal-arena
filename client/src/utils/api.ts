const apiBaseUrl =
  process.env.REACT_APP_API_BASE_URL || "http://localhost:5000";

type RequestInit = {
  method?: string;
  headers?: Record<string, string>;
  body?: string;
}

export function api(url: string, init?: RequestInit): Promise<Response> {
  return fetch(`${apiBaseUrl}${url}`, init);
}

export function post(url: string, body?: string) {
  return api(url, { method: "POST", body: JSON.stringify(body) });
}