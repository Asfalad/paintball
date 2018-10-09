using Microsoft.AspNet.Identity;
using Paintball.DAL;
using Paintball.DAL.Entities;
using Paintball.DAL.Repositories;
using Paintball.Web.Constants;
using Paintball.Web.Model;
using Paintball.Web.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.Web.Manager
{
    public partial class PaintballManager : IManager
    {
        #region Initialization

        private IDbContext _context;

        public void SetUser(string Id)
        {
            if(CurrentUser == null)
                CurrentUser = UserStore.FindById(Guid.Parse(Id));
        }

        public IdentityUser CurrentUser { get; set; }
        public IRepository<Certificate, int> CertificatesRepository { get; set; }
        public IRepository<Company, int> CompaniesRepository { get; set; }
        public IRepository<Equipment, int> EquipmentsRepository { get; set; }
        public IRepository<Event, int> EventsRepository { get; set; }
        public IRepository<Game, int> GamesRepository { get; set; }
        public IRepository<GameType, int> GameTypesRepository { get; set; }
        public IRepository<News, int> NewsRepository { get; set; }
        public IRepository<Operation, int> OperationsRepository { get; set; }
        public IRepository<Playground, int> PlaygroundsRepository { get; set; }
        public IRepository<DAL.Entities.Task, int> TasksRepository { get; set; }
        public IRepository<EquipmentOrder, int> EquipmentOrdersRepository { get; set; }
        public ILoggingService LoggingService { get; set; }

        public UserStore<IdentityUser> UserStore { get; set; }

        public RoleStore<IdentityRole> RoleStore { get; set; }

        public PaintballManager(
            IDbContext context,
            IRepository<Certificate, int> certificatesRepository,
            IRepository<Company, int> companiesRepository,
            IRepository<Equipment, int> equipmentsRepository,
            IRepository<Event, int> eventsRepository,
            IRepository<Game, int> gamesRepository,
            IRepository<GameType, int> gameTypesRepository,
            IRepository<News, int> newsRepository,
            IRepository<Operation, int> operationsRepository,
            IRepository<Playground, int> playgroundsRepository,
            IRepository<EquipmentOrder, int> equipmentOrderRepository,
            IRepository<DAL.Entities.Task, int> tasksRepository,
            UserStore<IdentityUser> userStore,
            RoleStore<IdentityRole> roleStore,
            ILoggingService loggingService
            )
        {
            _context = context;

            this.CertificatesRepository = certificatesRepository;
            this.CompaniesRepository = companiesRepository;
            this.EquipmentsRepository = equipmentsRepository;
            this.EventsRepository = eventsRepository;
            this.GamesRepository = gamesRepository;
            this.GameTypesRepository = gameTypesRepository;
            this.NewsRepository = newsRepository;
            this.OperationsRepository = operationsRepository;
            this.PlaygroundsRepository = playgroundsRepository;
            this.TasksRepository = tasksRepository;
            this.EquipmentOrdersRepository = equipmentOrderRepository;
            this.UserStore = userStore;
            this.RoleStore = roleStore;
            this.LoggingService = loggingService;
        }

        #endregion // Initialization

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        #endregion //Dispose

        #region Helpers

        [DebuggerStepThrough]
        static dynamic Combine(object item1, object item2)
        {
            if (item1 == null || item2 == null)
                return item1 ?? item2 ?? new ExpandoObject();

            dynamic expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>;
            foreach (System.Reflection.PropertyInfo fi in item1.GetType().GetProperties())
            {
                result[fi.Name] = fi.GetValue(item1, null);
            }
            foreach (System.Reflection.PropertyInfo fi in item2.GetType().GetProperties())
            {
                result[fi.Name] = fi.GetValue(item2, null);
            }
            return result;
        }

        public bool IsInCompany(int? companyId = null)
        {
            if(companyId != null)
            {
                if(CurrentUser.CompanyId != null)
                {
                    if(companyId.Value == CurrentUser.CompanyId)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                if(CurrentUser.CompanyId != null)
                {
                    if (CurrentUser.CompanyId.HasValue)
                    {
                        return CurrentUser.CompanyId > 0;
                    }
                }
                return false;
            }
        }

        #endregion // Helpers

        #region Order

        public async System.Threading.Tasks.Task<OperationResult<OrderResponse>> GetCompanyForOrder(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<OrderResponse>>(() =>
            {
                OperationResult<OrderResponse> result = new OperationResult<OrderResponse>();
                try
                {
                    var company = CompaniesRepository.Read(id);
                    if(company != null)
                    {
                        company.OwnerId = Guid.Empty;
                        company.Description = "";
                        company.LogoImage = "";
                        var playgrounds = PlaygroundsRepository.Search("CompanyId = @CompanyId",
                            new { PageNumber = 1, PageSize = 100, CompanyId = company.Id });

                        var gameTypes = GameTypesRepository.Search("CompanyId = @CompanyId",
                            new { PageNumber = 1, PageSize = 100, CompanyId = company.Id });

                        var equipment = EquipmentsRepository.Search("CompanyId = @CompanyId",
                            new { PageNumber = 1, PageSize = 100, CompanyId = company.Id });
                        foreach(var equip in equipment)
                        {
                            equip.State = "";
                        }

                        result.SingleResult = new OrderResponse
                        {
                            Company = company,
                            Playgrounds = playgrounds,
                            GameTypes = gameTypes,
                            Equipment = equipment
                        };
                        result.Result = true;
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }

        #endregion // Order
    }
}
