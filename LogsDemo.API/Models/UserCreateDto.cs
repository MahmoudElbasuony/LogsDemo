using LogsDemo.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogsDemo.API.Models
{
    public class UserCreateDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Name Required")]
        public string Name { get; set; }

        public long? ThrottlingLimit { get; set; } = Constants.DefaultThrottlingLimit;

        [RegularExpression("[0-9]+[smhd]", ErrorMessage = "Throttling Period Should be in format :  [INT][ s | m | h | d ]")]
        public string ThrottlingPeriod { get; set; } = Constants.DefaultThrottlingPeriod;
    }
}
