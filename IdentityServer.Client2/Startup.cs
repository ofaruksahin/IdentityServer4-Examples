using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Client2
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
            services.AddControllersWithViews();
            services.AddAuthentication(options =>
            {
                //Burada bir authentication schema tanımlıyorum
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies", options => {
                options.AccessDeniedPath = "/Home/AccessDenied";
            }) //Burada cookie based bir authentication yapmak istediğimi belirtiyorum ve bir şema adı belirtiyorum.
            .AddOpenIdConnect("oidc", options => //Burada cookie based authentication ile openidconnect entegrasyonu yapıyorum.
            {
                options.SignInScheme = "Cookies"; //Burada openidconnect ile kullandığım şemanın adını belirtiyorum.
                options.Authority = Configuration.GetValue<string>("Client:AuthorityUrl"); //token dağıtmaktan yetkili server url bilgimi veriyorum.
                options.ClientId = "Client2-MVC";
                options.ClientSecret = "secret";
                options.ResponseType = "code id_token"; //Burada identity server bana ne dönmeli onu söylüyorum.
                //Identity Serverdan bir authorization code ve idtoken istediğimi belirtiyorum.

                options.GetClaimsFromUserInfoEndpoint = true; //UserInfo endpointine giderek claimleri alacak
                options.SaveTokens = true; //Eğer oturum başarıyla açıldıysa claimleri alırken tokenleri de almak için kullanılır.
                options.Scope.Add("api1.read"); //Token içerisinde olan scopeları almak için kullanıyorum.
                options.Scope.Add("offline_access");
                options.Scope.Add("CountryAndCity");
                options.Scope.Add("Roles");

                options.ClaimActions.MapUniqueJsonKey("country", "country");
                options.ClaimActions.MapUniqueJsonKey("city", "city");
                options.ClaimActions.MapUniqueJsonKey("role", "role");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "role" //Role based authentication için hangi role claimini kullanacağını belirtiyoruz.
                };
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
