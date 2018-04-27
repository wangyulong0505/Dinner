using Dinner.Dapper.Entities;
using Dinner.Dapper.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dinner.WebApi.Controllers
{

    [Route("api/[controller]/[action]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        public UsersController(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<JsonResult> GetUsers()
        {
            List<Users> list = await userRepository.GetUsers();
            return Json(list);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task PostUser(Users entity)
        {
            entity.Password = Dapper.Helpers.Encrypt.Md5(entity.Password).ToUpper();
            await userRepository.PostUser(entity);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task PutUser(Users entity)
        {
            try
            {
                entity.Password = Dapper.Helpers.Encrypt.Md5(entity.Password).ToUpper();
                await userRepository.PutUser(entity);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteUser(Guid Id)
        {
            try
            {
                await userRepository.DeleteUser(Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}