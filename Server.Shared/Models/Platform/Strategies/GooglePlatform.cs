using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Models.Platform.Strategies
{
    internal class GooglePlatform : IUserPlatform
    {
        public async Task<GoogleJsonWebSignature.Payload> VerifyToken(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { "_webClientId" }
            };

            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
    }
}
