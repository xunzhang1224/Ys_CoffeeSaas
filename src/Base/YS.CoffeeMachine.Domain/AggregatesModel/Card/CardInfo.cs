using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Card
{
    /// <summary>
    /// 卡管理
    /// </summary>
    public class CardInfo : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNumber { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; private set; } = true;

        /// <summary>
        /// 绑定的设备
        /// </summary>
        public List<CardDeviceAssignment> Assignments { get; private set; }

        private CardInfo() { }

        /// <summary>
        /// 新增卡
        /// </summary>
        /// <param name="cardNumber"></param>
        public CardInfo(string cardNumber)
        {
            CardNumber = cardNumber;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="cardNumber"></param>
        public void UpdateCardNumber(string cardNumber)
        {
            CardNumber = cardNumber;
        }

        /// <summary>
        /// 绑定设备（先清除所有已有绑定，再绑定指定设备列表）
        /// </summary>
        /// <param name="deviceIds">要绑定的设备ID列表</param>
        public void AssignDevices(List<long> deviceIds)
        {
            // 检查是否有重复的设备ID
            var distinctDeviceIds = deviceIds.Distinct().ToList();
            Assignments.Clear();
            foreach (var deviceId in deviceIds)
            {
                var assignment = new CardDeviceAssignment(Id, deviceId);
                Assignments.Add(assignment);
            }
        }

        /// <summary>
        /// 启用/禁用
        /// </summary>
        public void UpdateIsEnable(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// 检查绑定是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return IsEnabled;
        }
    }
}
