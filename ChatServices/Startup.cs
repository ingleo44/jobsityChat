using System.IO;
using System.Reflection;
using System.Text;
using Business.Classes;
using Business.Interfaces;
using ChatServices.MiddleWare;
using ChatServices.OperationFilters;
using Entities.Classes;
using Entities.Repositories;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace ChatServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthentication, Authentication>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserSupervisor, UserSupervisor>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<TokenManagerMiddleWare>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.ConfigureSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"));
                //options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });

            services.AddDbContext<LocalDbContext>(options => options.UseInMemoryDatabase(databaseName: "JobsityChat"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "CHAT",
                    Description = "CHAT DEMO APP",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "LeonardoSanchez",
                        Email = "leonardosanchez4@hotmail.com",
                        Url = "https://twitter.com/ingleo44"
                    }
                });

            });

            var jwtSection = Configuration.GetSection("jwt");
            var jwtOptions = new JwtOptions();
            jwtSection.Bind(jwtOptions);
            services.Configure<JwtOptions>(jwtSection);
            services.ConfigureSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"));
                options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };


                });
            services.AddSession();
            services.AddCors(o => o.AddPolicy("ETPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CHAT API");
                    c.RoutePrefix = string.Empty;
                });
            }

            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseMiddleware<TokenManagerMiddleWare>();
            app.UseAuthentication();
            app.UseSession();
            

            app.UseHttpsRedirection();
            app.UseCors("ETPolicy");
            app.UseMvc();
        }
    }

   
}
