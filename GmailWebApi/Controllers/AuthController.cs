using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Gmail.v1;

namespace GmailWebApi.Controllers
{
    [Route("authorize")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> OAuthCallback([FromQuery] string? code)
        {
            // Log incoming request
            Console.WriteLine($"Received callback with code: {code}");

            if (string.IsNullOrEmpty(code))
                return BadRequest("Authorization code is missing.");

            // Exchange the authorization code for tokens
            var token = await ExchangeCodeForTokenAsync(code);

            return Ok("OAuth authentication successful!");
        }

        private async Task<TokenResponse> ExchangeCodeForTokenAsync(string code)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = "",
                ClientSecret = ""
            };

            // Create the authorization flow
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = new[] { GmailService.Scope.GmailSend }, // Adjust scopes as necessary
            });

            // Exchange the authorization code for a token
            var tokenResponse = await flow.ExchangeCodeForTokenAsync(
                "user", // Use the identifier for the user
                code,
                "https://localhost:5001/oauth2callback", // Must match the redirect URI registered in Google Cloud Console
                CancellationToken.None
            );

            //var tokenResponse = await tokenRequest.ExecuteAsync();
            return tokenResponse;
        }

    }
}
