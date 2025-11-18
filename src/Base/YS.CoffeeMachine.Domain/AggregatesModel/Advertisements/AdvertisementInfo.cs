using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Advertisements
{
    /// <summary>
    /// 广告信息聚合根
    /// </summary>
    public class AdvertisementInfo : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceInfoId { get; private set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public DeviceInfo DeviceInfo { get; private set; }

        #region 开机广告

        /// <summary>
        /// 开机广告音量
        /// </summary>
        public int PowerOnAdsVolume { get; private set; }

        /// <summary>
        /// 开机广告播放时间
        /// </summary>
        public int PowerOnAdsPlayTime { get; private set; }

        /// <summary>
        /// 开机广告内容（图片集合）
        /// </summary>
        public string PowerOnAdsImagesJson { get; private set; }
        #endregion

        #region 待机广告

        /// <summary>
        /// 待机广告音量
        /// </summary>
        public int StandbyAdsVolume { get; private set; }

        /// <summary>
        /// 待机广告播放时间
        /// </summary>
        public int StandbyAdsPlayTime { get; private set; }

        /// <summary>
        /// 待机广告等待时间
        /// </summary>
        public int StandbyAdsAwaitTime { get; private set; }

        /// <summary>
        /// 待机广告间隔时间
        /// </summary>
        public int StandbyAdsLoopTime { get; private set; }

        /// <summary>
        /// 待机广告循环类型（true:无限循环,false:播放自动退出）
        /// </summary>
        public bool StandbyAdsLoopType { get; private set; }

        /// <summary>
        /// 待机广告内容
        /// </summary>
        public string StandbyAdsImagesJson { get; private set; }
        #endregion

        #region 饮品制作广告
        /// <summary>
        /// 饮品制作广告音量
        /// </summary>
        public int ProductionAdsVolume { get; private set; }

        /// <summary>
        /// 饮品制作广告播放时间
        /// </summary>
        public int ProductionAdsPlayTime { get; private set; }

        /// <summary>
        /// 饮品制作广告内容
        /// </summary>
        public string ProductionAdsImagesJson { get; private set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        protected AdvertisementInfo() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deviceInfoId">设备Id</param>
        /// <param name="powerOnAdsVolume">开机广告音量</param>
        /// <param name="powerOnAdsPlayTime">开机广告播放时间</param>
        /// <param name="powerOnAdsImagesJson">开机广告内容（图片集合）</param>
        /// <param name="standbyAdsVolume">待机广告音量</param>
        /// <param name="standbyAdsPlayTime">待机广告播放时间</param>
        /// <param name="standbyAdsAwaitTime">待机广告等待时间</param>
        /// <param name="standbyAdsLoopTime">待机广告间隔时间</param>
        /// <param name="standbyAdsLoopType">待机广告循环类型（true:无限循环,false:播放自动退出）</param>
        /// <param name="standbyAdsImagesJson">待机广告内容</param>
        /// <param name="productionAdsVolume">饮品制作广告音量</param>
        /// <param name="productionAdsPlayTime">饮品制作广告播放时间</param>
        /// <param name="productionAdsImagesJson">饮品制作广告内容</param>
        public AdvertisementInfo(long deviceInfoId, int powerOnAdsVolume, int powerOnAdsPlayTime, string powerOnAdsImagesJson, int standbyAdsVolume, int standbyAdsPlayTime, int standbyAdsAwaitTime, int standbyAdsLoopTime, bool standbyAdsLoopType, string standbyAdsImagesJson, int productionAdsVolume, int productionAdsPlayTime, string productionAdsImagesJson)
        {
            DeviceInfoId = deviceInfoId;
            PowerOnAdsVolume = powerOnAdsVolume;
            PowerOnAdsPlayTime = powerOnAdsPlayTime;
            PowerOnAdsImagesJson = powerOnAdsImagesJson;
            StandbyAdsVolume = standbyAdsVolume;
            StandbyAdsPlayTime = standbyAdsPlayTime;
            StandbyAdsAwaitTime = standbyAdsAwaitTime;
            StandbyAdsLoopTime = standbyAdsLoopTime;
            StandbyAdsLoopType = standbyAdsLoopType;
            StandbyAdsImagesJson = standbyAdsImagesJson;
            ProductionAdsVolume = productionAdsVolume;
            ProductionAdsPlayTime = productionAdsPlayTime;
            ProductionAdsImagesJson = productionAdsImagesJson;
        }

        /// <summary>
        /// 更新广告信息
        /// </summary>
        /// <param name="powerOnAdsVolume">开机广告音量</param>
        /// <param name="powerOnAdsPlayTime">开机广告播放时间</param>
        /// <param name="powerOnAdsImagesJson">开机广告内容（图片集合）</param>
        /// <param name="standbyAdsVolume">待机广告音量</param>
        /// <param name="standbyAdsPlayTime">待机广告播放时间</param>
        /// <param name="standbyAdsAwaitTime">待机广告等待时间</param>
        /// <param name="standbyAdsLoopTime">待机广告间隔时间</param>
        /// <param name="standbyAdsLoopType">待机广告循环类型（true:无限循环,false:播放自动退出）</param>
        /// <param name="standbyAdsImagesJson">待机广告内容</param>
        /// <param name="productionAdsVolume">饮品制作广告音量</param>
        /// <param name="productionAdsPlayTime">饮品制作广告播放时间</param>
        /// <param name="productionAdsImagesJson">饮品制作广告内容</param>
        public void Update(int powerOnAdsVolume, int powerOnAdsPlayTime, string powerOnAdsImagesJson, int standbyAdsVolume, int standbyAdsPlayTime, int standbyAdsAwaitTime, int standbyAdsLoopTime, bool standbyAdsLoopType, string standbyAdsImagesJson, int productionAdsVolume, int productionAdsPlayTime, string productionAdsImagesJson)
        {
            PowerOnAdsVolume = powerOnAdsVolume;
            PowerOnAdsPlayTime = powerOnAdsPlayTime;
            PowerOnAdsImagesJson = powerOnAdsImagesJson;
            StandbyAdsVolume = standbyAdsVolume;
            StandbyAdsPlayTime = standbyAdsPlayTime;
            StandbyAdsAwaitTime = standbyAdsAwaitTime;
            StandbyAdsLoopTime = standbyAdsLoopTime;
            StandbyAdsLoopType = standbyAdsLoopType;
            StandbyAdsImagesJson = standbyAdsImagesJson;
            ProductionAdsVolume = productionAdsVolume;
            ProductionAdsPlayTime = productionAdsPlayTime;
            ProductionAdsImagesJson = productionAdsImagesJson;
        }
    }
}
