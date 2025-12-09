import { Toaster as Sonner } from "sonner";

export function Toaster(props) {
  return (
    <Sonner
      position="top-center"
      theme="dark"
      className="toaster group"
      style={{
        "--normal-bg": "var(--card)",
        "--normal-border": "var(--border)",
        "--normal-text": "var(--foreground)",

        "--success-bg": "var(--card)",
        "--success-border": "var(--primary)",
        "--success-text": "var(--primary)",

        "--error-bg": "var(--card)",
        "--error-border": "var(--destructive)",
        "--error-text": "var(--destructive)",
      }}
      toastOptions={{
        classNames: {
          toast: [
            "group toast",
            "rounded-lg",
            "border border-border/70",
            "shadow-[0_4px_20px_rgba(0,0,0,0.45)]",
            "backdrop-blur-sm",
            "bg-card/95",
            "text-foreground",
          ].join(" "),
          description: "text-muted-foreground",
          actionButton: [
            "bg-primary",
            "text-primary-foreground",
            "hover:brightness-110",
          ].join(" "),
          cancelButton: [
            "bg-muted/40",
            "text-muted-foreground",
            "hover:bg-muted/60",
          ].join(" "),
        },
      }}
      {...props}
    />
  );
}
