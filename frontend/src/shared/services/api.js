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
        const errorMessage = error.response?.data?.message || "Erro inesperado. Tente novamente.";
        const requestUrl = error.config?.url;

        // Só bloqueia o erro se não for a rota de login
        if (status === 401 && 
            !requestUrl.includes(ROUTES.login) &&
            !requestUrl.includes(ROUTES.signup)
        ) {    
            clearToken();
            window.location.href = ROUTES.login; 
            return new Promise(() => {});  
        }

        if (status === 403) {
            return Promise.reject({ status, message: errorMessage });
        }

        console.error(`Erro (${status}): ${errorMessage}`);

        return Promise.reject({ status: status || 500, message: errorMessage });
    }
);

export default api;