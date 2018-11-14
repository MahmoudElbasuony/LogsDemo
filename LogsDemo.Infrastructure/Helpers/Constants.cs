using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Infrastructure.Helpers
{
    public static class Constants
    {
        public const string DefaultDatabaseName = "DefaultDB";
        public const long DefaultThrottlingLimit = 2;
        public const string DefaultThrottlingPeriod = "5m";
        public const string DefaultClientIdHeaderName = "X-ClientId";
    }
}
