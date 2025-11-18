using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.DependencyInjection.Dependencies;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Utils
{
    /// <summary>
    /// 辅助类
    /// </summary>
    /// <param name="_context"></param>
    public class CommonHelper(CoffeeMachineDbContext _context)
    {
        /// <summary>
        /// 获取设备用户
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<long, List<long>>> GetUserByDeviceId(List<long> deviceIds)
        {
            // 设备对应企业
            var deviceEnterpriseDic = await _context.DeviceInfo.IgnoreQueryFilters().AsNoTracking().Where(w => deviceIds.Contains(w.Id)).ToDictionaryAsync(d => d.Id, d => d.EnterpriseinfoId);

            // 企业ids
            var enterpriseIds = deviceEnterpriseDic.Values.Distinct().ToList();

            var users = await _context.ApplicationUser.Where(w => enterpriseIds.Contains(w.EnterpriseId)).ToListAsync();

            // 企业对应用户
            var enterpriseUserDic = users.GroupBy(u => u.EnterpriseId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(u => u.Id).ToList());
            //var enterpriseUserDic = await _context.ApplicationUser
            //    .Where(w => enterpriseIds.Contains(w.EnterpriseId))
            //    .GroupBy(u => u.EnterpriseId)
            //    .ToDictionaryAsync(
            //        g => g.Key,
            //        g => g.Select(u => u.Id).ToList()
            //    );

            // 企业下默认用户
            var enterpriseDefalutDic = users.Where(w => w.IsDefault).ToDictionary(x => x.EnterpriseId, x => x.Id);

            // 拥有企业管理员角色用户
            var tt = await (
                 from a in _context.ApplicationUserRole
                 join b in _context.ApplicationRole on a.RoleId equals b.Id into ab
                 from b in ab.DefaultIfEmpty()
                 join c in _context.ApplicationUser on a.UserId equals c.Id into ac
                 from c in ac.DefaultIfEmpty()
                 where b.Code == "administrator" && enterpriseIds.Contains(c.EnterpriseId)
                 select new { c.EnterpriseId, a.UserId }
             ).ToListAsync();

            var enterpriseRoleDic = tt
                .GroupBy(x => x.EnterpriseId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.UserId).Distinct().ToList()
                );

            // 设备绑定和分组下的用户
            var directUsers = _context.DeviceUserAssociation
            .Where(x => deviceIds.Contains(x.DeviceId))
            .Select(x => new { x.DeviceId, UserId = x.UserId });

            var groupUsers = from gd in _context.GroupDevices
                             join gu in _context.GroupUsers
                                 on gd.GroupsId equals gu.GroupsId
                             where deviceIds.Contains(gd.DeviceInfoId)
                             select new { DeviceId = gd.DeviceInfoId, UserId = gu.ApplicationUserId };

            // UNION（自动去重）
            var unionResult = await directUsers
                .Union(groupUsers)
                .ToListAsync();

            Dictionary<long, List<long>> deviceUserDic = new Dictionary<long, List<long>>();
            foreach (var item in deviceEnterpriseDic)
            {
                var userIds = new List<long>();

                // 1、企业下默认用户
                if (enterpriseDefalutDic.TryGetValue(item.Value, out var t0) && t0 != null)
                {
                    userIds.Add(t0);
                }

                // 2、拥有企业管理员角色用户
                if (enterpriseRoleDic.TryGetValue(item.Value, out var t1) && t1 != null)
                {
                    userIds.AddRange(t1);
                }

                // 3、设备绑定和分组下的用户
                if (enterpriseUserDic.TryGetValue(item.Value, out var t2) && t2 != null)
                {
                    var enterpriseUserIds = unionResult.Where(w => t2.Contains(w.UserId) && w.DeviceId == item.Key).Select(s => s.UserId).ToList();
                    if (enterpriseUserIds != null && enterpriseUserIds.Count > 0)
                    {
                        userIds.AddRange(enterpriseUserIds);
                    }
                }

                deviceUserDic[item.Key] = userIds.Distinct().ToList();

            }

            return deviceUserDic;
        }

        /// <summary>
        /// 获取时区偏移量
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public async Task<int> GetTimeZoneOffset(long enterpriseId)
        {
            var timeZone = await (
               from a in _context.EnterpriseInfo
               join b in _context.AreaRelation on a.AreaRelationId equals b.Id into ab
               from b in ab.DefaultIfEmpty()
               where a.AreaRelationId != null && a.Id == enterpriseId
               select b.TimeZone
           ).FirstOrDefaultAsync();
            if (timeZone == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.A0003)]);
            return int.Parse(timeZone.Replace("UTC", ""));
        }

        /// <summary>
        /// 根据租户转换时间
        /// </summary>
        /// <returns></returns>
        public async Task<DateTime> GetDateTimeByEnterprise(long enterpriseId, DateTime oldTime)
        {
            var timeZone = await (
                from a in _context.EnterpriseInfo
                join b in _context.AreaRelation on a.AreaRelationId equals b.Id into ab
                from b in ab.DefaultIfEmpty()
                where a.AreaRelationId != null && a.Id == enterpriseId
                select b.TimeZone
            ).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(timeZone))
            {
                return oldTime;
            }
            else
            {

                // 解析偏移量
                int offsetHours = int.Parse(timeZone.Replace("UTC", ""));
                TimeSpan offset = TimeSpan.FromHours(offsetHours);

                // 转换为指定偏移时间
                DateTime targetTime = oldTime + offset;
                return targetTime;
            }
        }

        /// <summary>
        /// 根据userid获取 其可访问的设备id(调用该方法证明该用户不是管理员)
        /// </summary>
        /// <returns></returns>
        public async Task<List<long>> GetDeviceIdsByUserId(long userId)
        {
            // 第一部分：直接关联的设备
            var directDevices = _context.DeviceUserAssociation
                .Where(dua => dua.UserId == userId)
                .Select(dua => new
                {
                    deviceid = dua.DeviceId
                });

            // 第二部分：通过群组关联的设备
            var groupDevices = from gu in _context.GroupUsers
                               join gd in _context.GroupDevices on gu.GroupsId equals gd.GroupsId
                               where gu.ApplicationUserId == userId && gd.DeviceInfoId != null
                               select new
                               {
                                   deviceid = gd.DeviceInfoId
                               };

            // 合并结果
            var result = await directDevices
                .Union(groupDevices)
                .ToListAsync();

            return result == null ? new List<long>()
                : result.Select(x => x.deviceid).ToList();
        }
    }
}
