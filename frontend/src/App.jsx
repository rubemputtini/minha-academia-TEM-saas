import { Navigate, Route, Routes } from "react-router-dom";
import { ROUTES } from "./shared/routes/routes";
import { lazy, Suspense } from "react";
import LoadingCard from "./shared/ui/LoadingCard";
import ScrollToTop from "./shared/components/ScrollToTop";
import EquipmentSwipePage from "./features/equipments/pages/EquipmentSwipePage";
import ProtectedRoute from "./features/auth/ProtectedRoute";

const LandingPage = lazy(() => import("@/marketing/pages/LandingPage"));
const CoachSignupPage = lazy(() => import("@/features/auth/pages/CoachSignupPage"));
const CoachAfterPaymentPage = lazy(() => import("@/features/auth/pages/CoachAfterPaymentPage"));
const UserSignupPage = lazy(() => import("@/features/auth/pages/UserSignupPage"));
const LoginPage = lazy(() => import("@/features/auth/pages/LoginPage"));
const ForgotPasswordPage = lazy(() => import("@/features/auth/pages/ForgotPasswordPage"));
const ResetPasswordPage = lazy(() => import("@/features/auth/pages/ResetPasswordPage"));

export default function App() {
  return (
    <div className="text-foreground min-h-screen">
      <ScrollToTop />
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
              <ProtectedRoute>
                <EquipmentSwipePage />
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