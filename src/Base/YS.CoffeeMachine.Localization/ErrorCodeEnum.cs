namespace YS.CoffeeMachine.Localization
{
    /// <summary>
    /// 错误枚举
    /// </summary>
    public enum ErrorCodeEnum
    {
        #region 异常提示

        /// <summary>
        /// 数据异常
        /// </summary>
        D0000,

        /// <summary>
        /// 语种不能默认且禁用
        /// </summary>
        D0001,

        /// <summary>
        /// {0}不能为空
        /// </summary>
        D0002,

        /// <summary>
        /// 未找到语种{0}
        /// </summary>
        D0003,

        /// <summary>
        /// 未找到语种文本key
        /// </summary>
        D0004,

        /// <summary>
        /// 未找编号为{0}的设备
        /// </summary>
        D0005,

        /// <summary>
        /// 未找编号为{0}的风格
        /// </summary>
        D0006,

        /// <summary>
        /// 未查询到操作日志
        /// </summary>
        D0007,

        /// <summary>
        /// 设备不存在
        /// </summary>
        D0008,

        /// <summary>
        /// 设备已有广告信息，请勿重复添加
        /// </summary>
        D0009,

        /// <summary>
        /// 设备已有广告信息，请勿重复添加
        /// </summary>
        D0010,

        /// <summary>
        /// 父级不存在
        /// </summary>
        D0011,

        /// <summary>
        /// 总层级不能超过6级，当前已达到最大层级
        /// </summary>
        D0012,

        /// <summary>
        /// Id不能为空
        /// </summary>
        D0013,

        /// <summary>
        /// 数据不存在
        /// </summary>
        D0014,

        /// <summary>
        /// 不能添加自己为上级
        /// </summary>
        D0015,

        /// <summary>
        /// 新上级企业不能是当前企业的下级企业！
        /// </summary>
        D0016,

        /// <summary>
        /// 父菜单数据不存在
        /// </summary>
        D0017,

        /// <summary>
        /// 按钮只能添加在菜单下
        /// </summary>
        D0018,

        /// <summary>
        /// 默认角色不允许修改
        /// </summary>
        D0019,

        /// <summary>
        /// {0},创建人不存在，请检查参数是否异常
        /// </summary>
        D0020,

        /// <summary>
        /// {0},服务人员账号不存在
        /// </summary>
        D0021,

        /// <summary>
        /// 包含未绑定企业信息的用户，请检查
        /// </summary>
        D0022,

        /// <summary>
        /// 请勿授权给同企业的账号
        /// </summary>
        D0023,

        /// <summary>
        /// 当前状态不能操作
        /// </summary>
        D0024,

        /// <summary>
        /// 电话或邮箱已存在
        /// </summary>
        D0025,

        /// <summary>
        /// 您的初始密码为:{0}
        /// </summary>
        D0026,

        /// <summary>
        /// 初始密码
        /// </summary>
        D0027,

        /// <summary>
        /// 默认管理员不允许删除
        /// </summary>
        D0028,

        /// <summary>
        /// 用户不存在
        /// </summary>
        D0029,

        /// <summary>
        /// 用户或密码无效
        /// </summary>
        D0030,

        /// <summary>
        /// 邮箱不存在
        /// </summary>
        D0031,

        /// <summary>
        /// 您的验证码为{0}
        /// </summary>
        D0032,

        /// <summary>
        /// 验证码
        /// </summary>
        D0033,

        /// <summary>
        /// 验证码已过期
        /// </summary>
        D0034,

        /// <summary>
        /// 验证码错误
        /// </summary>
        D0035,

        /// <summary>
        /// 刷新令牌失效
        /// </summary>
        D0036,

        /// <summary>
        /// 令牌已被移除
        /// </summary>
        D0037,

        /// <summary>
        /// 令牌过期，请重新登录
        /// </summary>
        D0038,

        /// <summary>
        /// 默认管理员不允许重置密码
        /// </summary>
        D0039,

        /// <summary>
        /// 您的新密码为{0}
        /// </summary>
        D0040,

        /// <summary>
        /// 重置密码
        /// </summary>
        D0041,

        /// <summary>
        /// 默认管理员不允许禁用
        /// </summary>
        D0042,

        /// <summary>
        /// 旧密码不正常
        /// </summary>
        D0043,

        /// <summary>
        /// 用户未登录
        /// </summary>
        D0044,

        /// <summary>
        /// 饮品集合无效
        /// </summary>
        D0045,

        /// <summary>
        /// {0} 饮品集合参数异常
        /// </summary>
        D0046,

        /// <summary>
        /// {0} 饮品集合内无可用饮品
        /// </summary>
        D0047,

        /// <summary>
        /// 饮品历史版本参数异常
        /// </summary>
        D0048,

        /// <summary>
        /// 当前设备无饮品信息
        /// </summary>
        D0049,

        /// <summary>
        /// {0} 饮品合集名称已存在
        /// </summary>
        D0050,

        /// <summary>
        /// 不能应用到自己 {0}
        /// </summary>
        D0051,

        /// <summary>
        /// 源设备饮品不存在
        /// </summary>
        D0052,

        /// <summary>
        /// 模板饮品不存在
        /// </summary>
        D0053,

        /// <summary>
        /// SKU:{0}已存在，请勿重复添加
        /// </summary>
        D0054,

        /// <summary>
        /// 配方不能为空
        /// </summary>
        D0055,

        /// <summary>
        /// 配方参数异常，请检查
        /// </summary>
        D0056,

        /// <summary>
        /// 默认饮品不允许删除
        /// </summary>
        D0057,

        /// <summary>
        /// 数据异常，无默认风格
        /// </summary>
        D0058,

        /// <summary>
        /// 数据异常，设备型号不存在
        /// </summary>
        D0059,

        /// <summary>
        /// 未检测到导入数据
        /// </summary>
        D0060,

        /// <summary>
        /// {0}行，设备名称不能为空
        /// </summary>
        D0061,

        /// <summary>
        /// {0}行，设备编号不能为空
        /// </summary>
        D0062,

        /// <summary>
        /// {0}行，设备型号【{1}】不存在，请检查
        /// </summary>
        D0063,

        /// <summary>
        /// 国家：【{0}行，{1}】，数据不存在
        /// </summary>
        D0064,

        /// <summary>
        /// 设备不属于当前企业
        /// </summary>
        D0065,

        /// <summary>
        /// 租赁设备必须设置回收时间
        /// </summary>
        D0066,

        /// <summary>
        /// 只有租赁设备才能取消分配
        /// </summary>
        D0067,

        /// <summary>
        /// 企业不存在
        /// </summary>
        D0068,

        /// <summary>
        /// 最多只能添加8层子分组，当前已达到最大层级
        /// </summary>
        D0069,

        /// <summary>
        /// 新上级分组不能是当前分组的下级或子孙分组
        /// </summary>
        D0070,

        /// <summary>
        /// 必须添加6个料盒
        /// </summary>
        D0071,

        /// <summary>
        /// 设备已有设置信息，请勿重复添加
        /// </summary>
        D0072,

        /// <summary>
        /// 未找到描述为 '{0}' 的枚举值
        /// </summary>
        D0073,

        /// <summary>
        /// 存在数据重复，请检查！所在行：:
        /// </summary>
        D0074,

        /// <summary>
        /// 未找到对应的预警配置信息
        /// </summary>
        D0075,

        /// <summary>
        /// 分页参数异常
        /// </summary>
        D0076,

        /// <summary>
        /// enterpriseId：{0}未找到到企业数据
        /// </summary>
        D0077,

        /// <summary>
        /// 默认用户不存在
        /// </summary>
        D0078,

        /// <summary>
        /// 名称不能为空
        /// </summary>
        D0079,

        /// <summary>
        /// 企业类型不允许重复
        /// </summary>
        D0080,

        /// <summary>
        /// 默认数据不允许编辑
        /// </summary>
        D0081,

        /// <summary>
        /// 刷新令牌无效
        /// </summary>
        D0082,

        /// <summary>
        /// 账号已存在
        /// </summary>
        D0083,

        /// <summary>
        /// 时间段不能为空
        /// </summary>
        D0084,

        /// <summary>
        /// 暂无修改
        /// </summary>
        D0085,

        /// <summary>
        /// SKU不能为空
        /// </summary>
        D0086,

        /// <summary>
        /// 不能删除有在使用的资源
        /// </summary>
        D0087,

        /// <summary>
        /// 当前企业树下已存在名字：{0}
        /// </summary>
        D0088,

        /// <summary>
        /// 当前设备已被其他账号绑定，无法再次绑定
        /// </summary>
        D0089,

        /// <summary>
        /// 设备已激活，请勿重复操作
        /// </summary>
        D0090,

        /// <summary>
        /// 当前设备已绑定，请勿重复操作
        /// </summary>
        D0091,

        /// <summary>
        /// 邮箱不能为空
        /// </summary>
        D0092,

        /// <summary>
        /// 邮箱格式不正确
        /// </summary>
        D0093,

        /// <summary>
        /// 请求过于频繁，请稍后再试
        /// </summary>
        D0094,

        /// <summary>
        /// 验证码已发送，请稍后再试
        /// </summary>
        D0095,

        /// <summary>
        /// 企业注册邮箱验证
        /// </summary>
        D0096,

        /// <summary>
        /// 您的验证码是：{0}，请在5分钟内使用
        /// </summary>
        D0097,

        /// <summary>
        /// 邮箱验证码无效
        /// </summary>
        D0098,

        /// <summary>
        /// 企业当前状态不能更新资质信息
        /// </summary>
        D0099,

        /// <summary>
        /// 地区关系Id无效
        /// </summary>
        D0100,

        /// <summary>
        /// 国别信息不存在
        /// </summary>
        D0101,

        /// <summary>
        /// 您的企业资质信息已审核通过，请登录平台查看
        /// </summary>
        D0102,

        /// <summary>
        /// 您的企业资质信息审核失败，请登录平台查看
        /// </summary>
        D0103,

        /// <summary>
        /// 企业资质审核结果
        /// </summary>
        D0104,

        /// <summary>
        /// 手机号不能为空
        /// </summary>
        D0105,

        /// <summary>
        /// 手机号格式不正确
        /// </summary>
        D0106,

        /// <summary>
        /// 同一台设备不能多次绑定同一支付方式
        /// </summary>
        D0107,

        /// <summary>
        /// 该租户已存在重复的sku
        /// </summary>
        D0108,

        /// <summary>
        /// 短信验证码无效
        /// </summary>
        D0109,
        #endregion

        #region 数据验证

        /// <summary>
        /// {0}参数不能为空
        /// </summary>
        C0001,

        /// <summary>
        /// 参数超过最大长度
        /// </summary>
        C0002,

        /// <summary>
        /// 默认语种不允许删除
        /// </summary>
        C0003,

        /// <summary>
        /// {0}必须是有效枚举值
        /// </summary>
        C0004,

        /// <summary>
        /// {0}必须是有效的数值
        /// </summary>
        C0005,

        /// <summary>
        /// 选择的模板与语种不匹配
        /// </summary>
        C0006,

        /// <summary>
        /// 导入的数据不能为空
        /// </summary>
        C0007,

        /// <summary>
        /// 请输入有效的数值
        /// </summary>
        C0008,

        /// <summary>
        /// 邮箱地址格式不正确
        /// </summary>
        C0009,

        /// <summary>
        /// {0}必须是有效的枚举值
        /// </summary>
        C0010,

        /// <summary>
        /// 参数异常
        /// </summary>
        C0011,

        /// <summary>
        /// 数据已存在，请勿重新添加
        /// </summary>
        C0012,

        /// <summary>
        /// 设备mid无效
        /// </summary>
        C0013,

        /// <summary>
        /// 设备mid已被绑定
        /// </summary>
        C0014,

        /// <summary>
        /// 设备不在线
        /// </summary>
        C0015,

        /// <summary>
        /// mid参数不能为空
        /// </summary>
        C0016,

        /// <summary>
        /// 没有该设备操作权限
        /// </summary>
        C0017,

        /// <summary>
        /// 语种code或名字不能重复
        /// </summary>
        C0018,

        /// <summary>
        /// 卡号已存在
        /// </summary>
        C0019,

        /// <summary>
        /// 父级分类不存在
        /// </summary>
        C0020,

        /// <summary>
        /// 商品分类下有子分类，不能删除
        /// </summary>
        C0021,

        /// <summary>
        /// 优惠劵不可用
        /// </summary>
        C0022,

        /// <summary>
        /// 该商户号在平台无效
        /// </summary>
        C0023,

        /// <summary>
        /// 箱体已被绑定
        /// </summary>
        C0024,
        #endregion

        #region 配置

        /// <summary>
        /// redis未配置
        /// </summary>
        A0001,

        /// <summary>
        /// 开放平台路径未配置
        /// </summary>
        A0002,

        /// <summary>
        /// 租户未配置时区
        /// </summary>
        A0003,
        #endregion

        /// <summary>
        /// 开放平台调用异常
        /// </summary>
        K0001,
    }
}
