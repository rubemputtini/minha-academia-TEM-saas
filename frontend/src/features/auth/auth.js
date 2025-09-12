import { jwtDecode } from 'jwt-decode';

export const getToken = () => {
    return localStorage.getItem('token');
};

export const clearToken = () => {
    localStorage.removeItem('token');
};

export const setToken = (token) => {
    localStorage.setItem('token', token);
};

export const getUserRole = () => {
    const token = getToken();

    if (!token) return null;

    const decodedToken = jwtDecode(token);
    return decodedToken.role;
};

export const getUserId = () => {
    const token = getToken();

    if (!token) return null;

    const decodedToken = jwtDecode(token);
    return decodedToken.nameid;
};