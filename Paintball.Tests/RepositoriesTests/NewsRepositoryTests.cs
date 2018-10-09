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
    public class NewsRepositoryTests
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
        [TestCategory("NewsRepositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();

            Update_Item_Success(id);

            Delete_Item_Success(id);
        }

        public int CreateOrUpdate_Create_Item_Success()
        {
            News item = new News()
            {
                Title = "NewsTestName",
                PublishDate = DateTime.Now,
                Text = "NewsTestText",
                CompanyId = _companyToDelete.Id
            };

            item = _newsRepository.CreateOrUpdate(item);

            Assert.IsNotNull(item, "News are not created");

            return item.Id;
        }

        public void Update_Item_Success(int id)
        {
            News equip = _newsRepository.Read(id);

            Assert.IsNotNull(equip, "News not found");

            equip.Text = "NewsUpdateTestText";

            _newsRepository.CreateOrUpdate(equip);

            var updatedNews = _newsRepository.Read(id);

            Assert.IsNotNull(updatedNews, "updatedNews is null");

            Assert.AreEqual(equip.Text, updatedNews.Text, "updatedNews Text and news Text not same");
        }

        public void Delete_Item_Success(int id)
        {
            var item = _newsRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_newsRepository.Delete(id), "Not deleted");

            item = _newsRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<News> NewsToDelete = new List<News>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    News testNews = new News()
                    {
                        Title = "NewsTestName",
                        PublishDate = DateTime.Now,
                        Text = "NewsTestText",
                        CompanyId = _companyToDelete.Id
                    };

                    NewsToDelete.Add(_newsRepository.CreateOrUpdate(testNews));
                }

                var News = _newsRepository.GetAll(15, 1);

                Assert.AreEqual(15, News.Count(), "Items count are not equal 15");

                News = _newsRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, News.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var News in NewsToDelete)
                {
                    _newsRepository.Delete(News.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _newsRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Empty_News_Throws()
        {
            News item = new News();
            _newsRepository.CreateOrUpdate(item);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Title_Only_Throws()
        {
            News item = new News()
            {
                Title = "NewsTestName"
            };
            _newsRepository.CreateOrUpdate(item);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Title_PublishDate_Throws()
        {
            News item = new News()
            {
                Title = "NewsTestTitle",
                PublishDate = DateTime.Now
            };
            _newsRepository.CreateOrUpdate(item);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Title_Text_Throws()
        {
            News item = new News()
            {
                Title = "NewsTestTitle",
                Text = "NewsTestText"
            };
            _newsRepository.CreateOrUpdate(item);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_newsRepository.Read(-10));
            Assert.IsNull(_newsRepository.Read(100000));
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _newsRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            News item = new News()
            {
                Title = "NewsTestText",
                CompanyId = _companyToDelete.Id,
                PublishDate = DateTime.Now,
                Text = "NewsTestText"
            };

            item = _newsRepository.CreateOrUpdate(item);

            Assert.IsNotNull(item);

            item.Title = "TestTitle";

            _newsRepository.Update(item);

            item = _newsRepository.Read(item.Id);

            _newsRepository.Delete(item.Id);

            Assert.IsTrue(item.Title == "TestTitle");
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            News item = new News();
            _newsRepository.Update(item);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_newsRepository.Delete(-1));
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _newsRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _newsRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        public void GetAll_Wrong_Pages_Return_Empry_List()
        {
            var items = _newsRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #region Search

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        public void Search_Success()
        {
            List<News> itemsToDelete = new List<News>();
            for (int i = 0; i < 5; i++)
            {
                News item = new News()
                {
                    Title = "NewsTestName" + i,
                    PublishDate = DateTime.Now,
                    Text = "NewsTestText",
                    CompanyId = _companyToDelete.Id
                };
                itemsToDelete.Add(_newsRepository.CreateOrUpdate(item));
            }

            var items = _newsRepository.Search("Title = @Title", new { PageSize = 10, PageNumber = 1, Title = "NewsTestName1" });

            foreach (var item in itemsToDelete)
            {
                _newsRepository.Delete(item.Id);
            }

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual("NewsTestName1", items.FirstOrDefault().Title);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _newsRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _newsRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("NewsRepositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _newsRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
