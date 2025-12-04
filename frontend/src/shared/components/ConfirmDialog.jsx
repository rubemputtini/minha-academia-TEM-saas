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
    confirmLabel = "CONFIRMAR",
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
                    text-center justify-center
                    px-6 py-8
                    max-w-sm
                "
            >
                <DialogHeader className="text-center">
                    <DialogTitle className="text-xl font-semibold tracking-tight">
                        {title}
                    </DialogTitle>
                </DialogHeader>

                <DialogFooter
                    className="mt-6 flex flex-row items-center justify-center gap-3"
                >
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
