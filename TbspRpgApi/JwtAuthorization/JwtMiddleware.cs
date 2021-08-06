using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TbspRpgApi.JwtAuthorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtHelper _jwtHelper;


        public JwtMiddleware(RequestDelegate next, IJwtSettings jwtSettings)
        {
            _next = next;
            _jwtHelper = new JwtHelper(jwtSettings.Secret);
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var userId = _jwtHelper.ValidateToken(token);
                if (Guid.TryParse(userId, out var guidUserId))
                {
                    // attach user to context on successful jwt validation
                    context.Items["UserId"] = guidUserId;    
                }
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}