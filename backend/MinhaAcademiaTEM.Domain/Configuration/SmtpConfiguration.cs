namespace MinhaAcademiaTEM.Domain.Configuration;

public sealed class SmtpConfiguration
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}