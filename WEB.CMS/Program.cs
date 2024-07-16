using Caching.RedisWorker;
using Entities.ConfigModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Repositories.IRepositories;
using Repositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Authenticate/Login");
    options.LoginPath = new PathString("/Authenticate/Login");
    options.ReturnUrlParameter = "RequestPath";
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = "Net.Security.Cookie",
        Path = "/",
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
});


// Get config to instance model
builder.Services.Configure<DataBaseConfig>(builder.Configuration.GetSection("DataBaseConfig"));
builder.Services.Configure<MailConfig>(builder.Configuration.GetSection("MailConfig"));
builder.Services.Configure<DomainConfig>(builder.Configuration.GetSection("DomainConfig"));

// Register builder.Services
builder.Services.AddSingleton<IAllCodeRepository, AllCodeRepository>();
builder.Services.AddSingleton<ICommonRepository, CommonRepository>();
builder.Services.AddSingleton<IMenuRepository, MenuRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IPermissionRepository, PermissionRepository>();
builder.Services.AddTransient<IGroupProductRepository, GroupProductRepository>();
builder.Services.AddTransient<ILabelRepository, LabelRepository>();
builder.Services.AddTransient<IPositionRepository, PositionRepository>();
builder.Services.AddTransient<IProductClassificationRepository, ProductClassificationRepository>();
builder.Services.AddTransient<ICampaignAdsRepository, CampaignAdsRepository>();
builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<INoteRepository, NoteRepository>();
builder.Services.AddTransient<IAllCodeRepository, AllCodeRepository>();
builder.Services.AddTransient<IAttachFileRepository, AttachFileRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICashbackRepository, CashbackRepository>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<IArticleRepository, ArticleRepository>();
builder.Services.AddTransient<IProvinceRepository, ProvinceRepository>();
builder.Services.AddTransient<IDistrictRepository, DistrictRepository>();
builder.Services.AddTransient<IWardRepository, WardRepository>();
builder.Services.AddTransient<IMFARepository, MFARepository>();
builder.Services.AddTransient<IOrderProgressRepository, OrderProgessRepository>();
builder.Services.AddTransient<ILocationProductRepository, LocationProductRepository>();
builder.Services.AddTransient<IAutomaticPurchaseAmzRepository, AutomaticPurchaseAmzRepository>();
builder.Services.AddTransient<IAutomaticPurchaseHistoryRepository, AutomaticPurchaseHistoryRepository>();

// Setting Redis                     
builder.Services.AddSingleton<RedisConn>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();

}


app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Account",
    pattern: "{controller=Account}/{action=Login}/{requestPath?}");
app.MapControllerRoute(name: "setupManual",
  pattern: "/product/setup-manual",
  defaults: new { controller = "product", action = "SetupManual" });

app.Run();
