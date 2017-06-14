using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotonHostRuntimeInterfaces;
using ChatRoom.Logic;
using Common.Code;

namespace ChatRoom
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class MyClient : ClientPeer
    {
        //账号
        AccountHandler account;
        //聊天
        ChatHandler chat;

        public MyClient(InitRequest initRequest)
            : base(initRequest)
        {
            account = new AccountHandler();
            chat = new ChatHandler();
        }


        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="reasonCode"></param>
        /// <param name="reasonDetail"></param>
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            chat.OnDisconnect(this);
            account.OnDisconnect(this);
        }


        /// <summary>
        /// 客户发起请求
        /// </summary>
        /// <param name="operationRequest"></param>
        /// <param name="sendParameters"></param>
        protected override void OnOperationRequest(OperationRequest request, SendParameters sendParameters)
        {
            switch ((OpCode)request.OperationCode)
            {
                case OpCode.Account:
                    account.OnRequest(this, (byte)request.Parameters[80], request);
                    break;
                case OpCode.Chat:
                    chat.OnRequest(this, (byte)request.Parameters[80], request);
                    break;
                default:
                    break;
            }
        }

    }
}
