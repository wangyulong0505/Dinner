using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dinner.Dapper.Helpers
{
    /// <summary>
    /// 加密方法
    /// </summary>
    public class Encrypt
    {
        /// <summary>
        /// 生成32位MD5加密字符串
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static string Md5(string strInput)
        {
            MD5 md5Hash = MD5.Create();
            //获得输入字符的16位字节
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(strInput));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                //循环遍历每一位字节，转为16进制二位输出，然后拼接组成32位字节码
                sb.Append(data[i].ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成16位Md5加密字符串
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static string Md5(string strInput, bool point = false)
        {
            MD5 md5Hash = MD5.Create();
            //获得输入字符的16位字节
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(strInput));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                //循环遍历每一位字节，转为16进制二位输出，然后拼接组成32位字节码 
                //其实16位是在32位的基础上截取的
                sb.Append(data[i].ToString("x2"));
            }
            if (point)
            {
                return sb.Remove(0, 16).ToString();
            }
            else
            {
                return sb.ToString();
            }
        }
    }
}
