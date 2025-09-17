import * as React from "react";
import { Slot } from "@radix-ui/react-slot";
import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";
import { buttonVariants } from "./button-variants";

const SPINNER_SIZE_BY_BUTTON = {
  sm: 14,
  default: 16,
  lg: 20,
  icon: 16,
};

function Button({
  className,
  variant,
  size = "default",
  asChild = false,
  loading = false,
  spinnerSize,
  spinnerClassName,
  children,
  disabled,
  ...props
}) {
  const Comp = asChild ? Slot : "button";
  const finalSpinnerSize = spinnerSize ?? SPINNER_SIZE_BY_BUTTON[size] ?? 16;
  const isDisabled = loading || disabled;

  return (
    <Comp
      data-slot="button"
      data-loading={loading ? "true" : undefined}
      aria-busy={loading || undefined}
      disabled={isDisabled}
      className={cn(
        "cursor-pointer disabled:cursor-not-allowed disabled:pointer-events-none",
        buttonVariants({ variant, size }),
        className,
        loading && "opacity-60"
      )}
      {...props}
    >
      {loading ? (
        <Loader2
          className={cn("animate-spin", spinnerClassName)}
          style={{ width: finalSpinnerSize, height: finalSpinnerSize }}
          aria-hidden
        />
      ) : (
        children
      )}
    </Comp>
  );
}

export { Button };
