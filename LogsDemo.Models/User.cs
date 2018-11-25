using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LogsDemo.Models
{
    public class User<TKey> : BaseEntity<TKey>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Name Required")]
        public string Name { get; set; }
        /// <summary>
        /// Set Throttle value in seconds  
        /// </summary>
        public long ThrottlingLimit { get; set; } = 5;

        [RegularExpression("^[0-9]+[smhd]$", ErrorMessage = "Throttling Period Should be in format :  {INT}{ s | m | h | d }")]
        public string ThrottlingPeriod { get; set; } = "5m";



    }
}
