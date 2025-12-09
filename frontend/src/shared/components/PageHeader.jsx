export default function PageHeader({
    title,
    subtitle,
    eyebrow,
    align = "left",
    className = "",
}) {
    const alignClasses =
        align === "center"
            ? "text-center items-center md:text-center"
            : "text-left items-start";

    return (
        <header className={`space-y-1 pb-3 flex flex-col ${alignClasses} ${className}`}>
            {eyebrow && (
                <p className="text-xs uppercase tracking-[0.18em] text-foreground/60">
                    {eyebrow}
                </p>
            )}

            <h1 className="text-2xl md:text-3xl font-semibold tracking-tight">
                {title}
            </h1>

            {subtitle && (
                <p className="text-sm text-muted-foreground max-w-2xl">
                    {subtitle}
                </p>
            )}
        </header>
    );
}
