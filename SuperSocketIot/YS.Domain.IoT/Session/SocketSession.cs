using SuperSocket.Server;
using YS.CoffeeMachine.Domain.Shared.Const;

namespace YS.Domain.IoT.Session
{
    /// <summary>
    /// SocketSession
    /// </summary>
    public class SocketSession : AppSession
    {
        /// <summary>
        /// 是否登陆
        /// </summary>
        public bool IsLogin { get; set; }

        /// <summary>
        ///1000
        /// </summary>
        public bool IsTmp { get; set; }

        /// <summary>
        /// 生产编号
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 登出
        /// </summary>
        public void LogOut()
        {
            this.Mid = CacheConst.MidDead;
            this.IsLogin = false;
        }
    }
}
