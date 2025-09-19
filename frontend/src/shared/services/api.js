import axios from "axios";
import { ROUTES } from "../routes/routes";
import { clearToken, getToken } from "@/features/auth/auth";

const API_BASE = import.meta.env.VITE_API_BASE_URL;

const api = axios.create({
    baseURL: API_BASE,
    withCredentials: true,
});

api.interceptors.request.use(
    (config) => {
        const token = getToken();

        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
    },
    (error) => Promise.reject(error)
);

api.interceptors.response.use(
    (response) => response,
    (error) => {
        const status = error.response?.status;
        const url = error.config?.url || "";
        const isAuthEndpoint =
        url.includes("/api/v1/auth/login") ||
        url.includes("/api/v1/auth/register/coach") ||
        url.includes("/api/v1/auth/register/coach/after-payment") ||
        url.includes("/api/v1/auth/register/user");

        if (status === 401 && !isAuthEndpoint) {
            clearToken();
            window.location.replace(ROUTES.login);

            return new Promise(() => {}); // aborta chain
        }
        
        return Promise.reject(error);
    }
);

export default api;