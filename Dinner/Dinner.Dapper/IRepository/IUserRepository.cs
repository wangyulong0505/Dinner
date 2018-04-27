using Dinner.Dapper.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dinner.Dapper.IRepository
{
    public interface IUserRepository : IRepositoryBase<Users> 
    {
        #region 扩展的dapper操作

        //加一个带参数的存储过程
        string ExecExecQueryParamSP(string spName, string name, int Id);

        Task<List<Users>> GetUsers();

        Task PostUser(Users entity);

        Task PutUser(Users entity);

        Task DeleteUser(Guid Id);

        Task<Users> GetUserDetail(Guid Id);

        #endregion
    }
}
