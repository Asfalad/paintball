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
    public class PlaygroundsRepositoryTests
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
                Name ="TestCompanyName",
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
        [TestCategory("PlaygroundsRepositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();

            Update_Item_Success(id);

            Delete_Item_Success(id);
        }

        public int CreateOrUpdate_Create_Item_Success()
        {
            Playground item = new Playground()
            {
                CompanyId = _companyToDelete.Id,
                Name = "TestPlaygroundName",
                Price = 100
            };

            item = _playgroundRepository.CreateOrUpdate(item);

            Assert.IsNotNull(item, "Playground are not created");

            return item.Id;
        }

        public void Update_Item_Success(int id)
        {
            Playground item = _playgroundRepository.Read(id);

            Assert.IsNotNull(item, "Playground not found");

            item.Name = "PlaygroundUpdateTestName";

            _playgroundRepository.Update(item);

            var updatedPlayground = _playgroundRepository.Read(id);

            Assert.IsNotNull(updatedPlayground, "updatedPlayground is null");

            Assert.AreEqual("PlaygroundUpdateTestName", updatedPlayground.Name, "updatedPlayground Name and equipmet Name not same");
        }

        public void Delete_Item_Success(int id)
        {
            var item = _playgroundRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_playgroundRepository.Delete(id), "Not deleted");

            item = _playgroundRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<Playground> PlaygroundsToDelete = new List<Playground>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    Playground testPlayground = new Playground()
                    {
                        Name = "PlaygroundTestName",
                        CompanyId = _companyToDelete.Id,
                        Price = 100
                    };

                    PlaygroundsToDelete.Add(_playgroundRepository.CreateOrUpdate(testPlayground));
                }

                var Playgrounds = _playgroundRepository.GetAll(15, 1);

                Assert.AreEqual(15, Playgrounds.Count(), "Items count are not equal 15");

                Playgrounds = _playgroundRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, Playgrounds.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var Playground in PlaygroundsToDelete)
                {
                    _playgroundRepository.Delete(Playground.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _playgroundRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Empty_Playground_Throws()
        {
            Playground equip = new Playground();
            _playgroundRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_CompanyId_Only_Throws()
        {
            Playground equip = new Playground()
            {
                CompanyId = _companyToDelete.Id
            };
            _playgroundRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Name_Only_Throws()
        {
            Playground equip = new Playground()
            {
                Name = "PlaygroundTestName"
            };
            _playgroundRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_playgroundRepository.Read(-10));
            Assert.IsNull(_playgroundRepository.Read(100000));
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _playgroundRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            Playground comp = new Playground()
            {
                CompanyId = _companyToDelete.Id,
                Name = "PlaygroundTestText",
                Price = 100
            };

            comp = _playgroundRepository.CreateOrUpdate(comp);

            Assert.IsNotNull(comp);

            comp.Name = "PlaygroundTestName";

            _playgroundRepository.Update(comp);

            comp = _playgroundRepository.Read(comp.Id);

            _playgroundRepository.Delete(comp.Id);

            Assert.IsTrue(comp.Name == "PlaygroundTestName");
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            Playground item = new Playground();
            _playgroundRepository.Update(item);
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_playgroundRepository.Delete(-1));
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _playgroundRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _playgroundRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        public void GetAll_Wrong_Pages_Return_Empry_List()
        {
            var items = _playgroundRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #region Search

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        public void Search_Success()
        {
            List<Playground> itemsToDelete = new List<Playground>();
            for (int i = 0; i < 5; i++)
            {
                Playground item = new Playground()
                {
                    Name = "PlaygroundTestName" + i,
                    CompanyId = _companyToDelete.Id,
                    Price = 100
                };

                itemsToDelete.Add(_playgroundRepository.CreateOrUpdate(item));
            }

            var items = _playgroundRepository.Search("Name = @Name", new { PageSize = 10, PageNumber = 1, Name = "PlaygroundTestName1" });

            foreach (var item in itemsToDelete)
            {
                _playgroundRepository.Delete(item.Id);
            }

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual("PlaygroundTestName1", items.FirstOrDefault().Name);
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _playgroundRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _playgroundRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("PlaygroundsRepositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _playgroundRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
