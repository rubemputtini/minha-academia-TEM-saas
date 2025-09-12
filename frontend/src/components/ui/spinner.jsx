import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";

export function Spinner({ size = 24, className, ...props }) {
    const hasAria = Boolean(props["aria-label"]);
    return (
        <Loader2
            className={cn("animate-spin text-muted-foreground", className)}
            style={{ width: size, height: size }}
            aria-hidden={hasAria ? undefined : true}
            role={hasAria ? "status" : undefined}
            {...props}
        />
    );
}
