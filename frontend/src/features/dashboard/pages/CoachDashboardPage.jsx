import { useState } from "react";
import AppLayout from "@/shared/layout/AppLayout";
import { useCoachRevenue } from "../hooks/useCoachRevenue";
import { useTrainingSchedule } from "../hooks/useTrainingSchedule";
import { getStatus } from "../utils/trainingSchedule.utils";
import { TrainingScheduleCard } from "../components/TrainingScheduleCard";
import { PlanCard } from "../components/PlanCard";
import { RevenueCard } from "../components/RevenueCard";
import { InviteCard } from "../components/InviteCard";
import { RateModal } from "../components/RateModal";
import { TrainingDateModal } from "../components/TrainingDateModal";

export default function CoachDashboardPage() {
  const { loading: revenueLoading, saving: revenueSaving, revenue, saveRevenue } = useCoachRevenue();
  const { loading: scheduleLoading, saving: dateSaving, schedule, saveDate } = useTrainingSchedule();
  const [rateModalOpen, setRateModalOpen] = useState(false);
  const [editingItem, setEditingItem] = useState(null);

  const overdueCount = schedule.filter((t) => getStatus(t.nextTrainingChangeAt) === "late").length;
  const upcomingCount = schedule.filter((t) => getStatus(t.nextTrainingChangeAt) === "urgent").length;
  const atLimit = revenue?.usersLimit != null && schedule.length >= revenue.usersLimit;

  return (
    <AppLayout title="Dashboard">
      <div className="space-y-5 p-3 pb-10">
        <div className="grid grid-cols-1 items-start gap-6 lg:grid-cols-12">
          <div className="lg:col-span-8">
            <TrainingScheduleCard
              loading={scheduleLoading}
              schedule={schedule}
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
              currentUsers={schedule.length}
              loading={revenueLoading || scheduleLoading}
            />
            <RevenueCard
              currentUsers={schedule.length}
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
