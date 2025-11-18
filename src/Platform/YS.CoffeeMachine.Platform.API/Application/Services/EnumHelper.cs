using System.ComponentModel;
using System.Reflection;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.Services
{
    /// <summary>
    /// 获取项目所有枚举信息
    /// </summary>
    public class EnumHelper : IEnumHelper
    {
        /// <summary>
        /// 获取项目所有枚举信息
        /// </summary>
        /// <returns></returns>
        public Task<List<EnumInfo>> GetAllEnumInfos()
        {
            var enumInfos = new List<EnumInfo>();
            IEnumerable<Type> enumTypes = [typeof(AccountTypeEnum), typeof(UserStatusEnum), typeof(RoleStatusEnum), typeof(RoleTypeEnum), typeof(MenuTypeEnum),typeof(SysMenuTypeEnum),
                typeof(DeviceStatusEnum), typeof(DeviceSalesStatusEnum), typeof(AlarmStatusEnum),typeof(DeviceAllocationEnum),typeof(ServiceAuthorizationStateEnum),typeof(IsEnabledEnum),typeof(BeverageAppliedType),typeof(ReplaceContentType),
            typeof(UsageScenarioEnum),typeof(FormulaTypeEnum),typeof(TemperatureEnum),typeof(WashEnum),typeof(OperationPermissionEnum),typeof(DebuggingPermissionEnum),typeof(SetUpPermissionEnum),typeof(WeekEnum),typeof(WaterSupplyMethodEnum),
            typeof(OrderShipmentResult),typeof(OrderSaleResult),typeof(PaymentEnum),typeof(PayTypeEnum),typeof(PaymentConfigStatueEnum),typeof(PaymentCurrencyLocatonEnum),typeof(DocmentTypeEnum),typeof(EnterpriseOrganizationTypeEnum),typeof(RegistrationProgress)];
            foreach (var enumType in enumTypes)
            {
                if (enumType.IsEnum)  // 确保是枚举类型
                {
                    var enumInfo = new EnumInfo
                    {
                        EnumTypeName = enumType.Name,
                        Values = Enum.GetValues(enumType)  // 获取枚举的所有值
                            .Cast<Enum>()  // 将返回的值转换为 Enum 类型
                            .Select(e => new EnumValue
                            {
                                Name = e.ToString(),
                                Value = Convert.ToInt64(e),  // 将值转换为 long 类型
                                Description = GetEnumDescription(e)  // 获取枚举值的 Description
                            })
                            .ToList()
                    };
                    enumInfos.Add(enumInfo);
                }
            }
            return Task.FromResult(enumInfos);
        }
        // 获取枚举值的 Description 特性值
        private static string GetEnumDescription(Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return enumValue.ToString();
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();  // 获取 Description 特性
            return attribute?.Description ?? enumValue.ToString();  // 如果没有 Description 特性，则返回枚举值的名称
        }
        /// <summary>
        /// 根据Description获取枚举值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="description">枚举的Description</param>
        /// <returns>匹配的枚举值</returns>
        /// <exception cref="ArgumentException">当Description未找到匹配的枚举值时抛出</exception>
        public static TEnum GetEnumByDescription<TEnum>(string description) where TEnum : Enum
        {
            var type = typeof(TEnum);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null && attribute.Description == description)
                {
                    return (TEnum)field.GetValue(null);
                }
            }
            throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0073)], description);
        }

        /// <summary>
        /// 获取枚举Description集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetEnumDescriptions<T>() where T : Enum
        {
            var descriptions = new List<string>();

            // 获取枚举类型
            var enumType = typeof(T);

            // 获取所有枚举字段
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                // 获取字段上的 Description 特性
                var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null)
                {
                    // 将 Description 添加到集合中
                    descriptions.Add(descriptionAttribute.Description);
                }
            }

            return descriptions;
        }

        /// <summary>
        /// 根据字符串获取枚举值（支持枚举名和DescriptionAttribute）
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="value">要转换的字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <param name="throwIfNotFound">找不到时是否抛出异常</param>
        /// <returns>对应的枚举值</returns>
        public static TEnum ParseFromString<TEnum>(string value, bool ignoreCase = true, bool throwIfNotFound = false) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (throwIfNotFound)
                    throw new ArgumentException("输入字符串不能为空", nameof(value));
                return default;
            }

            // 1. 先尝试用枚举名解析
            if (Enum.TryParse(value, ignoreCase, out TEnum result))
            {
                return result;
            }

            // 2. 遍历 DescriptionAttribute
            foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description;
                if (!string.IsNullOrEmpty(description) && string.Equals(description, value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                {
                    return (TEnum)field.GetValue(null)!;
                }
            }

            if (throwIfNotFound)
                throw new ArgumentException($"未找到匹配的枚举值：{value}", nameof(value));

            return default;
        }
    }
}
