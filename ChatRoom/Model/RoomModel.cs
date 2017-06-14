using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Model
{
    public class RoomModel
    {
        public int Id;
        /// <summary>
        /// 房间内客户端和用户信息的映射
        /// </summary>
        public Dictionary<MyClient, AccountModel> clientAccountDict;

        public RoomModel(int id)
        {
            this.Id = id;
            clientAccountDict = new Dictionary<MyClient, AccountModel>();
        }

        /// <summary>
        /// 是否包含客户端
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool Contains(MyClient client)
        {
            return clientAccountDict.ContainsKey(client);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="client"></param>
        /// <param name="model"></param>
        public void Add(MyClient client, AccountModel model)
        {
            clientAccountDict.Add(client, model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="client"></param>
        public void Remove(MyClient client)
        {
            clientAccountDict.Remove(client);
        }
    }
}
