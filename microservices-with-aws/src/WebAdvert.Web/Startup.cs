using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAdvert.Web.Services;
using WebAdvert.Web.Models.Configuration;
using WebAdvert.Web.Services.Clients;
using Polly;
using System.Net.Http;
using System;
using Polly.Extensions.Http;
using System.Net;

namespace WebAdvert.Web
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
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.Configure<AwsSettings>(Configuration.GetSection("AWS"));

            services.AddCognitoIdentity();

            services.ConfigureApplicationCookie(options => { options.LoginPath = "/auth/signin"; });

            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizePage("/index");
                options.Conventions.AllowAnonymousToFolder("/pages/auth");
            });

            services.AddServerSideBlazor();

            services.AddScoped<AuthService>();

            services.AddHttpClient<AdvertApiClient>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPatternPolicy());

            services.AddHttpClient<FilesApiClient>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPatternPolicy());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapRazorPages();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPatternPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
