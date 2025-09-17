import React from "react";
import { AlertCircle, CheckCircle2, Info, TriangleAlert, X } from "lucide-react";
import { cn } from "@/lib/utils";

const styles = {
    base: "relative overflow-hidden rounded-xl p-3 md:p-4",
    wrap: "flex items-start gap-3",
    leftBar: "absolute inset-y-0 left-0 w-1.5",
    title: "font-semibold leading-none",
    body: "mt-1 text-sm/6",
};

const variants = {
    error: {
        border: "border border-red-400/25",
        bg: "bg-red-500/10",
        bar: "bg-red-500/80",
        text: "text-red-100",
        sub: "text-red-100/90",
        Icon: AlertCircle,
        aria: "assertive",
    },
    success: {
        border: "border border-emerald-400/25",
        bg: "bg-emerald-500/10",
        bar: "bg-emerald-500/80",
        text: "text-emerald-100",
        sub: "text-emerald-100/90",
        Icon: CheckCircle2,
        aria: "polite",
    },
    warning: {
        border: "border border-amber-400/25",
        bg: "bg-amber-500/10",
        bar: "bg-amber-500/80",
        text: "text-amber-100",
        sub: "text-amber-100/90",
        Icon: TriangleAlert,
        aria: "polite",
    },
    info: {
        border: "border border-sky-400/25",
        bg: "bg-sky-500/10",
        bar: "bg-sky-500/80",
        text: "text-sky-100",
        sub: "text-sky-100/90",
        Icon: Info,
        aria: "polite",
    },
};

export default function AlertBanner({
    variant = "info",
    title,
    message,
    onClose,
    className,
    compact = false,
}) {
    const empty =
        message == null ||
        (typeof message === "string" && message.trim().length === 0);

    if (empty) return null;

    const v = variants[variant] ?? variants.info;
    const Icon = v.Icon;

    return (
        <div
            role="alert"
            aria-live={v.aria}
            className={cn(
                styles.base,
                v.border,
                v.bg,
                compact ? "p-3" : "p-3 md:p-4",
                className
            )}
        >
            <div aria-hidden className={cn(styles.leftBar, v.bar)} />

            <div className={styles.wrap}>
                <Icon className={cn("h-5 w-5 shrink-0 mt-0.5", v.text)} aria-hidden />
                <div className="flex-1">
                    {title && <p className={cn(styles.title, v.text)}>{title}</p>}

                    {message && (
                        <div className={cn(styles.body, v.sub)}>
                            {typeof message === "string" ? <p>{message}</p> : message}
                        </div>
                    )}
                </div>

                {onClose && (
                    <button
                        type="button"
                        onClick={onClose}
                        className={cn(
                            "-m-1 rounded-md p-1",
                            "hover:bg-white/10",
                            "focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-white/40",
                            v.text
                        )}
                        aria-label="Fechar aviso"
                    >
                        <X className="h-4 w-4" aria-hidden />
                    </button>
                )}
            </div>
        </div>
    );
}
