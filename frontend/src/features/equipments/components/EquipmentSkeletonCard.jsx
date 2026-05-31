import { Skeleton } from "@/components/ui/skeleton";

export default function EquipmentSkeletonCard({ twoLines = false }) {
  return (
    <div className="flex flex-col rounded-xl border border-white/6 overflow-hidden">
      <Skeleton className="aspect-square w-full rounded-none bg-white/[0.04]" />
      <div className={`px-2.5 py-2 ${twoLines ? "space-y-1.5" : ""}`}>
        <Skeleton className="h-3 w-4/5 bg-white/[0.04]" />
        {twoLines && <Skeleton className="h-3 w-2/5 bg-white/[0.03]" />}
      </div>
    </div>
  );
}
