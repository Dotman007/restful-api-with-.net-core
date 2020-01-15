using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingRestFulAPI.AuthenticationHelper;
using BuildingRestFulAPI.DAL;
using BuildingRestFulAPI.Hubs;
using BuildingRestFulAPI.IdentityServerConfiguration;
using BuildingRestFulAPI.IdentityServerConfiguration.Services;
using BuildingRestFulAPI.Services;
using Hangfire;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace BuildingRestFulAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        //This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "RemitaDashboardAPI", Description = "RemitaDashboardAPI" });
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},

                };
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });
            services.AddSignalR();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidAudience = "remitadashboard",
                    ValidIssuer = "remitadashboard",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("remitadashboardKey"))
                };

            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors(c =>
            {
                c.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
            services.AddAuthentication("Basic").AddScheme<BasicAuthenticationOptions, BasicAuhenticationHandler>("Basic", null);
            services.AddTransient<IAuthenticationHandler, BasicAuhenticationHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICustomer, CustomerRepository>();
            services.AddTransient<IBank, BankRepository>();
            services.AddTransient<IAccount, AccountService>();
            services.AddTransient<IAccountCategory, AccountCategoryService>();
            services.AddTransient<IManagement, ManagementService>();
            services.AddTransient<IAgent, AgentService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAccountTransaction, AccountTransactionService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IHangFireService, HangFireService>();
            services.AddDbContext<StoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ShoppingConnection")));
            services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(Configuration.GetConnectionString("ShoppingConnection"));
            });
            services.AddHangfireServer();
            services.AddSession();
        }
        public static ConnectionManager ConnectionManager;


        //This method gets called by the runtime.Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                //The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            ConnectionManager = serviceProvider.GetService<ConnectionManager>();
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/transactionHub");
            });
            app.UseMvc();
            
        }
    }
}
