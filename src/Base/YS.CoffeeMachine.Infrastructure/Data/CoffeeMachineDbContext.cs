using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using YS.CoffeeMachine.Application.Commands.BasicCommands;
using YS.CoffeeMachine.Domain.AggregatesModel;
using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Dictionary;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.AggregatesModel.Card;
using YS.CoffeeMachine.Domain.AggregatesModel.ConsumerEntitys;
using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YS.CoffeeMachine.Domain.AggregatesModel.Currencys;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;
using YS.CoffeeMachine.Domain.AggregatesModel.Goods;
using YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.AggregatesModel.Payment;
using YS.CoffeeMachine.Domain.AggregatesModel.ServiceProviders;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.AggregatesModel.Strategy;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure.EntityConfigurations;
using YSCore.Base.DatabaseAccessor;
using YSCore.Base.DatabaseAccessor.Filter;
using YSCore.Provider.EntityFrameworkCore;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure
{
    /// <summary>
    /// 咖啡机商户端数据库上下文
    /// </summary>
    public class CoffeeMachineDbContext : AppDbContextBase
    {
        private readonly UserHttpContext _user;

        /// <summary>
        /// 咖啡机商户端数据库上下文
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mediator"></param>
        /// <param name="provider"></param>
        public CoffeeMachineDbContext(DbContextOptions<CoffeeMachineDbContext> options, IMediator mediator, IServiceProvider provider) : base(options, mediator, provider)
        {
            _user = provider.GetRequiredService<UserHttpContext>();
        }

        #region 平台端
        #region 币种信息

        /// <summary>
        /// 币种信息
        /// </summary>
        public DbSet<CurrencyInfo> CurrencyInfo { get; set; }

        /// <summary>
        /// 币种信息
        /// </summary>
        public DbSet<Currency> Currency { get; set; }
        #endregion
        #region 服务商

        /// <summary>
        /// 服务商信息
        /// </summary>
        public DbSet<ServiceProviderInfo> ServiceProviderInfo { get; set; }
        #endregion
        #region 企业基础信息

        /// <summary>
        /// 企业类型
        /// </summary>
        public DbSet<EnterpriseTypes> EnterpriseTypes { get; set; }
        #endregion
        #endregion

        #region 任务调度相关

        /// <summary>
        /// 任务调度信息
        /// </summary>
        public DbSet<TaskSchedulingInfo> TaskSchedulingInfo { get; set; }
        #endregion

        #region 国家地区相关

        /// <summary>
        /// 国家信息
        /// </summary>
        public DbSet<CountryInfo> CountryInfo { get; set; }

        /// <summary>
        /// 地区信息
        /// </summary>
        public DbSet<CountryRegion> CountryRegion { get; set; }
        #endregion

        #region 企业/用户/权限相关

        /// <summary>
        /// 企业信息
        /// </summary>
        public DbSet<EnterpriseInfo> EnterpriseInfo { get; set; }

        /// <summary>
        /// 企业资质信息
        /// </summary>
        public DbSet<EnterpriseQualificationInfo> EnterpriseQualificationInfo { get; set; }

        /// <summary>
        /// 企业用户关联表
        /// </summary>
        public DbSet<EnterpriseUser> EnterpriseUser { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        /// <summary>
        /// 角色信息
        /// </summary>
        public DbSet<ApplicationRole> ApplicationRole { get; set; }

        /// <summary>
        /// 菜单信息
        /// </summary>
        public DbSet<ApplicationMenu> ApplicationMenu { get; set; }

        /// <summary>
        /// 授权记录
        /// </summary>
        public DbSet<ServiceAuthorizationRecord> ServiceAuthorizationRecord { get; set; }

        /// <summary>
        /// 用户角色关联
        /// </summary>
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        #endregion

        #region 设备相关

        /// <summary>
        /// 设备型号
        /// </summary>
        public DbSet<DeviceModel> DeviceModel { get; set; }

        /// <summary>
        /// 物料信息
        /// </summary>
        public DbSet<DeviceMaterialInfo> DeviceMaterialInfo { get; set; }

        /// <summary>
        /// a
        /// </summary>
        public DbSet<DeviceEarlyWarnings> DeviceEarlyWarnings { get; set; }

        /// <summary>
        /// 设备异常
        /// </summary>
        public DbSet<DeviceAbnormal> DeviceAbnormal { get; set; }

        /// <summary>
        /// 设备基础信息
        /// </summary>
        public DbSet<DeviceInfo> DeviceInfo { get; set; }

        /// <summary>
        /// 设备基础信息
        /// </summary>
        public DbSet<DeviceBaseInfo> DeviceBaseInfo { get; set; }

        /// <summary>
        /// 设备容量配置
        /// </summary>
        public DbSet<DeviceCapacityCfg> DeviceCapacityCfg { get; set; }

        /// <summary>
        /// 企业设备关联
        /// </summary>
        public DbSet<EnterpriseDevices> EnterpriseDevices { get; set; }

        /// <summary>
        /// 设备用户关联
        /// </summary>
        public DbSet<Groups> Groups { get; set; }

        /// <summary>
        /// 组设备关联
        /// </summary>
        public DbSet<GroupDevices> GroupDevices { get; set; }

        /// <summary>
        /// 组用户关联
        /// </summary>
        public DbSet<GroupUsers> GroupUsers { get; set; }

        /// <summary>
        /// 设备用户关联
        /// </summary>
        public DbSet<DeviceUserAssociation> DeviceUserAssociation { get; set; }

        /// <summary>
        /// 设备软件信息
        /// </summary>
        public DbSet<DeviceSoftwareInfo> DeviceSoftwareInfo { get; set; }

        /// <summary>
        /// 设备属性信息(网络信息+屏幕)
        /// </summary>
        public DbSet<DeviceAttribute> DeviceAttribute { get; set; }

        #region 设置

        /// <summary>
        /// 设置信息
        /// </summary>
        public DbSet<SettingInfo> SettingInfo { get; set; }

        /// <summary>
        /// 时间区域信息
        /// </summary>
        public DbSet<TimeZoneInfos> TimeZoneInfos { get; set; }

        /// <summary>
        /// 界面风格
        /// </summary>
        public DbSet<InterfaceStyles> InterfaceStyles { get; set; }

        /// <summary>
        /// 语言信息
        /// </summary>
        public DbSet<LanguageInfo> LanguageInfo { get; set; }

        /// <summary>
        /// 语言文本
        /// </summary>
        public DbSet<LanguageTextEntity> LanguageText { get; set; }

        /// <summary>
        /// 文件中心
        /// </summary>
        public DbSet<FileCenter> FileCenter { get; set; }
        //public DbSet<BaseEntity> Customers { get; set; }

        /// <summary>
        /// 预警配置
        /// </summary>
        public DbSet<EarlyWarningConfig> EarlyWarningConfig { get; set; }

        /// <summary>
        /// 审计信息
        /// </summary>
        public DbSet<Audit> Audits { get; set; }

        /// <summary>
        /// 设备支付信息
        /// </summary>
        public DbSet<DevicePaymentConfig> DevicePaymentConfig { get; set; }

        /// <summary>
        /// 设备支付设置
        /// </summary>
        public DbSet<DevicePaymentSetting> DevicePaymentSetting { get; set; }
        #endregion

        #region 广告

        /// <summary>
        /// 广告信息
        /// </summary>
        public DbSet<AdvertisementInfo> AdvertisementInfo { get; set; }
        #endregion
        #endregion

        #region 饮品相关

        /// <summary>
        /// 商品分类
        /// </summary>
        public DbSet<ProductCategory> ProductCategory { get; set; }

        /// <summary>
        /// 饮品信息
        /// </summary>
        public DbSet<BeverageInfo> BeverageInfo { get; set; }

        /// <summary>
        /// 饮品版本信息
        /// </summary>
        public DbSet<BeverageVersion> BeverageVersion { get; set; }

        /// <summary>
        /// 配方信息
        /// </summary>
        public DbSet<FormulaInfo> FormulaInfo { get; set; }

        /// <summary>
        /// 饮品库
        /// </summary>
        public DbSet<BeverageInfoTemplate> BeverageInfoTemplate { get; set; }

        /// <summary>
        /// 饮品信息模板版本
        /// </summary>
        public DbSet<BeverageTemplateVersion> BeverageTemplateVersion { get; set; }

        /// <summary>
        /// 饮品集合
        /// </summary>
        public DbSet<BeverageCollection> BeverageCollection { get; set; }

        /// <summary>
        /// 料盒
        /// </summary>
        public DbSet<MaterialBox> MaterialBox { get; set; }

        /// <summary>
        /// 平台端饮品集合
        /// </summary>
        public DbSet<P_BeverageCollection> P_BeverageCollection { get; set; }

        /// <summary>
        /// 平台端饮品库饮品
        /// </summary>
        public DbSet<P_BeverageInfo> P_BeverageInfo { get; set; }

        /// <summary>
        /// 平台饮品历史版本
        /// </summary>
        public DbSet<P_BeverageVersion> P_BeverageVersion { get; set; }
        #endregion

        #region 支付相关

        /// <summary>
        /// 临时支付配置
        /// </summary>
        public DbSet<P_PaymentConfig> P_PaymentConfig { get; set; }

        /// <summary>
        /// 支付配置
        /// </summary>
        public DbSet<PaymentConfig> PaymentConfig { get; set; }
        #endregion

        #region 站内信相关

        /// <summary>
        /// 系统消息
        /// </summary>
        public DbSet<SystemMessages> SystemMessages { get; set; }

        /// <summary>
        /// 用户消息
        /// </summary>
        public DbSet<UserMessages> UserMessages { get; set; }

        /// <summary>
        /// 用户已读全局消息
        /// </summary>
        public DbSet<UserReadGlobalMessages> UserReadGlobalMessages { get; set; }
        #endregion

        #region 订单相关
        /// <summary>
        /// 订单信息
        /// </summary>
        public DbSet<OrderInfo> OrderInfo { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        public DbSet<OrderDetails> OrderDetails { get; set; }

        /// <summary>
        /// 订单详情物料使用信息
        /// </summary>
        public DbSet<OrderDetaliMaterial> OrderDetaliMaterial { get; set; }

        /// <summary>
        /// 退款订单信息
        /// </summary>
        public DbSet<OrderRefund> OrderRefund { get; set; }
        #endregion

        #region 国内微信支付宝支付相关

        /// <summary>
        /// 支付的服务商表(微信|支付宝)
        /// </summary>
        public DbSet<SystemPaymentServiceProvider> SystemPaymentServiceProvider { get; set; }

        /// <summary>
        /// 系统支付方式
        /// </summary>
        public DbSet<SystemPaymentMethod> SystemPaymentMethod { get; set; }

        /// <summary>
        /// 二级商户国内支付方式
        /// </summary>
        public DbSet<M_PaymentMethod> M_PaymentMethod { get; set; }

        /// <summary>
        /// 商户支付宝进件申请表
        /// </summary>
        public DbSet<M_PaymentAlipayApplyments> M_PaymentAlipayApplyments { get; set; }

        /// <summary>
        /// 商户微信进件申请表
        /// </summary>
        public DbSet<M_PaymentWechatApplyments> M_PaymentWechatApplyments { get; set; }

        /// <summary>
        /// 商户支付方式绑定的设备表
        /// </summary>
        public DbSet<M_PaymentMethodBindDevice> M_PaymentMethodBindDevice { get; set; }

        /// <summary>
        /// 分账配置表
        /// </summary>
        public DbSet<DivideAccountsConfig> DivideAccountsConfig { get; set; }
        #endregion

        #region 基础信息

        /// <summary>
        /// 基础地区信息
        /// </summary>
        public DbSet<SysProvinceCity> SysProvinceCity { get; set; }

        /// <summary>
        /// 服务条款信息
        /// </summary>
        public DbSet<TermServiceEntity> TermServiceEntity { get; set; }
        #endregion

        #region 消费者端

        /// <summary>
        /// 消费者端用户信息
        /// </summary>
        public DbSet<ClientUser> ClientUser { get; set; }
        #endregion

        /// <summary>
        /// DeviceRestockLog
        /// </summary>
        public DbSet<DeviceRestockLog> DeviceRestockLog { get; set; }

        /// <summary>
        /// DeviceRestockLog
        /// </summary>
        public DbSet<DeviceRestockLogSub> DeviceRestockLogSub { get; set; }
        /// <summary>
        /// a
        /// </summary>
        public DbSet<NoticeCfg> NoticeCfg { get; set; }

        /// <summary>
        /// 卡信息
        /// </summary>
        public DbSet<CardInfo> CardInfo { get; set; }

        /// <summary>
        /// 卡设备关联
        /// </summary>
        public DbSet<CardDeviceAssignment> CardDeviceAssignment { get; set; }

        /// <summary>
        /// 营销活动
        /// </summary>
        public DbSet<Promotion> Promotion { get; set; }

        /// <summary>
        /// 优惠劵
        /// </summary>
        public DbSet<Coupon> Coupon { get; set; }

        /// <summary>
        /// a
        /// </summary>
        public DbSet<NotityMsg> NotityMsg { get; set; }

        /// <summary>
        /// 区域关系
        /// </summary>
        public DbSet<AreaRelation> AreaRelation { get; set; }

        /// <summary>
        /// 清洗日志
        /// </summary>
        public DbSet<DeviceFlushComponentsLog> DeviceFlushComponentsLog { get; set; }

        /// <summary>
        /// 操作日志
        /// </summary>
        public DbSet<OperationLog> OperationLog { get; set; }

        /// <summary>
        /// 故障码
        /// </summary>
        public DbSet<FaultCodeEntity> FaultCodeEntitie { get; set; }

        /// <summary>
        /// 操作子日志
        /// </summary>
        public DbSet<OperationSubLog> OperationSubLog { get; set; }

        /// <summary>
        /// 文件信息
        /// </summary>
        public DbSet<FileManage> FileManage { get; set; }

        /// <summary>
        /// 文件关系信息
        /// </summary>
        public DbSet<FileRelation> FileRelation { get; set; }

        /// <summary>
        ///  字典信息
        /// </summary>
        public DbSet<DictionaryEntity> Dictionary { get; set; }

        /// <summary>
        ///  私有库
        /// </summary>
        public DbSet<PrivateGoodsRepository> PrivateGoodsRepository { get; set; }

        #region 设备广告
        /// <summary>
        /// 设备广告信息
        /// </summary>
        public DbSet<DeviceAdvertisement> DeviceAdvertisement { get; set; }

        /// <summary>
        /// 设备广告文件信息
        /// </summary>
        public DbSet<DeviceAdvertisementFile> DeviceAdvertisementFile { get; set; }
        #endregion

        /// <summary>
        /// 任务调度信息
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskSchedulingInfoEntityConfiguration).Assembly);

            modelBuilder.AddQueryFilter((IDeletedFilter x) => x.IsDelete == false);
            // 租户 过滤器
            modelBuilder.AddQueryFilter<IEnterpriseFilter>(x => x.EnterpriseinfoId == _user.TenantId);

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<EnterpriseBaseEntity>().HasQueryFilter(b => b.EnterpriseinfoId == _user.TenantId);
            //添加种子数据

            //if (!CurrencyInfo.Any())
            //foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            //{
            //    var region = new RegionInfo(culture.Name);
            //    var cultureInfo = new CurrencyInfo(culture.Name, region.CurrencySymbol, region.ISOCurrencySymbol, region.EnglishName, region.DisplayName);
            //    modelBuilder.Entity<LanguageInfo>().HasData(cultureInfo);
            //}
        }
        //public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var auditEntries = OnBeforeSaveChanges();
        //    var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //    await OnAfterSaveChanges(auditEntries);
        //    return result;
        //}

        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task BeforeSaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _user.UserId;
            var userName = _user.NickName;
            var tenantId = _user.TenantId;

            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntries.Add(auditEntry);

                var isbase = true;// 继承基类BaseEntity
                var isEnterprise = true;  //继承企业EnterpriseBaseEntity

                var baseEntity = entry.Entity as BaseEntity;
                var enterpriseEntity = entry.Entity as EnterpriseBaseEntity;

                // 判断是否baseentity
                if (baseEntity == null && enterpriseEntity == null)
                {
                    isbase = false;
                }

                if (enterpriseEntity == null)
                {
                    isEnterprise = false;
                }

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    auditEntry.NickName = userName;
                    auditEntry.UserId = userId;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            auditEntry.TrailType = TrailTypeEnum.Add;

                            if (isEnterprise)
                            {
                                enterpriseEntity.CreateUserId = userId;
                                enterpriseEntity.CreateUserName = userName;
                                if (tenantId > 0)
                                    enterpriseEntity.EnterpriseinfoId = tenantId;
                            }
                            else if (isbase)
                            {
                                baseEntity.CreateUserId = userId;
                                baseEntity.CreateUserName = userName;
                            }
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.TrailType = TrailTypeEnum.Delete;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                if (isEnterprise)
                                {
                                    enterpriseEntity.LastModifyTime = DateTime.UtcNow;
                                    enterpriseEntity.LastModifyUserId = userId;
                                    enterpriseEntity.LastModifyUserName = userName;
                                }
                                else if (isbase)
                                {
                                    baseEntity.LastModifyTime = DateTime.UtcNow;
                                    baseEntity.LastModifyUserId = userId;
                                    baseEntity.LastModifyUserName = userName;
                                }
                                auditEntry.TrailType = TrailTypeEnum.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                Audits.Add(auditEntry.ToAudit());
            }
            return Task.CompletedTask;

            //return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        //private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        //{
        //    if (auditEntries == null || auditEntries.Count == 0)
        //        return Task.CompletedTask;

        //    foreach (var auditEntry in auditEntries)
        //    {
        //        // Get the final value of the temporary properties
        //        foreach (var prop in auditEntry.TemporaryProperties)
        //        {
        //            if (prop.Metadata.IsPrimaryKey())
        //            {
        //                auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
        //            }
        //            else
        //            {
        //                auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
        //            }
        //        }

        //        // Save the Audit entry
        //        Audits.Add(auditEntry.ToAudit());
        //    }

        //    return SaveChangesAsync();
        //}
    }
}