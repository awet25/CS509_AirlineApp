using System.Threading.Tasks;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
 

 namespace AppBackend.Services
 {
    
 
public class SendGridEmailService : IEMailService
{      

    private readonly SendGridSettings _settings;

    public SendGridEmailService(IOptions<SendGridSettings>options)
    {
        _settings=options.Value;
    }
    public async Task SendEmail(string toEmail, string subject, string body)
    {
        var client= new SendGridClient(_settings.ApiKey);
        var from=new EmailAddress(_settings.SenderEmail,_settings.SenderName);
        var to= new EmailAddress(toEmail);
        var msg= MailHelper.CreateSingleEmail(from,to,subject,null,body);
        var response= await client.SendEmailAsync(msg);
        if ((int)response.StatusCode>=400)
        {
            var error = await response.Body.ReadAsStringAsync();
            throw new Exception($"Sender error:{error}");
        }
    }
}
 }