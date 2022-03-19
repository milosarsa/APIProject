using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using MyLog;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyLog
{
    public static class MyLogExtensions
    {
        public static IServiceCollection ConfigureMyLogger(this IServiceCollection service)
        {
            try
            {
                MyLog.ConfigureLogger();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Logger configuration failed with error:\n{ex.ToString()}");
            }
            
            service.AddScoped<IMyLogger, MyLogger>();
            return service;
        }

    }

    
}
