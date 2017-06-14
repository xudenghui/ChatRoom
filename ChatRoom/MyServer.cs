using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;

namespace ChatRoom
{
    public class MyServer : ApplicationBase
    {
        private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="str"></param>
        public static void Log(string str)
        {
            log.Info(str.ToString());
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="initRequest"></param>
        /// <returns></returns>
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new MyClient(initRequest);
        }

        /// <summary>
        /// 服务器启动时调用的方法
        /// </summary>
        protected override void Setup()
        {
            InitLogging();
        }

        protected virtual void InitLogging()
        {
            ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            GlobalContext.Properties["LogFileName"] = "My" + this.ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));
        }

        /// <summary>
        /// 服务器停止时调用的方法
        /// </summary>
        protected override void TearDown()
        {

        }
    }
}
