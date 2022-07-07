namespace NecManager.Web.Service.Extensions;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ServicesExtension
{
    public static IServiceCollection AddServiceClient(this IServiceCollection services)
    {
        return services;
    }
}
