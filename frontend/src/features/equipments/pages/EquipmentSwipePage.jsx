import AppLayout from "@/shared/layout/AppLayout";
import SwipeDeck from "@/marketing/components/preview/SwipeDeck";

import { useEquipmentSwipe } from "@/features/equipments/hooks/useEquipmentSwipe";
import { useEquipmentSelection } from "@/features/equipments/hooks/useEquipmentSelection";

import NoImg from "@/assets/no.png";
import YesImg from "@/assets/yes.png";
import BackImg from "@/assets/back.png";
import VideoImg from "@/assets/video.png";

import AlertBanner from "@/shared/components/AlertBanner";
import LoadingCard from "@/shared/ui/LoadingCard";
import ConfirmDialog from "@/shared/components/ConfirmDialog";
import SuccessDialog from "@/shared/components/SuccessDialog";

export default function EquipmentSwipePage() {
    const { items, loading, loadError } = useEquipmentSwipe();
    const {
        handleChoice,
        requestSave,
        confirmSave,
        cancelSave,
        saving,
        saveError,
        confirmOpen,
        saveSuccess,
        resetSuccess,
    } = useEquipmentSelection();

    const itemCount = items?.length ?? 0;

    const hasItems = !loadError && itemCount > 0;
    const isEmpty = !loadError && !loading && itemCount === 0;
    const isInitialLoading = loading && !hasItems && !loadError;

    return (
        <AppLayout>
            <div className="mx-auto w-full space-y-4 md:text-center">
                <header className="space-y-1 pb-3">
                    <p className="text-xs uppercase tracking-[0.18em] text-foreground/60">
                        Personalize a sua experiência
                    </p>

                    <h1 className="text-2xl md:text-3xl font-semibold">
                        Escolha os equipamentos disponíveis da sua academia
                    </h1>

                    <p className="text-sm text-muted-foreground">
                        Arraste o card para o lado ou use os botões abaixo para marcar se sua academia <strong>TEM ou NÃO TEM</strong> aquele equipamento.
                    </p>
                </header>

                {isInitialLoading && (
                    <div className="mt-4">
                        <LoadingCard />
                    </div>
                )}

                {!isInitialLoading && loadError && (
                    <AlertBanner
                        variant="error"
                        title="Não foi possível carregar seus equipamentos."
                        message={loadError}
                        className="mt-3"
                    />
                )}

                {!isInitialLoading && isEmpty && (
                    <AlertBanner
                        variant="info"
                        title="Nenhum equipamento cadastrado ainda."
                        message="Quando o seu treinador cadastrar os equipamentos da sua academia, eles vão aparecer aqui para você selecionar."
                        className="mt-3"
                    />
                )}

                {!isInitialLoading && hasItems && (
                    <section className="mt-4 space-y-4">
                        <SwipeDeck
                            items={items}
                            brandLogoSrc="/logo.png"
                            yesIconSrc={YesImg}
                            noIconSrc={NoImg}
                            backIconSrc={BackImg}
                            videoIconSrc={VideoImg}
                            infiniteLoop={false}
                            onChoice={handleChoice}
                            onFinish={requestSave}
                        />

                        {saveError && !saving && (
                            <AlertBanner
                                variant="error"
                                title="Erro ao salvar sua seleção."
                                message={saveError}
                                className="mt-2"
                                compact
                            />
                        )}
                    </section>
                )}

                <ConfirmDialog
                    open={confirmOpen}
                    onOpenChange={cancelSave}
                    title="Tem certeza que deseja salvar?"
                    onConfirm={confirmSave}
                    onCancel={cancelSave}
                    loading={saving}
                />

                <SuccessDialog
                    open={saveSuccess}
                    onOpenChange={(isOpen) => {
                        if (!isOpen) resetSuccess();
                    }}
                    title="Seleção salva com sucesso!"
                    onAction={resetSuccess}
                />
            </div>
        </AppLayout>
    );
}
