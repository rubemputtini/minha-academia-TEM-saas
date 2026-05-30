import { useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  CalendarDays,
  ClipboardList,
  MoreHorizontal,
  Pencil,
  Trash2,
  User,
  UserCheck,
  UserX,
  Users,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Skeleton } from "@/components/ui/skeleton";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from "@/components/ui/tooltip";
import ConfirmDialog from "@/shared/components/ConfirmDialog";
import { TrainingDateModal } from "@/shared/components/TrainingDateModal";
import { ROUTES } from "@/shared/routes/routes";
import { getInitials } from "@/shared/utils/getInitials";
import { calcDaysInfo } from "@/shared/utils/trainingSchedule.utils";
import { CARD_BASE } from "@/shared/styles/cards";

const DEFAULT_TRAINING_DATE = (() => {
  const d = new Date();
  d.setDate(d.getDate() + 30);
  
  return d.toISOString();
})();

function ClientAvatar({ name }) {
  return (
    <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-primary/10 text-xs font-semibold text-primary select-none">
      {getInitials(name)}
    </div>
  );
}

function ActiveStatusBadge({ isActive }) {
  return (
    <div className="flex items-center gap-2">
      <span
        className={`h-2 w-2 rounded-full ${
          isActive ? "bg-green-500" : "bg-muted-foreground/40"
        }`}
      />
      <span className={`text-sm ${isActive ? "text-foreground" : "text-muted-foreground"}`}>
        {isActive ? "Ativo" : "Inativo"}
      </span>
    </div>
  );
}

function TrainingDateCell({ isoDate }) {
  const { primary, subtitle, colorClass } = calcDaysInfo(isoDate);

  if (!subtitle) {
    return <span className={`text-sm ${colorClass}`}>{primary}</span>;
  }

  if (primary === "Hoje" || primary === "Amanhã") {
    return (
      <span className={`text-sm font-medium ${colorClass}`}>
        {primary}
        <span className="ml-1.5 text-xs font-normal opacity-70">· {subtitle}</span>
      </span>
    );
  }

  if (primary === "Atrasado") {
    return (
      <span className={`text-sm font-medium ${colorClass}`}>
        {subtitle}
        <span className="ml-1.5 text-xs font-normal opacity-80">· atrasado</span>
      </span>
    );
  }

  return (
    <span className={`text-sm ${colorClass}`}>
      {subtitle}
      <span className="ml-1.5 text-xs opacity-70">· {primary}</span>
    </span>
  );
}

function SkeletonRow() {
  return (
    <TableRow className="hover:bg-transparent border-b border-white/6">
      <TableCell className="pl-4 py-3">
        <div className="flex items-center gap-3">
          <Skeleton className="h-9 w-9 shrink-0 rounded-full bg-white/6" />
          <div className="space-y-2">
            <Skeleton className="h-3.5 w-36 bg-white/6" />
            <Skeleton className="h-3 w-48 bg-white/4" />
          </div>
        </div>
      </TableCell>
      <TableCell className="hidden md:table-cell">
        <Skeleton className="h-4 w-16 bg-white/4" />
      </TableCell>
      <TableCell className="hidden lg:table-cell">
        <Skeleton className="h-4 w-20 bg-white/4" />
      </TableCell>
      <TableCell className="text-right pr-3">
        <div className="flex items-center justify-end gap-1">
          <Skeleton className="h-8 w-8 rounded-md bg-white/4" />
          <Skeleton className="h-8 w-8 rounded-md bg-white/4" />
          <Skeleton className="h-8 w-8 rounded-md bg-white/4" />
          <Skeleton className="h-8 w-8 rounded-md bg-white/4" />
        </div>
      </TableCell>
    </TableRow>
  );
}

function EmptyState({ hasSearch }) {
  return (
    <TableRow className="hover:bg-transparent">
      <TableCell colSpan={4} className="py-20 text-center">
        <div className="flex flex-col items-center gap-3 text-muted-foreground">
          <div className="flex h-14 w-14 items-center justify-center rounded-full border border-white/8 bg-white/5">
            <Users className="h-6 w-6 opacity-50" />
          </div>
          <div className="space-y-1">
            <p className="text-sm font-medium text-foreground">
              {hasSearch ? "Nenhum resultado encontrado" : "Nenhum aluno cadastrado"}
            </p>
            <p className="text-xs text-muted-foreground">
              {hasSearch
                ? "Tente buscar por outro nome ou e-mail."
                : "Compartilhe seu código de indicação para receber alunos."}
            </p>
          </div>
        </div>
      </TableCell>
    </TableRow>
  );
}

export default function ClientTable({
  clients,
  loading,
  isFetching,
  actionLoadingId,
  search,
  onToggleActive,
  onDelete,
  onSaveTrainingDate,
}) {
  const navigate = useNavigate();
  const [pendingAction, setPendingAction] = useState(null);
  const [trainingClient, setTrainingClient] = useState(null);

  function requestAction(type, client) {
    setPendingAction({ type, client });
  }

  function confirmAction() {
    if (!pendingAction) return;

    const { type, client } = pendingAction;
    setPendingAction(null);
    
    if (type === "delete") onDelete(client.id);
    if (type === "toggle") onToggleActive(client.id, client.isActive);
  }

  function goToClientDetail(clientId) {
    navigate(ROUTES.coachClientDetail.replace(":clientId", clientId));
  }

  function goToClientEquipments(clientId) {
    navigate(ROUTES.coachClientEquipments.replace(":clientId", clientId));
  }

  return (
    <>
      <TooltipProvider delayDuration={300}>
        <div className={`${CARD_BASE} overflow-hidden transition-opacity duration-150 ${isFetching ? "opacity-60 pointer-events-none" : ""}`}>
          <Table>
            <TableHeader>
              <TableRow className="hover:bg-transparent border-b border-white/8 bg-white/[0.015]">
                <TableHead className="pl-4 py-3 text-xs font-medium uppercase tracking-wide">
                  Aluno
                </TableHead>
                <TableHead className="hidden md:table-cell text-xs font-medium uppercase tracking-wide">
                  Status
                </TableHead>
                <TableHead className="hidden lg:table-cell text-xs font-medium uppercase tracking-wide">
                  Troca de Treino
                </TableHead>
                <TableHead className="text-right pr-3 text-xs font-medium uppercase tracking-wide">
                  Ações
                </TableHead>
              </TableRow>
            </TableHeader>

            <TableBody>
              {loading ? (
                Array.from({ length: 5 }).map((_, i) => <SkeletonRow key={i} />)
              ) : clients.length === 0 ? (
                <EmptyState hasSearch={!!search} />
              ) : (
                clients.map((client) => {
                  const isRowLoading = actionLoadingId === client.id;

                  return (
                    <TableRow
                      key={client.id}
                      className="group border-b border-white/6 last:border-0 transition-colors hover:bg-white/[0.025]"
                    >
                      <TableCell className="pl-4 py-3">
                        <div className="flex items-center gap-3">
                          <ClientAvatar name={client.name} />
                          <div className="min-w-0">
                            <p className="truncate text-sm font-medium leading-none text-foreground">
                              {client.name}
                            </p>
                            <p className="mt-1 truncate text-xs text-muted-foreground">
                              {client.email}
                            </p>
                          </div>
                          <div className="md:hidden ml-auto shrink-0">
                            <span
                              className={`h-2 w-2 rounded-full inline-block ${
                                client.isActive ? "bg-green-500" : "bg-muted-foreground/40"
                              }`}
                            />
                          </div>
                        </div>
                      </TableCell>

                      <TableCell className="hidden md:table-cell">
                        <ActiveStatusBadge isActive={client.isActive} />
                      </TableCell>

                      <TableCell
                        className={`hidden lg:table-cell ${client.isActive ? "cursor-pointer group/date" : "opacity-40"}`}
                        onClick={client.isActive ? () => setTrainingClient(client) : undefined}
                      >
                        <div className="flex items-center gap-1.5">
                          <TrainingDateCell isoDate={client.nextTrainingChangeAt} />
                          
                          {client.isActive && (
                            <Pencil className="h-3 w-3 shrink-0 text-muted-foreground/30 opacity-0 group-hover/date:opacity-100 transition-opacity" />
                          )}
                        </div>
                      </TableCell>

                      <TableCell className="text-right pr-3">
                        {/* Ações no desktop */}
                        <div className="hidden md:flex items-center justify-end gap-0.5">
                          <Tooltip>
                            <TooltipTrigger asChild>
                              <Button
                                variant="ghost"
                                size="icon"
                                className="h-8 w-8 text-muted-foreground hover:text-foreground"
                                disabled={isRowLoading}
                                onClick={() => goToClientDetail(client.id)}
                              >
                                <User className="h-4 w-4" />
                              </Button>
                            </TooltipTrigger>
                            <TooltipContent>Ver conta</TooltipContent>
                          </Tooltip>

                          <Tooltip>
                            <TooltipTrigger asChild>
                              <Button
                                variant="ghost"
                                size="icon"
                                className="h-8 w-8 text-muted-foreground hover:text-foreground"
                                disabled={isRowLoading}
                                onClick={() => goToClientEquipments(client.id)}
                              >
                                <ClipboardList className="h-4 w-4" />
                              </Button>
                            </TooltipTrigger>
                            <TooltipContent>Ver equipamentos</TooltipContent>
                          </Tooltip>

                          <Tooltip>
                            <TooltipTrigger asChild>
                              <Button
                                variant="ghost"
                                size="icon"
                                className={`h-8 w-8 ${
                                  client.isActive
                                    ? "text-muted-foreground hover:text-amber-500"
                                    : "text-muted-foreground hover:text-green-500"
                                }`}
                                disabled={isRowLoading}
                                onClick={() => requestAction("toggle", client)}
                              >
                                {client.isActive ? (
                                  <UserX className="h-4 w-4" />
                                ) : (
                                  <UserCheck className="h-4 w-4" />
                                )}
                              </Button>
                            </TooltipTrigger>
                            <TooltipContent>
                              {client.isActive ? "Desativar aluno" : "Ativar aluno"}
                            </TooltipContent>
                          </Tooltip>

                          <Tooltip>
                            <TooltipTrigger asChild>
                              <Button
                                variant="ghost"
                                size="icon"
                                className="h-8 w-8 text-muted-foreground hover:text-destructive"
                                disabled={isRowLoading}
                                onClick={() => requestAction("delete", client)}
                              >
                                <Trash2 className="h-4 w-4" />
                              </Button>
                            </TooltipTrigger>
                            <TooltipContent>Remover aluno</TooltipContent>
                          </Tooltip>
                        </div>

                        {/* Dropdown no mobile */}
                        <div className="md:hidden">
                          <DropdownMenu>
                            <DropdownMenuTrigger asChild>
                              <Button
                                variant="ghost"
                                size="icon"
                                className="h-8 w-8"
                                disabled={isRowLoading}
                              >
                                <MoreHorizontal className="h-4 w-4" />
                              </Button>
                            </DropdownMenuTrigger>
                            <DropdownMenuContent align="end" className="w-48">
                              <DropdownMenuItem onClick={() => goToClientDetail(client.id)}>
                                <User className="mr-2 h-4 w-4" />
                                Ver conta
                              </DropdownMenuItem>
                              <DropdownMenuItem onClick={() => goToClientEquipments(client.id)}>
                                <ClipboardList className="mr-2 h-4 w-4" />
                                Ver equipamentos
                              </DropdownMenuItem>
                              <DropdownMenuItem
                                disabled={!client.isActive}
                                onClick={() => client.isActive && setTrainingClient(client)}
                              >
                                <CalendarDays className="mr-2 h-4 w-4" />
                                Data de treino
                              </DropdownMenuItem>
                              <DropdownMenuSeparator />
                              <DropdownMenuItem onClick={() => requestAction("toggle", client)}>
                                {client.isActive ? (
                                  <>
                                    <UserX className="mr-2 h-4 w-4 text-amber-500" />
                                    Desativar aluno
                                  </>
                                ) : (
                                  <>
                                    <UserCheck className="mr-2 h-4 w-4 text-green-500" />
                                    Ativar aluno
                                  </>
                                )}
                              </DropdownMenuItem>
                              <DropdownMenuItem
                                onClick={() => requestAction("delete", client)}
                                className="text-destructive focus:text-destructive"
                              >
                                <Trash2 className="mr-2 h-4 w-4" />
                                Remover aluno
                              </DropdownMenuItem>
                            </DropdownMenuContent>
                          </DropdownMenu>
                        </div>
                      </TableCell>
                    </TableRow>
                  );
                })
              )}
            </TableBody>
          </Table>
        </div>
      </TooltipProvider>

      <ConfirmDialog
        open={pendingAction?.type === "toggle"}
        onOpenChange={(open) => !open && setPendingAction(null)}
        title={
          pendingAction?.client?.isActive
            ? `Desativar ${pendingAction?.client?.name}?`
            : `Ativar ${pendingAction?.client?.name}?`
        }
        description={
          pendingAction?.client?.isActive
            ? "O aluno ficará inativo mas continua ocupando uma vaga no plano. Use isso para marcar quem não está pagando."
            : "O aluno voltará a aparecer como ativo no painel."
        }
        confirmLabel={pendingAction?.client?.isActive ? "DESATIVAR" : "ATIVAR"}
        loading={actionLoadingId !== null}
        onConfirm={confirmAction}
        onCancel={() => setPendingAction(null)}
      />

      <ConfirmDialog
        open={pendingAction?.type === "delete"}
        onOpenChange={(open) => !open && setPendingAction(null)}
        title={`Remover ${pendingAction?.client?.name}?`}
        description="Todos os dados serão apagados permanentemente, incluindo equipamentos e histórico. Essa ação não pode ser desfeita."
        confirmLabel="REMOVER"
        confirmVariant="destructive"
        loading={actionLoadingId !== null}
        onConfirm={confirmAction}
        onCancel={() => setPendingAction(null)}
      />

      {trainingClient && (
        <TrainingDateModal
          item={{
            name: trainingClient.name,
            nextTrainingChangeAt: trainingClient.nextTrainingChangeAt ?? DEFAULT_TRAINING_DATE,
          }}
          saving={actionLoadingId === trainingClient.id}
          onSave={(date) => onSaveTrainingDate(trainingClient.id, date)}
          onClose={() => setTrainingClient(null)}
        />
      )}
    </>
  );
}
