import AppLayout from "@/shared/layout/AppLayout";
import { useAdminDashboard } from "../hooks/useAdminDashboard";
import { AdminCoachListCard } from "../components/AdminCoachListCard";
import { AdminStatsOverviewCard } from "../components/AdminStatsOverviewCard";
import { AdminQuickLinksCard } from "../components/AdminQuickLinksCard";
import { AdminRevenueCard } from "../components/AdminRevenueCard";

export default function AdminDashboardPage() {
  const { loading, stats, coaches, totalCoaches } = useAdminDashboard();

  return (
    <AppLayout title="Dashboard">
      <div className="space-y-5 p-3 pb-10">
        <div className="grid grid-cols-1 gap-6 lg:grid-cols-12">
          <div className="lg:col-span-8 lg:h-full">
            <AdminCoachListCard
              loading={loading}
              coaches={coaches}
              totalCoaches={totalCoaches}
              stats={stats}
            />
          </div>

          <div className="flex flex-col gap-6 lg:col-span-4 lg:h-full">
            <AdminStatsOverviewCard loading={loading} stats={stats} />
            <AdminRevenueCard loading={loading} stats={stats} />
            <AdminQuickLinksCard />
          </div>
        </div>
      </div>
    </AppLayout>
  );
}
