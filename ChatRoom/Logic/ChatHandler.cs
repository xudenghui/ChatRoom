using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common.Code;
using ChatRoom.Cache;
using Common.Dto;
using ChatRoom.Model;
using Common.Model;

namespace ChatRoom.Logic
{
    public class ChatHandler : IHandler
    {
        ChatCache cache { get { return Factory.ChatCache; } }

        public void OnDisconnect(MyClient client)
        {
            //群发房间内剩余的客户端 有人离开了
            AccountModel model = cache.Leave(client);
            if (model == null)
                return;

            AccountDto accountDto = new AccountDto()
            { Account = model.Account, Password = model.Password };
            OperationResponse response =
                 new OperationResponse((byte)OpCode.Chat, new Dictionary<byte, object>());
            response.Parameters[80] = ChatCode.Leave;
            //群发给每一个客户端
            response.Parameters[0] = LitJson.JsonMapper.ToJson(accountDto);
            RoomModel room = cache.GetModel();

            response.DebugMessage = "有人离开了";

            foreach (var item in room.clientAccountDict.Keys)
            {
                item.SendOperationResponse(response, new SendParameters());
            }
        }

        public void OnRequest(MyClient client, byte subCode, OperationRequest request)
        {
            switch ((ChatCode)subCode)
            {
                case ChatCode.Enter:
                    enter(client);
                    break;
                case ChatCode.Talk:
                    string text = request.Parameters[0].ToString();
                    Talk(client, text);
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 客户端进入聊天房间
        /// </summary>
        /// <param name="client"></param>
        private void enter(MyClient client)
        {
            OperationResponse response =
                  new OperationResponse((byte)OpCode.Chat, new Dictionary<byte, object>());
            AccountModel account = Factory.AccountCache.GetModel(client);
            RoomModel room = cache.Enter(client, account);
            if (room == null)
            {
                response.DebugMessage = "进入失败.";
                response.ReturnCode = -1;
                client.SendOperationResponse(response, new SendParameters());
                return;
            }
            //成功进入(2个事情)

            //1.告诉他自己 进入成功 给他发送一个房间模型
            RoomDto roomDto = new RoomDto();
            foreach (var model in room.clientAccountDict.Values)
            {
                roomDto.accountList.Add(new AccountDto()
                { Account = model.Account, Password = model.Password });
            }
            response.Parameters[80] = ChatCode.Enter;
            response.Parameters[0] = LitJson.JsonMapper.ToJson(roomDto);
            response.DebugMessage = "进入成功.";
            response.ReturnCode = 0;
            client.SendOperationResponse(response, new SendParameters());

            //2.告诉房间的其他人 有新的客户端连接了
            AccountDto accountDto = new AccountDto()
            { Account = account.Account, Password = account.Password };
            response.Parameters[80] = ChatCode.Add;
            response.Parameters[0] = LitJson.JsonMapper.ToJson(accountDto);
            response.DebugMessage = "新的客户端进入.";
            response.ReturnCode = 1;
            foreach (var item in room.clientAccountDict.Keys)
            {
                if (item == client)
                    continue;

                item.SendOperationResponse(response, new SendParameters());
            }
        }

        /// <summary>
        /// 聊天
        /// </summary>
        /// <param name="client"></param>
        /// <param name="text"></param>
        private void Talk(MyClient client, string text)
        {
            OperationResponse response =
                  new OperationResponse((byte)OpCode.Chat, new Dictionary<byte, object>());
            response.Parameters[80] = ChatCode.Talk;
            AccountModel account = Factory.AccountCache.GetModel(client);
            RoomModel room = cache.GetModel();
            //群发给每一个客户端
            response.Parameters[0] = string.Format("{0} : {1}", account.Account, text);
            foreach (var item in room.clientAccountDict.Keys)
            {
                item.SendOperationResponse(response, new SendParameters());
            }
        }



    }
}
