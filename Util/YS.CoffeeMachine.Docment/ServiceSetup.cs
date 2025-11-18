namespace YS.CoffeeMachine.Provider
{
    using Magicodes.ExporterAndImporter.Core;
    using Magicodes.ExporterAndImporter.Excel;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Channels;
    using YS.CoffeeMachine.Provider.Dto.Docment;

    /// <summary>
    /// ServiceSetup
    /// </summary>
    public static class ServiceSetup
    {
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFileExcel(this IServiceCollection services)
        {

            // Register Channel as a singleton
            var channel = Channel.CreateUnbounded<FileExcelUploadTask>();
            services.AddSingleton(channel);

            // Register the IExporter service
            services.AddSingleton<IExporter, ExcelExporter>(); // Replace ExporterImplementation with your actual implementation
            services.AddSingleton<IImporter, ExcelImporter>();

            // Register the BackgroundService
            // 后台任务上传oss
            //services.AddHostedService<FileUploadBackgroundService>();

            return services;
        }
    }
}
