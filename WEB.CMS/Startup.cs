using Caching.RedisWorker;
using Entities.ConfigModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories;
using Repositories.IRepositories;
using Repositories.Repositories;
using System;
using WEB.Adavigo.CMS.Service;
using WEB.Adavigo.CMS.Service.ServiceInterface;
using WEB.CMS.Customize;

namespace WEB.CMS
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
            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddSession();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
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



            //services.ConfigureApplicationCookie(options =>
            //{                
            //    options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
            //});

            // services.AddRazorPages().AddRazorRuntimeCompilation();

            // Get config to instance model
            services.Configure<DataBaseConfig>(Configuration.GetSection("DataBaseConfig"));
            services.Configure<MailConfig>(Configuration.GetSection("MailConfig"));
            services.Configure<DomainConfig>(Configuration.GetSection("DomainConfig"));

            // Register services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IAllCodeRepository, AllCodeRepository>();
            services.AddSingleton<ICommonRepository, CommonRepository>();
            services.AddSingleton<IMenuRepository, MenuRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<ILabelRepository, LabelRepository>();
            services.AddTransient<IPositionRepository, PositionRepository>();
            services.AddTransient<INoteRepository, NoteRepository>();
            services.AddTransient<IAllCodeRepository, AllCodeRepository>();
            services.AddTransient<IAttachFileRepository, AttachFileRepository>();

            services.AddTransient<ICashbackRepository, CashbackRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IProvinceRepository, ProvinceRepository>();
            services.AddTransient<IDistrictRepository, DistrictRepository>();
            services.AddTransient<IWardRepository, WardRepository>();
            services.AddTransient<IMFARepository, MFARepository>();
            services.AddTransient<IOrderRepositor, OrderRepositor>();

            services.AddTransient<ICampaignRepository, CampaignRepository>();
            services.AddTransient<IPriceDetailRepository, PriceDetailRepository>();
            services.AddTransient<IGroupProductRepository, GroupProductRepository>();
            services.AddTransient<IProductRoomServiceRepository, ProductRoomServiceRepository>();
            services.AddTransient<IProductFlyTicketServiceRepository, ProductFlyTicketServiceRepository>();
            services.AddTransient<IServicePriceRoomRepository, ServicePriceRoomRepository>();
            services.AddSingleton<IRoomFunRepository, RoomFunRepository>();

            services.AddTransient<ITelegramRepository, TelegramRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IContractPayRepository, ContractPayRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IBagageRepository, BagageRepository>();
            services.AddTransient<IFlightSegmentRepository, FlightSegmentRepository>();
            services.AddTransient<IFlyBookingDetailRepository, FlyBookingDetailRepository>();
            services.AddTransient<IDepositHistoryRepository, DepositHistoryRepository>();
            services.AddTransient<ICustomerManagerRepository, CustomerManagerRepository>();
            services.AddTransient<IPaymentAccountRepository, PaymentAccountRepository>();
            services.AddTransient<IContractRepository, ContractRepository>();
            services.AddTransient<IPolicyRepository, PolicyRepository>();
            services.AddTransient<IIdentifierServiceRepository, IdentifierServiceRepository>();
            services.AddTransient<IAccountClientRepository, AccountClientRepository>();

            services.AddTransient<IHotelBookingRepositories, HotelBookingRepositories>();
            services.AddTransient<IHotelBookingRoomRepository, HotelBookingRoomsRepository>();
            services.AddTransient<IHotelBookingRoomRatesRepository, HotelBookingRoomRatesRepository>();
            services.AddTransient<IHotelBookingRoomExtraPackageRepository, HotelBookingRoomExtraPackageRepository>();
            services.AddTransient<IHotelBookingGuestRepository, HotelBookingGuestRepository>();
            services.AddTransient<IContactClientRepository, ContactClientRepository>();
            services.AddTransient<IBankingAccountRepository, BankingAccountRepository>();
            services.AddTransient<IUserAgentRepository, UserAgentRepository>();

            services.AddTransient<IAirlinesRepository, AirlinesRepository>();
            services.AddTransient<IPassengerRepository, PassengerRepository>();
            services.AddTransient<ITourRepository, TourRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<INationalRepository, NationalRepository>();
            services.AddTransient<IPaymentRequestRepository, PaymentRequestRepository>();
            services.AddTransient<IPaymentVoucherRepository, PaymentVoucherRepository>();
            services.AddTransient<IHotelBookingCodeRepository, HotelBookingCodeRepository>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();
            services.AddTransient<ITourPackagesOptionalRepository, TourPackagesOptionalRepository>();
            services.AddTransient<IPlaygroundDetaiRepository, PlaygroundDetaiRepository>();
            services.AddTransient<IInvoiceRequestRepository, InvoiceRequestRepository>();
			services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IOtherBookingRepository, OtherBookingRepository>();
            services.AddTransient<IVinWonderBookingRepository, VinWonderBookingRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IProgramsPackageReprository, ProgramsPackageReprository>();
            services.AddTransient<IProgramsReprository, ProgramsReprository>();
            services.AddTransient<IHotelRepository, HotelRepository>();
            services.AddTransient<IDebtStatisticRepository, DebtStatisticRepository>();
            // Setting Redis                     
            services.AddSingleton<RedisConn>();
            services.AddSingleton<ManagementUser>();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(name: "setupManual",
                  pattern: "/product/setup-manual",
                  defaults: new { controller = "product", action = "SetupManual" });
                endpoints.MapControllerRoute(name: "transactionsms",
                  pattern: "/transactionsms",
                  defaults: new { controller = "TransactionSms", action = "Index" });
                endpoints.MapControllerRoute(name: "Order",
                 pattern: "/Order/{id?}",
                 defaults: new { controller = "Order", action = "Orderdetails" });  
                endpoints.MapControllerRoute(name: "SetService",
                 pattern: "SetService/fly/detail/{group_booking_id}",
                 defaults: new { controller = "SetService", action = "FlyDetail" });
                endpoints.MapControllerRoute(name: "SetService",
                 pattern: "SetService/Tour/Detail/{id}",
                 defaults: new { controller = "SetService", action = "TourDetail" });
                endpoints.MapControllerRoute(name: "SetService",
                pattern: "SetService/Others/Detail/{id}",
                defaults: new { controller = "SetService", action = "OtherDetail" });
                endpoints.MapControllerRoute(name: "SetService",
                pattern: "SetService/VinWonder/Detail/{id}",
                defaults: new { controller = "SetService", action = "VinWonderDetail" });
              

                endpoints.MapControllerRoute(name: "AccountSetup",
                pattern: "/Account/2FA",
                defaults: new { controller = "Account", action = "Setup2FA" });
                endpoints.MapControllerRoute(name: "ProgramsPackage",
               pattern: "/ProgramsPackage/DetailListProgramsPackage/{id}/{Packageid}/{ProgramName}",
               defaults: new { controller = "ProgramsPackage", action = "DetailListProgramsPackage" });

                endpoints.MapControllerRoute(name: "ProgramsPackage",
           pattern: "/ProgramsPackage/ProgramsPriceHotelIndex",
           defaults: new { controller = "ProgramsPackage", action = "ProgramsPriceHotelIndex" });




            });
        }
    }
}
