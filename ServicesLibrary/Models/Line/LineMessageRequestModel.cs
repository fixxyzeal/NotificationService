using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServicesLibrary.Models.Line
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class MustHaveOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string[] list = value as string[];
            if (list != null)
            {
                return list.Length > 0 && !list.Contains(string.Empty);
            }
            return false;
        }
    }

    public class LineMessageRequestModel
    {
        [Required(ErrorMessage = "LineChannelAccessToken is required", AllowEmptyStrings = false)]
        public string LineChannelAccessToken { get; set; }

        [Required(ErrorMessage = "To is required", AllowEmptyStrings = false)]
        public string To { get; set; }

        [MustHaveOneElementAttribute(ErrorMessage = "Messages is required at least 1 message")]
        public string[] Messages { get; set; }
    }
}