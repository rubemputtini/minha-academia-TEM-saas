import { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ChevronLeft, Play, Search, ClipboardList, ImageOff, MessageSquare } from "lucide-react";
import AppLayout from "@/shared/layout/AppLayout";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Skeleton } from "@/components/ui/skeleton";
import VideoModal from "@/shared/components/VideoModal";
import EquipmentSkeletonCard from "@/features/equipments/components/EquipmentSkeletonCard";
import EquipmentSectionHeader from "@/features/equipments/components/EquipmentSectionHeader";
import { ROUTES } from "@/shared/routes/routes";
import { CARD_BASE } from "@/shared/styles/cards";
import { useClientEquipments } from "../hooks/useClientEquipments";
import { MUSCLE_GROUPS } from "@/shared/constants/muscleGroups";

function EquipmentCard({ item, onPlay }) {
  const hasVideo = !!item.videoUrl;
  const hasPhoto = !!item.photoUrl;

  return (
    <div className="rounded-xl border border-white/8 bg-[rgba(12,14,22,0.96)] overflow-hidden">
      {/* Photo */}
      <div className="relative aspect-square overflow-hidden bg-white/[0.02]">
        {hasPhoto ? (
          <img
            src={item.photoUrl}
            alt={item.name}
            className="h-full w-full object-cover"
            loading="lazy"
          />
        ) : (
          <div className="flex h-full w-full items-center justify-center">
            <ImageOff className="h-7 w-7 text-white/10" />
          </div>
        )}
        <div className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/40 via-transparent to-transparent" />
      </div>

      {/* Info */}
      <div className="flex items-start justify-between gap-1 px-2.5 py-2">
        <p className="line-clamp-2 min-h-[2.1rem] text-xs font-medium leading-snug text-foreground/85 flex-1">
          {item.name}
        </p>
        {hasVideo && (
          <button
            onClick={() => onPlay(item)}
            className="mt-0.5 shrink-0 flex h-5 w-5 items-center justify-center rounded-full border border-white/10 bg-white/[0.04] text-muted-foreground/50 transition-colors hover:border-primary/40 hover:text-primary"
          >
            <Play className="h-2.5 w-2.5 fill-current" />
          </button>
        )}
      </div>
    </div>
  );
}

function EmptyState({ isSearching, searchInput }) {
  return (
    <div className={`${CARD_BASE} relative overflow-hidden`}>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-white/10 to-transparent" />
      <div className="flex flex-col items-center gap-3 py-20 text-center">
        <div className="flex h-14 w-14 items-center justify-center rounded-full border border-white/8 bg-white/[0.03]">
          <ClipboardList className="h-6 w-6 opacity-30" />
        </div>
        <div className="space-y-1">
          <p className="text-sm font-medium text-foreground">
            {isSearching ? "Nenhum resultado encontrado" : "Nenhum equipamento selecionado"}
          </p>
          <p className="text-xs text-muted-foreground/50">
            {isSearching
              ? `Não há equipamentos com "${searchInput}".`
              : "Este aluno ainda não selecionou os equipamentos disponíveis."}
          </p>
        </div>
      </div>
    </div>
  );
}

export default function ClientEquipmentsPage() {
  const { clientId } = useParams();
  const navigate = useNavigate();
  const [videoItem, setVideoItem] = useState(null);

  const {
    loading,
    clientName,
    gymName,
    clientNote,
    total,
    equipments,
    grouped,
    isSearching,
    searchInput,
    setSearchInput,
  } = useClientEquipments(clientId);

  return (
    <AppLayout>
      <div className="max-w-5xl mx-auto px-4 py-6 space-y-5">

        {/* Header */}
        <div className="space-y-3">
          <Button
            variant="ghost"
            size="sm"
            className="h-8 gap-1.5 px-2 text-muted-foreground/60 hover:text-foreground -ml-2"
            onClick={() => navigate(ROUTES.coachClientDetail.replace(":clientId", clientId))}
          >
            <ChevronLeft className="h-4 w-4" />
            <span className="text-xs">Voltar</span>
          </Button>

          <div className="flex items-start justify-between gap-4">
            <div className="space-y-1">
              <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/40">
                Equipamentos
              </p>
              {loading ? (
                <>
                  <Skeleton className="h-7 w-48 bg-white/6" />
                  <Skeleton className="h-4 w-64 bg-white/4" />
                </>
              ) : (
                <>
                  <h1 className="text-2xl font-bold tracking-tight">{clientName || "Aluno"}</h1>
                  <p className="text-sm text-muted-foreground/55">
                    {gymName
                      ? `Equipamentos disponíveis na academia ${gymName}`
                      : "Equipamentos disponíveis"}
                  </p>
                </>
              )}
            </div>

            {!loading && total > 0 && (
              <div className="flex items-center gap-2 rounded-full border border-white/8 bg-white/[0.03] px-4 py-1.5 text-sm text-muted-foreground shrink-0 mt-1">
                <ClipboardList className="h-4 w-4" />
                <span>{total} {total === 1 ? "equipamento" : "equipamentos"}</span>
              </div>
            )}
          </div>
        </div>

        {/* Search */}
        <div className="relative">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground pointer-events-none" />
          <Input
            placeholder="Buscar equipamento..."
            value={searchInput}
            onChange={(e) => setSearchInput(e.target.value)}
            className="pl-9 max-w-sm"
          />
        </div>

        {/* Content */}
        {loading ? (
          <div className="space-y-6">
            {[6, 4, 5].map((n, sIdx) => (
              <div key={sIdx}>
                <div className="flex items-center gap-3 mb-3">
                  <Skeleton className="h-5 w-1 rounded-full bg-white/10" />
                  <Skeleton className="h-4 w-20 bg-white/6" />
                  <Skeleton className="h-3 w-24 bg-white/4" />
                  <div className="flex-1 h-px bg-white/4" />
                </div>
                <div className="grid grid-cols-3 gap-2.5 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-6">
                  {Array.from({ length: n }).map((_, i) => <EquipmentSkeletonCard key={i} />)}
                </div>
              </div>
            ))}
          </div>
        ) : equipments.length === 0 && !isSearching ? (
          <EmptyState isSearching={false} searchInput={searchInput} />
        ) : isSearching && equipments.length === 0 ? (
          <EmptyState isSearching={true} searchInput={searchInput} />
        ) : isSearching ? (
          <div className="space-y-3">
            <p className="text-xs text-muted-foreground/50">
              {equipments.length} {equipments.length === 1 ? "resultado" : "resultados"} para &ldquo;{searchInput}&rdquo;
            </p>
            <div className="grid grid-cols-3 gap-2.5 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-6">
              {equipments.map((item) => (
                <EquipmentCard key={item.equipmentId} item={item} onPlay={setVideoItem} />
              ))}
            </div>
          </div>
        ) : (
          <div className="space-y-8">
            {MUSCLE_GROUPS.filter((g) => grouped[g.value]).map((g) => (
              <div key={g.value}>
                <EquipmentSectionHeader muscleValue={g.value} count={grouped[g.value].length} />
                <div className="grid grid-cols-3 gap-2.5 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-6">
                  {grouped[g.value].map((item) => (
                    <EquipmentCard key={item.equipmentId} item={item} onPlay={setVideoItem} />
                  ))}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Nota do aluno */}
      {!loading && (
        <div className="max-w-5xl mx-auto px-4 pb-8">
          <div className={`${CARD_BASE} relative overflow-hidden`}>
            <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />
            <MessageSquare className="pointer-events-none absolute right-5 top-5 h-20 w-20 -rotate-6 text-white opacity-[0.035]" />

            <div className="px-6 pb-6 pt-5">
              <div className="space-y-0.5 mb-4">
                <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/50">
                  Observação
                </p>
                <h2 className="text-base font-semibold">Recado do aluno</h2>
              </div>

              {clientNote ? (
                <p className="text-sm text-foreground/75 leading-relaxed whitespace-pre-wrap">
                  {clientNote}
                </p>
              ) : (
                <p className="text-sm text-muted-foreground/35 italic">
                  Nenhuma observação registrada pelo aluno.
                </p>
              )}
            </div>
          </div>
        </div>
      )}

      <VideoModal
        open={!!videoItem}
        onOpenChange={(open) => !open && setVideoItem(null)}
        url={videoItem?.videoUrl}
        title={videoItem?.name}
      />
    </AppLayout>
  );
}
