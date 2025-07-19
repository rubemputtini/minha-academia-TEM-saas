namespace MinhaAcademiaTEM.Domain.Exceptions;

public class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    public ValidationException(IEnumerable<string> errors) : base("Uma ou mais validações falharam.")
    {
        Errors = errors.ToList().AsReadOnly();
    }

    public ValidationException(string message, IEnumerable<string> errors)
        : base(message)
    {
        Errors = errors.ToList().AsReadOnly();
    }
}