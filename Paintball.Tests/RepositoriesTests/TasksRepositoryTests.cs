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
    public class TasksRepositoryTests
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
        [TestCategory("TasksRepositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();

            Update_Item_Success(id);

            Delete_Item_Success(id);
        }

        public int CreateOrUpdate_Create_Item_Success()
        {
            Task item = new Task()
            {
                StaffId = _firstUser.Id,
                Text = "TaskTestText",
                IsCompleted = false,
                CompanyId = _companyToDelete.Id
            };

            item = _tasksRepository.CreateOrUpdate(item);

            Assert.IsNotNull(item, "Task are not created");

            return item.Id;
        }

        public void Update_Item_Success(int id)
        {
            Task item = _tasksRepository.Read(id);

            Assert.IsNotNull(item, "Task not found");

            item.IsCompleted = !item.IsCompleted;

            _tasksRepository.CreateOrUpdate(item);

            var updatedTask = _tasksRepository.Read(id);

            Assert.IsNotNull(updatedTask, "updatedTask is null");

            Assert.AreEqual(item.IsCompleted, updatedTask.IsCompleted, "updatedTask IsCompleted and equipmet IsCompleted not same");
        }

        public void Delete_Item_Success(int id)
        {
            var item = _tasksRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_tasksRepository.Delete(id), "Not deleted");

            item = _tasksRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<Task> TasksToDelete = new List<Task>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    Task testTask = new Task()
                    {
                        StaffId = _firstUser.Id,
                        Text = "TaskTestText",
                        IsCompleted = false,
                        CompanyId = _companyToDelete.Id
                    };

                    TasksToDelete.Add(_tasksRepository.CreateOrUpdate(testTask));
                }

                var Tasks = _tasksRepository.GetAll(15, 1);

                Assert.AreEqual(15, Tasks.Count(), "Items count are not equal 15");

                Tasks = _tasksRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, Tasks.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var Task in TasksToDelete)
                {
                    _tasksRepository.Delete(Task.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _tasksRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Empty_Task_Throws()
        {
            Task equip = new Task();
            _tasksRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_StaffId_Only_Throws()
        {
            Task equip = new Task()
            {
                StaffId = _firstUser.Id
            };
            _tasksRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Text_Only_Throws()
        {
            Task equip = new Task()
            {
                Text = "TaskTestText"
            };
            _tasksRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_tasksRepository.Read(-10));
            Assert.IsNull(_tasksRepository.Read(100000));
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _tasksRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            Task comp = new Task()
            {
                StaffId = _firstUser.Id,
                Text = "TaskTestText",
                IsCompleted = false,
                CompanyId = _companyToDelete.Id
            };

            comp = _tasksRepository.CreateOrUpdate(comp);

            Assert.IsNotNull(comp);

            comp.Text = "TaskUpdateTestText";

            _tasksRepository.Update(comp);

            comp = _tasksRepository.Read(comp.Id);

            _tasksRepository.Delete(comp.Id);

            Assert.IsTrue(comp.Text == "TaskUpdateTestText");
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            Task item = new Task();
            _tasksRepository.Update(item);
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_tasksRepository.Delete(-1));
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _tasksRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _tasksRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        public void GetAll_Wrong_Pages_Return_Empry_List()
        {
            var items = _tasksRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #region Search

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        public void Search_Success()
        {
            List<Task> itemsToDelete = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                Task item = new Task()
                {
                    StaffId = _firstUser.Id,
                    Text = "TaskTestText" + i,
                    IsCompleted = false,
                    CompanyId = _companyToDelete.Id
                };
                itemsToDelete.Add(_tasksRepository.CreateOrUpdate(item));
            }

            var items = _tasksRepository.Search("Text = @Text", new { PageSize = 10, PageNumber = 1, Text = "TaskTestText1" });

            foreach (var item in itemsToDelete)
            {
                _tasksRepository.Delete(item.Id);
            }

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual("TaskTestText1", items.FirstOrDefault().Text);
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _tasksRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _tasksRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("TasksRepositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _tasksRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
