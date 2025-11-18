using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared
{
    /// <summary>
    /// 模板
    /// </summary>
    public enum TemplateEnum
    {
        /// <summary>
        /// 预警通知
        /// </summary>
        EmailSubject,

        /// <summary>
        /// 故障通知
        /// </summary>
        EmailErrSubject,

        /// <summary>
        /// 【Yunshu】预警通知：位于{0}地区的设备"{1}"（ID：{2}），于 {3} 离线，请及时处理
        /// </summary>
        OfflineTemplate,

        /// <summary>
        /// 【Yunshu】预警通知：位于{0}地区的设备"{1}"（ID：{2}），于 {3} {4}库存告急，请及时补货确保正常运营
        /// </summary>
        ShortageTemplate,

        /// <summary>
        /// 【Yunshu】故障通知：位于{0}地区的设备"{1}"（ID：{2}），于 {3} 出现{4}故障，请及时处理
        /// </summary>
        ErrTemplate,

        /// <summary>
        /// 【Yunshu】清洗预警：您位于 {0} 的设备 “{1}”(ID: {2})，{3}于 {4} 使用次数达预警值，需进行清洗，请及时处理
        /// </summary>
        FlushTemplate,

        /// <summary>
        /// 【Yunshu】预警通知：位于{0}地区的设备"{1}"（ID：{2}），于 {3} 离线，请及时处理
        /// 短信
        /// </summary>
        SmsOfflineTemplate,

        /// <summary>
        /// 【Yunshu】预警通知：位于{0}地区的设备"{1}"（ID：{2}），于 {3} {4}库存告急，请及时补货确保正常运营
        /// 短信
        /// </summary>
        SmsShortageTemplate
    }
}
