using ServicesLibrary.Models.Line;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Models
{
    public enum HostedNotiJobType
    {
        LineNotification = 1,
        EmailNotification
    }

    public static class HostedNotiJobKey
    {
        public static readonly string HostedJob = "H";
    }

    public class HostedNotiJob
    {
        public int Type { get; set; }
        public LineMessageRequestModel LineMessageRequestModel { get; set; }
    }
}