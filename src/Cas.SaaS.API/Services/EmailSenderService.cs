using Cas.SaaS.API.Options;
using Cas.SaaS.Models;
using Cas.SaaS.Shared;
using MailKit.Net.Smtp;
using MimeKit;

namespace Cas.SaaS.API.Services;

/// <summary>
/// Сервис отправки кода подтверждения на почту
/// </summary>
public class EmailSenderService
{
    private readonly SmtpClientOptions _smtpClientOptions;
    private readonly CodeTemplateOptions _codeTemplateOptions;

    /// <summary>
    /// Конструктор сервиса отправки кода подтверждения на почту
    /// </summary>
    /// <param name="smtpClientOptions">Параметры Smtp-клиента</param>
    /// <param name="templateOptions">Параметры сообщения</param>
    public EmailSenderService(SmtpClientOptions smtpClientOptions, CodeTemplateOptions templateOptions)
    {
        _smtpClientOptions = smtpClientOptions;
        _codeTemplateOptions = templateOptions;
    }

    /// <summary>
    /// Отправка статуса заявки на почту
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> SendStatus(Application application)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(_codeTemplateOptions.From, _smtpClientOptions.Email));
        emailMessage.To.Add(new MailboxAddress("", application.Email));
        emailMessage.Subject = string.Format(_codeTemplateOptions.Subject, application.Id);
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = string.Format(_codeTemplateOptions.Body, application.Status.GetDisplayName())
        };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtpClientOptions.Host, _smtpClientOptions.Port, _smtpClientOptions.EnableSsl);
            await client.AuthenticateAsync(_smtpClientOptions.Email, _smtpClientOptions.Password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Ошибка отправки сообщения");
        }

        return true;
    }

    public async Task<bool> SendEditStatus(Application application)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(_codeTemplateOptions.From, _smtpClientOptions.Email));
        emailMessage.To.Add(new MailboxAddress("", application.Email));
        emailMessage.Subject = string.Format(_codeTemplateOptions.Subject, application.Id);
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = string.Format("Статус заявки был изменен на '{0}'", application.Status.GetDisplayName())
        };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtpClientOptions.Host, _smtpClientOptions.Port, _smtpClientOptions.EnableSsl);
            await client.AuthenticateAsync(_smtpClientOptions.Email, _smtpClientOptions.Password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Ошибка отправки сообщения");
        }

        return true;
    }
}