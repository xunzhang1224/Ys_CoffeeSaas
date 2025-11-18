using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.Iot.Api.Services
{
    /// <summary>
    /// 辅助类
    /// </summary>
    /// <param name="_context"></param>
    public class CommonHelper(CoffeeMachinePlatformDbContext _context)
    {
        /// <summary>
        /// 获取设备用户
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<long, List<long>>> GetUserByDeviceId(List<long> deviceIds)
        {
            // 设备对应企业
            var deviceEnterpriseDic = await _context.DeviceInfo.AsNoTracking().Where(w => deviceIds.Contains(w.Id)).ToDictionaryAsync(d => d.Id, d => d.EnterpriseinfoId);

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
        /// 根据租户转换时间
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDateTimeByEnterprise(long enterpriseId, DateTime oldTime)
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
                return oldTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {

                // 解析偏移量
                int offsetHours = int.Parse(timeZone.Replace("UTC", ""));
                TimeSpan offset = TimeSpan.FromHours(offsetHours);

                // 转换为指定偏移时间
                DateTime targetTime = oldTime + offset;
                return targetTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
