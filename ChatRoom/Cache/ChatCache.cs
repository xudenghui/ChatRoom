using ChatRoom.Model;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Cache
{
    public class ChatCache
    {
        private RoomModel room;

        public ChatCache()
        {
            room = new RoomModel(0);
        }

        /// <summary>
        /// 客户端进入
        /// </summary>
        /// <param name="client"></param>
        public RoomModel Enter(MyClient client, AccountModel model)
        {
            if (room.Contains(client))
                return null;

            room.Add(client, model);
            return room;
        }

        /// <summary>
        /// 用户离开
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public AccountModel Leave(MyClient client)
        {
            if (!room.clientAccountDict.ContainsKey(client))
                return null;

            AccountModel model = room.clientAccountDict[client];
            room.clientAccountDict.Remove(client);
            return model;
        }

        /// <summary>
        /// 获取房间模型
        /// </summary>
        /// <returns></returns>
        public RoomModel GetModel()
        {
            return room;
        }
    }
}
