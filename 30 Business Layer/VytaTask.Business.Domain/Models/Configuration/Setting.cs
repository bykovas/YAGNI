using VytaTask.Business.Contracts;
using VytaTask.Business.Contracts.Infrastructure;

namespace VytaTask.Business.Domain.Models.Configuration
{
    public class Setting : BaseEntity<int>, ISetting
    {
        public Setting() { }
        public Setting(string name, string value, int regionId)
        {
            this.Name = name;
            this.Value = value;
            this.RegionId = regionId;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public int RegionId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
