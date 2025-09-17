import { jwtDecode } from "jwt-decode";

const TOKEN_KEY = "token";

export const getToken = () => localStorage.getItem(TOKEN_KEY) || null;

export const setToken = (token) => {
  localStorage.setItem(TOKEN_KEY, token);

  // avisa o app (mesma aba)
  window.dispatchEvent(new Event("auth-changed"));
};

export const clearToken = () => {
  localStorage.removeItem(TOKEN_KEY);
  
  // avisa o app (mesma aba)
  window.dispatchEvent(new Event("auth-changed"));
};

const safeDecode = (token) => {
  try {
    return jwtDecode(token);
  } catch {
    return null;
  }
};

export const getTokenPayload = () => {
  const t = getToken();

  return t ? safeDecode(t) : null;
};

export const isTokenExpired = () => {
  const payload = getTokenPayload();
  if (!payload?.exp) return false; // se não tiver exp, não forçamos logout

  return payload.exp * 1000 <= Date.now();
};

export const getUserRole = () => {
  const p = getTokenPayload();
  if (!p) return null;
  
  return p.role;
};

export const getUserId = () => {
  const p = getTokenPayload();
  if (!p) return null;
  
  return p.nameid;
};