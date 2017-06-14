using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Logic
{
    public interface IHandler
    {
        /// <summary>
        /// 客户端发起请求
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="subCode">子操作</param>
        /// <param name="request">请求</param>
        void OnRequest(MyClient client, byte subCode, OperationRequest request);

        /// <summary>
        /// 客户端下线
        /// </summary>
        /// <param name="client"></param>
        void OnDisconnect(MyClient client);

    }
}
