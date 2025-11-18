namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 表示设备地理位置信息的值对象结构体。
    /// 用于封装设备所在的具体地理坐标、国家地区信息及详细地址。
    /// </summary>
    public struct DevicePositionVoInfo
    {
        /// <summary>
        /// 获取或设置经度，默认值为中国北京标准经度（116.4074）。
        /// 经度范围：-180 到 180。
        /// </summary>
        public double Longitude { get; private set; } = 116.4074;

        /// <summary>
        /// 获取或设置纬度，默认值为中国北京标准纬度（39.9042）。
        /// 纬度范围：-90 到 90。
        /// </summary>
        public double Latitude { get; private set; } = 39.9042;

        /// <summary>
        /// 获取或设置国家唯一标识符（ID），用于关联国家信息。
        /// </summary>
        public long CountryId { get; private set; }

        /// <summary>
        /// 获取或设置地区 ID 集合，表示该设备所属的多个地区层级。
        /// 格式通常为逗号分隔的字符串。
        /// </summary>
        public string CountryRegionIds { get; private set; }

        /// <summary>
        /// 获取或设置国家/地区名称组合文本，用于展示设备所在区域。
        /// </summary>
        public string CountryRegionText { get; private set; }

        /// <summary>
        /// 获取或设置设备的详细地址信息，如街道、建筑编号等。
        /// </summary>
        public string DetailedAddress { get; private set; }

        /// <summary>
        /// 使用指定参数初始化一个新的 DevicePositionVoInfo 实例。
        /// </summary>
        /// <param name="countryId">国家唯一标识符。</param>
        /// <param name="countryRegionIds">地区 ID 集合。</param>
        /// <param name="detailedAddress">详细地址信息。</param>
        /// <param name="countryRegionText">国家/地区名称组合文本。</param>
        public DevicePositionVoInfo(long countryId, string countryRegionIds, string detailedAddress, string countryRegionText)
        {
            CountryId = countryId;
            CountryRegionIds = countryRegionIds;
            DetailedAddress = detailedAddress;
            CountryRegionText = countryRegionText;
        }

        /// <summary>
        /// 更新当前实例的经纬度，并进行有效性校验。
        /// </summary>
        /// <param name="latitude">新的纬度值。</param>
        /// <param name="longitude">新的经度值。</param>
        /// <exception cref="ArgumentOutOfRangeException">当纬度或经度超出合法范围时抛出异常。</exception>
        /// <exception cref="ArgumentException">当国家 ID 为空或无效时抛出异常。</exception>
        public void SetLatitudelongitude(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentOutOfRangeException(nameof(latitude));

            if (longitude < -180 || longitude > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude));

            if (CountryId == 0)
                throw new ArgumentException("Country cannot be empty");

            Latitude = latitude;
            Longitude = longitude;
        }
    }
}