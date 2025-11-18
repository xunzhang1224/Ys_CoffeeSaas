using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.IServices
{
    /// <summary>
    /// 密码哈希处理行为的接口
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// 创建随机密码
        /// </summary>
        /// <returns></returns>
        string CreateRandomPassword();
        /// <summary>
        /// 生成哈希密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string HashPassword(string password);

        /// <summary>
        /// 返回密码比较的结果
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="providedPassword"></param>
        /// <returns></returns>
        PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}
