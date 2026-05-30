import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";

export default function ConfirmDialog({
    open,
    onOpenChange,
    title = "Tem certeza que deseja continuar?",
    description,
    confirmLabel = "CONFIRMAR",
    confirmVariant = "default",
    cancelLabel = "CANCELAR",
    onConfirm,
    onCancel,
    loading = false,
}) {
    const handleChange = (isOpen) => {
        if (!isOpen && !loading) onCancel?.();
        onOpenChange?.(isOpen);
    };

    return (
        <Dialog open={open} onOpenChange={handleChange}>
            <DialogContent
                className="
                    rounded-3xl border border-white/10
                    bg-gradient-to-b from-background/95 to-background/98
                    shadow-[0_24px_80px_rgba(0,0,0,0.85)]
                    px-6 py-7
                    max-w-sm
                "
            >
                <DialogHeader>
                    <DialogTitle className="text-lg font-semibold tracking-tight">
                        {title}
                    </DialogTitle>
                    {description && (
                        <p className="text-sm text-muted-foreground leading-relaxed">
                            {description}
                        </p>
                    )}
                </DialogHeader>

                <DialogFooter className="flex flex-row items-center gap-3 mt-2">
                    <Button
                        variant="outline"
                        size="lg"
                        onClick={onCancel}
                        disabled={loading}
                        className="flex-1"
                    >
                        {cancelLabel}
                    </Button>

                    <Button
                        variant={confirmVariant}
                        size="lg"
                        loading={loading}
                        onClick={onConfirm}
                        className="flex-1"
                    >
                        {confirmLabel}
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
