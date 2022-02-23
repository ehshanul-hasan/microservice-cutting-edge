using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Logging.Enrichers
{
    public interface ICustomLogEnricher
    {
        string CorrelationId { get; set; }
    }
}
