using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paintball.DAL;
using Paintball.DAL.Entities;
using Paintball.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace Paintball.Tests.RepositoriesTests
{
    [TestClass]
    public class OperationsRepositoryTests
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
        [TestCategory("OperationsRepositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();

            Update_Item_Success(id);

            Delete_Item_Success(id);
        }

        public int CreateOrUpdate_Create_Item_Success()
        {
            Operation certificate = new Operation()
            {
                Price = 100,
                Date = DateTime.Now,
                CompanyId = _companyToDelete.Id
            };

            var cer = _operationsRepository.CreateOrUpdate(certificate);

            Assert.IsNotNull(cer, "Operation are not created");

            return cer.Id;
        }

        public void Update_Item_Success(int id)
        {
            Operation certificate = _operationsRepository.Read(id);

            Assert.IsNotNull(certificate);

            certificate.Price = 1;

            _operationsRepository.CreateOrUpdate(certificate);

            var updatedOperation = _operationsRepository.Read(id);

            Assert.IsNotNull(updatedOperation);

            Assert.AreEqual(certificate.Price, updatedOperation.Price);
        }

        public void Delete_Item_Success(int id)
        {
            var item = _operationsRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_operationsRepository.Delete(id), "Not deleted");

            item = _operationsRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        #region GetAll

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<Operation> certificatesToDelete = new List<Operation>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    Operation testOperation = new Operation()
                    {
                        Price = i + 1,
                        Date = DateTime.Now,
                        CompanyId = _companyToDelete.Id
                    };

                    certificatesToDelete.Add(_operationsRepository.CreateOrUpdate(testOperation));
                }

                var certificates = _operationsRepository.GetAll(15, 1);

                Assert.AreEqual(15, certificates.Count(), "Items count are not equal 15");

                certificates = _operationsRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, certificates.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var certificate in certificatesToDelete)
                {
                    _operationsRepository.Delete(certificate.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _operationsRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _operationsRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        public void GetAll_Wrong_Pages_Returns_Empty_List()
        {
            var items = _operationsRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #endregion //GetAll

        #region CreateOrUpdate

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _operationsRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Empty_Operation_Throws()
        {
            Operation cert = new Operation();
            _operationsRepository.CreateOrUpdate(cert);
        }
        
        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Price_Zero_Throws()
        {
            Operation cert = new Operation()
            {
                Price = 0,
                Date = DateTime.Now,
                CompanyId = _companyToDelete.Id
            };
            _operationsRepository.CreateOrUpdate(cert);
        }
        
        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Operation_With_Price_Only_Throws()
        {
            Operation cert = new Operation()
            {
                Price = 100
            };
            _operationsRepository.CreateOrUpdate(cert);
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Operation_With_Price_Date_Throws()
        {
            Operation cert = new Operation()
            {
                Price = 1,
                Date = DateTime.Now
            };
            _operationsRepository.CreateOrUpdate(cert);
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Operation_With_Price_CompanyId_Throws()
        {
            Operation cert = new Operation()
            {
                Price = 1,
                CompanyId = 1
            };
            _operationsRepository.CreateOrUpdate(cert);
        }

        #endregion // CreateOrUpdate

        #region Update

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _operationsRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            Operation cert = new Operation()
            {
                Price = 1,
                Date = DateTime.Now,
                CompanyId = _companyToDelete.Id
            };

            cert = _operationsRepository.CreateOrUpdate(cert);

            Assert.IsNotNull(cert);

            cert.Price = 1000;

            _operationsRepository.Update(cert);

            cert = _operationsRepository.Read(cert.Id);

            _operationsRepository.Delete(cert.Id);

            Assert.IsTrue(cert.Price == 1000);
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            Operation cert = new Operation();
            _operationsRepository.Update(cert);
        }

        #endregion // Update

        #region Read

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_operationsRepository.Read(-10));
            Assert.IsNull(_operationsRepository.Read(1000000));
        }

        #endregion // Read

        #region Delete

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_operationsRepository.Delete(-1));
        }

        #endregion // Delete

        #region Search

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        public void Search_Success()
        {
            List<Operation> itemsToDelete = new List<Operation>();
            for (int i = 0; i < 5; i++)
            {
                Operation item = new Operation()
                {
                    Price = i + 1,
                    Date = DateTime.Now,
                    CompanyId = _companyToDelete.Id
                };
                itemsToDelete.Add(_operationsRepository.CreateOrUpdate(item));
            }

            var items = _operationsRepository.Search("Price = @Price", new { PageSize = 10, PageNumber = 1, Price = 1 });

            foreach (var item in itemsToDelete)
            {
                _operationsRepository.Delete(item.Id);
            }

            Assert.AreNotEqual(0, items.Count());
            Assert.AreEqual(1, items.FirstOrDefault().Price);
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _operationsRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _operationsRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("OperationsRepositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _operationsRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
