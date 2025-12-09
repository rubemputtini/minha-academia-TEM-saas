import { Skeleton } from "@/components/ui/skeleton";
import { Card, CardHeader, CardContent } from "@/components/ui/card";

export default function AccountSkeleton() {
    return (
        <div className="space-y-4">
            <Card className="bg-card/80 border-border">
                <CardHeader className="flex flex-row items-center gap-4">
                    <Skeleton className="h-12 w-12 rounded-full" />
                    <div className="space-y-2">
                        <Skeleton className="h-4 w-40" />
                        <Skeleton className="h-3 w-64" />
                    </div>
                </CardHeader>
            </Card>

            <Card className="bg-card/80 border-border">
                <CardHeader>
                    <Skeleton className="h-4 w-32" />
                </CardHeader>
                <CardContent className="space-y-3">
                    <Skeleton className="h-10 w-full" />
                    <Skeleton className="h-10 w-full" />
                </CardContent>
            </Card>

            <Card className="bg-card/80 border-border">
                <CardHeader>
                    <Skeleton className="h-4 w-40" />
                </CardHeader>
                <CardContent className="space-y-3">
                    <Skeleton className="h-10 w-full" />
                    <Skeleton className="h-10 w-full" />
                    <Skeleton className="h-10 w-full" />
                </CardContent>
            </Card>
        </div>
    );
}