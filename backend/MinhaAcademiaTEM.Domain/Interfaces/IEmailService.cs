using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IEmailService
{
    Task<bool> SendNewCoachEmailAsync(Coach coach);
    Task<bool> SendEmailAsync(
        string toName,
        string toEmail,
        string subject,
        string templateName,
        Dictionary<string, string> templateData,
        string fromName = "Minha Academia TEM?",
        string fromEmail = "contato@rubemputtini.com.br");

    Task<bool> SendPasswordResetEmailAsync(string name, string email, string resetLink);
}