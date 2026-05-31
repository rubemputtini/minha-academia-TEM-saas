import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { CheckCircle2, Pencil, RotateCcw } from "lucide-react";

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
import PageHeader from "@/shared/components/PageHeader";
import { Button } from "@/components/ui/button";
import { ROUTES } from "@/shared/routes/routes";

export default function EquipmentSwipePage() {
    const navigate = useNavigate();
    const { items, loading, loadError, alreadyCompleted } = useEquipmentSwipe();
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

    const [completedThisSession, setCompletedThisSession] = useState(false);
    const [restarted, setRestarted] = useState(false);
    const [restartCount, setRestartCount] = useState(0);

    useEffect(() => {
        if (saveSuccess) {
            setCompletedThisSession(true);
            setRestarted(false);
        }
    }, [saveSuccess]);

    function handleRestart() {
        setRestarted(true);
        setRestartCount((c) => c + 1);
    }

    const completed = !restarted && (alreadyCompleted || completedThisSession);

    const itemCount = items?.length ?? 0;
    const hasItems = !loadError && itemCount > 0;
    const isEmpty = !loadError && !loading && itemCount === 0;
    const isInitialLoading = loading && !hasItems && !loadError;

    return (
        <AppLayout>
            <div>
                {/* Conteúdo da página — desfocado quando completo */}
                <div className={`mx-auto w-full space-y-4 md:text-center transition-all duration-300 ${completed ? "blur-sm opacity-55 pointer-events-none select-none" : ""}`}>
                    <PageHeader
                        eyebrow="Personalize a sua experiência"
                        title="Escolha os equipamentos disponíveis da sua academia"
                        subtitle={
                            <>
                                Arraste o card para o lado ou use os botões abaixo para marcar se sua academia{" "}
                                <strong>TEM ou NÃO TEM</strong> aquele equipamento.
                            </>
                        }
                        align="center"
                    />

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
                                key={restartCount}
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
                </div>

                {/* Overlay quando completo — fixed cobre o viewport inteiro sem bordas visíveis */}
                {completed && (
                    <div className="fixed inset-0 z-40 flex items-center justify-center px-6 pointer-events-none">
                        {/* Scrim: cobre o viewport inteiro uniformemente */}
                        <div className="absolute inset-0 bg-[rgba(6,8,14,0.45)]" />

                        {/* Glow emerald ambiente */}
                        <div className="pointer-events-none absolute inset-0 flex items-center justify-center">
                            <div className="h-[420px] w-[420px] rounded-full bg-emerald-500/12 blur-[80px]" />
                        </div>

                        {/* Conteúdo flutuante */}
                        <div className="relative flex flex-col items-center gap-6 text-center drop-shadow-[0_2px_24px_rgba(0,0,0,0.9)] pointer-events-auto">
                            <div className="relative">
                                <div className="absolute inset-0 scale-[2.2] rounded-full bg-emerald-500/20 blur-2xl" />
                                <div className="relative flex h-16 w-16 items-center justify-center rounded-full border border-emerald-500/35 bg-[rgba(10,12,20,0.9)] backdrop-blur-sm">
                                    <CheckCircle2 className="h-7 w-7 text-emerald-400" />
                                </div>
                            </div>

                            <div className="space-y-2">
                                <p className="text-3xl font-bold tracking-tight text-white">Pronto.</p>
                                <p className="text-sm text-white/55 max-w-[280px] leading-relaxed">
                                    Sua seleção está registrada. Na página de equipamentos você também pode editar suas escolhas e deixar um recado para o seu treinador.
                                </p>
                            </div>

                            <div className="flex gap-3">
                                <Button
                                    variant="ghost"
                                    size="sm"
                                    onClick={handleRestart}
                                    className="rounded-xl border border-white/15 gap-1.5 text-white/60 hover:text-white hover:bg-white/8 backdrop-blur-sm"
                                >
                                    <RotateCcw className="h-3.5 w-3.5" />
                                    Refazer
                                </Button>
                                <Button
                                    size="sm"
                                    onClick={() => navigate(ROUTES.equipments)}
                                    className="rounded-xl gap-1.5"
                                >
                                    <Pencil className="h-3.5 w-3.5" />
                                    Editar seleção
                                </Button>
                            </div>
                        </div>
                    </div>
                )}
            </div>

            <ConfirmDialog
                open={confirmOpen}
                onOpenChange={cancelSave}
                title="Salvar seleção de equipamentos?"
                description="Sua escolha será registrada e compartilhada com o seu treinador."
                confirmLabel="Salvar"
                onConfirm={confirmSave}
                onCancel={cancelSave}
                loading={saving}
            />

            <SuccessDialog
                open={saveSuccess}
                onOpenChange={(isOpen) => {
                    if (!isOpen) resetSuccess();
                }}
                title="Seleção salva!"
                onAction={resetSuccess}
            />
        </AppLayout>
    );
}
