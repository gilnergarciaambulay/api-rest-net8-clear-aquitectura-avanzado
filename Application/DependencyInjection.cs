using Microsoft.Extensions.DependencyInjection;

using Application.Services;
using Application.Interfaces.IService;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
