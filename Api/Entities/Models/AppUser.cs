using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Entities.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}