IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [ApplicationMenu] (
        [Id] bigint NOT NULL,
        [ParentId] bigint NULL,
        [MenuType] nvarchar(30) NOT NULL,
        [SysMenuType] nvarchar(30) NOT NULL,
        [Title] nvarchar(64) NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [Path] nvarchar(128) NOT NULL,
        [Component] nvarchar(128) NULL,
        [Rank] int NOT NULL,
        [Redirect] nvarchar(128) NULL,
        [Icon] nvarchar(128) NULL,
        [ExtraIcon] nvarchar(128) NULL,
        [EnterTransition] nvarchar(128) NULL,
        [LeaveTransition] nvarchar(128) NULL,
        [Auths] nvarchar(128) NULL,
        [FrameSrc] nvarchar(256) NULL,
        [FrameLoading] bit NOT NULL,
        [KeepAlive] bit NOT NULL,
        [HiddenTag] bit NOT NULL,
        [FixedTag] bit NOT NULL,
        [ShowLink] bit NOT NULL,
        [ShowParent] bit NOT NULL,
        [IsDefault] bit NOT NULL,
        [Remark] nvarchar(256) NULL,
        [ActivePath] nvarchar(128) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_ApplicationMenu] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicationMenu_ApplicationMenu_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [ApplicationMenu] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [ApplicationRole] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [Status] nvarchar(30) NOT NULL,
        [Type] nvarchar(30) NOT NULL,
        [SysMenuType] nvarchar(30) NOT NULL,
        [IsDefault] bit NOT NULL,
        [HasSuperAdmin] bit NULL,
        [Sort] int NOT NULL,
        [Remark] nvarchar(128) NULL,
        [Code] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_ApplicationRole] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [ApplicationUser] (
        [Id] bigint NOT NULL,
        [EnterpriseId] bigint NOT NULL,
        [Account] nvarchar(64) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [NickName] nvarchar(64) NULL,
        [AreaCode] nvarchar(64) NULL,
        [Phone] nvarchar(64) NULL,
        [VerifyPhone] bit NOT NULL,
        [Email] nvarchar(64) NOT NULL,
        [Status] nvarchar(30) NOT NULL,
        [AccountType] nvarchar(30) NOT NULL,
        [SysMenuType] nvarchar(30) NOT NULL,
        [IsDefault] bit NOT NULL,
        [Remark] nvarchar(128) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_ApplicationUser] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [AreaRelation] (
        [Id] bigint NOT NULL,
        [Area] nvarchar(64) NOT NULL,
        [Country] nvarchar(256) NOT NULL,
        [AreaCode] nvarchar(256) NOT NULL,
        [Language] nvarchar(256) NOT NULL,
        [CurrencyId] bigint NOT NULL,
        [TimeZone] nvarchar(256) NOT NULL,
        [TermServiceUrl] nvarchar(256) NOT NULL,
        [Enabled] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_AreaRelation] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [Audit] (
        [Id] bigint NOT NULL,
        [TableName] nvarchar(256) NOT NULL,
        [KeyValues] nvarchar(max) NOT NULL,
        [OldValues] nvarchar(max) NULL,
        [NewValues] nvarchar(max) NULL,
        [TrailType] int NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_Audit] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [BeverageCollection] (
        [Id] bigint NOT NULL,
        [EnterpriseInfoId] bigint NOT NULL,
        [DeviceModelId] bigint NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [BeverageIds] nvarchar(max) NOT NULL,
        [BeverageNames] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_BeverageCollection] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [BeverageInfoTemplate] (
        [Id] bigint NOT NULL,
        [CategoryIds] nvarchar(max) NULL,
        [EnterpriseInfoId] bigint NULL,
        [DeviceModelId] bigint NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [BeverageIcon] nvarchar(256) NOT NULL,
        [Code] nvarchar(64) NOT NULL,
        [CodeIsShow] bit NOT NULL,
        [Temperature] int NOT NULL,
        [Remarks] nvarchar(256) NULL,
        [ProductionForecast] nvarchar(256) NOT NULL,
        [ForecastQuantity] float NOT NULL,
        [DisplayStatus] bit NOT NULL,
        [SellStradgy] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_BeverageInfoTemplate] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [CardInfo] (
        [Id] bigint NOT NULL,
        [CardNumber] nvarchar(256) NOT NULL,
        [IsEnabled] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_CardInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [CountryInfo] (
        [Id] bigint NOT NULL,
        [CountryName] nvarchar(64) NOT NULL,
        [CountryCode] nvarchar(64) NOT NULL,
        [Continent] nvarchar(256) NOT NULL,
        [IsEnabled] nvarchar(30) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        CONSTRAINT [PK_CountryInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [Currency] (
        [Id] bigint NOT NULL,
        [Code] nvarchar(256) NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [CurrencySymbol] nvarchar(256) NOT NULL,
        [CurrencyShowFormat] nvarchar(max) NOT NULL,
        [Accuracy] int NOT NULL,
        [RoundingType] nvarchar(max) NOT NULL,
        [Enabled] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_Currency] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [CurrencyInfo] (
        [Id] bigint NOT NULL,
        [Culture] nvarchar(64) NOT NULL,
        [CurrencySymbol] nvarchar(256) NOT NULL,
        [CurrencyCode] nvarchar(64) NOT NULL,
        [RegionName] nvarchar(256) NOT NULL,
        [CountryName] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_CurrencyInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceAbnormal] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [DeviceName] nvarchar(256) NULL,
        [DeviceModelName] nvarchar(256) NULL,
        [TransId] nvarchar(256) NULL,
        [CounterNo] int NOT NULL,
        [Slot] int NOT NULL,
        [Code] nvarchar(256) NOT NULL,
        [Desc] nvarchar(256) NULL,
        [CodeType] int NOT NULL,
        [Status] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        [BaseEnterpriseId] bigint NULL,
        [BaseEnterpriseName] nvarchar(256) NULL,
        [BaseDeviceId] bigint NULL,
        [BaseDeviceName] nvarchar(256) NULL,
        [BaseDeviceModelId] bigint NULL,
        [BaseDeviceModelName] nvarchar(256) NULL,
        CONSTRAINT [PK_DeviceAbnormal] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceAdvertisement] (
        [Id] bigint NOT NULL,
        [DeviceId] bigint NOT NULL,
        [Type] int NOT NULL,
        [CarouselIntervalSecond] int NOT NULL,
        [StandbyWaiteSecond] int NULL,
        [Enabled] bit NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceAdvertisement] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceAdvertisementFile] (
        [Id] bigint NOT NULL,
        [DeviceAdvertisementId] bigint NOT NULL,
        [Url] nvarchar(256) NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [Duration] int NULL,
        [Sort] int NOT NULL,
        [Suffix] nvarchar(256) NOT NULL,
        [IsFullScreenAd] bit NOT NULL,
        [FileLength] int NOT NULL,
        [Enable] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceAdvertisementFile] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceAttribute] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [Key] nvarchar(256) NOT NULL,
        [Name] nvarchar(256) NULL,
        [Value] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceAttribute] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceBaseInfo] (
        [Id] bigint NOT NULL,
        [Mid] nvarchar(256) NOT NULL,
        [MachineStickerCode] nvarchar(256) NOT NULL,
        [BoxId] nvarchar(256) NOT NULL,
        [DeviceModelId] bigint NULL,
        [IsLeaveFactory] int NOT NULL,
        [SSID] nvarchar(256) NULL,
        [HardwareCapability] int NULL,
        [SoftwareCapability] int NULL,
        [MAC] nvarchar(256) NULL,
        [VersionNumber] nvarchar(256) NULL,
        [SkinPluginVersion] nvarchar(256) NULL,
        [LanguagePack] nvarchar(256) NULL,
        [SoftwareUpdateLastTime] datetime2 NULL,
        [IsOnline] bit NOT NULL,
        [UpdateOnlineTime] datetime2 NULL,
        [UpdateOfflineTime] datetime2 NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceBaseInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceCapacityCfg] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [CapacityId] int NOT NULL,
        [Name] nvarchar(256) NULL,
        [CapacityType] int NOT NULL,
        [CfgInfo] nvarchar(MAX) NULL,
        [Premission] int NOT NULL,
        [Structure] int NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceCapacityCfg] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceEarlyWarnings] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [WarningType] int NOT NULL,
        [DeviceMaterialId] bigint NULL,
        [IsOn] bit NOT NULL,
        [WarningValue] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceEarlyWarnings] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceFlushComponents] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [Type] int NOT NULL,
        [Index] int NOT NULL,
        [Name] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceFlushComponents] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceFlushComponentsLog] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [MachineStickerCode] nvarchar(256) NOT NULL,
        [DeviceName] nvarchar(256) NULL,
        [Type] nvarchar(256) NOT NULL,
        [FlushType] int NOT NULL,
        [Parts] nvarchar(256) NOT NULL,
        [Status] int NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_DeviceFlushComponentsLog] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceInitialization] (
        [Mid] nvarchar(256) NOT NULL,
        [EquipmentNumber] nvarchar(256) NULL,
        [IMEI] nvarchar(256) NULL,
        [IsBind] bit NOT NULL,
        [PriKey] nvarchar(256) NOT NULL,
        [PubKey] nvarchar(256) NOT NULL,
        [ChanneId] nvarchar(256) NULL,
        [CreateDate] datetime2 NOT NULL,
        CONSTRAINT [PK_DeviceInitialization] PRIMARY KEY ([Mid])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceMaterialInfo] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [Type] int NOT NULL,
        [Index] int NOT NULL,
        [Name] nvarchar(256) NULL,
        [Capacity] int NOT NULL,
        [Stock] int NOT NULL,
        [IsSugar] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceMaterialInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceMetrics] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [CounterNo] int NOT NULL,
        [MetricType] int NOT NULL,
        [Type] int NOT NULL,
        [Index] int NOT NULL,
        [Value] nvarchar(256) NULL,
        [Status] int NOT NULL,
        [Description] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceMetrics] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceModel] (
        [Id] bigint NOT NULL,
        [Key] nvarchar(64) NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [Type] nvarchar(256) NOT NULL,
        [MaxCassetteCount] int NOT NULL,
        [Remark] nvarchar(512) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceModel] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DevicePaymentConfig] (
        [Id] bigint NOT NULL,
        [DeviceInfoId] bigint NULL,
        [PaymentConfigId] bigint NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DevicePaymentConfig] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DevicePaymentSetting] (
        [Id] bigint NOT NULL,
        [Currency] nvarchar(256) NULL,
        [PayWait] int NULL,
        [CurrencyLocationEnable] nvarchar(max) NULL,
        [PaymentCurrencyLocaton] nvarchar(max) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DevicePaymentSetting] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceRestockLog] (
        [Id] bigint NOT NULL,
        [DeviceId] bigint NOT NULL,
        [DeviceName] nvarchar(256) NULL,
        [DeviceCode] nvarchar(256) NOT NULL,
        [Code] nvarchar(256) NOT NULL,
        [DeviceDZ] nvarchar(256) NULL,
        [Type] int NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_DeviceRestockLog] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceSoftwareInfo] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [ProgramType] int NOT NULL,
        [Title] nvarchar(256) NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [VersionName] nvarchar(256) NULL,
        [VerId] bigint NULL,
        [Version] nvarchar(256) NULL,
        [Extra] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceSoftwareInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceVersionManage] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [DeviceType] nvarchar(256) NOT NULL,
        [VersionNumber] nvarchar(256) NULL,
        [DeviceModelId] bigint NULL,
        [ProgramType] int NULL,
        [ProgramTypeName] nvarchar(256) NULL,
        [VersionType] int NULL,
        [Url] nvarchar(256) NOT NULL,
        [Remark] nvarchar(256) NULL,
        [Enabled] int NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceVersionManage] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceVsersionUpdateRecord] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [DeviceVersionManageId] bigint NOT NULL,
        [Type] int NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [ProgramType] int NULL,
        [VersionType] int NULL,
        [ProgramTypeName] nvarchar(256) NULL,
        [PushState] int NOT NULL,
        [Message] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceVsersionUpdateRecord] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DictionaryEntity] (
        [Key] nvarchar(256) NOT NULL,
        [Value] nvarchar(256) NOT NULL,
        [ParentKey] nvarchar(256) NULL,
        [Remark] nvarchar(256) NULL,
        [IsEnabled] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_DictionaryEntity] PRIMARY KEY ([Key]),
        CONSTRAINT [FK_DictionaryEntity_DictionaryEntity_ParentKey] FOREIGN KEY ([ParentKey]) REFERENCES [DictionaryEntity] ([Key]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DivideAccountsConfig] (
        [Id] bigint NOT NULL,
        [SysPaymentMethodId] bigint NOT NULL,
        [MerchantId] bigint NOT NULL,
        [MerchantName] nvarchar(100) NOT NULL,
        [Bibliography] decimal(18,2) NOT NULL,
        [Remarks] nvarchar(100) NULL,
        [type] nvarchar(100) NULL,
        [AlipayRoyaltyType] int NULL,
        [account] nvarchar(100) NULL,
        [name] nvarchar(100) NULL,
        [relation_type] nvarchar(100) NULL,
        [VendCodes] nvarchar(max) NULL,
        [IsEnabled] int NOT NULL DEFAULT 1,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_DivideAccountsConfig] PRIMARY KEY ([Id])
    );
    DECLARE @defaultSchema AS sysname;
    SET @defaultSchema = SCHEMA_NAME();
    DECLARE @description AS sql_variant;
    SET @description = N'商户号';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'DivideAccountsConfig', 'COLUMN', N'MerchantId';
    SET @description = N'商户名称';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'DivideAccountsConfig', 'COLUMN', N'MerchantName';
    SET @description = N'分账比例%';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'DivideAccountsConfig', 'COLUMN', N'Bibliography';
    SET @description = N'备注';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'DivideAccountsConfig', 'COLUMN', N'Remarks';
    SET @description = N'微信接收方类型';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'DivideAccountsConfig', 'COLUMN', N'type';
    SET @description = N'支付宝接收方类型';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'DivideAccountsConfig', 'COLUMN', N'AlipayRoyaltyType';
    SET @description = N'接收方账号';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'DivideAccountsConfig', 'COLUMN', N'account';
    SET @description = N'分账接收方全称,分账接收方真实姓名';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'DivideAccountsConfig', 'COLUMN', N'name';
    SET @description = N'与分账方的关系类型';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'DivideAccountsConfig', 'COLUMN', N'relation_type';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [EnterpriseTypes] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [Status] bit NOT NULL,
        [Astrict] bit NOT NULL,
        [IsDefault] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_EnterpriseTypes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [FaultCode] (
        [Code] nvarchar(50) NOT NULL,
        [LanCode] nvarchar(64) NOT NULL,
        [Name] nvarchar(128) NOT NULL,
        [Type] int NULL,
        [Description] nvarchar(512) NULL,
        [Remark] nvarchar(512) NULL,
        CONSTRAINT [PK_FaultCode] PRIMARY KEY ([Code])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [FileCenter] (
        [Id] bigint NOT NULL,
        [FileName] nvarchar(256) NOT NULL,
        [Code] nvarchar(256) NOT NULL,
        [FileCenterType] int NOT NULL,
        [SysMenuType] int NOT NULL,
        [Url] nvarchar(256) NOT NULL,
        [FileState] int NOT NULL,
        [FaildMessage] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_FileCenter] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [FileManage] (
        [Id] bigint NOT NULL,
        [FileName] nvarchar(256) NOT NULL,
        [FilePath] nvarchar(256) NOT NULL,
        [FileType] nvarchar(256) NOT NULL,
        [FileSize] nvarchar(256) NOT NULL,
        [ResourceUsage] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_FileManage] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [FileRelation] (
        [Id] bigint NOT NULL,
        [FileId] bigint NOT NULL,
        [TargetId] bigint NOT NULL,
        [TargetType] int NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_FileRelation] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [Groups] (
        [Id] bigint NOT NULL,
        [EnterpriseInfoId] bigint NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [PId] bigint NULL,
        [Path] nvarchar(max) NULL,
        [Remark] nvarchar(128) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_Groups] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [InterfaceStyles] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [Code] nvarchar(256) NOT NULL,
        [Preview] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_InterfaceStyles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [LanguageInfo] (
        [Code] nvarchar(256) NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [IsEnabled] int NOT NULL,
        [IsDefault] int NOT NULL,
        CONSTRAINT [PK_LanguageInfo] PRIMARY KEY ([Code])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [M_PaymentAlipayApplyments] (
        [Id] bigint NOT NULL,
        [PaymentOriginId] bigint NOT NULL,
        [OrderId] nvarchar(64) NULL,
        [MerchantType] int NOT NULL,
        [Mcc] nvarchar(10) NOT NULL,
        [LegalCertType] int NOT NULL,
        [LegalCertFrontImage] nvarchar(255) NOT NULL,
        [LegalCertBackImage] nvarchar(255) NOT NULL,
        [LegalCertName] nvarchar(64) NOT NULL,
        [Phone] nvarchar(20) NOT NULL,
        [LegalCertNo] nvarchar(64) NOT NULL,
        [LegalCertAddress] nvarchar(100) NOT NULL,
        [LegalCertValidTimeBegin] nvarchar(50) NULL,
        [LegalCertValidTimeEnd] nvarchar(50) NULL,
        [MerchantShortName] nvarchar(128) NOT NULL,
        [BusinessAddress] nvarchar(1000) NOT NULL,
        [ServicePhone] nvarchar(20) NOT NULL,
        [BusinessAddressDetail] nvarchar(256) NOT NULL,
        [InDoorImages] nvarchar(256) NOT NULL,
        [OutDoorImages] nvarchar(256) NOT NULL,
        [BusinessLicenseImage] nvarchar(256) NULL,
        [AlipayLogonId] nvarchar(64) NOT NULL,
        [UnifiedSocialCreditCode] nvarchar(32) NULL,
        [BusinessLicenseName] nvarchar(64) NULL,
        [BusinessLicenseLegalName] nvarchar(64) NULL,
        [BusinessLicenseAddress] nvarchar(256) NULL,
        [ContactInfos] nvarchar(1000) NULL,
        [BizCards] nvarchar(2000) NULL,
        [LicenseAuthLetterImage] nvarchar(256) NULL,
        [Service] nvarchar(100) NOT NULL,
        [SignTimeWithIsv] nvarchar(20) NULL,
        [Sites] nvarchar(2000) NULL,
        [InvoiceInfo] nvarchar(2000) NULL,
        [BindingAlipayLogonId] nvarchar(64) NOT NULL,
        [Addtime] datetime2 NULL,
        [MerchantName] nvarchar(128) NOT NULL,
        [ApplyTime] datetime2 NULL,
        [ExtInfo] nvarchar(2048) NULL,
        [Smid] nvarchar(50) NULL,
        [CardAliasNo] nvarchar(50) NULL,
        [Memo] nvarchar(255) NULL,
        [Flowstatus] int NOT NULL,
        [RejectReason] nvarchar(255) NULL,
        [AppAuthToken] nvarchar(64) NULL,
        [VerifyUserId] bigint NULL,
        [VerifyTime] datetime2 NULL,
        [IsArtificialApplyment] int NULL DEFAULT 0,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_M_PaymentAlipayApplyments] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [M_PaymentMethod] (
        [Id] bigint NOT NULL,
        [SystemPaymentMethodId] bigint NOT NULL,
        [Phone] nvarchar(20) NOT NULL,
        [DomesticMerchantType] int NULL,
        [PaymentMode] int NOT NULL DEFAULT 0,
        [PaymentEntryStatus] int NOT NULL,
        [BindType] int NOT NULL DEFAULT 0,
        [IsEnabled] int NOT NULL DEFAULT 1,
        [Remark] nvarchar(500) NULL,
        [MerchantId] nvarchar(50) NOT NULL,
        [PaymentParameters] text NULL,
        [SystemPaymentServiceProviderId] bigint NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_M_PaymentMethod] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [M_PaymentMethodBindDevice] (
        [Id] bigint NOT NULL,
        [PaymentMethodId] bigint NOT NULL,
        [DeviceId] bigint NOT NULL,
        [SystemPaymentMethodId] bigint NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_M_PaymentMethodBindDevice] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [M_PaymentWechatApplyments] (
        [Id] bigint NOT NULL,
        [ApplymentId] nvarchar(256) NULL,
        [PaymentOriginId] bigint NOT NULL,
        [IdDocType] int NOT NULL,
        [IdCardName] nvarchar(50) NOT NULL,
        [IdCardNumber] nvarchar(30) NOT NULL,
        [IdCardAddress] nvarchar(255) NOT NULL,
        [IdCardValidTimeBegin] nvarchar(20) NOT NULL,
        [IdCardValidTime] nvarchar(20) NOT NULL,
        [IdCardCopy] nvarchar(255) NOT NULL,
        [IdCardNational] nvarchar(255) NOT NULL,
        [MobilePhone] nvarchar(20) NOT NULL,
        [Email] nvarchar(50) NOT NULL,
        [MerchantShortName] nvarchar(50) NOT NULL,
        [ServicePhone] nvarchar(20) NOT NULL,
        [BizProvinceCode] nvarchar(10) NULL,
        [BizCityCode] nvarchar(10) NULL,
        [BizStoreAddress] nvarchar(200) NULL,
        [StoreEntrancePic] nvarchar(255) NULL,
        [IndoorPic] nvarchar(255) NULL,
        [OrganizationType] int NOT NULL,
        [BusinessLicenseCopy] nvarchar(255) NULL,
        [BusinessLicenseNumber] nvarchar(18) NULL,
        [MerchantName] nvarchar(100) NULL,
        [LegalPerson] nvarchar(100) NULL,
        [LicenseAddress] nvarchar(200) NULL,
        [CertificateLetterCopy] nvarchar(255) NULL,
        [AccountName] nvarchar(50) NOT NULL,
        [AccountBank] nvarchar(100) NOT NULL,
        [BankAliasCode] nvarchar(50) NULL,
        [AccountNumber] nvarchar(50) NULL,
        [BankProvinceCode] nvarchar(10) NULL,
        [BankCityCode] nvarchar(10) NULL,
        [BankBranchId] nvarchar(50) NULL,
        [BankName] nvarchar(200) NULL,
        [ApplymentState] int NULL,
        [ApplymentStateDesc] nvarchar(255) NULL,
        [SignState] nvarchar(16) NULL,
        [SignUrl] nvarchar(255) NULL,
        [SubMchId] nvarchar(32) NULL,
        [FAccountName] nvarchar(100) NULL,
        [FAccountNo] nvarchar(50) NULL,
        [PayAmount] int NULL,
        [DestinationAccountNumber] nvarchar(50) NULL,
        [DestinationAccountName] nvarchar(100) NULL,
        [DestinationAccountBank] nvarchar(200) NULL,
        [DestinationCity] nvarchar(100) NULL,
        [DestinationRemark] nvarchar(100) NULL,
        [Deadline] nvarchar(20) NULL,
        [LegalValidationUrl] nvarchar(255) NULL,
        [AuditDetail] text NULL,
        [ApplyState] nvarchar(32) NULL,
        [RejectReason] nvarchar(255) NULL,
        [FlowStatus] int NOT NULL,
        [VerifyUserId] bigint NULL,
        [VerifyTime] datetime2 NULL,
        [CreateUserId] bigint NOT NULL,
        [CreatedOnUtc] datetime2 NOT NULL,
        [UpdateUserId] bigint NULL,
        [UpdatedOnUtc] datetime2 NULL,
        [IsArtificialApplyment] int NULL DEFAULT 0,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_M_PaymentWechatApplyments] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [NoticeCfg] (
        [Id] bigint NOT NULL,
        [Type] int NOT NULL,
        [UserId] bigint NOT NULL,
        [UserName] nvarchar(256) NOT NULL,
        [Status] bit NOT NULL,
        [Method] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_NoticeCfg] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [NotityMsg] (
        [Id] bigint NOT NULL,
        [MsgName] nvarchar(256) NOT NULL,
        [Type] int NOT NULL,
        [ContactAddress] nvarchar(256) NOT NULL,
        [Msg] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_NotityMsg] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [OperationLog] (
        [Id] bigint NOT NULL,
        [Code] nvarchar(256) NOT NULL,
        [Mid] nvarchar(256) NOT NULL,
        [OperationResult] int NOT NULL,
        [OperationName] nvarchar(256) NOT NULL,
        [RequestWayType] nvarchar(256) NOT NULL,
        [RequestType] int NOT NULL,
        [IpAddress] nvarchar(256) NOT NULL,
        [RequestUrl] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        [BaseEnterpriseId] bigint NULL,
        [BaseEnterpriseName] nvarchar(256) NULL,
        [BaseDeviceId] bigint NULL,
        [BaseDeviceName] nvarchar(256) NULL,
        [BaseDeviceModelId] bigint NULL,
        [BaseDeviceModelName] nvarchar(256) NULL,
        CONSTRAINT [PK_OperationLog] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [OrderInfo] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [Code] nvarchar(256) NOT NULL,
        [BizNo] nvarchar(256) NOT NULL,
        [ThirdOrderId] nvarchar(256) NULL,
        [ThirdOrderNo] nvarchar(256) NULL,
        [CustomContent] nvarchar(256) NULL,
        [Amount] decimal(18,2) NOT NULL,
        [CurrencyCode] nvarchar(256) NOT NULL,
        [CurrencySymbol] nvarchar(256) NOT NULL,
        [Provider] nvarchar(256) NOT NULL,
        [PayTimeSp] bigint NOT NULL,
        [PayDateTime] datetime2 NULL,
        [ShipmentResult] int NOT NULL,
        [SaleResult] int NOT NULL,
        [ErrCode] nvarchar(256) NULL,
        [PayErrMsg] nvarchar(256) NULL,
        [ReturnAmount] decimal(18,2) NOT NULL,
        [OrderType] int NULL,
        [OrderStatus] int NULL,
        [PaymentMerchantId] nvarchar(256) NULL,
        [SystemPaymentMethodId] bigint NULL,
        [SystemPaymentServiceProviderId] bigint NULL,
        [OrderPaymentType] int NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        [BaseEnterpriseId] bigint NULL,
        [BaseEnterpriseName] nvarchar(256) NULL,
        [BaseDeviceId] bigint NULL,
        [BaseDeviceName] nvarchar(256) NULL,
        [BaseDeviceModelId] bigint NULL,
        [BaseDeviceModelName] nvarchar(256) NULL,
        CONSTRAINT [PK_OrderInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [OrderRefund] (
        [Id] bigint NOT NULL,
        [OrderId] nvarchar(32) NOT NULL,
        [OrderDetailId] nvarchar(32) NOT NULL,
        [RefundOrderNo] nvarchar(128) NOT NULL,
        [ItemCode] nvarchar(256) NULL,
        [ProductId] bigint NULL,
        [BarCode] nvarchar(100) NOT NULL,
        [Name] nvarchar(300) NULL,
        [MainImage] nvarchar(300) NULL,
        [RefundAmount] decimal(18,4) NOT NULL DEFAULT 0.0,
        [RefundReason] nvarchar(100) NULL,
        [RefundStatus] nvarchar(max) NOT NULL DEFAULT N'Success',
        [HandlingMethod] nvarchar(max) NOT NULL DEFAULT N'FullRefund',
        [OrderCreatedOnUtc] datetime2 NOT NULL,
        [InitiationTime] datetime2 NOT NULL,
        [SuccessTime] datetime2 NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        [BaseEnterpriseId] bigint NULL,
        [BaseEnterpriseName] nvarchar(256) NULL,
        [BaseDeviceId] bigint NULL,
        [BaseDeviceName] nvarchar(256) NULL,
        [BaseDeviceModelId] bigint NULL,
        [BaseDeviceModelName] nvarchar(256) NULL,
        CONSTRAINT [PK_OrderRefund] PRIMARY KEY ([Id])
    );
    DECLARE @defaultSchema AS sysname;
    SET @defaultSchema = SCHEMA_NAME();
    DECLARE @description AS sql_variant;
    SET @description = N'主订单号（Order表的OrderId字段）';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'OrderId';
    SET @description = N'（OrderDetail表的Id字段）';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'OrderDetailId';
    SET @description = N'退款的交易订单号（支付平台OrderRefund表的Id）';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'RefundOrderNo';
    SET @description = N'饮品SKU';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'ItemCode';
    SET @description = N'商品表的Id（Product表的Id）';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'ProductId';
    SET @description = N'商品条码（Product表的BarCode）';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'BarCode';
    SET @description = N'商品名称（Product表的Name）';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'Name';
    SET @description = N'商品主图（Product表的MainImage）';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'MainImage';
    SET @description = N'退款金额';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'RefundAmount';
    SET @description = N'退款原因';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'RefundReason';
    SET @description = N'退款状态';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'RefundStatus';
    SET @description = N'处理方式';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'HandlingMethod';
    SET @description = N'订单创建时间（Order表的OrderCreatedOnUtc，一定要保持一致）';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'OrderCreatedOnUtc';
    SET @description = N'退款发起时间';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'InitiationTime';
    SET @description = N'退款成功时间';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'OrderRefund', 'COLUMN', N'SuccessTime';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [P_BeverageCollection] (
        [Id] bigint NOT NULL,
        [LanguageKey] nvarchar(256) NOT NULL,
        [DeviceModelId] bigint NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [BeverageIds] nvarchar(max) NOT NULL,
        [BeverageNames] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_P_BeverageCollection] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [P_BeverageInfo] (
        [Id] bigint NOT NULL,
        [Price] decimal(18,2) NULL,
        [DiscountedPrice] decimal(18,2) NULL,
        [LanguageKey] nvarchar(256) NOT NULL,
        [DeviceModelId] bigint NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [BeverageIcon] nvarchar(256) NOT NULL,
        [Code] nvarchar(64) NULL,
        [Temperature] nvarchar(30) NOT NULL,
        [Remarks] nvarchar(256) NOT NULL,
        [ProductionForecast] nvarchar(256) NOT NULL,
        [ForecastQuantity] float NOT NULL,
        [IsDefault] bit NOT NULL,
        [DisplayStatus] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_P_BeverageInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [P_FileManage] (
        [Id] bigint NOT NULL IDENTITY,
        [FileName] nvarchar(256) NOT NULL,
        [FilePath] nvarchar(256) NOT NULL,
        [FileType] nvarchar(256) NOT NULL,
        [FileSize] nvarchar(256) NOT NULL,
        [ResourceUsage] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_P_FileManage] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [P_PaymentConfig] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [Countrys] nvarchar(256) NOT NULL,
        [PaymentModel] nvarchar(max) NOT NULL,
        [PictureUrl] nvarchar(256) NOT NULL,
        [Enabled] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_P_PaymentConfig] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [PaymentConfig] (
        [Id] bigint NOT NULL,
        [P_PaymentConfigId] bigint NOT NULL,
        [Email] nvarchar(256) NOT NULL,
        [PaymentConfigStatue] nvarchar(max) NOT NULL,
        [MerchantCode] nvarchar(256) NOT NULL,
        [PaymentPlatformAppId] nvarchar(256) NOT NULL,
        [Remark] nvarchar(256) NOT NULL,
        [Enabled] nvarchar(max) NOT NULL,
        [PictureUrl] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_PaymentConfig] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [PrivateGoodsRepository] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [Sku] nvarchar(256) NOT NULL,
        [SuggestedPrice] decimal(18,2) NOT NULL,
        [IsEnable] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_PrivateGoodsRepository] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [ProductCategory] (
        [Id] bigint NOT NULL,
        [ParentId] bigint NULL,
        [Name] nvarchar(64) NOT NULL,
        [ImageUrl] nvarchar(256) NOT NULL,
        [IconUrl] nvarchar(256) NULL,
        [ProductCategoryType] nvarchar(30) NOT NULL,
        [IsEnabled] int NOT NULL,
        [Level] int NOT NULL,
        [Sort] int NOT NULL,
        [Description] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_ProductCategory] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [Promotion] (
        [Id] bigint NOT NULL,
        [Sort] int NULL,
        [Name] nvarchar(200) NOT NULL,
        [Number] int NOT NULL,
        [StartTime] datetime2 NOT NULL,
        [EndTime] datetime2 NOT NULL,
        [CouponType] int NOT NULL,
        [TotalLimit] int NOT NULL,
        [LimitedCount] int NOT NULL,
        [ParticipatingUsers] nvarchar(max) NULL,
        [DiscountType] int NOT NULL,
        [DiscountValue] decimal(18,2) NOT NULL,
        [MaxDiscountAmount] decimal(18,2) NULL,
        [ApplicableProductsType] int NOT NULL DEFAULT 0,
        [ApplicableDrinks] nvarchar(max) NULL,
        [MinOrderAmount] decimal(18,2) NOT NULL DEFAULT 0.0,
        [CanCombineWithOtherOffers] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_Promotion] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [ServiceProviderInfo] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [Tel] nvarchar(64) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_ServiceProviderInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [SysProvinceCity] (
        [Name] nvarchar(50) NOT NULL,
        [Code] nvarchar(50) NOT NULL,
        [ParentCode] nvarchar(50) NOT NULL,
        [Type] int NOT NULL,
        [CreateOnUtc] datetime2 NOT NULL
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [SystemMessages] (
        [Id] bigint NOT NULL,
        [Title] nvarchar(100) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [MessageType] int NOT NULL,
        [TargetUserId] bigint NULL,
        [TargetGroup] nvarchar(500) NULL,
        [IsPopup] bit NOT NULL,
        [Priority] tinyint NOT NULL,
        [ExpireTime] datetime2 NULL,
        [IsCanceled] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_SystemMessages] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [SystemPaymentMethod] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [FatherId] bigint NOT NULL DEFAULT CAST(0 AS bigint),
        [PaymentImage] nvarchar(500) NOT NULL,
        [OnlinePayment] bit NOT NULL,
        [OfflinePayment] bit NOT NULL,
        [Country] nvarchar(500) NOT NULL,
        [LanguageTextCode] nvarchar(100) NOT NULL,
        [IsEnabled] int NOT NULL DEFAULT 1,
        [PaymentPlatformId] bigint NOT NULL DEFAULT CAST(0 AS bigint),
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_SystemPaymentMethod] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [SystemPaymentServiceProvider] (
        [Id] bigint NOT NULL,
        [SpMerchantId] nvarchar(20) NOT NULL,
        [Name] nvarchar(50) NOT NULL,
        [AppletAppID] nvarchar(50) NULL,
        [AppKey] text NULL,
        [ApiV3Key] text NULL,
        [NotifyUrl] nvarchar(100) NULL,
        [PaymentPlatformType] int NOT NULL DEFAULT 0,
        [IsDefault] int NOT NULL,
        [CretFileUrl] nvarchar(255) NULL,
        [CretPassWrod] nvarchar(255) NULL,
        [PlatformSerialNumber] nvarchar(255) NULL,
        [PlatformPublicKey] text NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_SystemPaymentServiceProvider] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [TaskSchedulingInfo] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [Description] nvarchar(512) NULL,
        [CronExpression] nvarchar(64) NOT NULL,
        [IsEnabled] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_TaskSchedulingInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [TermServiceEntity] (
        [Id] bigint NOT NULL,
        [Title] nvarchar(128) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [Description] nvarchar(256) NULL,
        [Enabled] nvarchar(30) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_TermServiceEntity] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [TimeZoneInfo] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [Code] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_TimeZoneInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [ApplicationRoleMenu] (
        [Id] bigint NOT NULL,
        [RoleId] bigint NOT NULL,
        [MenuId] bigint NOT NULL,
        [IsHalf] bit NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_ApplicationRoleMenu] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicationRoleMenu_ApplicationMenu_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [ApplicationMenu] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ApplicationRoleMenu_ApplicationRole_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [ApplicationRole] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [ApplicationUserRole] (
        [Id] bigint NOT NULL,
        [UserId] bigint NOT NULL,
        [RoleId] bigint NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_ApplicationUserRole] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicationUserRole_ApplicationRole_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [ApplicationRole] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ApplicationUserRole_ApplicationUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUser] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [ServiceAuthorizationRecord] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(64) NOT NULL,
        [FounderId] bigint NOT NULL,
        [FounderUserId] bigint NOT NULL,
        [ServiceUserAccount] nvarchar(256) NOT NULL,
        [ServiceUserId] bigint NOT NULL,
        [AuthorizationStartTime] datetime2 NOT NULL,
        [AuthorizationEndTime] datetime2 NULL,
        [State] nvarchar(30) NOT NULL,
        [Remark] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_ServiceAuthorizationRecord] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ServiceAuthorizationRecord_ApplicationUser_FounderUserId] FOREIGN KEY ([FounderUserId]) REFERENCES [ApplicationUser] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ServiceAuthorizationRecord_ApplicationUser_ServiceUserId] FOREIGN KEY ([ServiceUserId]) REFERENCES [ApplicationUser] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [BeverageTemplateVersion] (
        [Id] bigint NOT NULL,
        [BeverageInfoTemplateId] bigint NOT NULL,
        [VersionType] nvarchar(30) NOT NULL,
        [BeverageInfoDataString] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_BeverageTemplateVersion] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BeverageTemplateVersion_BeverageInfoTemplate_BeverageInfoTemplateId] FOREIGN KEY ([BeverageInfoTemplateId]) REFERENCES [BeverageInfoTemplate] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [FormulaInfoTemplate] (
        [Id] bigint NOT NULL,
        [BeverageInfoTemplateId] bigint NOT NULL,
        [MaterialBoxId] bigint NULL,
        [MaterialBoxName] nvarchar(64) NULL,
        [Sort] int NOT NULL,
        [FormulaType] int NOT NULL,
        [SpecsString] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_FormulaInfoTemplate] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FormulaInfoTemplate_BeverageInfoTemplate_BeverageInfoTemplateId] FOREIGN KEY ([BeverageInfoTemplateId]) REFERENCES [BeverageInfoTemplate] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [CountryRegion] (
        [Id] bigint NOT NULL,
        [RegionName] nvarchar(128) NOT NULL,
        [ParentID] bigint NULL,
        [Code] nvarchar(256) NULL,
        [ParentCode] nvarchar(256) NULL,
        [Type] int NULL,
        [HasChildren] bit NOT NULL,
        [Sort] int NOT NULL,
        [CountryID] bigint NOT NULL,
        [IsEnabled] nvarchar(30) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        CONSTRAINT [PK_CountryRegion] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CountryRegion_CountryInfo_CountryID] FOREIGN KEY ([CountryID]) REFERENCES [CountryInfo] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CountryRegion_CountryRegion_ParentID] FOREIGN KEY ([ParentID]) REFERENCES [CountryRegion] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceHardwares] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [Code] nvarchar(256) NOT NULL,
        [HardwareName] nvarchar(256) NULL,
        [HardwareType] int NOT NULL,
        [PictureUrl] nvarchar(256) NULL,
        [Status] bit NOT NULL,
        [Message] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceHardwares] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DeviceHardwares_DeviceBaseInfo_DeviceBaseId] FOREIGN KEY ([DeviceBaseId]) REFERENCES [DeviceBaseInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceInfo] (
        [Id] bigint NOT NULL,
        [DeviceBaseId] bigint NOT NULL,
        [Name] nvarchar(64) NULL,
        [ActiveTime] datetime2 NULL,
        [DeviceActiveState] int NULL,
        [EquipmentNumber] nvarchar(64) NULL,
        [Mid] nvarchar(256) NULL,
        [DeviceModelId] bigint NULL,
        [IsLeaveFactory] int NULL,
        [VersionNumber] nvarchar(64) NULL,
        [SkinPluginVersion] nvarchar(64) NULL,
        [LanguagePack] nvarchar(64) NULL,
        [UpdateTime] datetime2 NULL,
        [SSID] nvarchar(64) NULL,
        [MAC] nvarchar(64) NULL,
        [ICCID] nvarchar(64) NULL,
        [UsedTrafficThisMonth] nvarchar(64) NULL,
        [RemainingTrafficThisMonth] nvarchar(64) NULL,
        [Longitude] nvarchar(64) NULL,
        [Latitude] nvarchar(64) NULL,
        [LatestOnlineTime] datetime2 NULL,
        [LatestOfflineTime] datetime2 NULL,
        [CountryId] bigint NULL,
        [CountryRegionIds] nvarchar(256) NULL,
        [CountryRegionText] nvarchar(256) NULL,
        [DetailedAddress] nvarchar(256) NULL,
        [DeviceStatus] int NOT NULL,
        [UsageScenario] nvarchar(30) NULL,
        [POSMachineNumber] nvarchar(64) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_DeviceInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DeviceInfo_DeviceModel_DeviceModelId] FOREIGN KEY ([DeviceModelId]) REFERENCES [DeviceModel] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceRestockLogSub] (
        [Id] bigint NOT NULL,
        [HGType] int NOT NULL,
        [MaterialId] bigint NOT NULL,
        [MaterialName] nvarchar(256) NOT NULL,
        [OldValue] int NOT NULL,
        [Value] int NOT NULL,
        [NewValue] int NOT NULL,
        [DeviceRestockLogId] bigint NOT NULL,
        CONSTRAINT [PK_DeviceRestockLogSub] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DeviceRestockLogSub_DeviceRestockLog_DeviceRestockLogId] FOREIGN KEY ([DeviceRestockLogId]) REFERENCES [DeviceRestockLog] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [EnterpriseInfo] (
        [Id] bigint NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [EnterpriseTypeId] bigint NULL,
        [AreaRelationId] bigint NULL,
        [Pid] bigint NULL,
        [IsDefault] bit NOT NULL,
        [MenuIds] nvarchar(max) NOT NULL,
        [HalfMenuIds] nvarchar(max) NULL,
        [H5MenuIds] nvarchar(max) NOT NULL,
        [H5HalfMenuIds] nvarchar(max) NULL,
        [OrganizationType] int NULL,
        [RegistrationProgress] int NULL,
        [Remark] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_EnterpriseInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EnterpriseInfo_EnterpriseInfo_Pid] FOREIGN KEY ([Pid]) REFERENCES [EnterpriseInfo] ([Id]),
        CONSTRAINT [FK_EnterpriseInfo_EnterpriseTypes_EnterpriseTypeId] FOREIGN KEY ([EnterpriseTypeId]) REFERENCES [EnterpriseTypes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [GroupUsers] (
        [Id] bigint NOT NULL,
        [GroupsId] bigint NOT NULL,
        [ApplicationUserId] bigint NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_GroupUsers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GroupUsers_ApplicationUser_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [ApplicationUser] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_GroupUsers_Groups_GroupsId] FOREIGN KEY ([GroupsId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [SysLanguageText] (
        [Id] bigint NOT NULL,
        [Code] nvarchar(50) NOT NULL,
        [Value] nvarchar(max) NOT NULL,
        [LangCode] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_SysLanguageText] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SysLanguageText_LanguageInfo_LangCode] FOREIGN KEY ([LangCode]) REFERENCES [LanguageInfo] ([Code]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [OperationSubLog] (
        [OperationLogId] bigint NOT NULL,
        [Mid] nvarchar(256) NOT NULL,
        [DeviceName] nvarchar(256) NOT NULL,
        [DeviceModelName] nvarchar(256) NULL,
        [RequestMsg] nvarchar(MAX) NULL,
        [AppliedType] int NULL,
        [ContentType] int NULL,
        [ReplaceTarget] nvarchar(256) NULL,
        [OperationResult] int NOT NULL,
        [ErrorMsg] nvarchar(256) NULL,
        CONSTRAINT [PK_OperationSubLog] PRIMARY KEY ([Mid], [OperationLogId]),
        CONSTRAINT [FK_OperationSubLog_OperationLog_OperationLogId] FOREIGN KEY ([OperationLogId]) REFERENCES [OperationLog] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [OrderDetails] (
        [Id] bigint NOT NULL,
        [OrderId] bigint NOT NULL,
        [CounterNo] int NOT NULL,
        [SlotNo] int NOT NULL,
        [ItemCode] nvarchar(256) NOT NULL,
        [BeverageName] nvarchar(256) NOT NULL,
        [Url] nvarchar(256) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Quantity] int NOT NULL,
        [Result] int NOT NULL,
        [Error] int NOT NULL,
        [ErrorDescription] nvarchar(256) NULL,
        [ActionTimeSp] bigint NOT NULL,
        [BeverageInfoData] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderDetails_OrderInfo_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [OrderInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [P_BeverageVersion] (
        [Id] bigint NOT NULL,
        [BeverageInfoId] bigint NOT NULL,
        [VersionType] nvarchar(30) NOT NULL,
        [BeverageInfoDataString] NVARCHAR(MAX) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_P_BeverageVersion] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_P_BeverageVersion_P_BeverageInfo_BeverageInfoId] FOREIGN KEY ([BeverageInfoId]) REFERENCES [P_BeverageInfo] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [Coupon] (
        [Id] bigint NOT NULL,
        [CampaignId] bigint NOT NULL,
        [UserId] bigint NOT NULL,
        [UseType] int NOT NULL,
        [Status] int NOT NULL,
        [ValidFrom] datetime2 NULL,
        [ValidTo] datetime2 NULL,
        [UseDay] int NULL,
        [UsedTime] datetime2 NULL,
        [OrderId] bigint NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_Coupon] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Coupon_Promotion_CampaignId] FOREIGN KEY ([CampaignId]) REFERENCES [Promotion] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [UserMessages] (
        [Id] bigint NOT NULL,
        [MessageId] bigint NOT NULL,
        [UserId] bigint NOT NULL,
        [IsRead] bit NOT NULL,
        [ReadTime] datetime2 NULL,
        [IsPopupShown] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_UserMessages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserMessages_SystemMessages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [SystemMessages] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [UserReadGlobalMessages] (
        [Id] bigint NOT NULL,
        [MessageId] bigint NOT NULL,
        [UserId] bigint NOT NULL,
        [ReadTime] datetime2 NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_UserReadGlobalMessages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserReadGlobalMessages_SystemMessages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [SystemMessages] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [AdvertisementInfo] (
        [Id] bigint NOT NULL,
        [DeviceInfoId] bigint NOT NULL,
        [PowerOnAdsVolume] int NOT NULL,
        [PowerOnAdsPlayTime] int NOT NULL,
        [PowerOnAdsImagesJson] nvarchar(256) NOT NULL,
        [StandbyAdsVolume] int NOT NULL,
        [StandbyAdsPlayTime] int NOT NULL,
        [StandbyAdsAwaitTime] int NOT NULL,
        [StandbyAdsLoopTime] int NOT NULL,
        [StandbyAdsLoopType] bit NOT NULL,
        [StandbyAdsImagesJson] nvarchar(256) NOT NULL,
        [ProductionAdsVolume] int NOT NULL,
        [ProductionAdsPlayTime] int NOT NULL,
        [ProductionAdsImagesJson] nvarchar(256) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_AdvertisementInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AdvertisementInfo_DeviceInfo_DeviceInfoId] FOREIGN KEY ([DeviceInfoId]) REFERENCES [DeviceInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [AuthorizedDevices] (
        [Id] bigint NOT NULL,
        [ServiceAuthorizationRecordId] bigint NOT NULL,
        [DeviceId] bigint NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_AuthorizedDevices] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AuthorizedDevices_DeviceInfo_DeviceId] FOREIGN KEY ([DeviceId]) REFERENCES [DeviceInfo] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AuthorizedDevices_ServiceAuthorizationRecord_ServiceAuthorizationRecordId] FOREIGN KEY ([ServiceAuthorizationRecordId]) REFERENCES [ServiceAuthorizationRecord] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [BeverageInfo] (
        [Id] bigint NOT NULL,
        [DeviceId] bigint NOT NULL,
        [CategoryIds] nvarchar(max) NULL,
        [Price] decimal(18,4) NULL,
        [DiscountedPrice] decimal(18,4) NULL,
        [Name] nvarchar(64) NOT NULL,
        [BeverageIcon] nvarchar(256) NOT NULL,
        [Code] nvarchar(64) NOT NULL,
        [CodeIsShow] bit NOT NULL,
        [Temperature] nvarchar(30) NOT NULL,
        [Remarks] nvarchar(256) NULL,
        [ProductionForecast] nvarchar(256) NULL,
        [ForecastQuantity] float NOT NULL,
        [IsDefault] bit NOT NULL,
        [DisplayStatus] bit NOT NULL,
        [Sort] int NULL,
        [SellStradgy] nvarchar(256) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_BeverageInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BeverageInfo_DeviceInfo_DeviceId] FOREIGN KEY ([DeviceId]) REFERENCES [DeviceInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [CardDeviceAssignment] (
        [Id] bigint NOT NULL,
        [CardId] bigint NOT NULL,
        [DeviceId] bigint NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_CardDeviceAssignment] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CardDeviceAssignment_CardInfo_CardId] FOREIGN KEY ([CardId]) REFERENCES [CardInfo] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CardDeviceAssignment_DeviceInfo_DeviceId] FOREIGN KEY ([DeviceId]) REFERENCES [DeviceInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceServiceProviders] (
        [Id] bigint NOT NULL,
        [ServiceProviderInfoId] bigint NOT NULL,
        [DeviceInfoId] bigint NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceServiceProviders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DeviceServiceProviders_DeviceInfo_DeviceInfoId] FOREIGN KEY ([DeviceInfoId]) REFERENCES [DeviceInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [DeviceUserAssociation] (
        [Id] bigint NOT NULL,
        [DeviceId] bigint NOT NULL,
        [UserId] bigint NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_DeviceUserAssociation] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DeviceUserAssociation_ApplicationUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUser] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_DeviceUserAssociation_DeviceInfo_DeviceId] FOREIGN KEY ([DeviceId]) REFERENCES [DeviceInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [EarlyWarningConfig] (
        [Id] bigint NOT NULL,
        [DeviceInfoId] bigint NOT NULL,
        [WholeMachineCleaningSwitch] bit NOT NULL,
        [NextWholeMachineCleaningTime] datetime2 NOT NULL,
        [BrewingMachineCleaningSwitch] bit NOT NULL,
        [NextBrewingMachineCleaningTime] datetime2 NOT NULL,
        [MilkFrotherCleaningSwitch] bit NOT NULL,
        [NextMilkFrotherCleaningTime] datetime2 NOT NULL,
        [CoffeeWaterwayCleaningSwitch] bit NOT NULL,
        [NextCoffeeWaterwayCleaningTime] datetime2 NOT NULL,
        [SteamWaterwayCleaningSwitch] bit NOT NULL,
        [NextSteamWaterwayCleaningTime] datetime2 NOT NULL,
        [OfflineWarningSwitch] bit NOT NULL,
        [OfflineDays] int NOT NULL,
        [ShortageWarningSwitch] bit NOT NULL,
        [CoffeeBeanRemaining] float NOT NULL,
        [WaterRemaining] float NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_EarlyWarningConfig] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EarlyWarningConfig_DeviceInfo_DeviceInfoId] FOREIGN KEY ([DeviceInfoId]) REFERENCES [DeviceInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [GroupDevices] (
        [Id] bigint NOT NULL,
        [GroupsId] bigint NOT NULL,
        [DeviceInfoId] bigint NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_GroupDevices] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GroupDevices_DeviceInfo_DeviceInfoId] FOREIGN KEY ([DeviceInfoId]) REFERENCES [DeviceInfo] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_GroupDevices_Groups_GroupsId] FOREIGN KEY ([GroupsId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [SettingInfo] (
        [Id] bigint NOT NULL,
        [DeviceId] bigint NOT NULL,
        [IsShowEquipmentNumber] bit NOT NULL,
        [InterfaceStylesId] bigint NOT NULL,
        [WashType] int NOT NULL,
        [RegularWashTime] nvarchar(256) NULL,
        [WashWarning] int NULL,
        [AfterSalesPhone] nvarchar(256) NULL,
        [ExpectedUpdateTime] nvarchar(256) NOT NULL,
        [ScreenBrightness] int NOT NULL,
        [DeviceSound] int NOT NULL,
        [AdministratorPwd] nvarchar(256) NOT NULL,
        [ReplenishmentOfficerPwd] nvarchar(256) NOT NULL,
        [StartTime] nvarchar(256) NOT NULL,
        [StartWeek] int NOT NULL,
        [EndTime] nvarchar(256) NOT NULL,
        [EndWeek] int NOT NULL,
        [LanguageName] nvarchar(256) NOT NULL,
        [CurrencyCode] nvarchar(256) NOT NULL,
        [CurrencySymbol] nvarchar(256) NOT NULL,
        [CurrencyName] nvarchar(256) NOT NULL,
        [CurrencyPosition] int NOT NULL,
        [CurrencyDecimalDigits] int NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_SettingInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SettingInfo_DeviceInfo_DeviceId] FOREIGN KEY ([DeviceId]) REFERENCES [DeviceInfo] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_SettingInfo_InterfaceStyles_InterfaceStylesId] FOREIGN KEY ([InterfaceStylesId]) REFERENCES [InterfaceStyles] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [EnterpriseDevices] (
        [Id] bigint NOT NULL,
        [DeviceId] bigint NOT NULL,
        [BelongingEnterpriseId] bigint NOT NULL,
        [EnterpriseId] bigint NOT NULL,
        [DeviceAllocationType] nvarchar(30) NOT NULL,
        [RecyclingTime] datetime2 NULL,
        [AllocateTime] datetime2 NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        [EnterpriseinfoId] bigint NOT NULL,
        CONSTRAINT [PK_EnterpriseDevices] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EnterpriseDevices_DeviceInfo_DeviceId] FOREIGN KEY ([DeviceId]) REFERENCES [DeviceInfo] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_EnterpriseDevices_EnterpriseInfo_EnterpriseId] FOREIGN KEY ([EnterpriseId]) REFERENCES [EnterpriseInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [EnterpriseQualificationInfo] (
        [Id] bigint NOT NULL,
        [EnterpriseId] bigint NOT NULL,
        [LegalPersonName] nvarchar(64) NULL,
        [LegalPersonIdCardNumber] nvarchar(64) NULL,
        [FrontImageUrl] nvarchar(max) NULL,
        [BackImageUrl] nvarchar(max) NULL,
        [CustomerServiceEmail] nvarchar(100) NULL,
        [StoreAddress] nvarchar(500) NULL,
        [BusinessLicenseUrl] nvarchar(max) NULL,
        [Othercertificate] nvarchar(max) NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_EnterpriseQualificationInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EnterpriseQualificationInfo_EnterpriseInfo_EnterpriseId] FOREIGN KEY ([EnterpriseId]) REFERENCES [EnterpriseInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [EnterpriseRole] (
        [Id] bigint NOT NULL,
        [EnterpriseId] bigint NOT NULL,
        [RoleId] bigint NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_EnterpriseRole] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EnterpriseRole_ApplicationRole_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [ApplicationRole] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_EnterpriseRole_EnterpriseInfo_EnterpriseId] FOREIGN KEY ([EnterpriseId]) REFERENCES [EnterpriseInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [EnterpriseUser] (
        [Id] bigint NOT NULL,
        [EnterpriseId] bigint NOT NULL,
        [UserId] bigint NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_EnterpriseUser] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EnterpriseUser_ApplicationUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUser] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_EnterpriseUser_EnterpriseInfo_EnterpriseId] FOREIGN KEY ([EnterpriseId]) REFERENCES [EnterpriseInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [OrderDetaliMaterial] (
        [Id] bigint NOT NULL,
        [OrderDetailsId] bigint NOT NULL,
        [DeviceMaterialInfoId] bigint NOT NULL,
        [Value] int NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_OrderDetaliMaterial] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderDetaliMaterial_OrderDetails_OrderDetailsId] FOREIGN KEY ([OrderDetailsId]) REFERENCES [OrderDetails] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [BeverageVersion] (
        [Id] bigint NOT NULL,
        [BeverageInfoId] bigint NOT NULL,
        [VersionType] nvarchar(30) NOT NULL,
        [BeverageInfoDataString] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_BeverageVersion] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BeverageVersion_BeverageInfo_BeverageInfoId] FOREIGN KEY ([BeverageInfoId]) REFERENCES [BeverageInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [MaterialBox] (
        [Id] bigint NOT NULL,
        [SettingInfoId] bigint NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [IsActive] bit NOT NULL,
        [Sort] int NOT NULL,
        [MinMeasureWarning] float NOT NULL,
        [MinQuantityWarning] float NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_MaterialBox] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MaterialBox_SettingInfo_SettingInfoId] FOREIGN KEY ([SettingInfoId]) REFERENCES [SettingInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [FormulaInfo] (
        [Id] bigint NOT NULL,
        [BeverageInfoId] bigint NOT NULL,
        [MaterialBoxId] bigint NULL,
        [MaterialBoxName] nvarchar(64) NULL,
        [Sort] int NOT NULL,
        [FormulaType] nvarchar(30) NOT NULL,
        [SpecsString] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_FormulaInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FormulaInfo_BeverageInfo_BeverageInfoId] FOREIGN KEY ([BeverageInfoId]) REFERENCES [BeverageInfo] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_FormulaInfo_MaterialBox_MaterialBoxId] FOREIGN KEY ([MaterialBoxId]) REFERENCES [MaterialBox] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE TABLE [P_FormulaInfo] (
        [Id] bigint NOT NULL,
        [BeverageInfoId] bigint NOT NULL,
        [MaterialBoxId] bigint NULL,
        [MaterialBoxName] nvarchar(64) NULL,
        [Sort] int NOT NULL,
        [FormulaType] nvarchar(30) NOT NULL,
        [SpecsString] nvarchar(max) NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [LastModifyTime] datetime2 NULL,
        [CreateUserId] bigint NULL,
        [CreateUserName] nvarchar(256) NULL,
        [LastModifyUserId] bigint NULL,
        [LastModifyUserName] nvarchar(256) NULL,
        [IsDelete] bit NOT NULL,
        CONSTRAINT [PK_P_FormulaInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_P_FormulaInfo_MaterialBox_MaterialBoxId] FOREIGN KEY ([MaterialBoxId]) REFERENCES [MaterialBox] ([Id]),
        CONSTRAINT [FK_P_FormulaInfo_P_BeverageInfo_BeverageInfoId] FOREIGN KEY ([BeverageInfoId]) REFERENCES [P_BeverageInfo] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_AdvertisementInfo_DeviceInfoId] ON [AdvertisementInfo] ([DeviceInfoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_ApplicationMenu_ParentId] ON [ApplicationMenu] ([ParentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_ApplicationRoleMenu_MenuId] ON [ApplicationRoleMenu] ([MenuId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_ApplicationRoleMenu_RoleId] ON [ApplicationRoleMenu] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_ApplicationUserRole_RoleId] ON [ApplicationUserRole] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_ApplicationUserRole_UserId] ON [ApplicationUserRole] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_AuthorizedDevices_DeviceId] ON [AuthorizedDevices] ([DeviceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_AuthorizedDevices_ServiceAuthorizationRecordId] ON [AuthorizedDevices] ([ServiceAuthorizationRecordId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_BeverageInfo_DeviceId] ON [BeverageInfo] ([DeviceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_BeverageTemplateVersion_BeverageInfoTemplateId] ON [BeverageTemplateVersion] ([BeverageInfoTemplateId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_BeverageVersion_BeverageInfoId] ON [BeverageVersion] ([BeverageInfoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_CardDeviceAssignment_CardId] ON [CardDeviceAssignment] ([CardId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_CardDeviceAssignment_CardId_DeviceId] ON [CardDeviceAssignment] ([CardId], [DeviceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_CardDeviceAssignment_DeviceId] ON [CardDeviceAssignment] ([DeviceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_CardInfo_CardNumber] ON [CardInfo] ([CardNumber]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_CountryRegion_CountryID] ON [CountryRegion] ([CountryID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_CountryRegion_ParentID] ON [CountryRegion] ([ParentID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_Coupon_CampaignId] ON [Coupon] ([CampaignId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_Coupon_OrderId] ON [Coupon] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_Coupon_Status] ON [Coupon] ([Status]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_Coupon_Status_ValidFrom_ValidTo] ON [Coupon] ([Status], [ValidFrom], [ValidTo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_Coupon_UserId] ON [Coupon] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_Coupon_UserId_Status] ON [Coupon] ([UserId], [Status]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_DeviceHardwares_DeviceBaseId] ON [DeviceHardwares] ([DeviceBaseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_DeviceInfo_DeviceModelId] ON [DeviceInfo] ([DeviceModelId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_DeviceMaterialInfo_DeviceBaseId_Type_Index] ON [DeviceMaterialInfo] ([DeviceBaseId], [Type], [Index]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_DeviceModel_Key] ON [DeviceModel] ([Key]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_DeviceRestockLogSub_DeviceRestockLogId] ON [DeviceRestockLogSub] ([DeviceRestockLogId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_DeviceServiceProviders_DeviceInfoId] ON [DeviceServiceProviders] ([DeviceInfoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_DeviceUserAssociation_DeviceId] ON [DeviceUserAssociation] ([DeviceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_DeviceUserAssociation_UserId] ON [DeviceUserAssociation] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_DictionaryEntity_ParentKey] ON [DictionaryEntity] ([ParentKey]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_EarlyWarningConfig_DeviceInfoId] ON [EarlyWarningConfig] ([DeviceInfoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_EnterpriseDevices_DeviceId] ON [EnterpriseDevices] ([DeviceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_EnterpriseDevices_EnterpriseId] ON [EnterpriseDevices] ([EnterpriseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_EnterpriseInfo_EnterpriseTypeId] ON [EnterpriseInfo] ([EnterpriseTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_EnterpriseInfo_Pid] ON [EnterpriseInfo] ([Pid]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_EnterpriseQualificationInfo_EnterpriseId] ON [EnterpriseQualificationInfo] ([EnterpriseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_EnterpriseRole_EnterpriseId] ON [EnterpriseRole] ([EnterpriseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_EnterpriseRole_RoleId] ON [EnterpriseRole] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_EnterpriseUser_EnterpriseId] ON [EnterpriseUser] ([EnterpriseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_EnterpriseUser_UserId] ON [EnterpriseUser] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_FormulaInfo_BeverageInfoId] ON [FormulaInfo] ([BeverageInfoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_FormulaInfo_MaterialBoxId] ON [FormulaInfo] ([MaterialBoxId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_FormulaInfoTemplate_BeverageInfoTemplateId] ON [FormulaInfoTemplate] ([BeverageInfoTemplateId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_GroupDevices_DeviceInfoId] ON [GroupDevices] ([DeviceInfoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_GroupDevices_GroupsId] ON [GroupDevices] ([GroupsId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_GroupUsers_ApplicationUserId] ON [GroupUsers] ([ApplicationUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_GroupUsers_GroupsId] ON [GroupUsers] ([GroupsId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_MaterialBox_SettingInfoId] ON [MaterialBox] ([SettingInfoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_OperationSubLog_OperationLogId] ON [OperationSubLog] ([OperationLogId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_OrderDetails_OrderId] ON [OrderDetails] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_OrderDetaliMaterial_OrderDetailsId] ON [OrderDetaliMaterial] ([OrderDetailsId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_OrderInfo_ThirdOrderId] ON [OrderInfo] ([ThirdOrderId]) WHERE [ThirdOrderId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_OrderInfo_ThirdOrderNo] ON [OrderInfo] ([ThirdOrderNo]) WHERE [ThirdOrderNo] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_P_BeverageVersion_BeverageInfoId] ON [P_BeverageVersion] ([BeverageInfoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_P_FormulaInfo_BeverageInfoId] ON [P_FormulaInfo] ([BeverageInfoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_P_FormulaInfo_MaterialBoxId] ON [P_FormulaInfo] ([MaterialBoxId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PrivateGoodsRepository_EnterpriseinfoId_Sku] ON [PrivateGoodsRepository] ([EnterpriseinfoId], [Sku]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_Promotion_EndTime] ON [Promotion] ([EndTime]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_Promotion_StartTime] ON [Promotion] ([StartTime]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_Promotion_StartTime_EndTime] ON [Promotion] ([StartTime], [EndTime]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_ServiceAuthorizationRecord_FounderUserId] ON [ServiceAuthorizationRecord] ([FounderUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ServiceAuthorizationRecord_ServiceUserId] ON [ServiceAuthorizationRecord] ([ServiceUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_SettingInfo_DeviceId] ON [SettingInfo] ([DeviceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_SettingInfo_InterfaceStylesId] ON [SettingInfo] ([InterfaceStylesId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_SysLanguageText_Code_LangCode] ON [SysLanguageText] ([Code], [LangCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_SysLanguageText_LangCode] ON [SysLanguageText] ([LangCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_UserMessages_MessageId] ON [UserMessages] ([MessageId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_UserMessages_UserId_IsRead_IsPopupShown] ON [UserMessages] ([UserId], [IsRead], [IsPopupShown]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE INDEX [IX_UserReadGlobalMessages_MessageId] ON [UserReadGlobalMessages] ([MessageId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserReadGlobalMessages_UserId_MessageId] ON [UserReadGlobalMessages] ([UserId], [MessageId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251022071815_v1.0.1'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251022071815_v1.0.1', N'8.0.13');
END;
GO

COMMIT;
GO

