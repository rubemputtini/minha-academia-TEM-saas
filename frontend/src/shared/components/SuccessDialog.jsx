import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";

export default function SuccessDialog({
    open,
    onOpenChange,
    title = "Tudo certo!",
    actionLabel = "FECHAR",
    onAction,
}) {
    const handleChange = (isOpen) => {
        if (!isOpen) {
            onAction?.();
        }
        onOpenChange?.(isOpen);
    };

    const handleClick = () => {
        onAction?.();
        onOpenChange?.(false);
    };

    return (
        <Dialog open={open} onOpenChange={handleChange}>
            <DialogContent
                className="
                mx-auto
                w-[90%] max-w-sm
                rounded-3xl border border-white/10
                bg-gradient-to-b from-background/95 to-background/98
                shadow-[0_24px_80px_rgba(0,0,0,0.85)]
                text-center
                px-6 py-8
                grid place-items-center
                "
            >
                <DialogHeader className="text-center space-y-2">
                    <DialogTitle className="text-xl font-semibold tracking-tight">
                        {title}
                    </DialogTitle>
                </DialogHeader>

                <DialogFooter className="mt-6 flex justify-center">
                    <Button size="lg" onClick={handleClick}>
                        {actionLabel}
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
