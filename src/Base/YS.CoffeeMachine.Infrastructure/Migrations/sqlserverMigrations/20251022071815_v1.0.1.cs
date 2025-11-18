using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YS.CoffeeMachine.Infrastructure.Migrations.sqlserverMigrations
{
    /// <inheritdoc />
    public partial class v101 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationMenu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    MenuType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SysMenuType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Component = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Redirect = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ExtraIcon = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    EnterTransition = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LeaveTransition = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Auths = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    FrameSrc = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FrameLoading = table.Column<bool>(type: "bit", nullable: false),
                    KeepAlive = table.Column<bool>(type: "bit", nullable: false),
                    HiddenTag = table.Column<bool>(type: "bit", nullable: false),
                    FixedTag = table.Column<bool>(type: "bit", nullable: false),
                    ShowLink = table.Column<bool>(type: "bit", nullable: false),
                    ShowParent = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ActivePath = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationMenu_ApplicationMenu_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ApplicationMenu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SysMenuType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    HasSuperAdmin = table.Column<bool>(type: "bit", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    EnterpriseId = table.Column<long>(type: "bigint", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AreaCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    VerifyPhone = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SysMenuType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AreaRelation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AreaCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TermServiceUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Enabled = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaRelation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    KeyValues = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    TrailType = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeverageCollection",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    EnterpriseInfoId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceModelId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BeverageIds = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    BeverageNames = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeverageCollection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeverageInfoTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CategoryIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnterpriseInfoId = table.Column<long>(type: "bigint", nullable: true),
                    DeviceModelId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BeverageIcon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CodeIsShow = table.Column<bool>(type: "bit", nullable: false),
                    Temperature = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProductionForecast = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ForecastQuantity = table.Column<double>(type: "float", nullable: false),
                    DisplayStatus = table.Column<bool>(type: "bit", nullable: false),
                    SellStradgy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeverageInfoTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountryInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Continent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsEnabled = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CurrencySymbol = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CurrencyShowFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Accuracy = table.Column<int>(type: "int", nullable: false),
                    RoundingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabled = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Culture = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CurrencySymbol = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceAbnormal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeviceModelName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TransId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CounterNo = table.Column<int>(type: "int", nullable: false),
                    Slot = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CodeType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false),
                    BaseEnterpriseId = table.Column<long>(type: "bigint", nullable: true),
                    BaseEnterpriseName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BaseDeviceId = table.Column<long>(type: "bigint", nullable: true),
                    BaseDeviceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BaseDeviceModelId = table.Column<long>(type: "bigint", nullable: true),
                    BaseDeviceModelName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceAbnormal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceAdvertisement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CarouselIntervalSecond = table.Column<int>(type: "int", nullable: false),
                    StandbyWaiteSecond = table.Column<int>(type: "int", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceAdvertisement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceAdvertisementFile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceAdvertisementId = table.Column<long>(type: "bigint", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsFullScreenAd = table.Column<bool>(type: "bit", nullable: false),
                    FileLength = table.Column<int>(type: "int", nullable: false),
                    Enable = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceAdvertisementFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceAttribute",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceAttribute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceBaseInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Mid = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MachineStickerCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BoxId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeviceModelId = table.Column<long>(type: "bigint", nullable: true),
                    IsLeaveFactory = table.Column<int>(type: "int", nullable: false),
                    SSID = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    HardwareCapability = table.Column<int>(type: "int", nullable: true),
                    SoftwareCapability = table.Column<int>(type: "int", nullable: true),
                    MAC = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    VersionNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SkinPluginVersion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LanguagePack = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SoftwareUpdateLastTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    UpdateOnlineTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateOfflineTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceBaseInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCapacityCfg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    CapacityId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CapacityType = table.Column<int>(type: "int", nullable: false),
                    CfgInfo = table.Column<string>(type: "nvarchar(MAX)", maxLength: 256, nullable: true),
                    Premission = table.Column<int>(type: "int", nullable: false),
                    Structure = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCapacityCfg", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceEarlyWarnings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    WarningType = table.Column<int>(type: "int", nullable: false),
                    DeviceMaterialId = table.Column<long>(type: "bigint", nullable: true),
                    IsOn = table.Column<bool>(type: "bit", nullable: false),
                    WarningValue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceEarlyWarnings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceFlushComponents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceFlushComponents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceFlushComponentsLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    MachineStickerCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FlushType = table.Column<int>(type: "int", nullable: false),
                    Parts = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceFlushComponentsLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceInitialization",
                columns: table => new
                {
                    Mid = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EquipmentNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IMEI = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsBind = table.Column<bool>(type: "bit", nullable: false),
                    PriKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PubKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ChanneId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceInitialization", x => x.Mid);
                });

            migrationBuilder.CreateTable(
                name: "DeviceMaterialInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    IsSugar = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMaterialInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceMetrics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    CounterNo = table.Column<int>(type: "int", nullable: false),
                    MetricType = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceModel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MaxCassetteCount = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DevicePaymentConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceInfoId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentConfigId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevicePaymentConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DevicePaymentSetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PayWait = table.Column<int>(type: "int", nullable: true),
                    CurrencyLocationEnable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentCurrencyLocaton = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevicePaymentSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceRestockLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeviceCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeviceDZ = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceRestockLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceSoftwareInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    ProgramType = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    VersionName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    VerId = table.Column<long>(type: "bigint", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Extra = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSoftwareInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceVersionManage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    VersionNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeviceModelId = table.Column<long>(type: "bigint", nullable: true),
                    ProgramType = table.Column<int>(type: "int", nullable: true),
                    ProgramTypeName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    VersionType = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Enabled = table.Column<int>(type: "int", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceVersionManage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceVsersionUpdateRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceVersionManageId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProgramType = table.Column<int>(type: "int", nullable: true),
                    VersionType = table.Column<int>(type: "int", nullable: true),
                    ProgramTypeName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PushState = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceVsersionUpdateRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryEntity",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ParentKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsEnabled = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryEntity", x => x.Key);
                    table.ForeignKey(
                        name: "FK_DictionaryEntity_DictionaryEntity_ParentKey",
                        column: x => x.ParentKey,
                        principalTable: "DictionaryEntity",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DivideAccountsConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SysPaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    MerchantId = table.Column<long>(type: "bigint", maxLength: 100, nullable: false, comment: "商户号"),
                    MerchantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "商户名称"),
                    Bibliography = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "分账比例%"),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "备注"),
                    type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "微信接收方类型"),
                    AlipayRoyaltyType = table.Column<int>(type: "int", maxLength: 100, nullable: true, comment: "支付宝接收方类型"),
                    account = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "接收方账号"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "分账接收方全称,分账接收方真实姓名"),
                    relation_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "与分账方的关系类型"),
                    VendCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivideAccountsConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnterpriseTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Astrict = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaultCode",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaultCode", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "FileCenter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileCenterType = table.Column<int>(type: "int", nullable: false),
                    SysMenuType = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileState = table.Column<int>(type: "int", nullable: false),
                    FaildMessage = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileCenter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileManage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileSize = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ResourceUsage = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileRelation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FileId = table.Column<long>(type: "bigint", nullable: false),
                    TargetId = table.Column<long>(type: "bigint", nullable: false),
                    TargetType = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRelation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    EnterpriseInfoId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PId = table.Column<long>(type: "bigint", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterfaceStyles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Preview = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterfaceStyles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageInfo",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsEnabled = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageInfo", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "M_PaymentAlipayApplyments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PaymentOriginId = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MerchantType = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    Mcc = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LegalCertType = table.Column<int>(type: "int", maxLength: 16, nullable: false),
                    LegalCertFrontImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LegalCertBackImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LegalCertName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LegalCertNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LegalCertAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LegalCertValidTimeBegin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LegalCertValidTimeEnd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MerchantShortName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    BusinessAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ServicePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BusinessAddressDetail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    InDoorImages = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    OutDoorImages = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BusinessLicenseImage = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AlipayLogonId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    UnifiedSocialCreditCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    BusinessLicenseName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BusinessLicenseLegalName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BusinessLicenseAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ContactInfos = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BizCards = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LicenseAuthLetterImage = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Service = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SignTimeWithIsv = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Sites = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    InvoiceInfo = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BindingAlipayLogonId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Addtime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MerchantName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ApplyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtInfo = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Smid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CardAliasNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Flowstatus = table.Column<int>(type: "int", nullable: false),
                    RejectReason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AppAuthToken = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    VerifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    VerifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsArtificialApplyment = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_PaymentAlipayApplyments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "M_PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SystemPaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DomesticMerchantType = table.Column<int>(type: "int", nullable: true),
                    PaymentMode = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PaymentEntryStatus = table.Column<int>(type: "int", nullable: false),
                    BindType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsEnabled = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MerchantId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentParameters = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    SystemPaymentServiceProviderId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "M_PaymentMethodBindDevice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    SystemPaymentMethodId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_PaymentMethodBindDevice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "M_PaymentWechatApplyments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ApplymentId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PaymentOriginId = table.Column<long>(type: "bigint", nullable: false),
                    IdDocType = table.Column<int>(type: "int", nullable: false),
                    IdCardName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdCardNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IdCardAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdCardValidTimeBegin = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdCardValidTime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdCardCopy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdCardNational = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MobilePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MerchantShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServicePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BizProvinceCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BizCityCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BizStoreAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StoreEntrancePic = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IndoorPic = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OrganizationType = table.Column<int>(type: "int", nullable: false),
                    BusinessLicenseCopy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BusinessLicenseNumber = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    MerchantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LegalPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LicenseAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CertificateLetterCopy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountBank = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankAliasCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankProvinceCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BankCityCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BankBranchId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApplymentState = table.Column<int>(type: "int", nullable: true),
                    ApplymentStateDesc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SignState = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    SignUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SubMchId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    FAccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FAccountNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PayAmount = table.Column<int>(type: "int", nullable: true),
                    DestinationAccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DestinationAccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DestinationAccountBank = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DestinationCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DestinationRemark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Deadline = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LegalValidationUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AuditDetail = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    ApplyState = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RejectReason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FlowStatus = table.Column<int>(type: "int", nullable: false),
                    VerifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    VerifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsArtificialApplyment = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_PaymentWechatApplyments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoticeCfg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoticeCfg", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotityMsg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MsgName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ContactAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Msg = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotityMsg", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Mid = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    OperationResult = table.Column<int>(type: "int", nullable: false),
                    OperationName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RequestWayType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RequestUrl = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false),
                    BaseEnterpriseId = table.Column<long>(type: "bigint", nullable: true),
                    BaseEnterpriseName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BaseDeviceId = table.Column<long>(type: "bigint", nullable: true),
                    BaseDeviceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BaseDeviceModelId = table.Column<long>(type: "bigint", nullable: true),
                    BaseDeviceModelName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BizNo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ThirdOrderId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ThirdOrderNo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CustomContent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CurrencySymbol = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PayTimeSp = table.Column<long>(type: "bigint", nullable: false),
                    PayDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShipmentResult = table.Column<int>(type: "int", nullable: false),
                    SaleResult = table.Column<int>(type: "int", nullable: false),
                    ErrCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PayErrMsg = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ReturnAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: true),
                    PaymentMerchantId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SystemPaymentMethodId = table.Column<long>(type: "bigint", nullable: true),
                    SystemPaymentServiceProviderId = table.Column<long>(type: "bigint", nullable: true),
                    OrderPaymentType = table.Column<int>(type: "int", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false),
                    BaseEnterpriseId = table.Column<long>(type: "bigint", nullable: true),
                    BaseEnterpriseName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BaseDeviceId = table.Column<long>(type: "bigint", nullable: true),
                    BaseDeviceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BaseDeviceModelId = table.Column<long>(type: "bigint", nullable: true),
                    BaseDeviceModelName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderRefund",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, comment: "主订单号（Order表的OrderId字段）"),
                    OrderDetailId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, comment: "（OrderDetail表的Id字段）"),
                    RefundOrderNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, comment: "退款的交易订单号（支付平台OrderRefund表的Id）"),
                    ItemCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true, comment: "饮品SKU"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true, comment: "商品表的Id（Product表的Id）"),
                    BarCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "商品条码（Product表的BarCode）"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "商品名称（Product表的Name）"),
                    MainImage = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "商品主图（Product表的MainImage）"),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m, comment: "退款金额"),
                    RefundReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "退款原因"),
                    RefundStatus = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Success", comment: "退款状态"),
                    HandlingMethod = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "FullRefund", comment: "处理方式"),
                    OrderCreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "订单创建时间（Order表的OrderCreatedOnUtc，一定要保持一致）"),
                    InitiationTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "退款发起时间"),
                    SuccessTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "退款成功时间"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false),
                    BaseEnterpriseId = table.Column<long>(type: "bigint", nullable: true),
                    BaseEnterpriseName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BaseDeviceId = table.Column<long>(type: "bigint", nullable: true),
                    BaseDeviceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BaseDeviceModelId = table.Column<long>(type: "bigint", nullable: true),
                    BaseDeviceModelName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRefund", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_BeverageCollection",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    LanguageKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeviceModelId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BeverageIds = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    BeverageNames = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_BeverageCollection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_BeverageInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountedPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LanguageKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeviceModelId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BeverageIcon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Temperature = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProductionForecast = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ForecastQuantity = table.Column<double>(type: "float", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    DisplayStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_BeverageInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_FileManage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileSize = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ResourceUsage = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_FileManage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_PaymentConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Countrys = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PaymentModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Enabled = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_PaymentConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    P_PaymentConfigId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PaymentConfigStatue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MerchantCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PaymentPlatformAppId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Enabled = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrivateGoodsRepository",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SuggestedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsEnable = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateGoodsRepository", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProductCategoryType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsEnabled = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CouponType = table.Column<int>(type: "int", nullable: false),
                    TotalLimit = table.Column<int>(type: "int", nullable: false),
                    LimitedCount = table.Column<int>(type: "int", nullable: false),
                    ParticipatingUsers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApplicableProductsType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ApplicableDrinks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CanCombineWithOtherOffers = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProviderInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProviderInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysProvinceCity",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    CreateOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SystemMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    TargetUserId = table.Column<long>(type: "bigint", nullable: true),
                    TargetGroup = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPopup = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPaymentMethod",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FatherId = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    PaymentImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OnlinePayment = table.Column<bool>(type: "bit", nullable: false),
                    OfflinePayment = table.Column<bool>(type: "bit", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LanguageTextCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsEnabled = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    PaymentPlatformId = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPaymentServiceProvider",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SpMerchantId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AppletAppID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppKey = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    ApiV3Key = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    NotifyUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentPlatformType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsDefault = table.Column<int>(type: "int", nullable: false),
                    CretFileUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CretPassWrod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PlatformSerialNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PlatformPublicKey = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPaymentServiceProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskSchedulingInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CronExpression = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSchedulingInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TermServiceEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Enabled = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermServiceEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeZoneInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZoneInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRoleMenu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    MenuId = table.Column<long>(type: "bigint", nullable: false),
                    IsHalf = table.Column<bool>(type: "bit", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleMenu_ApplicationMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "ApplicationMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleMenu_ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApplicationRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRole_ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApplicationRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRole_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceAuthorizationRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FounderId = table.Column<long>(type: "bigint", nullable: false),
                    FounderUserId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceUserAccount = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ServiceUserId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorizationStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorizationEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAuthorizationRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceAuthorizationRecord_ApplicationUser_FounderUserId",
                        column: x => x.FounderUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceAuthorizationRecord_ApplicationUser_ServiceUserId",
                        column: x => x.ServiceUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BeverageTemplateVersion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BeverageInfoTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    VersionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BeverageInfoDataString = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeverageTemplateVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeverageTemplateVersion_BeverageInfoTemplate_BeverageInfoTemplateId",
                        column: x => x.BeverageInfoTemplateId,
                        principalTable: "BeverageInfoTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormulaInfoTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BeverageInfoTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    MaterialBoxId = table.Column<long>(type: "bigint", nullable: true),
                    MaterialBoxName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    FormulaType = table.Column<int>(type: "int", nullable: false),
                    SpecsString = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaInfoTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormulaInfoTemplate_BeverageInfoTemplate_BeverageInfoTemplateId",
                        column: x => x.BeverageInfoTemplateId,
                        principalTable: "BeverageInfoTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryRegion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ParentID = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ParentCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    HasChildren = table.Column<bool>(type: "bit", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    CountryID = table.Column<long>(type: "bigint", nullable: false),
                    IsEnabled = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryRegion_CountryInfo_CountryID",
                        column: x => x.CountryID,
                        principalTable: "CountryInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryRegion_CountryRegion_ParentID",
                        column: x => x.ParentID,
                        principalTable: "CountryRegion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeviceHardwares",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    HardwareName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    HardwareType = table.Column<int>(type: "int", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceHardwares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceHardwares_DeviceBaseInfo_DeviceBaseId",
                        column: x => x.DeviceBaseId,
                        principalTable: "DeviceBaseInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceBaseId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ActiveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceActiveState = table.Column<int>(type: "int", nullable: true),
                    EquipmentNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Mid = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeviceModelId = table.Column<long>(type: "bigint", nullable: true),
                    IsLeaveFactory = table.Column<int>(type: "int", nullable: true),
                    VersionNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SkinPluginVersion = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LanguagePack = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SSID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MAC = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ICCID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UsedTrafficThisMonth = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    RemainingTrafficThisMonth = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LatestOnlineTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LatestOfflineTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    CountryRegionIds = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CountryRegionText = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DetailedAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeviceStatus = table.Column<int>(type: "int", nullable: false),
                    UsageScenario = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    POSMachineNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceInfo_DeviceModel_DeviceModelId",
                        column: x => x.DeviceModelId,
                        principalTable: "DeviceModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceRestockLogSub",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    HGType = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<long>(type: "bigint", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    OldValue = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    NewValue = table.Column<int>(type: "int", nullable: false),
                    DeviceRestockLogId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceRestockLogSub", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceRestockLogSub_DeviceRestockLog_DeviceRestockLogId",
                        column: x => x.DeviceRestockLogId,
                        principalTable: "DeviceRestockLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnterpriseInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EnterpriseTypeId = table.Column<long>(type: "bigint", nullable: true),
                    AreaRelationId = table.Column<long>(type: "bigint", nullable: true),
                    Pid = table.Column<long>(type: "bigint", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    MenuIds = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    HalfMenuIds = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: true),
                    H5MenuIds = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    H5HalfMenuIds = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: true),
                    OrganizationType = table.Column<int>(type: "int", nullable: true),
                    RegistrationProgress = table.Column<int>(type: "int", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterpriseInfo_EnterpriseInfo_Pid",
                        column: x => x.Pid,
                        principalTable: "EnterpriseInfo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnterpriseInfo_EnterpriseTypes_EnterpriseTypeId",
                        column: x => x.EnterpriseTypeId,
                        principalTable: "EnterpriseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    GroupsId = table.Column<long>(type: "bigint", nullable: false),
                    ApplicationUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupUsers_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUsers_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysLanguageText",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    LangCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLanguageText", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysLanguageText_LanguageInfo_LangCode",
                        column: x => x.LangCode,
                        principalTable: "LanguageInfo",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationSubLog",
                columns: table => new
                {
                    OperationLogId = table.Column<long>(type: "bigint", nullable: false),
                    Mid = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeviceModelName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RequestMsg = table.Column<string>(type: "nvarchar(MAX)", maxLength: 256, nullable: true),
                    AppliedType = table.Column<int>(type: "int", nullable: true),
                    ContentType = table.Column<int>(type: "int", nullable: true),
                    ReplaceTarget = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    OperationResult = table.Column<int>(type: "int", nullable: false),
                    ErrorMsg = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationSubLog", x => new { x.Mid, x.OperationLogId });
                    table.ForeignKey(
                        name: "FK_OperationSubLog_OperationLog_OperationLogId",
                        column: x => x.OperationLogId,
                        principalTable: "OperationLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    CounterNo = table.Column<int>(type: "int", nullable: false),
                    SlotNo = table.Column<int>(type: "int", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BeverageName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    Error = table.Column<int>(type: "int", nullable: false),
                    ErrorDescription = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ActionTimeSp = table.Column<long>(type: "bigint", nullable: false),
                    BeverageInfoData = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_OrderInfo_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrderInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "P_BeverageVersion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BeverageInfoId = table.Column<long>(type: "bigint", nullable: false),
                    VersionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BeverageInfoDataString = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_BeverageVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_P_BeverageVersion_P_BeverageInfo_BeverageInfoId",
                        column: x => x.BeverageInfoId,
                        principalTable: "P_BeverageInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UseType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UseDay = table.Column<int>(type: "int", nullable: true),
                    UsedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coupon_Promotion_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", maxLength: 100, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPopupShown = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMessages_SystemMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "SystemMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserReadGlobalMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ReadTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReadGlobalMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReadGlobalMessages_SystemMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "SystemMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceInfoId = table.Column<long>(type: "bigint", nullable: false),
                    PowerOnAdsVolume = table.Column<int>(type: "int", nullable: false),
                    PowerOnAdsPlayTime = table.Column<int>(type: "int", nullable: false),
                    PowerOnAdsImagesJson = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    StandbyAdsVolume = table.Column<int>(type: "int", nullable: false),
                    StandbyAdsPlayTime = table.Column<int>(type: "int", nullable: false),
                    StandbyAdsAwaitTime = table.Column<int>(type: "int", nullable: false),
                    StandbyAdsLoopTime = table.Column<int>(type: "int", nullable: false),
                    StandbyAdsLoopType = table.Column<bool>(type: "bit", nullable: false),
                    StandbyAdsImagesJson = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProductionAdsVolume = table.Column<int>(type: "int", nullable: false),
                    ProductionAdsPlayTime = table.Column<int>(type: "int", nullable: false),
                    ProductionAdsImagesJson = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementInfo_DeviceInfo_DeviceInfoId",
                        column: x => x.DeviceInfoId,
                        principalTable: "DeviceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizedDevices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ServiceAuthorizationRecordId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizedDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorizedDevices_DeviceInfo_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "DeviceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorizedDevices_ServiceAuthorizationRecord_ServiceAuthorizationRecordId",
                        column: x => x.ServiceAuthorizationRecordId,
                        principalTable: "ServiceAuthorizationRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BeverageInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    DiscountedPrice = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BeverageIcon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CodeIsShow = table.Column<bool>(type: "bit", nullable: false),
                    Temperature = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProductionForecast = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ForecastQuantity = table.Column<double>(type: "float", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    DisplayStatus = table.Column<bool>(type: "bit", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    SellStradgy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeverageInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeverageInfo_DeviceInfo_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "DeviceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardDeviceAssignment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDeviceAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardDeviceAssignment_CardInfo_CardId",
                        column: x => x.CardId,
                        principalTable: "CardInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardDeviceAssignment_DeviceInfo_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "DeviceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceServiceProviders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ServiceProviderInfoId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceInfoId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceServiceProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceServiceProviders_DeviceInfo_DeviceInfoId",
                        column: x => x.DeviceInfoId,
                        principalTable: "DeviceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceUserAssociation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceUserAssociation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceUserAssociation_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceUserAssociation_DeviceInfo_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "DeviceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EarlyWarningConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceInfoId = table.Column<long>(type: "bigint", nullable: false),
                    WholeMachineCleaningSwitch = table.Column<bool>(type: "bit", nullable: false),
                    NextWholeMachineCleaningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrewingMachineCleaningSwitch = table.Column<bool>(type: "bit", nullable: false),
                    NextBrewingMachineCleaningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MilkFrotherCleaningSwitch = table.Column<bool>(type: "bit", nullable: false),
                    NextMilkFrotherCleaningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoffeeWaterwayCleaningSwitch = table.Column<bool>(type: "bit", nullable: false),
                    NextCoffeeWaterwayCleaningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SteamWaterwayCleaningSwitch = table.Column<bool>(type: "bit", nullable: false),
                    NextSteamWaterwayCleaningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OfflineWarningSwitch = table.Column<bool>(type: "bit", nullable: false),
                    OfflineDays = table.Column<int>(type: "int", nullable: false),
                    ShortageWarningSwitch = table.Column<bool>(type: "bit", nullable: false),
                    CoffeeBeanRemaining = table.Column<double>(type: "float", nullable: false),
                    WaterRemaining = table.Column<double>(type: "float", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarlyWarningConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EarlyWarningConfig_DeviceInfo_DeviceInfoId",
                        column: x => x.DeviceInfoId,
                        principalTable: "DeviceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupDevices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    GroupsId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceInfoId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupDevices_DeviceInfo_DeviceInfoId",
                        column: x => x.DeviceInfoId,
                        principalTable: "DeviceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupDevices_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SettingInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    IsShowEquipmentNumber = table.Column<bool>(type: "bit", nullable: false),
                    InterfaceStylesId = table.Column<long>(type: "bigint", nullable: false),
                    WashType = table.Column<int>(type: "int", nullable: false),
                    RegularWashTime = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    WashWarning = table.Column<int>(type: "int", nullable: true),
                    AfterSalesPhone = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ExpectedUpdateTime = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ScreenBrightness = table.Column<int>(type: "int", nullable: false),
                    DeviceSound = table.Column<int>(type: "int", nullable: false),
                    AdministratorPwd = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ReplenishmentOfficerPwd = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    StartWeek = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EndWeek = table.Column<int>(type: "int", nullable: false),
                    LanguageName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CurrencySymbol = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CurrencyName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CurrencyPosition = table.Column<int>(type: "int", nullable: false),
                    CurrencyDecimalDigits = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingInfo_DeviceInfo_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "DeviceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SettingInfo_InterfaceStyles_InterfaceStylesId",
                        column: x => x.InterfaceStylesId,
                        principalTable: "InterfaceStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EnterpriseDevices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    BelongingEnterpriseId = table.Column<long>(type: "bigint", nullable: false),
                    EnterpriseId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceAllocationType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RecyclingTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AllocateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterpriseDevices_DeviceInfo_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "DeviceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterpriseDevices_EnterpriseInfo_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "EnterpriseInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnterpriseQualificationInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    EnterpriseId = table.Column<long>(type: "bigint", nullable: false),
                    LegalPersonName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LegalPersonIdCardNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    FrontImageUrl = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: true),
                    BackImageUrl = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: true),
                    CustomerServiceEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StoreAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BusinessLicenseUrl = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: true),
                    Othercertificate = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseQualificationInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterpriseQualificationInfo_EnterpriseInfo_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "EnterpriseInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnterpriseRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    EnterpriseId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterpriseRole_ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApplicationRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterpriseRole_EnterpriseInfo_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "EnterpriseInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnterpriseUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    EnterpriseId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterpriseUser_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterpriseUser_EnterpriseInfo_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "EnterpriseInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetaliMaterial",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    OrderDetailsId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceMaterialInfoId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetaliMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetaliMaterial_OrderDetails_OrderDetailsId",
                        column: x => x.OrderDetailsId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BeverageVersion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BeverageInfoId = table.Column<long>(type: "bigint", nullable: false),
                    VersionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BeverageInfoDataString = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeverageVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeverageVersion_BeverageInfo_BeverageInfoId",
                        column: x => x.BeverageInfoId,
                        principalTable: "BeverageInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialBox",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SettingInfoId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    MinMeasureWarning = table.Column<double>(type: "float", nullable: false),
                    MinQuantityWarning = table.Column<double>(type: "float", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialBox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialBox_SettingInfo_SettingInfoId",
                        column: x => x.SettingInfoId,
                        principalTable: "SettingInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormulaInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BeverageInfoId = table.Column<long>(type: "bigint", nullable: false),
                    MaterialBoxId = table.Column<long>(type: "bigint", nullable: true),
                    MaterialBoxName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    FormulaType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SpecsString = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormulaInfo_BeverageInfo_BeverageInfoId",
                        column: x => x.BeverageInfoId,
                        principalTable: "BeverageInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulaInfo_MaterialBox_MaterialBoxId",
                        column: x => x.MaterialBoxId,
                        principalTable: "MaterialBox",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "P_FormulaInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BeverageInfoId = table.Column<long>(type: "bigint", nullable: false),
                    MaterialBoxId = table.Column<long>(type: "bigint", nullable: true),
                    MaterialBoxName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    FormulaType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SpecsString = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_FormulaInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_P_FormulaInfo_MaterialBox_MaterialBoxId",
                        column: x => x.MaterialBoxId,
                        principalTable: "MaterialBox",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_P_FormulaInfo_P_BeverageInfo_BeverageInfoId",
                        column: x => x.BeverageInfoId,
                        principalTable: "P_BeverageInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementInfo_DeviceInfoId",
                table: "AdvertisementInfo",
                column: "DeviceInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationMenu_ParentId",
                table: "ApplicationMenu",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleMenu_MenuId",
                table: "ApplicationRoleMenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleMenu_RoleId",
                table: "ApplicationRoleMenu",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRole_RoleId",
                table: "ApplicationUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRole_UserId",
                table: "ApplicationUserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedDevices_DeviceId",
                table: "AuthorizedDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedDevices_ServiceAuthorizationRecordId",
                table: "AuthorizedDevices",
                column: "ServiceAuthorizationRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_BeverageInfo_DeviceId",
                table: "BeverageInfo",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_BeverageTemplateVersion_BeverageInfoTemplateId",
                table: "BeverageTemplateVersion",
                column: "BeverageInfoTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_BeverageVersion_BeverageInfoId",
                table: "BeverageVersion",
                column: "BeverageInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CardDeviceAssignment_CardId",
                table: "CardDeviceAssignment",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardDeviceAssignment_CardId_DeviceId",
                table: "CardDeviceAssignment",
                columns: new[] { "CardId", "DeviceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardDeviceAssignment_DeviceId",
                table: "CardDeviceAssignment",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_CardNumber",
                table: "CardInfo",
                column: "CardNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegion_CountryID",
                table: "CountryRegion",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegion_ParentID",
                table: "CountryRegion",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_CampaignId",
                table: "Coupon",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_OrderId",
                table: "Coupon",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_Status",
                table: "Coupon",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_Status_ValidFrom_ValidTo",
                table: "Coupon",
                columns: new[] { "Status", "ValidFrom", "ValidTo" });

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_UserId",
                table: "Coupon",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_UserId_Status",
                table: "Coupon",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceHardwares_DeviceBaseId",
                table: "DeviceHardwares",
                column: "DeviceBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceInfo_DeviceModelId",
                table: "DeviceInfo",
                column: "DeviceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMaterialInfo_DeviceBaseId_Type_Index",
                table: "DeviceMaterialInfo",
                columns: new[] { "DeviceBaseId", "Type", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModel_Key",
                table: "DeviceModel",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceRestockLogSub_DeviceRestockLogId",
                table: "DeviceRestockLogSub",
                column: "DeviceRestockLogId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceServiceProviders_DeviceInfoId",
                table: "DeviceServiceProviders",
                column: "DeviceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceUserAssociation_DeviceId",
                table: "DeviceUserAssociation",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceUserAssociation_UserId",
                table: "DeviceUserAssociation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntity_ParentKey",
                table: "DictionaryEntity",
                column: "ParentKey");

            migrationBuilder.CreateIndex(
                name: "IX_EarlyWarningConfig_DeviceInfoId",
                table: "EarlyWarningConfig",
                column: "DeviceInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseDevices_DeviceId",
                table: "EnterpriseDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseDevices_EnterpriseId",
                table: "EnterpriseDevices",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseInfo_EnterpriseTypeId",
                table: "EnterpriseInfo",
                column: "EnterpriseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseInfo_Pid",
                table: "EnterpriseInfo",
                column: "Pid");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseQualificationInfo_EnterpriseId",
                table: "EnterpriseQualificationInfo",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseRole_EnterpriseId",
                table: "EnterpriseRole",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseRole_RoleId",
                table: "EnterpriseRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseUser_EnterpriseId",
                table: "EnterpriseUser",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseUser_UserId",
                table: "EnterpriseUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaInfo_BeverageInfoId",
                table: "FormulaInfo",
                column: "BeverageInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaInfo_MaterialBoxId",
                table: "FormulaInfo",
                column: "MaterialBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaInfoTemplate_BeverageInfoTemplateId",
                table: "FormulaInfoTemplate",
                column: "BeverageInfoTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDevices_DeviceInfoId",
                table: "GroupDevices",
                column: "DeviceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDevices_GroupsId",
                table: "GroupDevices",
                column: "GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_ApplicationUserId",
                table: "GroupUsers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupsId",
                table: "GroupUsers",
                column: "GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBox_SettingInfoId",
                table: "MaterialBox",
                column: "SettingInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationSubLog_OperationLogId",
                table: "OperationSubLog",
                column: "OperationLogId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetaliMaterial_OrderDetailsId",
                table: "OrderDetaliMaterial",
                column: "OrderDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderInfo_ThirdOrderId",
                table: "OrderInfo",
                column: "ThirdOrderId",
                unique: true,
                filter: "[ThirdOrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderInfo_ThirdOrderNo",
                table: "OrderInfo",
                column: "ThirdOrderNo",
                unique: true,
                filter: "[ThirdOrderNo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_P_BeverageVersion_BeverageInfoId",
                table: "P_BeverageVersion",
                column: "BeverageInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_P_FormulaInfo_BeverageInfoId",
                table: "P_FormulaInfo",
                column: "BeverageInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_P_FormulaInfo_MaterialBoxId",
                table: "P_FormulaInfo",
                column: "MaterialBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateGoodsRepository_EnterpriseinfoId_Sku",
                table: "PrivateGoodsRepository",
                columns: new[] { "EnterpriseinfoId", "Sku" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_EndTime",
                table: "Promotion",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_StartTime",
                table: "Promotion",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_StartTime_EndTime",
                table: "Promotion",
                columns: new[] { "StartTime", "EndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAuthorizationRecord_FounderUserId",
                table: "ServiceAuthorizationRecord",
                column: "FounderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAuthorizationRecord_ServiceUserId",
                table: "ServiceAuthorizationRecord",
                column: "ServiceUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SettingInfo_DeviceId",
                table: "SettingInfo",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SettingInfo_InterfaceStylesId",
                table: "SettingInfo",
                column: "InterfaceStylesId");

            migrationBuilder.CreateIndex(
                name: "IX_SysLanguageText_Code_LangCode",
                table: "SysLanguageText",
                columns: new[] { "Code", "LangCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SysLanguageText_LangCode",
                table: "SysLanguageText",
                column: "LangCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_MessageId",
                table: "UserMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_UserId_IsRead_IsPopupShown",
                table: "UserMessages",
                columns: new[] { "UserId", "IsRead", "IsPopupShown" });

            migrationBuilder.CreateIndex(
                name: "IX_UserReadGlobalMessages_MessageId",
                table: "UserReadGlobalMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReadGlobalMessages_UserId_MessageId",
                table: "UserReadGlobalMessages",
                columns: new[] { "UserId", "MessageId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementInfo");

            migrationBuilder.DropTable(
                name: "ApplicationRoleMenu");

            migrationBuilder.DropTable(
                name: "ApplicationUserRole");

            migrationBuilder.DropTable(
                name: "AreaRelation");

            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "AuthorizedDevices");

            migrationBuilder.DropTable(
                name: "BeverageCollection");

            migrationBuilder.DropTable(
                name: "BeverageTemplateVersion");

            migrationBuilder.DropTable(
                name: "BeverageVersion");

            migrationBuilder.DropTable(
                name: "CardDeviceAssignment");

            migrationBuilder.DropTable(
                name: "CountryRegion");

            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "CurrencyInfo");

            migrationBuilder.DropTable(
                name: "DeviceAbnormal");

            migrationBuilder.DropTable(
                name: "DeviceAdvertisement");

            migrationBuilder.DropTable(
                name: "DeviceAdvertisementFile");

            migrationBuilder.DropTable(
                name: "DeviceAttribute");

            migrationBuilder.DropTable(
                name: "DeviceCapacityCfg");

            migrationBuilder.DropTable(
                name: "DeviceEarlyWarnings");

            migrationBuilder.DropTable(
                name: "DeviceFlushComponents");

            migrationBuilder.DropTable(
                name: "DeviceFlushComponentsLog");

            migrationBuilder.DropTable(
                name: "DeviceHardwares");

            migrationBuilder.DropTable(
                name: "DeviceInitialization");

            migrationBuilder.DropTable(
                name: "DeviceMaterialInfo");

            migrationBuilder.DropTable(
                name: "DeviceMetrics");

            migrationBuilder.DropTable(
                name: "DevicePaymentConfig");

            migrationBuilder.DropTable(
                name: "DevicePaymentSetting");

            migrationBuilder.DropTable(
                name: "DeviceRestockLogSub");

            migrationBuilder.DropTable(
                name: "DeviceServiceProviders");

            migrationBuilder.DropTable(
                name: "DeviceSoftwareInfo");

            migrationBuilder.DropTable(
                name: "DeviceUserAssociation");

            migrationBuilder.DropTable(
                name: "DeviceVersionManage");

            migrationBuilder.DropTable(
                name: "DeviceVsersionUpdateRecord");

            migrationBuilder.DropTable(
                name: "DictionaryEntity");

            migrationBuilder.DropTable(
                name: "DivideAccountsConfig");

            migrationBuilder.DropTable(
                name: "EarlyWarningConfig");

            migrationBuilder.DropTable(
                name: "EnterpriseDevices");

            migrationBuilder.DropTable(
                name: "EnterpriseQualificationInfo");

            migrationBuilder.DropTable(
                name: "EnterpriseRole");

            migrationBuilder.DropTable(
                name: "EnterpriseUser");

            migrationBuilder.DropTable(
                name: "FaultCode");

            migrationBuilder.DropTable(
                name: "FileCenter");

            migrationBuilder.DropTable(
                name: "FileManage");

            migrationBuilder.DropTable(
                name: "FileRelation");

            migrationBuilder.DropTable(
                name: "FormulaInfo");

            migrationBuilder.DropTable(
                name: "FormulaInfoTemplate");

            migrationBuilder.DropTable(
                name: "GroupDevices");

            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropTable(
                name: "M_PaymentAlipayApplyments");

            migrationBuilder.DropTable(
                name: "M_PaymentMethod");

            migrationBuilder.DropTable(
                name: "M_PaymentMethodBindDevice");

            migrationBuilder.DropTable(
                name: "M_PaymentWechatApplyments");

            migrationBuilder.DropTable(
                name: "NoticeCfg");

            migrationBuilder.DropTable(
                name: "NotityMsg");

            migrationBuilder.DropTable(
                name: "OperationSubLog");

            migrationBuilder.DropTable(
                name: "OrderDetaliMaterial");

            migrationBuilder.DropTable(
                name: "OrderRefund");

            migrationBuilder.DropTable(
                name: "P_BeverageCollection");

            migrationBuilder.DropTable(
                name: "P_BeverageVersion");

            migrationBuilder.DropTable(
                name: "P_FileManage");

            migrationBuilder.DropTable(
                name: "P_FormulaInfo");

            migrationBuilder.DropTable(
                name: "P_PaymentConfig");

            migrationBuilder.DropTable(
                name: "PaymentConfig");

            migrationBuilder.DropTable(
                name: "PrivateGoodsRepository");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "ServiceProviderInfo");

            migrationBuilder.DropTable(
                name: "SysLanguageText");

            migrationBuilder.DropTable(
                name: "SysProvinceCity");

            migrationBuilder.DropTable(
                name: "SystemPaymentMethod");

            migrationBuilder.DropTable(
                name: "SystemPaymentServiceProvider");

            migrationBuilder.DropTable(
                name: "TaskSchedulingInfo");

            migrationBuilder.DropTable(
                name: "TermServiceEntity");

            migrationBuilder.DropTable(
                name: "TimeZoneInfo");

            migrationBuilder.DropTable(
                name: "UserMessages");

            migrationBuilder.DropTable(
                name: "UserReadGlobalMessages");

            migrationBuilder.DropTable(
                name: "ApplicationMenu");

            migrationBuilder.DropTable(
                name: "ServiceAuthorizationRecord");

            migrationBuilder.DropTable(
                name: "CardInfo");

            migrationBuilder.DropTable(
                name: "CountryInfo");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "DeviceBaseInfo");

            migrationBuilder.DropTable(
                name: "DeviceRestockLog");

            migrationBuilder.DropTable(
                name: "ApplicationRole");

            migrationBuilder.DropTable(
                name: "EnterpriseInfo");

            migrationBuilder.DropTable(
                name: "BeverageInfo");

            migrationBuilder.DropTable(
                name: "BeverageInfoTemplate");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "OperationLog");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "MaterialBox");

            migrationBuilder.DropTable(
                name: "P_BeverageInfo");

            migrationBuilder.DropTable(
                name: "LanguageInfo");

            migrationBuilder.DropTable(
                name: "SystemMessages");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "EnterpriseTypes");

            migrationBuilder.DropTable(
                name: "OrderInfo");

            migrationBuilder.DropTable(
                name: "SettingInfo");

            migrationBuilder.DropTable(
                name: "DeviceInfo");

            migrationBuilder.DropTable(
                name: "InterfaceStyles");

            migrationBuilder.DropTable(
                name: "DeviceModel");
        }
    }
}
