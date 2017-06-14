using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Cache
{
    public class Factory
    {
        public static AccountCache AccountCache = null;
        public static ChatCache ChatCache = null;

        static Factory()
        {
            AccountCache = new AccountCache();
            ChatCache = new ChatCache();
        }



    }
}
