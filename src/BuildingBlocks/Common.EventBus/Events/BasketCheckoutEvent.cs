using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EventBus.Events
{
    public class BasketCheckoutEvent : IntegrationBaseEvent
    {
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public string? TotalPrice { get; set; }
        public List<string>? Item { get; set; }
    }
}
