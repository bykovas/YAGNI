using System.Collections.Generic;
using VytaTask.Business.Contracts.Infrastructure;

namespace VytaTask.Business.Domain.Models.Common
{
    public class CommonSettings : ISettings
    {
        public CommonSettings()
        {
            IgnoreLogWordlist = new List<string>();
        }

        /// <summary>
        /// Gets or sets ignore words (phrases) to be ignored when logging errors/messages
        /// </summary>
        public List<string> IgnoreLogWordlist { get; set; }

        public bool UseStoredProceduresIfSupported { get; set; }
    }
}