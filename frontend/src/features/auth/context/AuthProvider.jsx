import { useEffect, useState } from "react";
import { AuthContext } from "./AuthContext";
import {
    getToken,
    setToken,
    clearToken,
    getUserRole,
    getUserId,
    isTokenExpired,
} from "../auth";

export const AuthProvider = ({ children }) => {
    const [loading, setLoading] = useState(true);
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [role, setRole] = useState(null);
    const [userId, setUserId] = useState(null);

    const refresh = () => {
        const token = getToken();

        if (token && !isTokenExpired()) {
            setIsAuthenticated(true);
            setRole(getUserRole());
            setUserId(getUserId());
        } else {
            if (token && isTokenExpired()) clearToken();

            setIsAuthenticated(false);
            setRole(null);
            setUserId(null);
        }
    };

    useEffect(() => {
        // inicial
        refresh();
        setLoading(false);

        // reage a mudanças do token:
        // - mesma aba (evento custom "auth-changed")
        // - outras abas (evento "storage")
        // - ao voltar o foco (expiração, etc.)
        const onChange = () => refresh();
        window.addEventListener("auth-changed", onChange);
        window.addEventListener("storage", onChange);
        window.addEventListener("focus", onChange);

        return () => {
            window.removeEventListener("auth-changed", onChange);
            window.removeEventListener("storage", onChange);
            window.removeEventListener("focus", onChange);
        };
    }, []);

    const login = (token) => {
        setToken(token); // dispara "auth-changed"
        refresh();
    };

    const logout = () => {
        clearToken(); // dispara "auth-changed"
        refresh();
    };

    return (
        <AuthContext.Provider
            value={{ isAuthenticated, role, userId, loading, login, logout }}
        >
            {children}
        </AuthContext.Provider>
    );
};