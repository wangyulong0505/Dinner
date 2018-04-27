using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Dinner.WebApi
{
    public class MyTokenValidate : ISecurityTokenValidator
    {
        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken(string securityToken)
        {
            return true;
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {

            ClaimsPrincipal principal = null;
            validatedToken = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(securityToken))
                {
                    if (securityToken.Contains("Bearer"))
                    {
                        int temp = securityToken.IndexOf("Bearer") + 6;
                        securityToken = securityToken.Substring(temp, securityToken.Length - temp).TrimStart();
                    }
                    //这里需要验证生成的Token
                    var token = new JwtSecurityToken(securityToken);
                    //获取到Token的一切信息
                    var payload = token.Payload;
                    var role = (from t in payload where t.Key == ClaimTypes.Role select t.Value).FirstOrDefault();
                    var name = (from t in payload where t.Key == ClaimTypes.Name select t.Value).FirstOrDefault();
                    var issuer = token.Issuer;
                    var key = token.SecurityKey;
                    var audience = token.Audiences;
                    var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, name.ToString()));
                    identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
                    principal = new ClaimsPrincipal(identity);
                }
            }
            catch
            {
                validatedToken = null;                 
                principal = null;
            }
            return principal;
        }
    }
}
