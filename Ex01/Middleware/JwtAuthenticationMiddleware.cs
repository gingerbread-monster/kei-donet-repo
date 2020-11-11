using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace Ex01.Middleware
{
    /// <summary>
    /// Middleware компонент который отвечает за считываение и валидацию
    /// JWT из заголовка клиентского запроса.
    /// </summary>
    public class JwtAuthenticationMiddleware
    {
        readonly RequestDelegate _next;

        readonly AppJwtData _appJwtData;

        static readonly string _authorizationHeaderName = "Authorization";

        public JwtAuthenticationMiddleware(
            RequestDelegate next,
            AppJwtData appJwtData)
        {
            _next = next;
            _appJwtData = appJwtData;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            bool containsAuthHeader = context.Request.Headers.ContainsKey(_authorizationHeaderName);

            if (containsAuthHeader)
            {
                string authHeader = context.Request.Headers[_authorizationHeaderName];

                string jwt = GetJwt(authHeader);

                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken validatedToken = null;

                try
                {
                    tokenHandler.ValidateToken(
                        token: jwt,
                        validationParameters: GetTokenValidationParameters(),
                        validatedToken: out validatedToken);
                }
                catch (Exception)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }

                if (context.Response.StatusCode == StatusCodes.Status200OK)
                {
                    await _next.Invoke(context);
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
        }

        /// <summary>
        /// Выделяет JWT из значения заголовка 'Authorization'.
        /// </summary>
        /// <param name="rawAuthHeaderValue">Необработанное значение заголовка 'Authorization'.</param>
        /// <returns>Значение JWT, либо null если аргумент имеет не валидное значение.</returns>
        string GetJwt(string rawAuthHeaderValue)
        {
            // Валидация размера JWT хедера
            int minRawAuthLength = 10;
            if (string.IsNullOrWhiteSpace(rawAuthHeaderValue) ||
                rawAuthHeaderValue.Length < minRawAuthLength)
                return null;

            // Удаляем приставку 'Bearer' и возвращаем готовый JWT
            return rawAuthHeaderValue.Replace("Bearer ", string.Empty);
        }

        /// <summary>
        /// Возращает объект <see cref="TokenValidationParameters"/> c
        /// установленными параметрами для валидации JWT.
        /// </summary>
        TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_appJwtData.SecretKey)),

                ValidateIssuer = true,
                ValidIssuer = _appJwtData.Issuer,

                ValidateAudience = true,
                ValidAudience = _appJwtData.Audience,

                ValidateLifetime = false
            };
        }
    }
}
