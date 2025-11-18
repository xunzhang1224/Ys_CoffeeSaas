using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.ApplicationInfoQueries
{
    /// <summary>
    /// 企业资质信息查询实现类
    /// </summary>
    public class EnterpriseQualificationInfoQueries(CoffeeMachineDbContext context, UserHttpContext userHttp) : IEnterpriseQualificationInfoQueries
    {
        /// <summary>
        /// 根据Id获取企业资质信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EnterpriseQualificationInfoOutput> GetEnterpriseQualificationInfoAsync()
        {
            // 获取企业资质信息
            //var info = await context.EnterpriseQualificationInfo.AsQueryable().AsNoTracking()
            //    .Include(s => s.EnterpriseInfo)
            //    .Select(s => new EnterpriseQualificationInfoOutput
            //    {
            //        EnterpriseId = s.EnterpriseId,
            //        EnterpriseName = s.EnterpriseInfo.Name,
            //        LegalPersonName = s.LegalPersonName,
            //        LegalPersonIdCardNumber = s.LegalPersonIdCardNumber,
            //        FrontImageUrl = s.FrontImageUrl,
            //        BackImageUrl = s.BackImageUrl,
            //        CustomerServiceEmail = s.CustomerServiceEmail,
            //        StoreAddress = s.StoreAddress,
            //        BusinessLicenseUrl = s.BusinessLicenseUrl,
            //        Othercertificate = s.Othercertificate,
            //        AreaRelationId = s.EnterpriseInfo.AreaRelationId ?? 0,
            //        RegistrationProgress = s.EnterpriseInfo.RegistrationProgress,
            //        organizationType = s.EnterpriseInfo.OrganizationType ?? Domain.AggregatesModel.UserInfo.EnterpriseOrganizationTypeEnum.Personal,
            //        Email = userHttp.Email,
            //        Remark = s.EnterpriseInfo.Remark
            //    })
            //    .FirstOrDefaultAsync(e => e.EnterpriseId == userHttp.TenantId) ?? new EnterpriseQualificationInfoOutput();

            // 获取企业资质信息
            var info = await context.EnterpriseInfo.AsQueryable().AsNoTracking()
                .Select(s => new EnterpriseQualificationInfoOutput
                {
                    EnterpriseId = s.Id,
                    EnterpriseName = s.Name,
                    AreaRelationId = s.AreaRelationId,
                    RegistrationProgress = s.RegistrationProgress,
                    organizationType = s.OrganizationType ?? Domain.AggregatesModel.UserInfo.EnterpriseOrganizationTypeEnum.Personal,
                    Email = userHttp.Email,
                    Remark = s.Remark
                })
                .FirstOrDefaultAsync(e => e.EnterpriseId == userHttp.TenantId) ?? new EnterpriseQualificationInfoOutput();

            var qualificationInfo = await context.EnterpriseQualificationInfo.AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync(e => e.EnterpriseId == userHttp.TenantId);
            if (qualificationInfo != null)
            {

                info.LegalPersonName = qualificationInfo.LegalPersonName;
                info.LegalPersonIdCardNumber = qualificationInfo.LegalPersonIdCardNumber;
                info.FrontImageUrl = qualificationInfo.FrontImageUrl;
                info.BackImageUrl = qualificationInfo.BackImageUrl;
                info.CustomerServiceEmail = qualificationInfo.CustomerServiceEmail;
                info.StoreAddress = qualificationInfo.StoreAddress;
                info.BusinessLicenseUrl = qualificationInfo.BusinessLicenseUrl;
                info.Othercertificate = qualificationInfo.Othercertificate;
            }

            if (info != null)
            {
                if (info.EnterpriseName == userHttp.Account + userHttp.Email)
                    info.EnterpriseName = string.Empty;
                var areaInfo = await context.AreaRelation.FirstOrDefaultAsync(w => w.Id == info.AreaRelationId);
                if (areaInfo != null)
                {
                    var dicInfo = await context.Dictionary.FirstOrDefaultAsync(w => w.Key == areaInfo.Country);
                    if (dicInfo != null)
                    {
                        info.CountryName = dicInfo.Value;
                    }
                }
            }
            return info;
        }

        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<bool?> IsAccountExists(string account)
        {
            return await context.ApplicationUser.AsQueryable().AsNoTracking()
                .AnyAsync(s => s.Account == account) ? true : null;
        }
    }
}