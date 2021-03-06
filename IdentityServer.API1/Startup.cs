using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.API1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
                {
                    //JWT tokeni yayınlayan server url verilmesi gerekiyor.
                    //Burası sayesinde sistem gidip serverden public key alacak ve token doğru mu ? yayınlayıcı kontrolü yapıyor.
                    options.Authority = "https://localhost:5001";
                    //Gelen token içerisinde aud property içerisinde bu değer olması gerekiyor.
                    //Audience değeri kaynak ismini belirtiyor. Yani bu servis bir kaynak oluyor bu kaynaktan veri isteyen token içerisinde
                    //aud içerisinde benim servisimin adı olması gerekiyor
                    options.Audience = "resource_api1";
                });

            //Burada kullanıcı neye göre authorize olacak onu tanımlamak için kullanıyoruz.
            services.AddAuthorization(options =>
            {
                //Koşullar policyler ile tanımlanıyor
                options.AddPolicy("ReadProduct", policyOptions =>
                 {
                     policyOptions.RequireClaim("scope","api1.read"); //scope alanında api1.read alanına göre yetkilendirme yapacağım
                 });

                options.AddPolicy("UpdateOrCreate", policyOptions =>
                 {
                     //scope alanında api1.write, api1.update alanına göre yetkilendirme yapacağım.
                     policyOptions.RequireClaim("scope", new[] { "api1.write","api1.update"});
                 });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
