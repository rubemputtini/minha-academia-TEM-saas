import * as React from "react"

import { cn } from "@/lib/utils"

const cardVariants = {
  default: "rounded-xl border bg-card text-card-foreground shadow",
  glass: `
    rounded-xl border border-white/14 bg-card/88 text-card-foreground shadow
    backdrop-blur-sm shadow-[0_12px_40px_-18px_rgba(0,0,0,0.65),inset_0_1px_0_rgba(255,255,255,0.06)]
    relative overflow-hidden
    before:pointer-events-none before:absolute before:inset-0 before:rounded-[inherit]
    before:bg-[linear-gradient(115deg,transparent,rgba(255,255,255,0.10)_15%,rgba(255,255,255,0.03)_40%,transparent_60%)]
    before:opacity-35
    after:pointer-events-none after:absolute after:inset-x-0 after:top-0 after:h-10
    after:rounded-t-[inherit] after:bg-[linear-gradient(to_bottom,rgba(255,255,255,0.05),transparent)]
  `,
}

const Card = React.forwardRef(({ className, variant = "default", ...props }, ref) => (
  <div ref={ref} className={cn(cardVariants[variant], className)} {...props} />
))
Card.displayName = "Card"

const CardHeader = React.forwardRef(({ className, ...props }, ref) => (
  <div
    ref={ref}
    className={cn("flex flex-col space-y-1.5 p-6", className)}
    {...props} />
))
CardHeader.displayName = "CardHeader"

const CardTitle = React.forwardRef(({ className, ...props }, ref) => (
  <div
    ref={ref}
    className={cn("font-semibold leading-none tracking-tight", className)}
    {...props} />
))
CardTitle.displayName = "CardTitle"

const CardDescription = React.forwardRef(({ className, ...props }, ref) => (
  <div
    ref={ref}
    className={cn("text-sm text-muted-foreground", className)}
    {...props} />
))
CardDescription.displayName = "CardDescription"

const CardContent = React.forwardRef(({ className, ...props }, ref) => (
  <div ref={ref} className={cn("p-6 pt-0", className)} {...props} />
))
CardContent.displayName = "CardContent"

const CardFooter = React.forwardRef(({ className, ...props }, ref) => (
  <div
    ref={ref}
    className={cn("flex items-center p-6 pt-0", className)}
    {...props} />
))
CardFooter.displayName = "CardFooter"

export { Card, CardHeader, CardFooter, CardTitle, CardDescription, CardContent }
