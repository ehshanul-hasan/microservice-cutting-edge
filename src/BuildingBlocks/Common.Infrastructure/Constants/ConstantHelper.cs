using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Constants
{
    public class ConstantHelper
    {
        public static readonly List<string> CONFIGURATION_KEY = new List<string> { "global/configuration", "service/common/configuration", "service/{servicename}/configuration" };
    }
}
