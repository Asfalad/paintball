using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paintball.DAL;
using Paintball.DAL.Repositories;
using Paintball.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace Paintball.Tests.RepositoriesTests
{
    [TestClass]
    public class EventRepositoryTests
    {
        #region Initialize 

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

        static private IDbContext _context;
        static private IRepository<Playground, int> _playgroundRepository;
        static private IRepository<Certificate, int> _certificateRepository;
        static private IRepository<Company, int> _companiesRepository;
        static private IRepository<Equipment, int> _equipmentsRepository;
        static private IRepository<EquipmentOrder, int> _equipmentOrdersRepository;
        static private IRepository<Event, int> _eventsRepository;
        static private IRepository<Game, int> _gamesRepository;
        static private IRepository<GameType, int> _gameTypesRepository;
        static private IRepository<News, int> _newsRepository;
        static private IRepository<Operation, int> _operationsRepository;
        static private IRepository<Task, int> _tasksRepository;
        static private UserStore<IdentityUser> _userStore;

        [ClassInitialize]
        public static void Initialization(TestContext testContext)
        {
            _context = new DbContext();

            _playgroundRepository = new PlaygroundsRepository(_context);
            _certificateRepository = new CertificateRepository(_context);
            _companiesRepository = new CompaniesRepository(_context);
            _equipmentsRepository = new EquipmentRepository(_context);
            _equipmentOrdersRepository = new EquipmentOrdersRepository(_context);
            _eventsRepository = new EventsRepository(_context);
            _gamesRepository = new GamesRepository(_context);
            _gameTypesRepository = new GameTypesRepository(_context);
            _newsRepository = new NewsRepository(_context);
            _operationsRepository = new OperationsRepository(_context);
            _tasksRepository = new TasksRepository(_context);

            _userStore = new UserStore<IdentityUser>(_context);

            _firstUser = _userStore.Users.FirstOrDefault();

            _companyToDelete = _companiesRepository.CreateOrUpdate(new Company
            {
                Name = "TestCompanyName",
                Adress = "TestAdress",
                OwnerId = _firstUser.Id
            });
            _firstUser.CompanyId = _companyToDelete.Id;
            _userStore.Update(_firstUser);

            _playgroundToDelete = _playgroundRepository.CreateOrUpdate(new Playground
            {
                CompanyId = _companyToDelete.Id,
                Name = "TestPlaygroundName",
                Price = 100
            });

            _certificateToDelete = _certificateRepository.CreateOrUpdate(new Certificate
            {
                FirstName = "TestCertificateFirstName",
                LastName = "TestCertificateLastName",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                Price = 100,
                CompanyId = _companyToDelete.Id
            });

            _equipmentToDelete = _equipmentsRepository.CreateOrUpdate(new Equipment
            {
                Name = "TestEquipmentName",
                Price = 100,
                Count = 50,
                CompanyId = _companyToDelete.Id
            });

            _gameTypeToDelete = _gameTypesRepository.CreateOrUpdate(new GameType
            {
                Name = "TestGameTypeName",
                Price = 100,
                CompanyId = _companyToDelete.Id
            });

            _gameToDelete = _gamesRepository.CreateOrUpdate(new Game
            {
                CreatorId = _firstUser.Id,
                BeginDate = DateTime.Now,
                GameType = _gameTypeToDelete.Id,
                Playground = _playgroundToDelete.Id,
                PlayerCount = 10,
                GamePrice = 100,
                CompanyId = _companyToDelete.Id
            });

            _equipmentOrderToDelete = _equipmentOrdersRepository.CreateOrUpdate(new EquipmentOrder
            {
                GameId = _gameToDelete.Id,
                EquipmentId = _equipmentToDelete.Id,
                Count = 10
            });

            _eventToDelete = _eventsRepository.CreateOrUpdate(new Event
            {
                GameId = _gameToDelete.Id,
                CompanyId = _companyToDelete.Id,
                Title = "NewTestTitle",
                Description = "<p>New Description</p><p>New Test Description</p>"
            });

            _newsToDelete = _newsRepository.CreateOrUpdate(new News
            {
                Title = "TestNewsTitle",
                PublishDate = DateTime.Now,
                Text = "TestNewsText",
                CompanyId = _companyToDelete.Id
            });

            _operationToDelete = _operationsRepository.CreateOrUpdate(new Operation
            {
                Price = _gameToDelete.GamePrice,
                Date = DateTime.Now,
                CompanyId = _companyToDelete.Id,
            });

            _taskToDelete = _tasksRepository.CreateOrUpdate(new Task
            {
                StaffId = _firstUser.Id,
                Text = "TestTaskText",
                CompanyId = _companyToDelete.Id,
                IsCompleted = false
            });
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            if (_context != null)
            {
                _tasksRepository.Delete(_taskToDelete.Id);
                _equipmentsRepository.Delete(_equipmentToDelete.Id);
                _certificateRepository.Delete(_certificateToDelete.Id);
                _tasksRepository.Delete(_taskToDelete.Id);
                _operationsRepository.Delete(_operationToDelete.Id);
                _newsRepository.Delete(_newsToDelete.Id);
                _eventsRepository.Delete(_eventToDelete.Id);
                _equipmentOrdersRepository.Delete(_equipmentOrderToDelete.Id);
                _gamesRepository.Delete(_gameToDelete.Id);
                _gameTypesRepository.Delete(_gameTypeToDelete.Id);
                _playgroundRepository.Delete(_playgroundToDelete.Id);
                _companiesRepository.Delete(_companyToDelete.Id);

                _firstUser.CompanyId = null;

                _userStore.Update(_firstUser);

                _context.Dispose();
            }
        }

        #endregion // Initialize

        #region Read Create Update Delete Test Success

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();
            
            Update_Item_Success(id);

            Delete_Item_Success(id);
            
        }

        public int CreateOrUpdate_Create_Item_Success()
        {
            Event equip = new Event()
            {
                GameId = _gameToDelete.Id,
                CompanyId = _companyToDelete.Id,
                Description = "<p>New Description</p><p>New Test Description</p>",
                Title = "NewTitle"
            };

            equip = _eventsRepository.CreateOrUpdate(equip);

            Assert.IsNotNull(equip, "Event are not created");

            return equip.Id;
        }

        public void Update_Item_Success(int id)
        {
            Event equip = _eventsRepository.Read(id);

            Assert.IsNotNull(equip, "Event not found");

            equip.Description = "EventUpdateTestDescription";

            _eventsRepository.CreateOrUpdate(equip);

            var updatedEvent = _eventsRepository.Read(id);

            Assert.IsNotNull(updatedEvent, "updatedEvent is null");

            Assert.AreEqual(equip.Description, updatedEvent.Description, "updatedEvent Name and equipmet Name not same");
        }

        public void Delete_Item_Success(int id)
        {
            var item = _eventsRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_eventsRepository.Delete(id), "Not deleted");

            item = _eventsRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<Event> EventsToDelete = new List<Event>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    Event testEvent = new Event()
                    {
                        Description = "<p>New Description</p><p>New Test Description</p>",
                        GameId = _gameToDelete.Id,
                        CompanyId = _companyToDelete.Id,
                        Title = "NewTitle"
                    };

                    EventsToDelete.Add(_eventsRepository.CreateOrUpdate(testEvent));
                }

                var Events = _eventsRepository.GetAll(15, 1);

                Assert.AreEqual(15, Events.Count(), "Items count are not equal 15");
                
                Events = _eventsRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, Events.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var Event in EventsToDelete)
                {
                    _eventsRepository.Delete(Event.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _eventsRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Empty_Event_Throws()
        {
            Event equip = new Event();
            _eventsRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_GameId_Only_Throws()
        {
            Event equip = new Event()
            {
                GameId = 2,
            };
            _eventsRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_CompanyId_Only_Throws()
        {
            Event equip = new Event()
            {
                CompanyId = 1
            };
            _eventsRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_eventsRepository.Read(-10));
            Assert.IsNull(_eventsRepository.Read(100000));
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _eventsRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            Event comp = new Event()
            {
                CompanyId = _companyToDelete.Id,
                GameId = _gameToDelete.Id,
                Description = "<p>New Description</p><p>New Test Description</p>",
                Title = "New Title"
            };

            comp = _eventsRepository.CreateOrUpdate(comp);

            Assert.IsNotNull(comp);

            comp.Description = string.Empty;

            _eventsRepository.Update(comp);

            comp = _eventsRepository.Read(comp.Id);

            _eventsRepository.Delete(comp.Id);

            Assert.IsTrue(comp.Description == string.Empty);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            Event comp = new Event();
            _eventsRepository.Update(comp);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_eventsRepository.Delete(-1));
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _eventsRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _eventsRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        public void GetAll_Wrong_Pages_Return_Empry_List()
        {
            var items = _eventsRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #region Search

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        public void Search_Success()
        {
            List<Event> itemsToDelete = new List<Event>();
            for (int i = 0; i < 5; i++)
            {
                Event item = new Event()
                {
                    GameId = _gameToDelete.Id,
                    CompanyId = _companyToDelete.Id,
                    Description = "EventTestDescription" + i,
                    Title = "NewTitle"
                };
                itemsToDelete.Add(_eventsRepository.CreateOrUpdate(item));
            }

            var items = _eventsRepository.Search("Description = @Description", new { PageSize = 10, PageNumber = 1, Description = "EventTestDescription1" });

            foreach (var item in itemsToDelete)
            {
                _eventsRepository.Delete(item.Id);
            }

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual("EventTestDescription1", items.FirstOrDefault().Description);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _eventsRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _eventsRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("EventsRepositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _eventsRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
