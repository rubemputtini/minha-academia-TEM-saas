import { Navigate, Route, Routes } from "react-router-dom";
import { ROUTES } from "./shared/routes/routes";
import { lazy, Suspense } from "react";
import LoadingCard from "./shared/ui/LoadingCard";
import ScrollToTop from "./shared/components/ScrollToTop";

const LandingPage = lazy(() => import("@/marketing/pages/LandingPage"));
const CoachSignupPage = lazy(() => import("@/features/auth/pages/CoachSignupPage"));
// const CoachAfterPaymentPage = lazy(() => import("@/features/auth/pages/CoachAfterPaymentPage"));

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
          {/* <Route
            path={ROUTES.coachAfterPayment}
            element={<CoachAfterPaymentPage />}
          /> */}

          <Route
            path={ROUTES.fallback}
            element={<Navigate to={ROUTES.home} replace />}
          />
        </Routes>
      </Suspense>
    </div>
  );
}