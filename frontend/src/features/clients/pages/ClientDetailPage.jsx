import { useParams, useNavigate } from "react-router-dom";
import { ArrowLeft, CalendarDays, ChevronRight, ClipboardList, UserRound } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import AppLayout from "@/shared/layout/AppLayout";
import LoadingCard from "@/shared/ui/LoadingCard";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";
import { getInitials } from "@/shared/utils/getInitials";
import { getStatus, calcDaysInfo } from "@/shared/utils/trainingSchedule.utils";
import { daysFromToday, fmtDue } from "@/shared/utils/date";
import { useClientDetail } from "../hooks/useClientDetail";
import { ROUTES } from "@/shared/routes/routes";

const AVATAR_BY_STATUS = {
  late:   "border border-red-400/50 bg-red-500/15 shadow-[0_0_40px_rgba(239,68,68,0.4)] text-red-300",
  urgent: "border border-amber-300/50 bg-amber-500/15 shadow-[0_0_40px_rgba(245,197,94,0.4)] text-amber-300",
  normal: "border border-primary/35 bg-primary/15 shadow-[0_0_40px_rgba(253,186,27,0.25)] text-primary",
  noDate: "border border-white/10 bg-white/[0.04] text-muted-foreground/50",
};

const SHIMMER_BY_STATUS = {
  late:   "from-transparent via-red-500/35 to-transparent",
  urgent: "from-transparent via-amber-400/35 to-transparent",
  normal: "from-transparent via-primary/40 to-transparent",
  noDate: "from-transparent via-white/12 to-transparent",
};

export default function ClientDetailPage() {
  const { clientId } = useParams();
  const navigate = useNavigate();
  const { loading, client } = useClientDetail(clientId);

  return (
    <AppLayout>
      <div className="max-w-3xl mx-auto px-4 py-6 space-y-3">
        <button
          onClick={() => navigate(ROUTES.coachUsers)}
          className="inline-flex items-center gap-1.5 text-sm text-muted-foreground hover:text-foreground transition-colors mb-3"
        >
          <ArrowLeft className="h-4 w-4" />
          Alunos
        </button>

        {loading ? (
          <LoadingCard />
        ) : !client ? (
          <p className="text-sm text-muted-foreground">Aluno não encontrado.</p>
        ) : (
          <ClientProfile client={client} clientId={clientId} navigate={navigate} />
        )}
      </div>
    </AppLayout>
  );
}

function ClientProfile({ client, clientId, navigate }) {
  const status = getStatus(client.nextTrainingChangeAt);

  return (
    <div className="space-y-3">
      <ProfileCard client={client} status={status} />
      <TrainingCard isoDate={client.nextTrainingChangeAt} status={status} />
      <EquipmentsRow
        onClick={() =>
          navigate(ROUTES.coachClientEquipments.replace(":clientId", clientId))
        }
      />
    </div>
  );
}

function ProfileCard({ client, status }) {
  const hasLocation = client.gymCity || client.gymCountry;
  const location = [client.gymCity, client.gymCountry].filter(Boolean).join(", ");

  return (
    <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
      <div className="pointer-events-none absolute -right-5 -top-6 opacity-[0.035]">
        <UserRound className="h-36 w-36" />
      </div>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />

      <CardContent className="relative z-10 px-6 py-5">
        <div className="flex items-center gap-5">
          <div
            className={cn(
              "h-16 w-16 shrink-0 rounded-full flex items-center justify-center text-lg font-bold select-none",
              AVATAR_BY_STATUS[status]
            )}
          >
            {getInitials(client.name)}
          </div>

          <div className="flex-1 min-w-0">
            <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/60 mb-1">
              Aluno
            </p>
            <h1 className="text-xl font-bold tracking-tight truncate">{client.name}</h1>
            <p className="mt-0.5 text-sm tracking-[0.04em] text-muted-foreground/75 truncate">
              {client.email}
            </p>
          </div>

          <div className="shrink-0 flex items-center gap-2">
            <span
              className={cn(
                "h-2 w-2 rounded-full",
                client.isActive
                  ? "bg-green-500 shadow-[0_0_8px_2px_rgba(34,197,94,0.5)]"
                  : "bg-muted-foreground/25"
              )}
            />
            <span className="text-xs text-muted-foreground hidden sm:inline">
              {client.isActive ? "Ativo" : "Inativo"}
            </span>
          </div>
        </div>

        {(client.gymName || hasLocation) && (
          <div className="mt-5 pt-4 border-t border-white/6 flex flex-wrap gap-x-8 gap-y-3">
            {client.gymName && <FactTile label="Academia" value={client.gymName} />}
            {hasLocation && <FactTile label="Localização" value={location} />}
          </div>
        )}
      </CardContent>
    </Card>
  );
}

function TrainingCard({ isoDate, status }) {
  const info = calcDaysInfo(isoDate);
  const days = isoDate ? daysFromToday(isoDate) : null;
  const absDays = days !== null ? Math.abs(days) : null;
  const showBigNumber = absDays !== null && absDays > 1;

  return (
    <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
      <div className="pointer-events-none absolute -right-4 -top-5 opacity-[0.04]">
        <CalendarDays className="h-32 w-32" />
      </div>
      <div
        className={cn(
          "pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r",
          SHIMMER_BY_STATUS[status]
        )}
      />

      <CardContent className="relative z-10 px-6 py-5">
        <p className="text-xs font-semibold uppercase tracking-[0.12em] text-muted-foreground/90 mb-4">
          Próxima troca de treino
        </p>

        <div className="flex items-end justify-between gap-4">
          <div>
            <p className={cn("text-2xl font-bold leading-none", info.colorClass)}>
              {info.primary}
            </p>
            {info.subtitle && (
              <p className="mt-2 text-xs tracking-[0.06em] text-muted-foreground/60">
                {info.subtitle}
              </p>
            )}
            {!isoDate && (
              <p className="mt-1 text-xs text-muted-foreground/50">
                Nenhuma data configurada
              </p>
            )}
          </div>

          {showBigNumber && (
            <div className="text-right shrink-0">
              <span
                className={cn(
                  "text-5xl font-bold font-mono leading-none tabular-nums",
                  info.colorClass
                )}
              >
                {absDays}
              </span>
              <p className="mt-1 text-[10px] uppercase tracking-[0.12em] text-muted-foreground/50">
                {days < 0 ? "dias atrás" : "dias"}
              </p>
            </div>
          )}
        </div>
      </CardContent>
    </Card>
  );
}

function EquipmentsRow({ onClick }) {
  return (
    <button
      onClick={onClick}
      className="group w-full rounded-2xl border border-white/8 bg-white/[0.02] px-5 py-4 flex items-center gap-4 text-left transition-all hover:bg-white/[0.04] hover:border-white/14"
    >
      <div className="h-10 w-10 rounded-full bg-primary/10 border border-primary/20 flex items-center justify-center shrink-0 transition-colors group-hover:bg-primary/15">
        <ClipboardList className="h-4 w-4 text-primary" />
      </div>
      <div className="flex-1 min-w-0">
        <p className="text-sm font-semibold">Equipamentos do aluno</p>
        <p className="text-xs text-muted-foreground/70 mt-0.5">
          Visualizar seleções de equipamento
        </p>
      </div>
      <ChevronRight className="h-4 w-4 text-muted-foreground/35 shrink-0 transition-transform group-hover:translate-x-0.5" />
    </button>
  );
}

function FactTile({ label, value }) {
  return (
    <div>
      <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/45">
        {label}
      </p>
      <p className="mt-0.5 text-sm font-medium">{value}</p>
    </div>
  );
}
