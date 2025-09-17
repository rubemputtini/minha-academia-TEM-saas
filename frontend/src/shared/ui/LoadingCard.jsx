import { Spinner } from "@/components/ui/spinner";
import { cn } from "@/lib/utils";

export default function LoadingCard({
    size = 32,
    fullScreen = false,
    marginY = 40,
    className,
    label = "Carregando…",
}) {
    return (
        <div
            className={cn(
                "flex flex-col items-center justify-center",
                fullScreen ? "min-h-[80vh]" : "",
                className
            )}
            style={!fullScreen ? { marginTop: marginY, marginBottom: marginY } : {}}
            aria-busy="true"
            aria-live="polite"
        >
            <Spinner size={size} aria-label={label} />
            <span className="mt-2 text-sm text-muted-foreground">{label}</span>
        </div>
    );
}
