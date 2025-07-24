using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class EmailService(
    IConfiguration configuration,
    ILogger<EmailService> logger,
    IOptions<SmtpConfiguration> smtpConfig,
    IWebHostEnvironment environment) : IEmailService
{
    public async Task<bool> SendNewCoachEmailAsync(Coach coach)
    {
        var data = new Dictionary<string, string>
        {
            { "Name", coach.Name },
            { "Email", coach.Email },
            { "Slug", coach.Slug },
        };

        return await SendEmailAsync(
            toName: "Equipe Minha Academia TEM?",
            toEmail: configuration["AdminSettings:AdminEmail"]!,
            subject: "Novo treinador cadastrado! - Minha Academia TEM?",
            templateName: "NewCoachTemplate",
            templateData: data);
    }

    public async Task<bool> SendEmailAsync(
        string toName,
        string toEmail,
        string subject,
        string templateName,
        Dictionary<string, string> templateData,
        string fromName = "Minha Academia TEM?",
        string fromEmail = "contato@rubemputtini.com.br")
    {
        var body = await GetEmailTemplateAsync(templateName);
        body = FillTemplateWithData(body, templateData);

        var mail = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(new MailAddress(toEmail, toName));

        using var smtpClient = new SmtpClient(smtpConfig.Value.Host, smtpConfig.Value.Port);

        smtpClient.Credentials = new NetworkCredential(smtpConfig.Value.UserName, smtpConfig.Value.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;

        try
        {
            await smtpClient.SendMailAsync(mail);
            logger.LogInformation("E-mail enviado com sucesso para {Email}", toEmail);

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao enviar e-mail para {Email}", toEmail);

            return false;
        }
    }

    public async Task<bool> SendPasswordResetEmailAsync(string name, string email, string resetLink)
    {
        var data = new Dictionary<string, string>
        {
            { "Name", name },
            { "ResetLink", resetLink }
        };

        return await SendEmailAsync(
            toName: name,
            toEmail: email,
            subject: "Redefinição de Senha - Minha Academia TEM?",
            templateName: "ResetPasswordTemplate",
            templateData: data);
    }

    private async Task<string> GetEmailTemplateAsync(string templateName)
    {
        var path = Path.Combine(environment.ContentRootPath, "EmailTemplates", $"{templateName}.html");

        if (!File.Exists(path))
            throw new FileNotFoundException($"Template {templateName} não encontrado.");

        return await File.ReadAllTextAsync(path);
    }

    private static string FillTemplateWithData(string template, Dictionary<string, string> data)
    {
        return data.Aggregate(template, (current, item) => current.Replace($"{{{{{item.Key}}}}}", item.Value));
    }
}