export const handleError = (error, defaultMessage = "Erro inesperado. Tente novamente.") => {
    const resp = error?.response;
    const data = resp?.data;

    const msg = (data?.Message || defaultMessage).trim();
    let det = (data?.Details || "").trim();

    if (det) {
        const parts = det.split(";").map(s => s.trim()).filter(Boolean);
        const unique = [...new Set(parts)].filter(p => p !== msg);
        det = unique.join("\n"); // mostra cada detalhe em uma linha
    }

    const message = [msg, det].filter(Boolean).join("\n");

    const customError = new Error(message);
    
    if (resp?.status) customError.status = resp.status;
    if (det) customError.details = det;

    throw customError;
};
