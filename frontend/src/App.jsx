import { Navigate, Route, Routes } from "react-router-dom";
import { ROUTES } from "./shared/routes/routes";
import { lazy, Suspense } from "react";
import LoadingCard from "./shared/ui/LoadingCard";
import ScrollToTop from "./shared/components/ScrollToTop";
import EquipmentSwipePage from "./features/equipments/pages/EquipmentSwipePage";
import ProtectedRoute from "./features/auth/ProtectedRoute";
import { ROLES } from "./features/auth/constants/roles";
import { Toaster } from "./components/ui/sonner";

const LandingPage = lazy(() => import("@/marketing/pages/LandingPage"));
const CoachSignupPage = lazy(() => import("@/features/auth/pages/CoachSignupPage"));
const CoachAfterPaymentPage = lazy(() => import("@/features/auth/pages/CoachAfterPaymentPage"));
const UserSignupPage = lazy(() => import("@/features/auth/pages/UserSignupPage"));
const LoginPage = lazy(() => import("@/features/auth/pages/LoginPage"));
const ForgotPasswordPage = lazy(() => import("@/features/auth/pages/ForgotPasswordPage"));
const ResetPasswordPage = lazy(() => import("@/features/auth/pages/ResetPasswordPage"));
const UserAccountPage = lazy(() => import("@/features/account/pages/UserAccountPage"));

export default function App() {
  return (
    <div className="text-foreground min-h-screen">
      <ScrollToTop />
      <Toaster />
      <Suspense fallback={<LoadingCard />}>
        <Routes>

          {/* Públicas */}
          <Route
            path={ROUTES.home}
            element={<LandingPage />}
          />
          <Route
            path={ROUTES.coachSignup}
            element={<CoachSignupPage />}
          />
          <Route
            path={ROUTES.coachAfterPayment}
            element={<CoachAfterPaymentPage />}
          />
          <Route
            path={ROUTES.userSignup}
            element={<UserSignupPage />}
          />
          <Route
            path={ROUTES.login}
            element={<LoginPage />}
          />
          <Route
            path={ROUTES.forgotPassword}
            element={<ForgotPasswordPage />}
          />
          <Route
            path={ROUTES.resetPassword}
            element={<ResetPasswordPage />}
          />

          {/* Privadas */}
          <Route
            path={ROUTES.app}
            element={
              <ProtectedRoute allowedRoles={[ROLES.USER]}>
                <EquipmentSwipePage />
              </ProtectedRoute>
            }
          />
          <Route
            path={ROUTES.account}
            element={
              <ProtectedRoute allowedRoles={[ROLES.USER]}>
                <UserAccountPage />
              </ProtectedRoute>
            }
          />

          <Route
            path={ROUTES.fallback}
            element={<Navigate to={ROUTES.home} replace />}
          />
        </Routes>
      </Suspense>
    </div>
  );
}