using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Text;
using MimeKit;
using System.IO;

namespace GmailWebApi.Helpers
{
    public class GmailServiceHelper
    {
        private static readonly string[] Scopes = { GmailService.Scope.GmailSend };
        private const string ApplicationName = "WebGmail";

        private static async Task<UserCredential> GetCredentialsAsync()
        {
            using var stream = new FileStream("googleusercontent.json", FileMode.Open, FileAccess.Read);
                var st = Scopes;
            var secrets = GoogleClientSecrets.FromStream(stream).Secrets;

            
                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("Gmail.Auth.Store"));
        }

        public static async Task SendEmailAsync(string to, string subject, string body)
        {
            var credential = await GetCredentialsAsync();
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Md Hasan Monsur", "hasanmcse@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = body };

            var rawMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(emailMessage.ToString()))
                              .Replace('+', '-')
                              .Replace('/', '_')
                              .Replace("=", "");

            var message = new Message { Raw = rawMessage };
            await service.Users.Messages.Send(message, "me").ExecuteAsync();
        }
    }
}
