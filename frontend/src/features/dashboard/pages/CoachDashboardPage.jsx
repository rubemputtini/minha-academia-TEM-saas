// src/features/dashboard/pages/CoachDashboardPage.jsx

import React from "react";
import AppLayout from "@/shared/layout/AppLayout";
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { ScrollArea } from "@/components/ui/scroll-area";
import {
  Dumbbell,
  Activity,
  MoreHorizontal,
  Clock,
  Settings2,
  ArrowUpRight,
  Bell,
  Users,
} from "lucide-react";
import { cn } from "@/lib/utils";

// -----------------------------------------------------------------------------
// MOCK DATA
// -----------------------------------------------------------------------------
const MOCK_PLAN = {
  name: "BASIC",
  studentsLimit: 5,
  currentStudents: 3,
};

const MOCK_TRAININGS = [
  {
    id: 1,
    name: "Simone Puttini",
    due: "17 Out",
    status: "late",
    gym: "SMARTFIT",
  },
  { id: 5, name: "Ana Paula", due: "10 Out", status: "late", gym: "BLUEFIT" },
  {
    id: 4,
    name: "Rafael Italo",
    due: "20 Dez",
    status: "urgent",
    gym: "IRONBERG",
  },
  {
    id: 2,
    name: "Clara Maria",
    due: "26 Dez",
    status: "normal",
    gym: "SMARTFIT",
  },
  {
    id: 3,
    name: "Pedro Santana",
    due: "16 Jan",
    status: "normal",
    gym: "BODYTECH",
  },
  {
    id: 6,
    name: "Marcos V.",
    due: "18 Jan",
    status: "normal",
    gym: "RESIDENCIAL",
  },
  { id: 7, name: "Julia M.", due: "01 Fev", status: "normal", gym: "BLUEFIT" },
];

const MOCK_NOTIFICATIONS = [
  {
    id: 1,
    message: "Sincronização com academia SmartFit concluída.",
    time: "HÁ 2 HORAS",
  },
  {
    id: 2,
    message: "Novo aluno cadastrado: João Silva.",
    time: "HÁ 5 HORAS",
  },
  {
    id: 3,
    message: "Equipamentos atualizados na Bluefit Vila Mariana.",
    time: "ONTEM",
  },
];

// -----------------------------------------------------------------------------
// PAGE
// -----------------------------------------------------------------------------
export default function CoachDashboardPage() {
  return (
    <AppLayout title="Dashboard">
      {/* Container travado para evitar scroll da página */}
      <div className="flex h-[calc(100vh-100px)] flex-col overflow-hidden">
        <div className="grid h-full grid-cols-1 gap-6 p-3 lg:grid-cols-12">
          {/* LADO ESQUERDO: RENOVAÇÕES (PRINCIPAL) */}
          <div className="flex h-full flex-col overflow-hidden lg:col-span-8">
            <Card className="flex flex-1 flex-col overflow-hidden rounded-2xl border border-white/10 bg-[rgba(10,12,20,0.96)] backdrop-blur-2xl shadow-[0_18px_60px_rgba(0,0,0,0.8)]">
              <CardHeader className="flex flex-row items-center justify-between border-b border-white/10 py-5">
                <div className="space-y-1">
                  <CardTitle className="text-2xl font-semibold tracking-tight text-foreground">
                    Renovações de treino
                  </CardTitle>
                  <CardDescription className="text-[11px] uppercase tracking-[0.28em] text-muted-foreground/80">
                    Fichas com vencimento próximo
                  </CardDescription>
                </div>

                <Badge className="rounded-full bg-white/5 px-4 py-1 font-mono text-[10px] tracking-[0.22em] text-muted-foreground">
                  {MOCK_TRAININGS.length} PENDENTES
                </Badge>
              </CardHeader>

              <CardContent className="flex-1 overflow-hidden p-0">
                <ScrollArea className="h-full">
                  <div className="px-1">
                    {MOCK_TRAININGS.map((item) => (
                      <div
                        key={item.id}
                        className="group flex items-center justify-between border-b border-white/5 px-6 py-5 transition-colors hover:bg-white/[0.03]"
                      >
                        {/* Lado esquerdo */}
                        <div className="flex items-center gap-5">
                          <StatusIndicator status={item.status} />
                          <div>
                            <p className="text-sm font-semibold uppercase tracking-tight text-foreground group-hover:text-primary transition-colors">
                              {item.name}
                            </p>
                            <p className="mt-1 flex items-center gap-1.5 text-[10px] font-semibold tracking-[0.18em] text-muted-foreground">
                              <Dumbbell className="h-3 w-3 opacity-60" />
                              {item.gym}
                            </p>
                          </div>
                        </div>

                        {/* Lado direito */}
                        <div className="flex items-center gap-8">
                          <div className="flex flex-col text-right">
                            <span
                              className={cn(
                                "text-xs font-mono font-bold leading-none",
                                item.status === "late"
                                  ? "text-red-400"
                                  : item.status === "urgent"
                                  ? "text-amber-400"
                                  : "text-muted-foreground/80"
                              )}
                            >
                              {item.due}
                            </span>
                            <span className="mt-1 text-[9px] uppercase tracking-[0.24em] text-muted-foreground/60">
                              Vencimento
                            </span>
                          </div>

                          <Button
                            variant="ghost"
                            size="icon"
                            className="h-8 w-8 rounded-full text-muted-foreground/40 hover:bg-white/5 hover:text-foreground"
                          >
                            <MoreHorizontal className="h-4 w-4" />
                          </Button>
                        </div>
                      </div>
                    ))}
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>
          </div>

          {/* LADO DIREITO: PLANO + EQUIPAMENTOS + ATIVIDADE */}
          <div className="flex h-full flex-col gap-6 overflow-hidden lg:col-span-4">
            {/* CARD SEU PLANO – KPI HERO */}
            <Card className="relative shrink-0 overflow-hidden rounded-2xl border border-white/12 bg-[rgba(16,18,28,0.96)] backdrop-blur-2xl shadow-[0_16px_50px_rgba(0,0,0,0.75)]">
              {/* Watermark */}
              <div className="pointer-events-none absolute -right-2 -top-4 opacity-[0.04]">
                <Users className="h-24 w-24" />
              </div>

              <CardContent className="relative z-10 flex items-center justify-between px-6 py-5">
                <div className="space-y-2">
                  <p className="text-[11px] font-semibold uppercase tracking-[0.24em] text-muted-foreground">
                    Seu plano
                  </p>

                  <div className="mt-1 flex items-end gap-2 font-mono">
                    <span className="text-6xl font-bold leading-none text-foreground">
                      {MOCK_PLAN.currentStudents}
                    </span>
                    <span className="pb-1 text-xl text-muted-foreground">
                      / {MOCK_PLAN.studentsLimit}
                    </span>
                  </div>

                  <p className="text-[11px] text-muted-foreground/80">
                    Alunos ativos neste plano.
                  </p>
                </div>

                <Badge className="rounded-full bg-primary/90 px-4 py-1 text-[11px] font-semibold tracking-[0.22em] text-black">
                  {MOCK_PLAN.name}
                </Badge>
              </CardContent>
            </Card>

            {/* CARD EQUIPAMENTOS */}
            <Card className="flex shrink-0 items-center justify-between rounded-2xl border border-white/10 bg-[rgba(18,20,30,0.96)] backdrop-blur-2xl p-5 shadow-[0_14px_45px_rgba(0,0,0,0.65)]">
              <div className="flex items-center gap-4">
                <div className="flex h-12 w-12 items-center justify-center rounded-xl border border-primary/25 bg-gradient-to-br from-primary/30 to-primary/5 text-primary shadow-[0_0_25px_rgba(253,186,27,0.25)]">
                  <Settings2 className="h-6 w-6" />
                </div>
                <div className="space-y-0.5">
                  <p className="text-sm font-semibold tracking-tight text-foreground">
                    Equipamentos
                  </p>
                  <p className="text-[11px] text-muted-foreground leading-tight">
                    Ajustar visibilidade por academia.
                  </p>
                </div>
              </div>

              <Button
                size="icon"
                variant="ghost"
                className="h-9 w-9 rounded-full border border-white/12 bg-white/5 text-muted-foreground hover:bg-white/10 hover:text-foreground"
              >
                <ArrowUpRight className="h-4 w-4" />
              </Button>
            </Card>

            {/* CARD ATIVIDADE RECENTE */}
            <Card className="flex flex-1 flex-col rounded-2xl border border-white/10 bg-[rgba(12,14,22,0.96)] backdrop-blur-2xl shadow-[0_16px_55px_rgba(0,0,0,0.7)]">
              <CardHeader className="px-6 pb-1 pt-4">
                <div className="flex items-center gap-2">
                  <div className="flex h-7 w-7 items-center justify-center rounded-full bg-primary/10 text-primary">
                    <Bell className="h-4 w-4" />
                  </div>
                  <CardTitle className="text-[11px] font-semibold uppercase tracking-[0.24em] text-muted-foreground">
                    Atividade recente
                  </CardTitle>
                </div>
              </CardHeader>

              <CardContent className="flex-1 overflow-hidden p-0">
                <ScrollArea className="h-full px-6 pb-6">
                  <div className="relative mt-3 ml-2 space-y-7 border-l border-white/10 pl-5">
                    {/* Linha com gradient */}
                    <div className="pointer-events-none absolute -left-[1px] top-0 h-full w-[2px] bg-gradient-to-b from-primary/60 via-primary/10 to-transparent" />

                    {MOCK_NOTIFICATIONS.map((notif) => (
                      <div key={notif.id} className="relative">
                        <div className="absolute -left-[26px] top-1 h-3 w-3 rounded-full bg-primary shadow-[0_0_14px_rgba(253,186,27,0.9)] ring-4 ring-background/90" />
                        <p className="text-[13px] font-medium leading-snug text-foreground/90">
                          {notif.message}
                        </p>
                        <p className="mt-1 text-[10px] font-mono uppercase tracking-[0.18em] text-muted-foreground/70">
                          {notif.time}
                        </p>
                      </div>
                    ))}
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>
          </div>
        </div>
      </div>
    </AppLayout>
  );
}

// -----------------------------------------------------------------------------
// SUBCOMPONENTES
// -----------------------------------------------------------------------------
function StatusIndicator({ status }) {
  if (status === "late") {
    return (
      <div className="flex h-10 w-10 items-center justify-center rounded-full border border-red-400/40 bg-red-500/10 shadow-[0_0_28px_rgba(239,68,68,0.45)]">
        <Clock className="h-5 w-5 text-red-400" />
      </div>
    );
  }

  if (status === "urgent") {
    return (
      <div className="flex h-10 w-10 items-center justify-center rounded-full border border-amber-300/40 bg-amber-500/10 shadow-[0_0_28px_rgba(245,197,94,0.45)]">
        <Activity className="h-5 w-5 text-amber-300" />
      </div>
    );
  }

  return (
    <div className="flex h-10 w-10 items-center justify-center rounded-full border border-white/8 bg-white/5 opacity-60">
      <div className="h-2 w-2 rounded-full bg-zinc-500/70" />
    </div>
  );
}
