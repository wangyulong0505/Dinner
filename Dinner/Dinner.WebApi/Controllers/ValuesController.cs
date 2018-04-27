using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Dinner.WebApi.Models;
using Microsoft.Extensions.Options;

namespace Dinner.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : Controller
    {
        private readonly JwtSettings setting;
        public ValuesController(IOptions<JwtSettings> _setting)
        {
            setting = _setting.Value;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        public IActionResult GetGenerateJWT()
        {
            try
            {
                var claims = new Claim[] 
                {
                    new Claim(ClaimTypes.Name, "wangshibang"),
                    new Claim(ClaimTypes.Role, "admin, Manage")
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    setting.Issuer,
                    setting.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    creds);
                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }   
            
        }
    }
}
