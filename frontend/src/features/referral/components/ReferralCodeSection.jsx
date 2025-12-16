import CopyField from "@/shared/components/CopyField";

export default function ReferralCodeSection({ referralCode }) {
  return (
    <>
      <div className="relative">
        <div className="absolute inset-0 flex items-center">
          <span className="w-full border-t border-border/40" />
        </div>

        <div className="relative flex justify-center text-xs uppercase">
          <span className="bg-card px-2 font-medium text-muted-foreground">
            Código de indicação
          </span>
        </div>
      </div>

      <div className="mx-auto flex max-w-md flex-col items-center justify-center space-y-4 text-center">
        <p className="text-sm text-muted-foreground">
          O treinador indicado ganha desconto no 1º mês, e você recebe o seu na
          fatura seguinte.
        </p>

        <div className="w-full sm:w-3/4">
          <CopyField value={referralCode} />
        </div>
      </div>
    </>
  );
}
