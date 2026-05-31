import { ChevronLeft, ChevronRight, Search, Users } from "lucide-react";
import AppLayout from "@/shared/layout/AppLayout";
import { Separator } from "@/components/ui/separator";
import PageHeader from "@/shared/components/PageHeader";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useClients } from "../hooks/useClients";
import ClientTable from "../components/ClientTable";

export default function ClientsPage() {
  const {
    loading,
    isFetching,
    clients,
    total,
    page,
    totalPages,
    searchInput,
    debouncedSearch,
    actionLoadingId,
    setPage,
    setSearchInput,
    removeClient,
    toggleActive,
    saveTrainingDate,
  } = useClients();

  return (
    <AppLayout>
      <div className="max-w-5xl mx-auto px-4 py-6 space-y-6">
        <div className="flex items-start justify-between gap-4">
          <PageHeader
            title="Meus Alunos"
            subtitle="Gerencie os alunos vinculados ao seu perfil."
            align="left"
          />
          {!loading && (
            <div className="flex items-center gap-2 rounded-full border border-white/8 bg-white/[0.03] px-4 py-1.5 text-sm text-muted-foreground shrink-0 mt-1">
              <Users className="h-4 w-4" />
              <span>{total} {total === 1 ? "aluno" : "alunos"}</span>
            </div>
          )}
        </div>

        <Separator />

        <div className="relative">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground pointer-events-none" />
          <Input
            placeholder="Buscar por nome ou e-mail..."
            value={searchInput}
            onChange={(e) => setSearchInput(e.target.value)}
            className="pl-9 max-w-sm"
          />
        </div>

        <ClientTable
          clients={clients}
          loading={loading}
          isFetching={isFetching}
          actionLoadingId={actionLoadingId}
          search={debouncedSearch}
          onToggleActive={toggleActive}
          onDelete={removeClient}
          onSaveTrainingDate={saveTrainingDate}
        />

        {totalPages > 1 && (
          <div className="flex items-center justify-between text-sm text-muted-foreground">
            <span>
              Página {page} de {totalPages}
            </span>
            <div className="flex items-center gap-1">
              <Button
                variant="outline"
                size="icon"
                className="h-8 w-8"
                disabled={page === 1 || loading || isFetching}
                onClick={() => setPage((p) => p - 1)}
              >
                <ChevronLeft className="h-4 w-4" />
              </Button>
              <Button
                variant="outline"
                size="icon"
                className="h-8 w-8"
                disabled={page === totalPages || loading || isFetching}
                onClick={() => setPage((p) => p + 1)}
              >
                <ChevronRight className="h-4 w-4" />
              </Button>
            </div>
          </div>
        )}
      </div>
    </AppLayout>
  );
}
