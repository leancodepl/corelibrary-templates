using Autofac;
using LeanCode.Components;
using LncdApp.DomainName.Services.DataAccess;
using LncdApp.DomainName.Services.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LncdApp.DomainName.Services
{
    public class DomainNameModule : AppModule
    {
        private readonly string connectionString;

        public DomainNameModule(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DomainNameDbContext>(opts =>
                opts.UseSqlServer(connectionString, cfg =>
                    cfg.MigrationsAssembly("LncdApp.Migrations")));

            services.AddIdentity<AuthUser, AuthRole>()
                .AddEntityFrameworkStores<DomainNameDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#$%&'*+-/=?^_`{|}~.""(),:;<>@[\] ";
                options.Password.RequiredLength = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
        }

        protected override void Load(ContainerBuilder builder)
        {
            var self = typeof(DomainNameModule).Assembly;

            builder.Register(c => c.Resolve<DomainNameDbContext>())
                .AsImplementedInterfaces();
        }
    }
}
