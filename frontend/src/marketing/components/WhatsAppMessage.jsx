export default function WhatsAppMessage({
    text,
    time = "09:30",
    className = "",
    align = "left", // "left" | "right" | "center"
}) {
    const alignClass =
        align === "right"
            ? "self-end justify-end"
            : align === "center"
                ? "self-center justify-center"
                : "self-start";

    return (
        <div className={`flex ${alignClass} max-w-[78%] sm:max-w-[72%] ${className}`}>
            <div
                className={[
                    "relative px-3 py-2",
                    "bg-white rounded-2xl",
                    "shadow-[0_1px_0.5px_rgba(0,0,0,0.08)]",
                    "text-[14.5px] leading-snug text-[#111B21]",
                ].join(" ")}
            >
                <p className="pr-10">{text}</p>
                <span
                    className="absolute bottom-1 right-2 text-[11px] text-gray-400"
                    aria-label={`Enviado às ${time}`}
                >
                    {time}
                </span>
            </div>
        </div>
    );
}
