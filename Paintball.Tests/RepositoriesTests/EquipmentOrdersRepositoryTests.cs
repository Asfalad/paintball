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
    public class EquipmentOrdersRepositoryTests
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
        [TestCategory("EquipmentOrdersREpositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();

            Update_Item_Success(id);

            Delete_Item_Success(id);
        }

        public int CreateOrUpdate_Create_Item_Success()
        {
            EquipmentOrder eo = new EquipmentOrder()
            {
                EquipmentId = _equipmentToDelete.Id,
                GameId = _gameToDelete.Id,
                Count = 50
            };

            var cer = _equipmentOrdersRepository.CreateOrUpdate(eo);

            Assert.IsNotNull(cer, "Certificate are not created");

            return cer.Id;
        }

        public void Update_Item_Success(int id)
        {
            EquipmentOrder item = _equipmentOrdersRepository.Read(id);

            Assert.IsNotNull(item);

            item.Count = 5;

            _equipmentOrdersRepository.CreateOrUpdate(item);

            var updatedCertificate = _equipmentOrdersRepository.Read(id);

            Assert.IsNotNull(updatedCertificate);

            Assert.AreEqual(item.Count, updatedCertificate.Count);
        }

        public void Delete_Item_Success(int id)
        {
            var item = _equipmentOrdersRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_equipmentOrdersRepository.Delete(id), "Not deleted");

            item = _equipmentOrdersRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        #region GetAll

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<EquipmentOrder> certificatesToDelete = new List<EquipmentOrder>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    EquipmentOrder eo = new EquipmentOrder()
                    {
                        EquipmentId = _equipmentToDelete.Id,
                        GameId = _gameToDelete.Id,
                        Count = 50
                    };

                    certificatesToDelete.Add(_equipmentOrdersRepository.CreateOrUpdate(eo));
                }

                var certificates = _equipmentOrdersRepository.GetAll(15, 1);

                Assert.AreEqual(15, certificates.Count(), "Items count are not equal 15");

                certificates = _equipmentOrdersRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, certificates.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var certificate in certificatesToDelete)
                {
                    _equipmentOrdersRepository.Delete(certificate.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _equipmentOrdersRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _equipmentOrdersRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        public void GetAll_Wrong_Pages_Returns_Empty_List()
        {
            var items = _equipmentOrdersRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #endregion //GetAll

        #region CreateOrUpdate

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _equipmentOrdersRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_New_Certificate_Throws()
        {
            EquipmentOrder cert = new EquipmentOrder();
            _equipmentOrdersRepository.CreateOrUpdate(cert);
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Equipment_With_Equipment_Only_Throws()
        {
            EquipmentOrder equip = new EquipmentOrder()
            {
                EquipmentId = _equipmentToDelete.Id
            };
            _equipmentOrdersRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Certificate_Without_Count_Throws()
        {
            EquipmentOrder cert = new EquipmentOrder()
            {
                EquipmentId = _equipmentToDelete.Id,
                GameId = _gameToDelete.Id
            };
            _equipmentOrdersRepository.CreateOrUpdate(cert);
        }

        #endregion // CreateOrUpdate

        #region Update

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _equipmentOrdersRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            EquipmentOrder order = new EquipmentOrder
            {
                EquipmentId = _equipmentToDelete.Id,
                GameId = _gameToDelete.Id,
                Count = 1
            };
            order = _equipmentOrdersRepository.CreateOrUpdate(order);

            Assert.IsNotNull(order);

            order.Count = 100;

            bool result = _equipmentOrdersRepository.Update(order);

            Assert.IsTrue(result);

            order = _equipmentOrdersRepository.Read(order.Id);

            _equipmentOrdersRepository.Delete(order.Id);

            Assert.IsTrue(order.Count == 100);
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            EquipmentOrder cert = new EquipmentOrder();
            _equipmentOrdersRepository.Update(cert);
        }

        #endregion // Update

        #region Read

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_equipmentOrdersRepository.Read(-10));
            Assert.IsNull(_equipmentOrdersRepository.Read(1000000));
        }

        #endregion // Read

        #region Delete

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_equipmentOrdersRepository.Delete(-1));
        }

        #endregion // Delete

        #region Search

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        public void Search_Success()
        {
            List<EquipmentOrder> itemsToDelete = new List<EquipmentOrder>();

            for (int i = 0; i < 5; i++)
            {
                EquipmentOrder item = new EquipmentOrder()
                {
                    EquipmentId = _equipmentToDelete.Id,
                    GameId = _gameToDelete.Id,
                    Count = i + 1
                };
                itemsToDelete.Add(_equipmentOrdersRepository.CreateOrUpdate(item));
            }

            var items = _equipmentOrdersRepository.Search("Count = @Count", new { PageSize = 10, PageNumber = 1, Count = 1 });

            foreach (var item in itemsToDelete)
            {
                _equipmentOrdersRepository.Delete(item.Id);
            }

            Assert.AreNotEqual(0, items.Count());
            Assert.AreEqual(1, items.FirstOrDefault().Count);
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _equipmentOrdersRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _equipmentOrdersRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("EquipmentOrdersREpositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _equipmentOrdersRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
