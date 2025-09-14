export default function StepTitle({ children, small = false }) {
    return (
        <div>
            <h3
                className={[
                    "font-semibold tracking-tight leading-tight",
                    small ? "text-lg" : "text-xl",
                ].join(" ")}
            >
                {children}
            </h3>
        </div>
    );
}