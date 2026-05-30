import { useState } from "react";
import AppLayout from "@/shared/layout/AppLayout";
import { useCoachRevenue } from "../hooks/useCoachRevenue";
import { useTrainingSchedule } from "../hooks/useTrainingSchedule";
import { getStatus } from "@/shared/utils/trainingSchedule.utils";
import { TrainingScheduleCard } from "../components/TrainingScheduleCard";
import { PlanCard } from "../components/PlanCard";
import { RevenueCard } from "../components/RevenueCard";
import { InviteCard } from "../components/InviteCard";
import { RateModal } from "../components/RateModal";
import { TrainingDateModal } from "@/shared/components/TrainingDateModal";

export default function CoachDashboardPage() {
  const { loading: revenueLoading, saving: revenueSaving, revenue, saveRevenue } = useCoachRevenue();
  const { loading: scheduleLoading, saving: dateSaving, schedule, totalClients, saveDate } = useTrainingSchedule();
  const [rateModalOpen, setRateModalOpen] = useState(false);
  const [editingItem, setEditingItem] = useState(null);

  const urgentSchedule = schedule.filter((t) => {
    const s = getStatus(t.nextTrainingChangeAt);
    return s === "late" || s === "urgent";
  });
  const overdueCount = urgentSchedule.filter((t) => getStatus(t.nextTrainingChangeAt) === "late").length;
  const upcomingCount = urgentSchedule.filter((t) => getStatus(t.nextTrainingChangeAt) === "urgent").length;
  const atLimit = revenue?.usersLimit != null && totalClients >= revenue.usersLimit;

  return (
    <AppLayout title="Dashboard">
      <div className="space-y-5 p-3 pb-10">
        <div className="grid grid-cols-1 items-start gap-6 lg:grid-cols-12">
          <div className="lg:col-span-8">
            <TrainingScheduleCard
              loading={scheduleLoading}
              schedule={urgentSchedule}
              totalActiveClients={schedule.length}
              overdueCount={overdueCount}
              upcomingCount={upcomingCount}
              onEditItem={setEditingItem}
              atLimit={atLimit}
              coachCode={revenue?.coachCode}
            />
          </div>

          <div className="flex flex-col gap-6 lg:col-span-4">
            <PlanCard
              planName={revenue?.subscriptionPlan}
              usersLimit={revenue?.usersLimit}
              currentUsers={totalClients}
              loading={revenueLoading || scheduleLoading}
            />
            <RevenueCard
              currentUsers={totalClients}
              revenue={revenue}
              loading={revenueLoading}
              onEdit={() => setRateModalOpen(true)}
            />
            <InviteCard
                coachCode={revenue?.coachCode}
                loading={revenueLoading}
              />
          </div>
        </div>
      </div>

      {rateModalOpen && (
        <RateModal
          revenue={revenue}
          saving={revenueSaving}
          onSave={saveRevenue}
          onClose={() => setRateModalOpen(false)}
        />
      )}

      {editingItem && (
        <TrainingDateModal
          item={editingItem}
          saving={dateSaving}
          onSave={(date) => saveDate(editingItem.userId, date)}
          onClose={() => setEditingItem(null)}
        />
      )}
    </AppLayout>
  );
}
