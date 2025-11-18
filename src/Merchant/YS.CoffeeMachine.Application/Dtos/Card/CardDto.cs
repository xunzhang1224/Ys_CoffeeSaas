using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Dtos.Card
{
    /// <summary>
    /// 卡信息分页
    /// </summary>
    public class CardDto : QueryRequest
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNumber { get; set; }
    }
}
