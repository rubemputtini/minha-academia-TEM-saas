import { Skeleton } from "@/components/ui/skeleton";
import { Card, CardHeader, CardContent } from "@/components/ui/card";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";

export default function AccountSkeleton() {
    return (
        <div className="space-y-4">
            <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
                <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-white/10 to-transparent" />
                <CardHeader className="flex flex-row items-center gap-4 relative z-10">
                    <Skeleton className="h-14 w-14 rounded-full bg-white/6" />
                    <div className="space-y-2">
                        <Skeleton className="h-4 w-40 bg-white/6" />
                        <Skeleton className="h-3 w-56 bg-white/4" />
                    </div>
                </CardHeader>
            </Card>

            <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
                <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-white/10 to-transparent" />
                <CardHeader className="relative z-10">
                    <Skeleton className="h-4 w-32 bg-white/6" />
                </CardHeader>
                <CardContent className="space-y-3 relative z-10">
                    <Skeleton className="h-9 w-full bg-white/4" />
                    <Skeleton className="h-9 w-full bg-white/4" />
                </CardContent>
            </Card>

            <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
                <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-white/10 to-transparent" />
                <CardHeader className="relative z-10">
                    <Skeleton className="h-4 w-40 bg-white/6" />
                </CardHeader>
                <CardContent className="space-y-3 relative z-10">
                    <Skeleton className="h-9 w-full bg-white/4" />
                    <Skeleton className="h-9 w-full bg-white/4" />
                    <Skeleton className="h-9 w-full bg-white/4" />
                </CardContent>
            </Card>
        </div>
    );
}
