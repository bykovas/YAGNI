using System;
using System.Security.Principal;
using Newtonsoft.Json;
using VytaTask.Business.Contracts;

namespace VytaTask.Business.Domain.Models.Users
{
    public sealed class User : BaseEntity<int>, IPrincipal
    {
        public User(string email)
        {
            this.Identity = new GenericIdentity(email);
            this.Email = email;
        }

        public Guid Uid { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public bool IsInRole(string role)
        {
            //ToDo: throw new NotImplementedException();
            return true;
        }

        [JsonIgnore]
        [JsonProperty(Required = Required.Default)]
        public IIdentity Identity { get; private set; }
    }
}
