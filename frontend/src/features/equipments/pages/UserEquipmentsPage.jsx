import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Play, Search, Dumbbell, ImageOff, AlertTriangle } from "lucide-react";
import AppLayout from "@/shared/layout/AppLayout";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Skeleton } from "@/components/ui/skeleton";
import VideoModal from "@/shared/components/VideoModal";
import EquipmentNoteCard from "../components/EquipmentNoteCard";
import EquipmentSkeletonCard from "../components/EquipmentSkeletonCard";
import EquipmentSectionHeader from "../components/EquipmentSectionHeader";
import { CARD_BASE } from "@/shared/styles/cards";
import { MUSCLE_GROUPS } from "@/shared/constants/muscleGroups";
import { useUserEquipments } from "../hooks/useUserEquipments";
import { cn } from "@/lib/utils";

function Stamp({ isSelected }) {
  return (
    <span
      className={cn(
        "inline-block px-2 py-[3px] rounded-[3px] font-black text-[9px] uppercase tracking-[0.18em]",
        "border-[1.5px] -rotate-2 select-none transition-all duration-150",
        isSelected
          ? "border-emerald-400/80 text-emerald-200 bg-black/65"
          : "border-white/20 text-white/40 bg-black/55"
      )}
    >
      {isSelected ? "TEM" : "NÃO TEM"}
    </span>
  );
}

function EquipmentCard({ item, isSelected, onToggle, onPlay }) {
  const hasVideo = !!item.videoUrl;
  const hasPhoto = !!item.photoUrl;

  return (
    <button
      type="button"
      aria-pressed={isSelected}
      aria-label={`${item.name} — ${isSelected ? "disponível, clique para remover" : "não disponível, clique para adicionar"}`}
      onClick={() => onToggle(item.equipmentId)}
      className={cn(
        "group relative flex flex-col rounded-xl overflow-hidden text-left w-full",
        "border transition-all duration-150",
        "focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-white/20",
        isSelected
          ? "border-emerald-500/50 shadow-[0_0_18px_rgba(52,211,153,0.10)]"
          : "border-white/8 hover:border-white/18 shadow-none"
      )}
    >
      {/* Photo */}
      <div className="relative aspect-square overflow-hidden bg-white/[0.02] shrink-0">
        {hasPhoto ? (
          <img
            src={item.photoUrl}
            alt=""
            className={cn(
              "h-full w-full object-cover transition-opacity duration-150",
              !isSelected && "opacity-25 group-hover:opacity-40"
            )}
            loading="lazy"
          />
        ) : (
          <div
            className={cn(
              "flex h-full w-full items-center justify-center transition-opacity duration-150",
              !isSelected && "opacity-25 group-hover:opacity-40"
            )}
          >
            <ImageOff className="h-7 w-7 text-white/15" />
          </div>
        )}

        <div className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/70 via-black/10 to-transparent" />

        {/* Stamp + video aligned at bottom */}
        <div className="absolute bottom-2 left-2 right-2 z-10 flex items-end justify-between gap-1">
          <Stamp isSelected={isSelected} />
          {hasVideo && (
            <button
              onClick={(e) => { e.stopPropagation(); onPlay(item); }}
              aria-label={`Ver vídeo de ${item.name}`}
              className="flex h-5 w-5 shrink-0 items-center justify-center rounded-full border border-white/20 bg-black/60 text-white/55 transition-colors hover:border-primary/50 hover:text-primary"
            >
              <Play className="h-2.5 w-2.5 fill-current" />
            </button>
          )}
        </div>
      </div>

      {/* Info */}
      <div
        className={cn(
          "flex-1 flex flex-col px-2.5 pt-2 pb-2.5 transition-colors duration-150",
          isSelected
            ? "bg-emerald-950/15 border-t border-emerald-500/15"
            : "border-t border-white/[0.04]"
        )}
      >
        <p
          className={cn(
            "line-clamp-2 text-xs font-medium leading-snug min-h-[2.1rem] transition-colors duration-150",
            isSelected
              ? "text-foreground/85"
              : "text-foreground/35 group-hover:text-foreground/55"
          )}
        >
          {item.name}
        </p>
      </div>
    </button>
  );
}

function FloatingSaveBar({ changesCount, saving, onSave, onReset }) {
  return (
    <div className="fixed bottom-6 left-1/2 -translate-x-1/2 z-50 animate-in slide-in-from-bottom-4 duration-300 w-[calc(100vw-2rem)] max-w-md">
      <div
        className={cn(
          "flex items-center justify-between gap-3 rounded-2xl px-5 py-3.5",
          "border border-amber-500/25 bg-[rgba(10,12,20,0.97)] backdrop-blur-xl",
          "shadow-[0_8px_40px_rgba(0,0,0,0.85),0_0_0_1px_rgba(245,158,11,0.10)]"
        )}
      >
        <div className="flex items-center gap-2.5 min-w-0">
          <AlertTriangle className="h-4 w-4 shrink-0 text-amber-400 animate-pulse" />
          <div className="min-w-0">
            <p className="text-sm font-semibold text-foreground/95 whitespace-nowrap">
              Alterações não salvas
            </p>
            <p className="text-xs text-muted-foreground/55 whitespace-nowrap">
              {changesCount} {changesCount === 1 ? "equipamento alterado" : "equipamentos alterados"}
            </p>
          </div>
        </div>

        <div className="flex items-center gap-2 shrink-0">
          <button
            type="button"
            onClick={onReset}
            disabled={saving}
            className="text-xs text-muted-foreground/50 hover:text-muted-foreground/80 transition-colors px-2 py-1 disabled:pointer-events-none"
          >
            Descartar
          </button>
          <Button
            size="sm"
            disabled={saving}
            loading={saving}
            onClick={onSave}
            className="rounded-xl px-4 h-9 text-xs font-semibold bg-amber-500 hover:bg-amber-400 text-black whitespace-nowrap"
          >
            Salvar agora
          </Button>
        </div>
      </div>
    </div>
  );
}

function UnsavedDialog({ open, onContinue, onLeave }) {
  if (!open) return null;

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black/75 backdrop-blur-sm p-4"
      onClick={(e) => e.target === e.currentTarget && onContinue()}
    >
      <div className="w-full max-w-sm rounded-2xl border border-white/12 bg-[rgba(16,18,28,0.98)] p-6 shadow-[0_24px_80px_rgba(0,0,0,0.9)] backdrop-blur-2xl">
        <div className="flex items-center gap-3 mb-4">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full border border-amber-500/20 bg-amber-500/10">
            <AlertTriangle className="h-5 w-5 text-amber-400" />
          </div>
          <div>
            <h2 className="text-base font-semibold text-foreground">Sair sem salvar?</h2>
            <p className="text-xs text-muted-foreground/55 mt-0.5">As mudanças serão perdidas</p>
          </div>
        </div>

        <p className="text-sm text-muted-foreground/70 leading-relaxed mb-6">
          Você tem alterações na seleção de equipamentos que ainda não foram salvas.
        </p>

        <div className="flex gap-3">
          <Button
            type="button"
            variant="ghost"
            onClick={onContinue}
            className="flex-1 rounded-xl border border-white/10 text-[11px] uppercase tracking-[0.18em] text-muted-foreground hover:bg-white/5"
          >
            Cancelar
          </Button>
          <Button
            type="button"
            onClick={onLeave}
            className="flex-1 rounded-xl text-[11px] uppercase tracking-[0.18em] bg-red-500/15 border border-red-500/25 text-red-400 hover:bg-red-500/25 hover:border-red-500/40"
          >
            Sair
          </Button>
        </div>
      </div>
    </div>
  );
}

export default function UserEquipmentsPage() {
  const [videoItem, setVideoItem] = useState(null);
  const [showUnsavedDialog, setShowUnsavedDialog] = useState(false);
  const [pendingNavTarget, setPendingNavTarget] = useState(null);
  const navigate = useNavigate();

  const {
    loading,
    items,
    grouped,
    selected,
    toggle,
    reset,
    selectAll,
    clearAll,
    isDirty,
    changesCount,
    save,
    saving,
    total,
    selectedCount,
    searchInput,
    setSearchInput,
    isSearching,
  } = useUserEquipments();

  const allSelected = total > 0 && selectedCount === total;

  // Block browser close / refresh
  useEffect(() => {
    if (!isDirty) return;

    const handler = (e) => { e.preventDefault(); e.returnValue = ""; };
    window.addEventListener("beforeunload", handler);

    return () => window.removeEventListener("beforeunload", handler);
  }, [isDirty]);

  // Intercept browser back/forward button
  useEffect(() => {
    if (!isDirty) return;

    const handler = () => {
      window.history.pushState(null, "", window.location.href);
      setShowUnsavedDialog(true);
    };

    window.history.pushState(null, "", window.location.href);
    window.addEventListener("popstate", handler);

    return () => window.removeEventListener("popstate", handler);
  }, [isDirty]);

  // Intercept all link clicks before React Router handles them
  const isDirtyRef = useRef(isDirty);
  isDirtyRef.current = isDirty;

  useEffect(() => {
    const handleClick = (e) => {
      if (!isDirtyRef.current) return;

      const anchor = e.target.closest("a[href]");
      
      if (!anchor) return;
      
      const href = anchor.getAttribute("href");
      
      if (!href || href.startsWith("#") || href.startsWith("mailto:") || anchor.target === "_blank") return;
      
      e.preventDefault();
      e.stopImmediatePropagation();
      setPendingNavTarget(href);
      setShowUnsavedDialog(true);
    };

    document.addEventListener("click", handleClick, true);
    
    return () => document.removeEventListener("click", handleClick, true);
  }, []);

  const progressPct = total > 0 ? Math.round((selectedCount / total) * 100) : 0;

  return (
    <AppLayout>
      <div className="max-w-5xl mx-auto px-4 py-6 space-y-6 pb-28">

        {/* Header */}
        <div className="space-y-1">
          <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/40">
            Minha academia
          </p>
          <h1 className="text-2xl font-bold tracking-tight">Meus equipamentos</h1>
          <p className="text-sm text-muted-foreground/55">
            Marque os equipamentos que você tem acesso. Seu treinador usa essa lista para montar seus treinos.
          </p>

          {/* Progress */}
          {loading ? (
            <div className="flex items-center gap-3 pt-2">
              <Skeleton className="h-1 w-28 rounded-full bg-white/6" />
              <Skeleton className="h-3 w-32 bg-white/4" />
            </div>
          ) : total > 0 ? (
            <div className="flex items-center gap-3 pt-2">
              <div className="h-1 w-28 rounded-full bg-white/6 overflow-hidden">
                <div
                  className="h-full rounded-full bg-emerald-500/65 transition-all duration-500"
                  style={{ width: `${progressPct}%` }}
                />
              </div>
              <span className="text-xs text-muted-foreground/45 tabular-nums">
                {selectedCount} de {total} selecionados
              </span>
            </div>
          ) : null}
        </div>

        {/* Search + bulk actions */}
        <div className="flex items-center gap-3 flex-wrap">
          <div className="relative flex-1 min-w-[200px] max-w-sm">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground pointer-events-none" />
            <Input
              placeholder="Buscar equipamento..."
              value={searchInput}
              onChange={(e) => setSearchInput(e.target.value)}
              className="pl-9"
            />
          </div>

          {!loading && total > 0 && (
            <button
              type="button"
              onClick={allSelected ? clearAll : selectAll}
              className="text-xs text-muted-foreground/50 hover:text-foreground/70 transition-colors whitespace-nowrap px-1"
            >
              {allSelected ? "Desmarcar todos" : "Selecionar todos"}
            </button>
          )}
        </div>

        {/* Content */}
        {loading ? (
          <div className="space-y-8">
            {[6, 4, 5].map((n, sIdx) => (
              <div key={sIdx}>
                <div className="flex items-center gap-3 mb-4">
                  <Skeleton className="h-6 w-1.5 rounded-full bg-white/10" />
                  <Skeleton className="h-5 w-24 bg-white/6" />
                  <Skeleton className="h-4 w-10 bg-white/4" />
                  <div className="flex-1 h-px bg-white/4" />
                </div>
                <div className="grid grid-cols-3 gap-2.5 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-6">
                  {Array.from({ length: n }).map((_, i) => <EquipmentSkeletonCard twoLines key={i} />)}
                </div>
              </div>
            ))}
          </div>
        ) : total === 0 ? (
          <div className={`${CARD_BASE} relative overflow-hidden`}>
            <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-white/10 to-transparent" />
            <div className="flex flex-col items-center gap-3 py-20 text-center">
              <div className="flex h-14 w-14 items-center justify-center rounded-full border border-white/8 bg-white/[0.03]">
                <Dumbbell className="h-6 w-6 opacity-30" />
              </div>
              <div className="space-y-1">
                <p className="text-sm font-medium text-foreground">Nenhum equipamento disponível</p>
                <p className="text-xs text-muted-foreground/50">
                  Seu treinador ainda não cadastrou equipamentos para a sua academia.
                </p>
              </div>
            </div>
          </div>
        ) : isSearching && items.length === 0 ? (
          <div className={`${CARD_BASE} relative overflow-hidden`}>
            <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-white/10 to-transparent" />
            <div className="flex flex-col items-center gap-3 py-16 text-center">
              <div className="flex h-14 w-14 items-center justify-center rounded-full border border-white/8 bg-white/[0.03]">
                <Search className="h-6 w-6 opacity-30" />
              </div>
              <div className="space-y-1">
                <p className="text-sm font-medium text-foreground">Nenhum resultado encontrado</p>
                <p className="text-xs text-muted-foreground/50">
                  Não há equipamentos com &ldquo;{searchInput}&rdquo;.
                </p>
              </div>
            </div>
          </div>
        ) : isSearching ? (
          <div className="space-y-3">
            <p className="text-xs text-muted-foreground/50">
              {items.length} {items.length === 1 ? "resultado" : "resultados"} para &ldquo;{searchInput}&rdquo;
            </p>
            <div className="grid grid-cols-3 gap-2.5 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-6">
              {items.map((item) => (
                <EquipmentCard
                  key={item.equipmentId}
                  item={item}
                  isSelected={selected.has(item.equipmentId)}
                  onToggle={toggle}
                  onPlay={setVideoItem}
                />
              ))}
            </div>
          </div>
        ) : (
          <div className="space-y-8">
            {MUSCLE_GROUPS.filter((g) => grouped?.[g.value]).map((g) => {
              const groupItems = grouped[g.value];
              const groupSelected = groupItems.filter((e) => selected.has(e.equipmentId)).length;
              
              return (
                <div key={g.value}>
                  <EquipmentSectionHeader
                    muscleValue={g.value}
                    count={groupItems.length}
                    selectedCount={groupSelected}
                  />
                  <div className="grid grid-cols-3 gap-2.5 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-6">
                    {groupItems.map((item) => (
                      <EquipmentCard
                        key={item.equipmentId}
                        item={item}
                        isSelected={selected.has(item.equipmentId)}
                        onToggle={toggle}
                        onPlay={setVideoItem}
                      />
                    ))}
                  </div>
                </div>
              );
            })}
          </div>
        )}

        {/* Nota */}
        {!loading && (
          <div>
            <EquipmentNoteCard />
          </div>
        )}
      </div>

      {isDirty && (
        <FloatingSaveBar
          changesCount={changesCount}
          saving={saving}
          onSave={save}
          onReset={reset}
        />
      )}

      <UnsavedDialog
        open={showUnsavedDialog}
        onContinue={() => {
          setShowUnsavedDialog(false);
          setPendingNavTarget(null);
        }}
        onLeave={() => {
          const target = pendingNavTarget;
          setShowUnsavedDialog(false);
          setPendingNavTarget(null);
          reset();
          if (target) navigate(target);
        }}
      />

      <VideoModal
        open={!!videoItem}
        onOpenChange={(open) => !open && setVideoItem(null)}
        url={videoItem?.videoUrl}
        title={videoItem?.name}
      />
    </AppLayout>
  );
}
