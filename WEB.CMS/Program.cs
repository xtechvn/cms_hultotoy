using Caching.RedisWorker;
using Entities.ConfigModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Repositories.IRepositories;
using Repositories.Repositories;
using WEB.CMS.Customize;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Account/RedirectLogin");
    options.LoginPath = new PathString("/Account/RedirectLogin");
    options.ReturnUrlParameter = "url";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // nếu dùng ExpireTimeSpan thì  SlidingExpiration phải set là false. Như vậy cho dù tương tác hay k tương tác thì đều timeout theo thời gian đã set
    options.SlidingExpiration = true; //được sử dụng để thiết lập thời gian sống của cookie dựa trên thời gian cuối cùng mà người dùng đã tương tác với ứng dụng . Nếu người dùng tiếp tục tương tác với ứng dụng trước khi cookie hết hạn, thời gian sống của cookie sẽ được gia hạn thêm.

    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = "Net.Security.Cookie",
        Path = "/",
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };

});
var Configuration = builder.Configuration;
builder.Services.Configure<DataBaseConfig>(Configuration.GetSection("DataBaseConfig"));
builder.Services.Configure<MailConfig>(Configuration.GetSection("MailConfig"));
builder.Services.Configure<DomainConfig>(Configuration.GetSection("DomainConfig"));

// Register services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSingleton<IAllCodeRepository, AllCodeRepository>();
builder.Services.AddSingleton<ICommonRepository, CommonRepository>();
builder.Services.AddSingleton<IMenuRepository, MenuRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IPermissionRepository, PermissionRepository>();
builder.Services.AddTransient<IPositionRepository, PositionRepository>();
builder.Services.AddTransient<INoteRepository, NoteRepository>();
builder.Services.AddTransient<IAllCodeRepository, AllCodeRepository>();
builder.Services.AddTransient<IAttachFileRepository, AttachFileRepository>();
builder.Services.AddTransient<IArticleRepository, ArticleRepository>();
builder.Services.AddTransient<IProvinceRepository, ProvinceRepository>();
builder.Services.AddTransient<IDistrictRepository, DistrictRepository>();
builder.Services.AddTransient<IWardRepository, WardRepository>();
builder.Services.AddTransient<IMFARepository, MFARepository>();
builder.Services.AddTransient<IGroupProductRepository, GroupProductRepository>();
builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<ICustomerManagerRepository, CustomerManagerRepository>();
builder.Services.AddTransient<IAccountClientRepository, AccountClientRepository>();
builder.Services.AddTransient<IBankingAccountRepository, BankingAccountRepository>();
builder.Services.AddTransient<IUserAgentRepository, UserAgentRepository>();
builder.Services.AddTransient<INationalRepository, NationalRepository>();
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddTransient<ILabelRepository, LabelRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ILocationProductRepository, LocationProductRepository>();
// Setting Redis                     
builder.Services.AddSingleton<RedisConn>();
builder.Services.AddSingleton<ManagementUser>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error/Index");
    app.UseHsts();
}
app.UseSession();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
// app.UseAntiXssMiddleware();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "setupManual",
                 pattern: "/product/setup-manual",
                 defaults: new { controller = "product", action = "SetupManual" });
app.MapControllerRoute(name: "transactionsms",
  pattern: "/transactionsms",
  defaults: new { controller = "TransactionSms", action = "Index" });
app.MapControllerRoute(name: "Order",
 pattern: "/Order/{id?}",
 defaults: new { controller = "Order", action = "Orderdetails" });
app.MapControllerRoute(name: "SetService",
 pattern: "SetService/fly/detail/{group_booking_id}",
 defaults: new { controller = "SetService", action = "FlyDetail" });
app.MapControllerRoute(name: "SetService",
 pattern: "SetService/Tour/Detail/{id}",
 defaults: new { controller = "SetService", action = "TourDetail" });
app.MapControllerRoute(name: "SetService",
pattern: "SetService/Others/Detail/{id}",
defaults: new { controller = "SetService", action = "OtherDetail" });
app.MapControllerRoute(name: "SetService",
pattern: "SetService/VinWonder/Detail/{id}",
defaults: new { controller = "SetService", action = "VinWonderDetail" });


app.MapControllerRoute(name: "AccountSetup",
pattern: "/Account/2FA",
defaults: new { controller = "Account", action = "Setup2FA" });
app.MapControllerRoute(name: "ProgramsPackage",
pattern: "/ProgramsPackage/DetailListProgramsPackage/{id}/{Packageid}/{ProgramName}",
defaults: new { controller = "ProgramsPackage", action = "DetailListProgramsPackage" });

app.MapControllerRoute(name: "ProgramsPackage",
pattern: "/ProgramsPackage/ProgramsPriceHotelIndex",
defaults: new { controller = "ProgramsPackage", action = "ProgramsPriceHotelIndex" });
app.Run();
