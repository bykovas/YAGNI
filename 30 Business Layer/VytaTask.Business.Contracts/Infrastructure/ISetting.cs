using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VytaTask.Business.Contracts.Infrastructure
{
    public interface ISetting
    {
        string Name { get; set; }
        string Value { get; set; }
        int RegionId { get; set; }
    }
}
