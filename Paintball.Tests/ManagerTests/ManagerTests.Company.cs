using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paintball.Web.Manager;
using Paintball.DAL;
using Paintball.DAL.Entities;
using Paintball.DAL.Repositories;
using Paintball.Web.Services;
using System.Collections.Generic;
using System.Linq;

namespace Paintball.Tests.ManagerTests
{
    public class TestLoggingService : ILoggingService
    {
        public List<Exception> Exceptions { get; set; }
        public TestLoggingService()
        {
            Exceptions = new List<Exception>();
        }
        public void Log(Exception exception)
        {
            Exceptions.Add(exception);
        }
    }
    [TestClass]
    public partial class ManagerTests
    {
        #region Initialization

        private static IManager _manager;

        static Playground _playgroundToDelete;
        static Certificate _certificateToDelete;
        static Company _companyToDelete;
        static Equipment _equipmentToDelete;
        static EquipmentOrder _equipmentOrderToDelete;
        static Event _eventToDelete;
        static Game _gameToDelete;
        static GameType _gameTypeToDelete;
        static News _newsToDelete;
        static Operation _operationToDelete;
        static Task _taskToDelete;
        static IdentityUser _firstUser;
        static TestLoggingService _loggingService;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            IDbContext context = new DbContext();

            IRepository<Certificate, int> certificatesRepository = new CertificateRepository(context);
            IRepository<Company, int> companiesRepository = new CompaniesRepository(context);
            IRepository<Equipment, int> equipmentsRepository = new EquipmentRepository(context);
            IRepository<Event, int> eventsRepository = new EventsRepository(context);
            IRepository<Game, int> gamesRepository = new GamesRepository(context);
            IRepository<GameType, int> gameTypesRepository = new GameTypesRepository(context);
            IRepository<News, int> newsRepository = new NewsRepository(context);
            IRepository<Operation, int> operationsRepository = new OperationsRepository(context);
            IRepository<Playground, int> playgroundsRepository = new PlaygroundsRepository(context);
            IRepository<Task, int> tasksRepository = new TasksRepository(context);
            IRepository<EquipmentOrder, int> equipmentOrdersRepository = new EquipmentOrdersRepository(context);
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>(context);
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            _loggingService = new TestLoggingService();

            _manager = new PaintballManager(context,
                certificatesRepository,
                companiesRepository,
                equipmentsRepository,
                eventsRepository,
                gamesRepository,
                gameTypesRepository,
                newsRepository,
                operationsRepository,
                playgroundsRepository,
                equipmentOrdersRepository,
                tasksRepository,
                userStore,
                roleStore,
                _loggingService
                );

            _firstUser = _manager.UserStore.Users.FirstOrDefault();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            _manager.TasksRepository.Delete(_taskToDelete.Id);
            _manager.EquipmentsRepository.Delete(_equipmentToDelete.Id);
            _manager.CertificatesRepository.Delete(_certificateToDelete.Id);
            _manager.TasksRepository.Delete(_taskToDelete.Id);
            _manager.OperationsRepository.Delete(_operationToDelete.Id);
            _manager.NewsRepository.Delete(_newsToDelete.Id);
            _manager.EventsRepository.Delete(_eventToDelete.Id);
            _manager.EquipmentOrdersRepository.Delete(_equipmentOrderToDelete.Id);
            _manager.GamesRepository.Delete(_gameToDelete.Id);
            _manager.GameTypesRepository.Delete(_gameTypeToDelete.Id);
            _manager.PlaygroundsRepository.Delete(_playgroundToDelete.Id);
            _manager.CompaniesRepository.Delete(_companyToDelete.Id);

            _firstUser.CompanyId = null;

            _manager.UserStore.Update(_firstUser);

            _loggingService.Exceptions.Clear();
        }

        [TestInitialize]
        public void TestInitialization()
        {
            _companyToDelete = _manager.CompaniesRepository.CreateOrUpdate(new Company
            {
                Name = "TestCompanyName",
                Adress = "TestAdress",
                OwnerId = _firstUser.Id
            });

            _firstUser.CompanyId = _companyToDelete.Id;
            _manager.UserStore.Update(_firstUser);

            _playgroundToDelete = _manager.PlaygroundsRepository.CreateOrUpdate(new Playground
            {
                CompanyId = _companyToDelete.Id,
                Name = "TestPlaygroundName",
                Price = 100
            });

            _certificateToDelete = _manager.CertificatesRepository.CreateOrUpdate(new Certificate
            {
                FirstName = "TestCertificateFirstName",
                LastName = "TestCertificateLastName",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                Price = 100,
                CompanyId = _companyToDelete.Id
            });

            _equipmentToDelete = _manager.EquipmentsRepository.CreateOrUpdate(new Equipment
            {
                Name = "TestEquipmentName",
                Price = 100,
                Count = 50,
                CompanyId = _companyToDelete.Id
            });

            _gameTypeToDelete = _manager.GameTypesRepository.CreateOrUpdate(new GameType
            {
                Name = "TestGameTypeName",
                Price = 100,
                CompanyId = _companyToDelete.Id
            });

            _gameToDelete = _manager.GamesRepository.CreateOrUpdate(new Game
            {
                CreatorId = _firstUser.Id,
                BeginDate = DateTime.Now,
                GameType = _gameTypeToDelete.Id,
                Playground = _playgroundToDelete.Id,
                PlayerCount = 10,
                GamePrice = 100,
                CompanyId = _companyToDelete.Id
            });

            _equipmentOrderToDelete = _manager.EquipmentOrdersRepository.CreateOrUpdate(new EquipmentOrder
            {
                GameId = _gameToDelete.Id,
                EquipmentId = _equipmentToDelete.Id,
                Count = 10
            });

            _eventToDelete = _manager.EventsRepository.CreateOrUpdate(new Event
            {
                GameId = _gameToDelete.Id,
                CompanyId = _companyToDelete.Id
            });

            _newsToDelete = _manager.NewsRepository.CreateOrUpdate(new News
            {
                Title = "TestNewsTitle",
                PublishDate = DateTime.Now,
                Text = "TestNewsText",
                CompanyId = _companyToDelete.Id
            });

            _operationToDelete = _manager.OperationsRepository.CreateOrUpdate(new Operation
            {
                Price = _gameToDelete.GamePrice,
                Date = DateTime.Now,
                CompanyId = _companyToDelete.Id,
            });

            _taskToDelete = _manager.TasksRepository.CreateOrUpdate(new Task
            {
                StaffId = _firstUser.Id,
                Text = "TestTaskText",
                CompanyId = _companyToDelete.Id,
                IsCompleted = false
            });
        }

        #endregion // Initialization

        
    }
}
