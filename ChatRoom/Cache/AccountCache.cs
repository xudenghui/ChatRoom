using ChatRoom.Model;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Cache
{
    public class AccountCache
    {
        #region 注册
        /// <summary>
        /// 账号Id和模型的映射
        /// </summary>
        private Dictionary<int, AccountModel> idModelDict;
        //private int index = 0;

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="acc">账号</param>
        /// <param name="pwd">密码</param>
        public void Add(string acc, string pwd)
        {
            //不使用数据库
            //idModelDict.Add(index, new AccountModel(index, acc, pwd));
            //index++;
            //存到数据库
            AccountModel model = new AccountModel();
            model.Account = acc;
            model.Password = pwd;
            model.Id = model.Add();
            //存到内存
            idModelDict.Add(model.Id, model);
        }

        /// <summary>
        /// 是否有相同账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool HasAccount(string account)
        {
            foreach (AccountModel m in idModelDict.Values)
            {
                if (account == m.Account)
                    return true;
            }
            return false;
            //没有 -> 访问数据库
            //AccountModel model = new AccountModel();
            //if (!model.Exists(account))
            //    return false;
            ////有就获取 添加到内存中
            //model.GetModel(account);
            //idModelDict.Add(model.Id, model);
            //return true;
        }

        /// <summary>
        /// 检测账号密码是否匹配
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsMatch(string account, string password)
        {
            //访问内存
            foreach (AccountModel m in idModelDict.Values)
            {
                if (m.Account == account && m.Password == password)
                    return true;
            }
            ////使用内存
            //return false;
            //没有 -> 访问数据库
            AccountModel model = new AccountModel();
            if (!model.Exists(account))
                return false;
            //有就获取 添加到内存中
            model.GetModel(account);
            MyServer.Log(model.Account+" ; "+model.Password);
            idModelDict.Add(model.Id, model);
            return true;
        }

        #endregion

        #region 登录

        /// <summary>
        /// 在线玩家客户端和账号模型的映射
        /// </summary>
        private Dictionary<MyClient, AccountModel> clientModelDict;

        /// <summary>
        /// 玩家是否在线
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsOnline(MyClient client)
        {
            return clientModelDict.ContainsKey(client);
        }

        /// <summary>
        /// 玩家上线
        /// </summary>
        /// <param name="client"></param>
        public void Online(MyClient client, string acc, string pwd)
        {
            foreach (AccountModel model in idModelDict.Values)
            {
                if (model.Account == acc && model.Password == pwd)
                    clientModelDict.Add(client, model);
            }
        }

        /// <summary>
        /// 玩家下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(MyClient client)
        {
            clientModelDict.Remove(client);
        }

        /// <summary>
        /// 获取模型
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public AccountModel GetModel(MyClient client)
        {
            AccountModel model = null;
            clientModelDict.TryGetValue(client, out model);
            return model;
        }

        #endregion

        public AccountCache()
        {
            idModelDict = new Dictionary<int, AccountModel>();
            clientModelDict = new Dictionary<MyClient, AccountModel>();
        }

    }
}
