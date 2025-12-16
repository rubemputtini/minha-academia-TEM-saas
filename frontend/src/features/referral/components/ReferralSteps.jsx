import {
  GiftTopIcon,
  ShareIcon,
  UserPlusIcon,
} from "@heroicons/react/24/outline";

export default function ReferralSteps() {
  return (
    <div className="rounded-xl border border-border/30 bg-muted/10 p-5">
      <div className="grid gap-6 sm:grid-cols-3">
        {/* Passo 1 */}
        <div className="flex flex-col items-center gap-3 text-center">
          <div className="flex h-10 w-10 items-center justify-center rounded-full border border-border bg-background shadow-sm">
            <ShareIcon className="h-5 w-5 text-muted-foreground" />
          </div>

          <div className="space-y-1">
            <p className="text-xs font-semibold text-foreground">
              1. Compartilhe
            </p>
            <p className="text-[11px] leading-relaxed text-muted-foreground">
              Envie seu código único para outros treinadores.
            </p>
          </div>
        </div>

        {/* Passo 2 */}
        <div className="flex flex-col items-center gap-3 text-center">
          <div className="flex h-10 w-10 items-center justify-center rounded-full border border-border bg-background shadow-sm">
            <UserPlusIcon className="h-5 w-5 text-muted-foreground" />
          </div>

          <div className="space-y-1">
            <p className="text-xs font-semibold text-foreground">
              2. Eles usam
            </p>
            <p className="text-[11px] leading-relaxed text-muted-foreground">
              O treinador indicado ganha{" "}
              <span className="font-medium text-emerald-400">50% OFF</span> no
              1º mês.
            </p>
          </div>
        </div>

        {/* Passo 3 */}
        <div className="flex flex-col items-center gap-3 text-center">
          <div className="flex h-10 w-10 items-center justify-center rounded-full border border-emerald-500/40 bg-background shadow-sm shadow-emerald-900/10">
            <GiftTopIcon className="h-5 w-5 text-emerald-400" />
          </div>

          <div className="space-y-1">
            <p className="text-xs font-semibold text-foreground">
              3. Você ganha
            </p>
            <p className="text-[11px] leading-relaxed text-muted-foreground">
              Sua próxima fatura vem com{" "}
              <span className="font-medium text-emerald-400">
                50% de desconto
              </span>
              .
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
