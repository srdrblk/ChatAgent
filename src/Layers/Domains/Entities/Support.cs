using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Support
    {
        public User User { get; set; }
        public string Subject { get; set; }

        public DateTime CreatedDate { get; set; }   
    }
}
