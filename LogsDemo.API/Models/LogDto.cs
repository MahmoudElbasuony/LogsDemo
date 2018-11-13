using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogsDemo.API.Models
{
    public class LogDto
    {
        public LogDto()
        {
            this.Params = new List<string>();
        }
        public string ID { get; set; }
        public string Severity { get; set; }
        public string Type { get; set; }
        public ICollection<string> Params { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
    }
}
