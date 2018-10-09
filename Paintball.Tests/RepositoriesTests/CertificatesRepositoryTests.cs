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
    public class CertificatesRepositoryTests
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
                GamePrice = _playgroundToDelete.Price + _gameTypeToDelete.Price,
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
        [TestCategory("CertificatesRepositoryTests")]
        public void Create_Update_Delete_Test_Success()
        {
            int id = CreateOrUpdate_Create_Item_Success();

            Update_Item_Success(id);

            Delete_Item_Success(id);
        }
        
        public int CreateOrUpdate_Create_Item_Success()
        {
            Certificate certificate = new Certificate()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                MiddleName = "TestMiddleName",
                CompanyId = _companyToDelete.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                Price = 1000
            };

            var cer = _certificateRepository.CreateOrUpdate(certificate);

            Assert.IsNotNull(cer, "Certificate are not created");

            return cer.Id;
        }

        public void Update_Item_Success(int id)
        {
            Certificate certificate = _certificateRepository.Read(id);

            Assert.IsNotNull(certificate);

            certificate.FirstName = "CertificateTestFirstName";
            certificate.LastName = "CertificateTestLastName";
            certificate.MiddleName = "CertificateTestMiddleName";
            certificate.Price = 1;

            _certificateRepository.CreateOrUpdate(certificate);

            var updatedCertificate = _certificateRepository.Read(id);

            Assert.IsNotNull(updatedCertificate);

            Assert.AreEqual(certificate.FirstName, updatedCertificate.FirstName);
            Assert.AreEqual(certificate.LastName, updatedCertificate.LastName);
            Assert.AreEqual(certificate.MiddleName, updatedCertificate.MiddleName);
            Assert.AreEqual(certificate.Price, updatedCertificate.Price);
            Assert.AreEqual(certificate.StartDate, updatedCertificate.StartDate);
            Assert.AreEqual(certificate.EndDate, updatedCertificate.EndDate);
            Assert.AreEqual(certificate.CompanyId, updatedCertificate.CompanyId);
            Assert.AreEqual(certificate.OwnerId, updatedCertificate.OwnerId);
            Assert.AreEqual(certificate.Id, updatedCertificate.Id);
        }
        
        public void Delete_Item_Success(int id)
        {
            var item = _certificateRepository.Read(id);

            Assert.IsNotNull(item, "Item not exist");

            Assert.IsTrue(_certificateRepository.Delete(id), "Not deleted");

            item = _certificateRepository.Read(id);

            Assert.IsNull(item);
        }

        #endregion // Read Create Update Delete Test Success

        #region GetAll

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        public void GetAll_ReturnsResult_Success()
        {
            List<Certificate> certificatesToDelete = new List<Certificate>();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    Certificate testCertificate = new Certificate()
                    {
                        FirstName = "TestFirstName",
                        LastName = "TestLastName",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddYears(1),
                        Price = i,
                        CompanyId = _companyToDelete.Id
                    };

                    certificatesToDelete.Add(_certificateRepository.CreateOrUpdate(testCertificate));
                }

                var certificates = _certificateRepository.GetAll(15, 1);

                Assert.AreEqual(15, certificates.Count(), "Items count are not equal 15");

                certificates = _certificateRepository.GetAll(10, 2);   // Second Page

                Assert.AreEqual(10, certificates.Count(), "Items count are not equal 10");
            }
            finally
            {
                foreach (var certificate in certificatesToDelete)
                {
                    _certificateRepository.Delete(certificate.Id);
                }
            }
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Size_Throws()
        {
            _certificateRepository.GetAll(-1);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAll_Wrong_Page_Number_Throws()
        {
            _certificateRepository.GetAll(20, -1);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        public void GetAll_Wrong_Pages_Returns_Empty_List()
        {
            var items = _certificateRepository.GetAll(100, 20);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count(), "Returns not empty list");
        }

        #endregion //GetAll

        #region CreateOrUpdate

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrUpdate_Null_Throws()
        {
            _certificateRepository.CreateOrUpdate(null);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Empty_Certificate_Throws()
        {
            Certificate cert = new Certificate();
            _certificateRepository.CreateOrUpdate(cert);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Certificate_With_FirstNameOnly_Throws()
        {
            Certificate cert = new Certificate()
            {
                FirstName = "CertificateTestFirstName"
            };
            _certificateRepository.CreateOrUpdate(cert);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Certificate_With_FirstName_And_LastName_Throws()
        {
            Certificate cert = new Certificate()
            {
                FirstName = "CertificateTestFirstName",
                LastName = "CertificateTestLastName"
            };
            _certificateRepository.CreateOrUpdate(cert);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Certificate_With_FirstName_LastName_StartDate_Throws()
        {
            Certificate cert = new Certificate()
            {
                FirstName = "CertificateTestFirstName",
                LastName = "CertificateTestLastName",
                StartDate = DateTime.Now
            };
            _certificateRepository.CreateOrUpdate(cert);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Certificate_With_FirstName_LastName_StartDate_EndDate_Throws()
        {
            Certificate cert = new Certificate()
            {
                FirstName = "CertificateTestFirstName",
                LastName = "CertificateTestLastName",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1)
            };
            _certificateRepository.CreateOrUpdate(cert);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrUpdate_Certificate_With_FirstName_LastName_StartDate_EndDate_Price_Throws()
        {
            Certificate cert = new Certificate()
            {
                FirstName = "CertificateTestFirstName",
                LastName = "CertificateTestLastName",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                Price = 1
            };
            _certificateRepository.CreateOrUpdate(cert);
        }

        #endregion // CreateOrUpdate

        #region Update

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Argument_Throws()
        {
            _certificateRepository.Update(null);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        public void Update_Value_Reseting_Value_Success()
        {
            Certificate cert = new Certificate()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                MiddleName = "MiddleName",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                CompanyId = _companyToDelete.Id
            };

            cert = _certificateRepository.CreateOrUpdate(cert);

            Assert.IsNotNull(cert);

            cert.FirstName = string.Empty;
            cert.LastName = string.Empty;
            cert.MiddleName = string.Empty;

            _certificateRepository.Update(cert);

            cert = _certificateRepository.Read(cert.Id);

            _certificateRepository.Delete(cert.Id);

            Assert.IsTrue(cert.FirstName == string.Empty);
            Assert.IsTrue(cert.LastName == string.Empty);
            Assert.IsTrue(cert.MiddleName == string.Empty);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Wrong_Argument_Throws()
        {
            Certificate cert = new Certificate();
            _certificateRepository.Update(cert);
        }

        #endregion // Update

        #region Read

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        public void Read_Wrong_Argument_Return_Null()
        {
            Assert.IsNull(_certificateRepository.Read(-10));
            Assert.IsNull(_certificateRepository.Read(1000000));
        }

        #endregion // Read

        #region Delete

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        public void Delete_Wrong_Id_Return_False()
        {
            Assert.IsFalse(_certificateRepository.Delete(-1));
        }

        #endregion // Delete

        #region Search

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        public void Search_Success()
        {
            List<Certificate> itemsToDelete = new List<Certificate>();
            for(int i = 0; i < 5; i++)
            {
                Certificate item = new Certificate()
                {
                    FirstName = "TestFirstName",
                    LastName = "TestLastName",
                    MiddleName = "TestMiddleName",
                    CompanyId = _companyToDelete.Id,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddYears(1),
                    OwnerId = Guid.Parse("c76e0147-5532-4dac-85b9-828961dfb4b4"),
                    Price = i + 1
                };
                itemsToDelete.Add(_certificateRepository.CreateOrUpdate(item));
            }

            var items = _certificateRepository.Search("Price = @Price", new { PageSize = 10, PageNumber = 1, Price = 5 });

            foreach(var item in itemsToDelete)
            {
                _certificateRepository.Delete(item.Id);
            }

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(5, items.FirstOrDefault().Price);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Query_Throws()
        {
            _certificateRepository.Search(string.Empty, new { PageSize = 10, PageNumber = 1 });
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Empty_Data_Throws()
        {
            _certificateRepository.Search("Id = @Id", null);
        }

        [TestMethod]
        [TestCategory("CertificatesRepositoryTests")]
        [ExpectedException(typeof(SqlException))]
        public void Search_Missing_Arguments_Throws()
        {
            _certificateRepository.Search("Id = @Id", new { Id = 1 });
        }

        #endregion // Search
    }
}
