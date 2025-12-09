import AppLayout from "@/shared/layout/AppLayout";
import { Separator } from "@/components/ui/separator";
import { Form } from "@/components/ui/form";

import PageHeader from "@/shared/components/PageHeader";
import AccountSkeleton from "../components/AccountSkeleton";
import AccountSummaryCard from "../components/AccountSummaryCard";
import AccountPersonalCard from "../components/AccountPersonalCard";
import AccountGymCard from "../components/AccountGymCard";

import { useAccountUser } from "../hooks/useAccountUser";

export default function AccountUserPage() {
    const {
        loading,
        saving,
        isEditing,
        fieldEditable,
        nameValue,
        emailValue,
        inputClass,
        startEditing,
        handleCancelEdit,
        submitHandler,
        form,
        control,
    } = useAccountUser();

    return (
        <AppLayout>
            <div className="max-w-4xl mx-auto px-4 py-6 space-y-6">
                <PageHeader
                    title="Minha conta"
                    subtitle="Gerencie seus dados pessoais e da academia vinculada."
                    align="left"
                />

                <Separator />

                {loading ? (
                    <AccountSkeleton />
                ) : (
                    <>
                        <AccountSummaryCard name={nameValue} email={emailValue} />

                        <Form {...form}>
                            <form onSubmit={submitHandler} className="space-y-4">
                                <AccountPersonalCard
                                    control={control}
                                    isEditing={isEditing}
                                    saving={saving}
                                    fieldEditable={fieldEditable}
                                    inputClass={inputClass}
                                    onStartEdit={startEditing}
                                    onCancelEdit={handleCancelEdit}
                                />

                                <AccountGymCard
                                    control={control}
                                    isEditing={isEditing}
                                    fieldEditable={fieldEditable}
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
