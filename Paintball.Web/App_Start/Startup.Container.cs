namespace Paintball.Web
{
    using System.Reflection;
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Paintball.Web.Services;
    using Owin;
    using DAL;
    using DAL.Repositories;
    using DAL.Entities;
    using Manager;
    using App_Start;
    using Microsoft.AspNet.Identity.Owin;
    using System;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Microsoft.Owin;
    using System.Collections.Generic;
    using Autofac.Core;
    using Microsoft.Owin.Security.DataProtection;
    using Autofac.Integration.WebApi;
    using System.Web.Http;
    using Controllers;
    using Microsoft.Owin.Security.DataHandler;
    using WebApiTesting.Controllers;

    /// <summary>
    /// Register types into the Autofac Inversion of Control (IOC) container. Autofac makes it easy to register common 
    /// MVC types like the <see cref="UrlHelper"/> using the <see cref="AutofacWebTypesModule"/>. Feel free to change 
    /// this to another IoC container of your choice but ensure that common MVC types like <see cref="UrlHelper"/> are 
    /// registered. See http://autofac.readthedocs.org/en/latest/integration/aspnet.html.
    /// </summary>
    public partial class Startup
    {
        public static void ConfigureContainer(IAppBuilder app)
        {
            IContainer container = CreateContainer(app);
            app.UseAutofacMiddleware(container);

            // Register MVC Types 
            
            app.UseAutofacMvc();
        }

        private static IContainer CreateContainer(IAppBuilder app)
        {
            ContainerBuilder builder = new ContainerBuilder();
            Assembly assembly = Assembly.GetExecutingAssembly();

            RegisterServices(builder, app);
            RegisterMvc(builder, assembly);
            
            IContainer container = builder.Build();

            SetMvcDependencyResolver(container);
            
            return container;
        }

        private static void RegisterServices(ContainerBuilder builder, IAppBuilder app)
        {
            builder.RegisterType<AccountApiController>().InstancePerRequest();
            builder.RegisterType<CertificatesApiController>().InstancePerRequest();
            builder.RegisterType<EventsApiController>().InstancePerRequest();
            builder.RegisterType<ImageApiController>().InstancePerRequest();
            builder.RegisterType<CompaniesApiController>().InstancePerRequest();
            builder.RegisterType<EquipmentApiController>().InstancePerRequest();
            builder.RegisterType<EquipmentOrdersApiController>().InstancePerRequest();
            builder.RegisterType<EventsApiController>().InstancePerRequest();
            builder.RegisterType<GamesApiController>().InstancePerRequest();
            builder.RegisterType<GameTypesApiController>().InstancePerRequest();
            builder.RegisterType<NewsApiController>().InstancePerRequest();
            builder.RegisterType<OperationsApiController>().InstancePerRequest();
            builder.RegisterType<PlaygroundsApiController>().InstancePerRequest();
            builder.RegisterType<TasksApiController>().InstancePerRequest();
            builder.RegisterType<UsersApiController>().InstancePerRequest();
            builder.RegisterType<OrderApiController>().InstancePerRequest();
            builder.RegisterType<MyGamesApiController>().InstancePerRequest();

            builder.RegisterType<DbContext>().As<IDbContext>().InstancePerRequest();
            builder.RegisterType<IdentitySignInManager>().As<SignInManager<IdentityUser, Guid>>().InstancePerRequest();

            builder.RegisterType<CertificateRepository>().As<IRepository<Certificate, int>>().InstancePerRequest();
            builder.RegisterType<CompaniesRepository>().As<IRepository<Company, int>>().InstancePerRequest();
            builder.RegisterType<EquipmentRepository>().As<IRepository<Equipment, int>>().InstancePerRequest();
            builder.RegisterType<EquipmentOrdersRepository>().As<IRepository<EquipmentOrder, int>>().InstancePerRequest();
            builder.RegisterType<EventsRepository>().As<IRepository<Event, int>>().InstancePerRequest();
            builder.RegisterType<GamesRepository>().As<IRepository<Game, int>>().InstancePerRequest();
            builder.RegisterType<GameTypesRepository>().As<IRepository<GameType, int>>().InstancePerRequest();
            builder.RegisterType<NewsRepository>().As<IRepository<News, int>>().InstancePerRequest();
            builder.RegisterType<OperationsRepository>().As<IRepository<Operation, int>>().InstancePerRequest();
            builder.RegisterType<PlaygroundsRepository>().As<IRepository<Playground, int>>().InstancePerRequest();
            builder.RegisterType<TasksRepository>().As<IRepository<Task, int>>().InstancePerRequest();
            
            builder.RegisterType<UserStore<IdentityUser>>().InstancePerRequest();
            builder.RegisterType<RoleStore<IdentityRole>>().InstancePerRequest();
            builder.Register<IAuthenticationManager>((c, p) => c.Resolve<IOwinContext>().Authentication).InstancePerRequest();
            builder.RegisterType<PaintballManager>().As<IManager>().InstancePerRequest();

            var dataProtectionProvider = app.GetDataProtectionProvider();
            builder.Register<UserManager<IdentityUser, Guid>>((c, p) => BuildUserManager(c, p, dataProtectionProvider));
            builder.Register<ISecureDataFormat<AuthenticationTicket>>((c, p) => new TicketDataFormat(dataProtectionProvider.Create("ASP.NET Identity")));

            builder.RegisterType<BrowserConfigService>().As<IBrowserConfigService>().InstancePerRequest();
            builder.RegisterType<CacheService>().As<ICacheService>().SingleInstance();
            //builder.RegisterType<FeedService>().As<IFeedService>().InstancePerRequest();
            builder.RegisterType<LoggingService>().As<ILoggingService>().SingleInstance();
            builder.RegisterType<ManifestService>().As<IManifestService>().InstancePerRequest();
            builder.RegisterType<OpenSearchService>().As<IOpenSearchService>().InstancePerRequest();
            builder.RegisterType<RobotsService>().As<IRobotsService>().InstancePerRequest();
            builder.RegisterType<SitemapService>().As<ISitemapService>().InstancePerRequest();
            builder.RegisterType<SitemapPingerService>().As<ISitemapPingerService>().InstancePerRequest();
        }

        private static void RegisterMvc(ContainerBuilder builder, Assembly assembly)
        {
            // Register Common MVC Types
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register MVC Filters
            builder.RegisterFilterProvider();

            // Register MVC Controllers
            builder.RegisterControllers(assembly);
            
            //builder.RegisterApiControllers(assembly);
            //builder.RegisterApiControllers("Api", assembly);

        }

        private static UserManager<IdentityUser, Guid> BuildUserManager(
        IComponentContext context,
        IEnumerable<Parameter> parameters,
        IDataProtectionProvider dataProtectionProvider)
        {
            var manager =
            new IdentityUserManager(context.Resolve<UserStore<IdentityUser>>());
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<IdentityUser, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<IdentityUser, Guid>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<IdentityUser, Guid>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            
            manager.UserTokenProvider =
                new DataProtectorTokenProvider<IdentityUser, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
            
            return manager;
        }

        /// <summary>
        /// Sets the ASP.NET MVC dependency resolver.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void SetMvcDependencyResolver(IContainer container)
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}