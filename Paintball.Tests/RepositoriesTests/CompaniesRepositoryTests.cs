using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paintball.DAL;
using Paintball.DAL.Entities;
using Paintball.DAL.Repositories;
using System.Linq;
using System.Data.SqlClient;

namespace Paintball.Tests.RepositoriesTests
{
    /// <summary>
    /// Сводное описание для CompaniesRepositoryTests
    /// </summary>
    [TestClass]
    public class CompaniesRepositoryTests
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
        [TestCategory("CompanysRepositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();

            Update_Item_Success(id);

            Delete_Item_Success(id);
        }

        public int CreateOrUpdate_Create_Item_Success()
        {
            Company Company = new Company()
            {
                Name = "TestName",
                Adress = "TestAddress",
                OwnerId = _firstUser.Id,
                Description = "TestDescription"
            };

            var comp = _companiesRepository.CreateOrUpdate(Company);

            Assert.IsNotNull(comp, "Company are not created");

            return comp.Id;
        }

        public void Update_Item_Success(int id)
        {
            Company comp = _companiesRepository.Read(id);

            Assert.IsNotNull(comp);

            comp.Name = "CompanyTestName";
            comp.Adress = "CompanyTestAdress";
            comp.Description = "CompanyTestDescription";

            _companiesRepository.CreateOrUpdate(comp);

            var updatedCompany = _companiesRepository.Read(id);

            Assert.IsNotNull(updatedCompany);

            Assert.AreEqual(comp.Name, updatedCompany.Name);
            Assert.AreEqual(comp.Adress, updatedCompany.Adress);
            Assert.AreEqual(comp.Description, updatedCompany.Description);
        }

        public void Delete_Item_Success(int id)
        {
            var item = _companiesRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_companiesRepository.Delete(id), "Not deleted");

            item = _companiesRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<Company> CompanysToDelete = new List<Company>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    Company testCompany = new Company()
                    {
                        Name = "CompanyTestName",
                        Adress = "CompanyTestAdress",
                        Description = "CompanyTestDescription",
                        OwnerId = _firstUser.Id
                    };

                    CompanysToDelete.Add(_companiesRepository.CreateOrUpdate(testCompany));
                }

                var companies = _companiesRepository.GetAll(15, 1);

                Assert.AreEqual(15, companies.Count(), "Items count are not equal 15");

                companies = _companiesRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, companies.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var Company in CompanysToDelete)
                {
                    _companiesRepository.Delete(Company.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _companiesRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Empty_Company_Throws()
        {
            Company comp = new Company();
            _companiesRepository.CreateOrUpdate(comp);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Company_With_Name_Only_Throws()
        {
            Company comp = new Company()
            {
                Name = "CompanyTestName"
            };
            _companiesRepository.CreateOrUpdate(comp);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Company_With_Name_And_Adress_Throws()
        {
            Company comp = new Company()
            {
                Name = "CompanyTestName",
                Adress = "CompanyTestAdress"
            };
            _companiesRepository.CreateOrUpdate(comp);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_companiesRepository.Read(-10));
            Assert.IsNull(_companiesRepository.Read(1000000));
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _companiesRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            Company comp = new Company()
            {
                Name = "CompanyTestName",
                Adress = "CompanyTestName",
                Description = "CompanyTestDescription",
                OwnerId = _firstUser.Id
            };

            comp = _companiesRepository.CreateOrUpdate(comp);

            Assert.IsNotNull(comp);

            comp.Name = string.Empty;
            comp.Description = string.Empty;
            comp.Adress = string.Empty;

            _companiesRepository.Update(comp);

            comp = _companiesRepository.Read(comp.Id);

            _companiesRepository.Delete(comp.Id);

            Assert.IsTrue(comp.Name == string.Empty);
            Assert.IsTrue(comp.Description == string.Empty);
            Assert.IsTrue(comp.Adress == string.Empty);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            Company comp = new Company();
            _companiesRepository.Update(comp);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_companiesRepository.Delete(-1));
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _companiesRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _companiesRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        public void GetAll_Wrong_Pages_Returns_Empty_List()
        {
            var items = _companiesRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #region Search

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        public void Search_Success()
        {
            List<Company> itemsToDelete = new List<Company>();
            for (int i = 0; i < 5; i++)
            {
                Company item = new Company()
                {
                    Name = "CompanyTestName" + i,
                    Adress = "CompanyTestAdress",
                    Description = "CompanyTestDescription",
                    OwnerId = Guid.Parse("c76e0147-5532-4dac-85b9-828961dfb4b4")
                };

                itemsToDelete.Add(_companiesRepository.CreateOrUpdate(item));
            }

            var items = _companiesRepository.Search("Name = @Name", new { PageSize = 10, PageNumber = 1, Name = "CompanyTestName1" });

            foreach (var item in itemsToDelete)
            {
                _companiesRepository.Delete(item.Id);
            }

            Assert.AreNotEqual(0, items.Count());
            Assert.AreEqual("CompanyTestName1", items.FirstOrDefault().Name);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _companiesRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _companiesRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("CompanysRepositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _companiesRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
