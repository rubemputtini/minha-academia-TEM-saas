export const handleError = (
  error,
  defaultMessage = "Erro inesperado. Tente novamente."
) => {
  const resp = error?.response;
  const data = resp?.data;

  let msgFromApi = "";
  let detailsFromApi = "";

  if (typeof data?.Message === "string") {
    msgFromApi = data.Message;
  } else if (typeof data?.message === "string") {
    msgFromApi = data.message;
  }

  if (!msgFromApi && data?.errors && typeof data.errors === "object") {
    const allErrors = Object.values(data.errors)
      .flatMap((v) => (Array.isArray(v) ? v : [v]))
      .map((v) => String(v).trim())
      .filter(Boolean);

    if (allErrors.length > 0) {
      msgFromApi = allErrors.join("\n");
    }
  }

  if (typeof data?.Details === "string") {
    detailsFromApi = data.Details.trim();
  }

  const baseMsg = (msgFromApi || defaultMessage).trim();

  let det = detailsFromApi;
  if (det) {
    const parts = det
      .split(";")
      .map((s) => s.trim())
      .filter(Boolean);

    const unique = [...new Set(parts)].filter((p) => p !== baseMsg);
    det = unique.join("\n"); // cada detalhe em uma linha
  }

  const message = [baseMsg, det].filter(Boolean).join("\n");

  const customError = new Error(message);

  if (resp?.status) customError.status = resp.status;
  if (det) customError.details = det;

  throw customError;
};
