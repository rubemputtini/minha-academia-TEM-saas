import AppLayout from "@/shared/layout/AppLayout";
import { Separator } from "@/components/ui/separator";
import { Form } from "@/components/ui/form";

import PageHeader from "@/shared/components/PageHeader";
import AccountSkeleton from "../components/AccountSkeleton";

import CoachSummaryCard from "../components/CoachSummaryCard";
import CoachPersonalCard from "../components/CoachPersonalCard";
import CoachAddressCard from "../components/CoachAddressCard";

import { useAccountCoach } from "../hooks/useAccountCoach";

export default function CoachAccountPage() {
    const {
        loading,
        saving,
        isEditing,
        isEditable,
        nameValue,
        emailValue,
        inputClass,
        startEditing,
        handleCancelEdit,
        submitHandler,
        form,
        control,
        coachCode,
    } = useAccountCoach();

    return (
        <AppLayout>
            <div className="mx-auto max-w-4xl space-y-6 px-4 py-6">
                <PageHeader
                    title="Minha conta"
                    subtitle="Gerencie seus dados pessoais, endereço e configurações da sua conta."
                    align="left"
                />

                <Separator />

                {loading ? (
                    <AccountSkeleton />
                ) : (
                    <>
                        <CoachSummaryCard
                            name={nameValue}
                            email={emailValue}
                            coachCode={coachCode}
                        />

                        <Form {...form}>
                            <form onSubmit={submitHandler} className="space-y-4">
                                <CoachPersonalCard
                                    control={control}
                                    isEditing={isEditing}
                                    saving={saving}
                                    isEditable={isEditable}
                                    inputClass={inputClass}
                                    onStartEdit={startEditing}
                                    onCancelEdit={handleCancelEdit}
                                />

                                <CoachAddressCard
                                    control={control}
                                    isEditing={isEditing}
                                    isEditable={isEditable}
                                    inputClass={inputClass}
                                />
                            </form>
                        </Form>
                    </>
                )}
            </div>
        </AppLayout>
    );
}
