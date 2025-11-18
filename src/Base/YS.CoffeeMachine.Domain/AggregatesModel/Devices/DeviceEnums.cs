using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 设备状态
    /// </summary>
    [Flags]
    public enum DeviceStatusEnum
    {
        //[Description("离线")]
        //Offline = 0,
        //[Description("在线")]
        //Online = 1 << 1,
        //[Description("缺料")]
        //Shortage = 1 << 2,
        //[Description("提示")]
        //Prompt = 1 << 3,
        //[Description("故障")]
        //Fault = 1 << 4

        [Description("离线")]
        Offline = 0,
        [Description("在线")]
        Online = 1,
    }

    /// <summary>
    /// 使用场景
    /// </summary>
    public enum UsageScenarioEnum
    {
        [Description("写字楼")]
        OfficeBuilding = 0,
        [Description("餐饮连锁店")]
        RestaurantChains = 1,
        [Description("便利店")]
        ConvenienceStore = 2,
        [Description("星级酒店")]
        StarredHotel = 3,
        [Description("商场")]
        Mall = 4,
        [Description("政府机构")]
        GovernmentOrgans = 5,
        [Description("机场")]
        Airport = 6,
        [Description("培训机构")]
        TrainingInstitutions = 7,
        [Description("咖啡馆")]
        Cafe = 8,
        [Description("火车站")]
        TrainStation = 9,
        [Description("医院")]
        Hospital = 10,
        [Description("加油站")]
        GasStation = 11,
        [Description("产业园")]
        IndustrialPark = 12,
        [Description("超市")]
        Supermarket = 13,
        [Description("其他")]
        Other = 14
    }

    /// <summary>
    /// 设备售卖状态
    /// </summary>
    public enum DeviceSalesStatusEnum
    {
        [Description("待投放")]
        WaitDeployed = 0,
        [Description("启用")]
        Enable = 1,
        [Description("禁用")]
        Disable = 2
    }

    /// <summary>
    /// 报警状态
    /// </summary>
    public enum AlarmStatusEnum
    {
        [Description("报警")]
        Alarm = 0,
        [Description("未报警")]
        NoAlarm = 1
    }

    /// <summary>
    /// 设备分配方式
    /// </summary>
    public enum DeviceAllocationEnum
    {
        /// <summary>
        /// 售卖
        /// </summary>
        [Description("售卖")]
        Sale = 0,
        /// <summary>
        /// 租赁
        /// </summary>
        [Description("租赁")]
        Lease = 1
    }

    /// <summary>
    /// 设备是否出厂
    /// </summary>
    public enum IsLeaveFactoryEnum
    {
        [Description("已出厂")]
        Yes = 0,
        [Description("未出厂")]
        No = 1
    }

    /// <summary>
    /// 设备激活状态
    /// </summary>
    public enum DeviceActiveEnum
    {
        [Description("未激活")]
        NoActive = 0,
        [Description("已激活")]
        Active = 1,
        [Description("已过期")]
        Expire = 2,
    }
}
