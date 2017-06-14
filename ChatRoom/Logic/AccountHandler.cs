using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common.Code;
using ChatRoom.Cache;
using Common.Dto;
using LitJson;

namespace ChatRoom.Logic
{
    public class AccountHandler : IHandler
    {

        AccountCache cache = Factory.AccountCache;

        public void OnDisconnect(MyClient client)
        {
            cache.Offline(client);
        }

        public void OnRequest(MyClient client, byte subCode, OperationRequest request)
        {
            AccountDto dto = JsonMapper.ToObject<AccountDto>(request.Parameters[0].ToString());
            switch ((AccountCode)subCode)
            {
                case AccountCode.Login:
                    login(client, dto);
                    break;
                case AccountCode.Register:
                    register(client, dto);
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 注册处理
        /// </summary>
        private void register(MyClient client, AccountDto account)
        {
            OperationResponse response =
                new OperationResponse((byte)OpCode.Account, new Dictionary<byte, object>());
            response.Parameters[80] = AccountCode.Register;
            if (cache.HasAccount(account.Account))
            {
                response.DebugMessage = "账号已存在.";
                response.ReturnCode = -1;
                client.SendOperationResponse(response, new SendParameters());
                return;
            }
            else
            {
                cache.Add(account.Account, account.Password);

                response.DebugMessage = "注册成功.";
                response.ReturnCode = 0;
                client.SendOperationResponse(response, new SendParameters());
            }
        }


        /// <summary>
        /// 登录处理
        /// </summary>
        private void login(MyClient client, AccountDto account)
        {
            OperationResponse response =
                  new OperationResponse((byte)OpCode.Account, new Dictionary<byte, object>());
            response.Parameters[80] = AccountCode.Login;
            if (!cache.IsMatch(account.Account, account.Password))
            {
                response.DebugMessage = "账号密码不匹配.";
                response.ReturnCode = -1;
                client.SendOperationResponse(response, new SendParameters());
                return;
            }
            else if (cache.IsOnline(client))
            {
                response.DebugMessage = "玩家在线.";
                response.ReturnCode = -2;
                client.SendOperationResponse(response, new SendParameters());
                return;
            }
            else
            {
                cache.Online(client, account.Account, account.Password);
                response.DebugMessage = "登陆成功.";
                response.ReturnCode = 0;
                client.SendOperationResponse(response, new SendParameters());
            }
        }

    }
}
