using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paintball.DAL;
using Paintball.DAL.Entities;
using Paintball.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Paintball.Tests.RepositoriesTests
{
    [TestClass]
    public class GameTypesRepositoryTests
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
        [TestCategory("GameTypesRepositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();

            Update_Item_Success(id);

            Delete_Item_Success(id);
        }

        public int CreateOrUpdate_Create_Item_Success()
        {
            GameType item = new GameType()
            {
                CompanyId = _companyToDelete.Id,
                Name = "GameTypesTestName",
                Price = 100
            };

            item = _gameTypesRepository.CreateOrUpdate(item);

            Assert.IsNotNull(item, "GameType are not created");

            return item.Id;
        }

        public void Update_Item_Success(int id)
        {
            GameType item = _gameTypesRepository.Read(id);

            Assert.IsNotNull(item, "GameType not found");

            item.Name = "GameTypeUpdateTestDescription";

            _gameTypesRepository.CreateOrUpdate(item);

            var updatedGameType = _gameTypesRepository.Read(id);

            Assert.IsNotNull(updatedGameType, "updatedGameType is null");

            Assert.AreEqual(item.Name, updatedGameType.Name, "updatedGameType Name and equipmet Name not same");
        }

        public void Delete_Item_Success(int id)
        {
            var item = _gameTypesRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_gameTypesRepository.Delete(id), "Not deleted");

            item = _gameTypesRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<GameType> GameTypesToDelete = new List<GameType>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    GameType testGameType = new GameType()
                    {
                        CompanyId = _companyToDelete.Id,
                        Name = "GameTypesTestName",
                        Price = 100
                    };

                    GameTypesToDelete.Add(_gameTypesRepository.CreateOrUpdate(testGameType));
                }

                var GameTypes = _gameTypesRepository.GetAll(15, 1);

                Assert.AreEqual(15, GameTypes.Count(), "Items count are not equal 15");

                GameTypes = _gameTypesRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, GameTypes.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var GameType in GameTypesToDelete)
                {
                    _gameTypesRepository.Delete(GameType.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _gameTypesRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Empty_GameType_Throws()
        {
            GameType equip = new GameType();
            _gameTypesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Name_Only_Throws()
        {
            GameType equip = new GameType()
            {
                Name = "GameTypesTestName"
            };
            _gameTypesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_CompanyId_Only_Throws()
        {
            GameType equip = new GameType()
            {
                CompanyId = _companyToDelete.Id
            };
            _gameTypesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_gameTypesRepository.Read(-10));
            Assert.IsNull(_gameTypesRepository.Read(100000));
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _gameTypesRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            GameType comp = new GameType()
            {
                CompanyId = _companyToDelete.Id,
                Name = "GameTypeTestDescription",
                Price = 100
            };

            comp = _gameTypesRepository.CreateOrUpdate(comp);

            Assert.IsNotNull(comp);

            comp.Name = string.Empty;

            _gameTypesRepository.Update(comp);

            comp = _gameTypesRepository.Read(comp.Id);

            _gameTypesRepository.Delete(comp.Id);

            Assert.IsTrue(comp.Name == string.Empty);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            GameType comp = new GameType();
            _gameTypesRepository.Update(comp);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_gameTypesRepository.Delete(-1));
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _gameTypesRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _gameTypesRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        public void GetAll_Wrong_Pages_Return_Empry_List()
        {
            var items = _gameTypesRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #region Search

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        public void Search_Success()
        {
            List<GameType> itemsToDelete = new List<GameType>();
            for (int i = 0; i < 5; i++)
            {
                GameType item = new GameType()
                {
                    CompanyId = _companyToDelete.Id,
                    Name = "GameTypesTestName"+ i,
                    Price = 100
                };
                itemsToDelete.Add(_gameTypesRepository.CreateOrUpdate(item));
            }

            var items = _gameTypesRepository.Search("Name = @Name", new { PageSize = 10, PageNumber = 1, Name = "GameTypesTestName1" });

            foreach (var item in itemsToDelete)
            {
                _gameTypesRepository.Delete(item.Id);
            }

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual("GameTypesTestName1", items.FirstOrDefault().Name);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _gameTypesRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _gameTypesRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("GameTypesRepositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _gameTypesRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
