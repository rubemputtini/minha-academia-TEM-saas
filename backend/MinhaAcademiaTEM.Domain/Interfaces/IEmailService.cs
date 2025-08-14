using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IEmailService
{
    Task<bool> SendNewCoachEmailAsync(Coach coach);
    Task<bool> SendNewClientEmailAsync(User user, Gym gym);
    Task<bool> SendWelcomeFreeCoachEmailAsync(Coach coach);
    Task<bool> SendSubscriptionConfirmedEmailAsync(Coach coach);

    Task<bool> SendEmailAsync(
        string toName,
        string toEmail,
        string subject,
        string templateName,
        Dictionary<string, string> templateData,
        string fromName = "Minha Academia TEM?",
        string fromEmail = "contato@minhaacademiatem.com.br");

    Task<bool> SendPasswordResetEmailAsync(string name, string email, string resetLink);
}