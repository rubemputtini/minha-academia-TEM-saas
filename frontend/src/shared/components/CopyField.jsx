import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
    ClipboardDocumentIcon,
    CheckIcon,
} from "@heroicons/react/24/outline";

export default function CopyField({ value }) {
    const [copied, setCopied] = useState(false);

    const handleCopy = async () => {
        if (!value) return;

        await navigator.clipboard.writeText(value);
        setCopied(true);
        setTimeout(() => setCopied(false), 700);
    };

    return (
        <div className="space-y-2">
            <div className="inline-flex max-w-full items-center gap-3 rounded-md border border-primary/40 bg-primary/5 px-3 py-2 shadow-sm">
                <span className="truncate font-mono text-sm text-primary">
                    {value}
                </span>

                <Button
                    type="button"
                    size="icon"
                    variant="ghost"
                    onClick={handleCopy}
                    disabled={!value}
                    className={`h-7 w-7 shrink-0 rounded-md transition-all ${copied
                        ? "scale-110 bg-yellow-500/20 text-yellow-300"
                        : "hover:bg-primary/10 hover:text-primary"
                        }`}
                >
                    {copied ? (
                        <CheckIcon className="size-4" />
                    ) : (
                        <ClipboardDocumentIcon className="size-4" />
                    )}
                </Button>
            </div>
        </div>
    );
}
