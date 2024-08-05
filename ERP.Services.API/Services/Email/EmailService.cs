using System.Reflection;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using User.Api.Application.Services;

namespace ERP.Services.API.Services.Email;

public class MailConfig
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public string From { get; set; }
    public string Port { get; set; }
}

public class  EmailService : IEmailService
{
    private readonly IWebHostEnvironment _environment;
    private readonly MailConfig _mailConfig;

    public EmailService(IOptions<MailConfig> config, IWebHostEnvironment environment)
    {
        _environment = environment;
        _mailConfig = config.Value;
    }


    public async Task Send<T>(string to, string subject, string template, T data, string from = null)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(from ?? _mailConfig.From));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;


        // Get mail template
        string fullFormatPath = Path.Combine(_environment.WebRootPath, "Content", template);
        string[] imgPaths = Directory.GetFiles(Path.Combine(fullFormatPath, "Resources"));

        string htmlFormat;

        var builder = new BodyBuilder();

        using (FileStream fs = new FileStream(Path.Combine(fullFormatPath, "template.html"), FileMode.Open))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                htmlFormat = await sr.ReadToEndAsync();
            }
        }

        // Add pictures to embedded resources and replace links to pictures in the message
        foreach (string imgpath in imgPaths)
        {
            var image = await builder.LinkedResources.AddAsync(imgpath);
            image.ContentId = MimeUtils.GenerateMessageId();
            htmlFormat = htmlFormat.Replace(Path.GetFileName(imgpath), string.Format("cid:{0}", image.ContentId));
        }

        Type myType = data.GetType();
        List<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

        foreach (PropertyInfo prop in props)
        {
            string propValue = prop.GetValue(data, null)?.ToString();

            htmlFormat = htmlFormat.Replace("{{" + prop.Name.ToLowerInvariant() + "}}",
                propValue);
        }

        builder.HtmlBody = htmlFormat;

        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailConfig.Host, Convert.ToInt32(_mailConfig.Port), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailConfig.Username, _mailConfig.Password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}