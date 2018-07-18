using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Configuration;
using System.Security.Cryptography;

namespace 人事管理系统
{
    class 窗口
    {
        //储存全局变量
        public static 登录 登录 = null;
        public static 员工自助查询 员工自助查询 = null;
        public static 请假 请假 = null;
        public static 员工专用.报告错误 报告错误 = null;
        public static 连接 连接 = null;
        public static 等待 等待 = null;
        public static 人事管理系统.人事专用.收到错误反馈 收到错误反馈 = null;
        public static 人事管理系统.人事专用.人事选择进入界面 人事选择进入界面 = null;
        public static 修改密码 修改密码 = null;
        public static 人事专用.新建部门 新建部门 = null;
        public static 人事专用.删除 删除 = null;
        public static 人事专用.请假信息 请假信息 = null;
        public static 设置 设置 = null;
        public static 人事专用.录入 录入 = null;
        public static 人事专用.更新.更新详细信息 更新详细信息 = null;
        public static 人事专用.查询员工个人信息 查询员工个人信息 = null;
        public static 人事专用.编辑公告 编辑公告 = null;
        public static OleDbConnection conn = new OleDbConnection("");
        public static int leave_count = 0;
        public static int error_count = 0;
        public static string ID = "";
        public static string ip = "";
        public static string add = "";
        //创建文件
        public static void newfile()
        {
            FileStream fs = new FileStream(Application.StartupPath + "\\x", FileMode.OpenOrCreate);
            fs.Close();
        }
        //设置config文件
        public static void SetValue(string key, string value)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
        }
        //获取config的值
        public static string GetValue(string key)
        {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] == null)
                    return "";
                else
                    return config.AppSettings.Settings[key].Value;
        }
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /**//**//**//// <summary>
                    /// DES加密字符串 
                    /// </summary> 
                    /// <param name="encryptString">待加密的字符串</param> 
                    /// <param name="encryptKey">加密密钥,要求为8位</param> 
                    /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /**//**//**//// <summary>
                    /// DES解密字符串 
                    /// </summary> 
                    /// <param name="decryptString">待解密的字符串</param> 
                    /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param> 
                    /// <returns>解密成功返回解密后的字符串，失败返源串</returns> 
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
    }
}