const apiBaseUrl = process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:5001";

type RequestInit = {
  method?: string;
  headers?: Record<string, string>;
  body?: string;
  signal?: AbortSignal;
};

export function api(url: string, init?: RequestInit): Promise<Response> {
  return fetch(`${apiBaseUrl}${url}`, init);
}

export function post(url: string, body?: string) {
  return api(url, { method: "POST", body: JSON.stringify(body) });
}

export function get(url: string, init?: Omit<RequestInit, "method">) {
  return api(url, { method: "GET", ...(init || {}) });
}
