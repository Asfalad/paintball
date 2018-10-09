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
    public class GamesRepositoryTests
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
        [TestCategory("GamesRepositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();

            Update_Item_Success(id);

            Delete_Item_Success(id);
        }

        public int CreateOrUpdate_Create_Item_Success()
        {
            Game equip = new Game()
            {
                CreatorId = _firstUser.Id,
                BeginDate = DateTime.Now,
                GameType = _gameTypeToDelete.Id,
                Playground = _playgroundToDelete.Id,
                PlayerCount = 10,
                GamePrice = 1000,
                CompanyId = _companyToDelete.Id
            };

            equip = _gamesRepository.CreateOrUpdate(equip);

            Assert.IsNotNull(equip, "Game are not created");

            return equip.Id;
        }

        public void Update_Item_Success(int id)
        {
            Game equip = _gamesRepository.Read(id);

            Assert.IsNotNull(equip, "Game not found");

            equip.GamePrice = 2000;
            equip.GameType = _gameTypeToDelete.Id;

            _gamesRepository.CreateOrUpdate(equip);

            var updatedGame = _gamesRepository.Read(id);

            Assert.IsNotNull(updatedGame, "updatedGame is null");

            Assert.AreEqual(equip.GamePrice, updatedGame.GamePrice, "updatedGame Name and game GameType not same");
            Assert.AreEqual(equip.GameType, updatedGame.GameType, "updatedGame GameType and game GameType not same");
        }

        public void Delete_Item_Success(int id)
        {
            var item = _gamesRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_gamesRepository.Delete(id), "Not deleted");

            item = _gamesRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<Game> GamesToDelete = new List<Game>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    Game equip = new Game()
                    {
                        CreatorId = _firstUser.Id,
                        BeginDate = DateTime.Now,
                        GameType = _gameTypeToDelete.Id,
                        Playground = _playgroundToDelete.Id,
                        PlayerCount = 10,
                        GamePrice = i + 1,
                        CompanyId = _companyToDelete.Id
                    };

                    GamesToDelete.Add(_gamesRepository.CreateOrUpdate(equip));
                }

                var Games = _gamesRepository.GetAll(15, 1);

                Assert.AreEqual(15, Games.Count(), "Items count are not equal 15");

                Games = _gamesRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, Games.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var Game in GamesToDelete)
                {
                    _gamesRepository.Delete(Game.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _gamesRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Empty_Game_Throws()
        {
            Game equip = new Game();
            _gamesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_CreatorId_Only_Throws()
        {
            Game equip = new Game()
            {
                CreatorId = _firstUser.Id
            };

            _gamesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_CreatorId_BeginDate_Throws()
        {
            Game equip = new Game()
            {
                CreatorId = _firstUser.Id,
                BeginDate = DateTime.Now
            };

            _gamesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_CreatorId_BeginDate_GameType_Throws()
        {
            Game equip = new Game()
            {
                CreatorId = _firstUser.Id,
                BeginDate = DateTime.Now,
                GameType = _gameTypeToDelete.Id
            };

            _gamesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_CreatorId_BeginDate_GameType_Playground_Throws()
        {
            Game equip = new Game()
            {
                CreatorId = _firstUser.Id,
                BeginDate = DateTime.Now,
                GameType = _gameTypeToDelete.Id,
                Playground = _playgroundToDelete.Id
            };

            _gamesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_CreatorId_BeginDate_GameType_Playground_PlayerCount_Throws()
        {
            Game equip = new Game()
            {
                CreatorId = _firstUser.Id,
                BeginDate = DateTime.Now,
                GameType = _gameTypeToDelete.Id,
                Playground = _playgroundToDelete.Id,
                PlayerCount = 10
            };

            _gamesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_CreatorId_BeginDate_GameType_Playground_PlayerCount_GamePrice_Throws()
        {
            Game equip = new Game()
            {
                CreatorId = _firstUser.Id,
                BeginDate = DateTime.Now,
                GameType = _gameTypeToDelete.Id,
                Playground = _playgroundToDelete.Id,
                PlayerCount = 10,
                GamePrice = 10
            };

            _gamesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_CreatorId_BeginDate_GameType_Playground_CompanyId_Throws()
        {
            Game equip = new Game()
            {
                CreatorId = _firstUser.Id,
                BeginDate = DateTime.Now,
                GameType = _gameTypeToDelete.Id,
                Playground = _playgroundToDelete.Id,
                CompanyId = _companyToDelete.Id
            };

            _gamesRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_gamesRepository.Read(-10));
            Assert.IsNull(_gamesRepository.Read(100000));
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _gamesRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            Game equip = new Game()
            {
                CreatorId = _firstUser.Id,
                BeginDate = DateTime.Now,
                GameType = _gameTypeToDelete.Id,
                Playground = _playgroundToDelete.Id,
                PlayerCount = 10,
                GamePrice = 1000,
                CompanyId = _companyToDelete.Id
            };

            equip = _gamesRepository.CreateOrUpdate(equip);

            Assert.IsNotNull(equip);

            equip.GamePrice = 2000;

            _gamesRepository.Update(equip);

            equip = _gamesRepository.Read(equip.Id);

            _gamesRepository.Delete(equip.Id);

            Assert.IsTrue(equip.GamePrice == 2000);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            Game comp = new Game();
            _gamesRepository.Update(comp);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_gamesRepository.Delete(-1));
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _gamesRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _gamesRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        public void GetAll_Wrong_Pages_Return_Empry_List()
        {
            var items = _gamesRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #region Search

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        public void Search_Success()
        {
            List<Game> itemsToDelete = new List<Game>();
            for (int i = 0; i < 5; i++)
            {
                Game item = new Game()
                {
                    CreatorId = _firstUser.Id,
                    BeginDate = DateTime.Now,
                    GameType = _gameTypeToDelete.Id,
                    Playground = _playgroundToDelete.Id,
                    PlayerCount = 10,
                    GamePrice = i + 1,
                    CompanyId = _companyToDelete.Id
                };
                itemsToDelete.Add(_gamesRepository.CreateOrUpdate(item));
            }

            var items = _gamesRepository.Search("GamePrice = @GamePrice", new { PageSize = 10, PageNumber = 1, GamePrice = 1 });

            foreach (var item in itemsToDelete)
            {
                _gamesRepository.Delete(item.Id);
            }

            Assert.AreNotEqual(0, items.Count());
            Assert.AreEqual(1, items.FirstOrDefault().GamePrice);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _gamesRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _gamesRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("GamesRepositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _gamesRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
