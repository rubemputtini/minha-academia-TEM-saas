export const handleError = (error, defaultMessage) => {
    const errorMessage = error?.message || defaultMessage;

    const customError = new Error(errorMessage);

    if (error.status) {
        customError.status = error.status;
    }

    if (error.details) {
        customError.details = error.details;
    }

    throw customError;
};
