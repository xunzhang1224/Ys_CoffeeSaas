using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Dtos.BasicDtos;

namespace YS.CoffeeMachine.Application.Dtos.TestDtos
{
    /// <summary>
    /// 时区测试Dto
    /// </summary>
    public class TimezoneTestDto : TimezoneOffsetBaseDto
    {
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
    }
}