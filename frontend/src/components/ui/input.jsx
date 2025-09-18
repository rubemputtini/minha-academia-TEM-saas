import * as React from "react";
import { cn } from "@/lib/utils";
import { cva } from "class-variance-authority";

export const inputVariants = cva(
  [
    "flex w-full rounded-md border bg-transparent px-3 text-base shadow-sm transition-colors",
    "file:border-0 file:bg-transparent file:text-sm file:font-medium file:text-foreground",
    "placeholder:text-muted-foreground",
    "focus-visible:outline-none",
    "disabled:cursor-not-allowed disabled:opacity-50",
    "md:text-sm",
  ].join(" "),
  {
    variants: {
      size: {
        sm: "h-8",
        md: "h-9",
        lg: "h-11",
      },
      look: {
        default: "border-input focus-visible:ring-1 focus-visible:ring-ring",
        soft:
          "rounded-xl border-white/12 bg-white/[0.03] placeholder:text-foreground/40 " +
          "focus-visible:ring-2 focus-visible:ring-amber-400/80 focus-visible:border-transparent",
      },
    },
    defaultVariants: {
      size: "md",
      look: "default",
    },
  }
);

const Input = React.forwardRef(function Input(
  { className, size, look, type, ...props },
  ref
) {
  return (
    <input
      type={type}
      className={cn(inputVariants({ size, look }), className)}
      ref={ref}
      {...props}
    />
  );
});

export { Input };
