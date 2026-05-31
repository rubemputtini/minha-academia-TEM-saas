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
const CoachSignupPage = lazy(() =>
  import("@/features/auth/pages/CoachSignupPage")
);
const CoachAfterPaymentPage = lazy(() =>
  import("@/features/auth/pages/CoachAfterPaymentPage")
);
const UserSignupPage = lazy(() =>
  import("@/features/auth/pages/UserSignupPage")
);
const LoginPage = lazy(() => import("@/features/auth/pages/LoginPage"));
const ForgotPasswordPage = lazy(() =>
  import("@/features/auth/pages/ForgotPasswordPage")
);
const ResetPasswordPage = lazy(() =>
  import("@/features/auth/pages/ResetPasswordPage")
);
const UserAccountPage = lazy(() =>
  import("@/features/account/pages/UserAccountPage")
);
const CoachAccountPage = lazy(() =>
  import("@/features/account/pages/CoachAccountPage")
);
const CoachSubscriptionPage = lazy(() =>
  import("@/features/subscription/pages/CoachSubscriptionPage")
);
const CoachReferralPage = lazy(() =>
  import("@/features/referral/pages/CoachReferralPage")
);
const CoachDashboardPage = lazy(() =>
  import("@/features/dashboard/pages/CoachDashboardPage")
);
const AdminDashboardPage = lazy(() =>
  import("@/features/admin-dashboard/pages/AdminDashboardPage")
);
const ClientsPage = lazy(() =>
  import("@/features/clients/pages/ClientsPage")
);
const ClientDetailPage = lazy(() =>
  import("@/features/clients/pages/ClientDetailPage")
);
const ClientEquipmentsPage = lazy(() =>
  import("@/features/clients/pages/ClientEquipmentsPage")
);
const UserEquipmentsPage = lazy(() =>
  import("@/features/equipments/pages/UserEquipmentsPage")
);

export default function App() {
  return (
    <div className="text-foreground min-h-screen">
      <ScrollToTop />
      <Toaster />
      <Suspense fallback={<LoadingCard />}>
        <Routes>
          {/* Públicas */}
          <Route path={ROUTES.home} element={<LandingPage />} />
          <Route path={ROUTES.coachSignup} element={<CoachSignupPage />} />
          <Route
            path={ROUTES.coachAfterPayment}
            element={<CoachAfterPaymentPage />}
          />
          <Route path={ROUTES.userSignup} element={<UserSignupPage />} />
          <Route path={ROUTES.login} element={<LoginPage />} />
          <Route
            path={ROUTES.forgotPassword}
            element={<ForgotPasswordPage />}
          />
          <Route path={ROUTES.resetPassword} element={<ResetPasswordPage />} />

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
            path={ROUTES.equipments}
            element={
              <ProtectedRoute allowedRoles={[ROLES.USER]}>
                <UserEquipmentsPage />
              </ProtectedRoute>
            }
          />
          <Route
            path={ROUTES.coachDashboard}
            element={
              <ProtectedRoute allowedRoles={[ROLES.COACH, ROLES.ADMIN]}>
                <CoachDashboardPage />
              </ProtectedRoute>
            }
          />
          <Route
            path={ROUTES.coachAccount}
            element={
              <ProtectedRoute allowedRoles={[ROLES.COACH, ROLES.ADMIN]}>
                <CoachAccountPage />
              </ProtectedRoute>
            }
          />
          <Route
            path={ROUTES.coachSubscription}
            element={
              <ProtectedRoute allowedRoles={[ROLES.COACH, ROLES.ADMIN]}>
                <CoachSubscriptionPage />
              </ProtectedRoute>
            }
          />
          <Route
            path={ROUTES.coachReferral}
            element={
              <ProtectedRoute allowedRoles={[ROLES.COACH, ROLES.ADMIN]}>
                <CoachReferralPage />
              </ProtectedRoute>
            }
          />
          <Route
            path={ROUTES.coachUsers}
            element={
              <ProtectedRoute allowedRoles={[ROLES.COACH, ROLES.ADMIN]}>
                <ClientsPage />
              </ProtectedRoute>
            }
          />
          <Route
            path={ROUTES.coachClientDetail}
            element={
              <ProtectedRoute allowedRoles={[ROLES.COACH, ROLES.ADMIN]}>
                <ClientDetailPage />
              </ProtectedRoute>
            }
          />
          <Route
            path={ROUTES.coachClientEquipments}
            element={
              <ProtectedRoute allowedRoles={[ROLES.COACH, ROLES.ADMIN]}>
                <ClientEquipmentsPage />
              </ProtectedRoute>
            }
          />

          <Route
            path={ROUTES.adminDashboard}
            element={
              <ProtectedRoute allowedRoles={[ROLES.ADMIN]}>
                <AdminDashboardPage />
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
