using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Models
{
    public class Social : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
