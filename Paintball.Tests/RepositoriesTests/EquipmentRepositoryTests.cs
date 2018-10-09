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
    public class EquipmentRepositoryTests
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
        [TestCategory("EquipmentsRepositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();

            Update_Item_Success(id);

            Delete_Item_Success(id);
        }

        public int CreateOrUpdate_Create_Item_Success()
        {
            Equipment equip = new Equipment()
            {
                Name = "EquipmentTestName",
                Price = 1,
                Count = 1,
                CompanyId = _companyToDelete.Id
            };

            equip = _equipmentsRepository.CreateOrUpdate(equip);

            Assert.IsNotNull(equip, "Equipment are not created");

            return equip.Id;
        }

        public void Update_Item_Success(int id)
        {
            Equipment equip = _equipmentsRepository.Read(id);

            Assert.IsNotNull(equip, "Equipment not found");

            equip.Name = "EquipmentUpdateTestName";
            equip.Price = 100;
            equip.Count = 100;

            _equipmentsRepository.CreateOrUpdate(equip);

            var updatedEquipment = _equipmentsRepository.Read(id);

            Assert.IsNotNull(updatedEquipment, "updatedEquipment is null");

            Assert.AreEqual(equip.Name, updatedEquipment.Name, "updatedEquipment Name and equipmet Name not same");
            Assert.AreEqual(equip.Price, updatedEquipment.Price, "updatedEquipment Price and equipmet Price not same");
            Assert.AreEqual(equip.Count, updatedEquipment.Count, "updatedEquipment Count and equipmet Count not same");
        }

        public void Delete_Item_Success(int id)
        {
            var item = _equipmentsRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_equipmentsRepository.Delete(id), "Not deleted");

            item = _equipmentsRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<Equipment> EquipmentsToDelete = new List<Equipment>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    Equipment testEquipment = new Equipment()
                    {
                        Name = "EquipmentTestName",
                        Price = i + 1,
                        Count = i + 1,
                        CompanyId = _companyToDelete.Id
                    };

                    EquipmentsToDelete.Add(_equipmentsRepository.CreateOrUpdate(testEquipment));
                }

                var equipments = _equipmentsRepository.GetAll(15, 1);

                Assert.AreEqual(15, equipments.Count(), "Items count are not equal 15");

                equipments = _equipmentsRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, equipments.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var Equipment in EquipmentsToDelete)
                {
                    _equipmentsRepository.Delete(Equipment.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _equipmentsRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Empty_Equipment_Throws()
        {
            Equipment equip = new Equipment();
            _equipmentsRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Name_Only_Throws()
        {
            Equipment equip = new Equipment()
            {
                Name = "EquipmentTestName"
            };
            _equipmentsRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Name_Price_Throws()
        {
            Equipment equip = new Equipment()
            {
                Name = "EquipmentTestName",
                Price = 1
            };
            _equipmentsRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Name_Price_Count_Throws()
        {
            Equipment equip = new Equipment()
            {
                Name = "EquipmentTestName",
                Price = 1,
                Count = 1
            };
            _equipmentsRepository.CreateOrUpdate(equip);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_equipmentsRepository.Read(-10));
            Assert.IsNull(_equipmentsRepository.Read(100000));
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _equipmentsRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            Equipment comp = new Equipment()
            {
                Name = "EquipmentTestName",
                Price = 100,
                Count = 100,
                CompanyId = _companyToDelete.Id
            };

            comp = _equipmentsRepository.CreateOrUpdate(comp);

            Assert.IsNotNull(comp);

            comp.Name = string.Empty;
            comp.Price = 0;
            comp.Count = 1;
            comp.CompanyId = _companyToDelete.Id;

            _equipmentsRepository.Update(comp);

            comp = _equipmentsRepository.Read(comp.Id);

            _equipmentsRepository.Delete(comp.Id);

            Assert.IsTrue(comp.Name == string.Empty);
            Assert.IsTrue(comp.Price == 0 );
            Assert.IsTrue(comp.Count == 1 );
            Assert.IsTrue(comp.CompanyId == _companyToDelete.Id);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            Equipment comp = new Equipment();
            _equipmentsRepository.Update(comp);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_equipmentsRepository.Delete(-1));
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _equipmentsRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _equipmentsRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        public void GetAll_Wrong_Pages_Return_Empry_List()
        {
            var items = _equipmentsRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #region Search

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        public void Search_Success()
        {
            List<Equipment> itemsToDelete = new List<Equipment>();
            for (int i = 0; i < 5; i++)
            {
                Equipment item = new Equipment()
                {
                    Name = "EquipmentTestName",
                    Price = i + 1,
                    Count = i + 1,
                    CompanyId = _companyToDelete.Id
                };
                itemsToDelete.Add(_equipmentsRepository.CreateOrUpdate(item));
            }

            var items = _equipmentsRepository.Search("Price = @Price", new { PageSize = 10, PageNumber = 1, Price = 1 });

            foreach (var item in itemsToDelete)
            {
                _equipmentsRepository.Delete(item.Id);
            }

            Assert.AreNotEqual(0, items.Count());
            Assert.AreEqual(1, items.FirstOrDefault().Price);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _equipmentsRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _equipmentsRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("EquipmentsRepositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _equipmentsRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
