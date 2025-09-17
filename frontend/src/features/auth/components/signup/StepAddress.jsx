import { Input } from "@/components/ui/input";
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from "@/components/ui/form";
import CountrySelect from "@/features/auth/components/CountrySelect";
import RequiredMark from "./RequiredMark";

export default function StepAddress({ control, inputClass, watch, setValue }) {
    return (
        <div className="grid grid-cols-1 gap-5 md:grid-cols-12">

            <FormField control={control} name="street" render={({ field }) => (
                <FormItem className="md:col-span-7 space-y-2">
                    <FormLabel className="block">Rua<RequiredMark /></FormLabel>
                    <FormControl>
                        <Input {...field} placeholder="Av. Exemplo" maxLength={100} className={inputClass} />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="number" render={({ field }) => (
                <FormItem className="md:col-span-3 space-y-2">
                    <FormLabel className="block">Número<RequiredMark /></FormLabel>
                    <FormControl>
                        <Input {...field} placeholder="123" maxLength={20} className={inputClass} />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="complement" render={({ field }) => (
                <FormItem className="md:col-span-2 space-y-2">
                    <FormLabel className="block">Complemento</FormLabel>
                    <FormControl>
                        <Input {...field} placeholder="Casa" maxLength={100} className={inputClass} />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="neighborhood" render={({ field }) => (
                <FormItem className="md:col-span-3 space-y-2">
                    <FormLabel className="block">Bairro<RequiredMark /></FormLabel>
                    <FormControl>
                        <Input {...field} placeholder="Centro" maxLength={60} className={inputClass} />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="city" render={({ field }) => (
                <FormItem className="md:col-span-6 space-y-2">
                    <FormLabel className="block">Cidade<RequiredMark /></FormLabel>
                    <FormControl>
                        <Input {...field} placeholder="Brasília" maxLength={60} className={inputClass} />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="state" render={({ field }) => (
                <FormItem className="md:col-span-3 space-y-2">
                    <FormLabel className="block">Estado<RequiredMark /></FormLabel>
                    <FormControl>
                        <Input {...field} placeholder="DF" maxLength={50} className={inputClass} />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="postalCode" render={({ field }) => (
                <FormItem className="md:col-span-4 space-y-2">
                    <FormLabel className="block">Código Postal<RequiredMark /></FormLabel>
                    <FormControl>
                        <Input {...field} placeholder="99999-999" maxLength={20} className={inputClass} />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="country" render={() => (
                <FormItem className="md:col-span-4 space-y-2">
                    <FormLabel className="block">País<RequiredMark /></FormLabel>
                    <FormControl>
                        <CountrySelect
                            value={watch("country")}
                            onChange={(val) => setValue("country", val, { shouldValidate: true })}
                            className={inputClass}
                        />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />
        </div>
    );
}
