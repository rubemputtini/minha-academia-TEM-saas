import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { AspectRatio } from "@/components/ui/aspect-ratio";

function toEmbed(url) {
    if (!url) return "";

    const ytWatch = url.match(/youtube\.com\/watch\?v=([^&]+)/i);
    const ytShort = url.match(/youtu\.be\/([^?]+)/i);
    const vimeo = url.match(/vimeo\.com\/(\d+)/i);

    if (ytWatch) return `https://www.youtube.com/embed/${ytWatch[1]}?rel=0&autoplay=1`;
    if (ytShort) return `https://www.youtube.com/embed/${ytShort[1]}?rel=0&autoplay=1`;
    if (vimeo) return `https://player.vimeo.com/video/${vimeo[1]}?autoplay=1`;

    return url; // qualquer outro link: iframe direto
}

export default function VideoModal({ open, onOpenChange, url, title = "Vídeo" }) {
    const src = toEmbed(url);

    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent
                className="w-[92vw] max-w-[860px] rounded-2xl border border-white/10 bg-black/75 text-white backdrop-blur-xl shadow-2xl p-0"
            >

                <DialogHeader className="px-4 sm:px-6 pt-4 sm:pt-5 pb-2">
                    <DialogTitle className="font-semibold text-white/90">
                        {title}
                    </DialogTitle>
                </DialogHeader>

                <div className="px-3 sm:px-6 pb-4 sm:pb-6">
                    <AspectRatio ratio={16 / 9}>
                        {src ? (
                            <iframe
                                key={src}
                                src={src}
                                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
                                allowFullScreen
                                loading="lazy"
                                className="h-full w-full"
                            />
                        ) : (
                            <div className="grid h-full w-full place-items-center rounded-lg bg-black/50">
                                <p className="text-white/70">Sem vídeo disponível</p>
                            </div>
                        )}
                    </AspectRatio>
                </div>
            </DialogContent>
        </Dialog>
    );
}
