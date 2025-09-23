import { Navigate } from "react-router-dom";
import { ROUTES } from "@/shared/routes/routes";
import LoadingCard from "@/shared/ui/LoadingCard";
import { useAuth } from "./hooks/useAuth";

export default function ProtectedRoute({ children, publicRoute = false, requireAdmin = false }) {
    const { isAuthenticated, role, loading } = useAuth();

    if (loading)
        return <LoadingCard fullScreen size={40} />

    if (publicRoute)
        return children;

    if (!isAuthenticated)
        return <Navigate to={ROUTES.login} replace />;

    if (requireAdmin && role !== "admin")
        return <Navigate to={ROUTES.dashboard} replace />;

    return children;
}
