using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Client1
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
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddScoped<IApiResourceHttpClient, ApiResourceHttpClient>();
            services.Configure<ClientOptions>(Configuration.GetSection("Client")); //IOptions<T> sınıfında bir objeyi DI container'a ekliyor
            services.AddSingleton<ClientOptions>(sp => //DI Eklenen IOptions<T> nesnesini singleton olarak di container'a ekliyorum.
            {
                var instance = sp.GetRequiredService<IOptions<ClientOptions>>();
                return instance.Value;
            });

            services.AddAuthentication(options =>
            {
                //Burada bir authentication schema tanımlıyorum
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies",options => {
                options.AccessDeniedPath = "/Home/AccessDenied";
            }) //Burada cookie based bir authentication yapmak istediğimi belirtiyorum ve bir şema adı belirtiyorum.
            .AddOpenIdConnect("oidc",options => //Burada cookie based authentication ile openidconnect entegrasyonu yapıyorum.
            {
                options.SignInScheme = "Cookies"; //Burada openidconnect ile kullandığım şemanın adını belirtiyorum.
                options.Authority = Configuration.GetValue<string>("Client:AuthorityUrl"); //token dağıtmaktan yetkili server url bilgimi veriyorum.
                options.ClientId = "Client1-MVC"; 
                options.ClientSecret = "secret";
                options.ResponseType = "code id_token"; //Burada identity server bana ne dönmeli onu söylüyorum.
                //Identity Serverdan bir authorization code ve idtoken istediğimi belirtiyorum.

                options.GetClaimsFromUserInfoEndpoint = true; //UserInfo endpointine giderek claimleri alacak
                options.SaveTokens = true; //Eğer oturum başarıyla açıldıysa claimleri alırken tokenleri de almak için kullanılır.
                options.Scope.Add("api1.read"); //Token içerisinde olan scopeları almak için kullanıyorum.
                options.Scope.Add("offline_access");
                options.Scope.Add("CountryAndCity");
                options.Scope.Add("Roles");
                options.Scope.Add("email");

                options.ClaimActions.MapUniqueJsonKey("country", "country");
                options.ClaimActions.MapUniqueJsonKey("city", "city");
                options.ClaimActions.MapUniqueJsonKey("role", "role");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "role", //Role based authentication için hangi role claimini kullanacağını belirtiyoruz.
                    NameClaimType = "name" //HttpContext.User.Name dendiğinde hangi scopedan data çekeceğini belirtmek için kullanıyorum
                };
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
