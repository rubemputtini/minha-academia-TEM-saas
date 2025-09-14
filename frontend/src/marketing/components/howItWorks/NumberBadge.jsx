export default function NumberBadge({ value, size = "md", className = "" }) {
    const cls = size === "sm" ? "h-9 w-9 text-[12px]" : "h-11 w-11 text-[13px] md:h-12 md:w-12 md:text-sm";
    return (
        <span
            className={[
                "grid place-items-center rounded-full",
                "border border-amber-300/55 text-foreground font-semibold font-mono",
                "bg-background",
                cls,
                className,
            ].join(" ")}
        >
            {value}
        </span>
    );
}