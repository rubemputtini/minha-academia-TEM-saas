import * as React from "react";
import * as AccordionPrimitive from "@radix-ui/react-accordion";
import { cn } from "@/lib/utils";
import { PlusSmallIcon, MinusSmallIcon } from "@heroicons/react/24/solid";

const Accordion = AccordionPrimitive.Root;

const AccordionItem = React.forwardRef(({ className, ...props }, ref) => (
  <AccordionPrimitive.Item
    ref={ref}
    className={cn(
      "rounded-2xl ring-1 ring-white/10 bg-card/50 backdrop-blur",
      "transition-shadow hover:shadow-[0_8px_24px_rgba(0,0,0,0.25)]",
      className
    )}
    {...props}
  />
));
AccordionItem.displayName = "AccordionItem";

const AccordionTrigger = React.forwardRef(({ className, children, ...props }, ref) => (
  <AccordionPrimitive.Header className="flex">
    <AccordionPrimitive.Trigger
      ref={ref}
      className={cn(
        "group flex w-full items-center justify-between gap-4 px-5 py-4 rounded-2xl",
        "text-left font-medium text-foreground/90",
        "focus:outline-none focus-visible:ring-2 focus-visible:ring-primary/40",
        "transition-colors hover:bg-white/2.5",
        className
      )}
      {...props}
    >
      <span className="leading-snug">{children}</span>

      <span
        className={cn(
          "inline-flex h-7 w-7 items-center justify-center rounded-full",
          "ring-1 ring-white/15 bg-white/5",
          "transition-all duration-200",
          "group-data-[state=open]:bg-primary/10 group-data-[state=open]:ring-primary/20"
        )}
      >
        <PlusSmallIcon className="h-4 w-4 text-foreground/70 group-data-[state=open]:hidden transition-transform" />
        <MinusSmallIcon className="h-4 w-4 text-primary hidden group-data-[state=open]:block transition-transform" />
      </span>
    </AccordionPrimitive.Trigger>
  </AccordionPrimitive.Header>
));
AccordionTrigger.displayName = "AccordionTrigger";

const AccordionContent = React.forwardRef(({ className, children, ...props }, ref) => (
  <AccordionPrimitive.Content
    ref={ref}
    className={cn(
      "px-5 pb-5 text-sm text-foreground/75",
      "relative before:content-[''] before:block before:h-px before:w-full before:bg-white/10 before:mx-0 before:mb-4",
      "data-[state=open]:animate-in data-[state=open]:fade-in data-[state=open]:slide-in-from-top-1",
      "data-[state=closed]:animate-out data-[state=closed]:fade-out data-[state=closed]:slide-out-to-top-1",
      className
    )}
    {...props}
  >
    {children}
  </AccordionPrimitive.Content>
));
AccordionContent.displayName = "AccordionContent";

export { Accordion, AccordionItem, AccordionTrigger, AccordionContent };
