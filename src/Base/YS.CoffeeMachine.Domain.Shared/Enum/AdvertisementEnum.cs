using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 广告枚举
    /// </summary>
    public enum AdvertisementEnum
    {
        [Description("半屏广告")]
        HalfScreen = 0,
        [Description("全屏广告")]
        FullScreen = 1,
    }
}
