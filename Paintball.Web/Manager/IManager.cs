using Microsoft.AspNet.Identity;
using Paintball.DAL;
using Paintball.DAL.Entities;

using Paintball.DAL.Repositories;
using Paintball.Web.Model;
using Paintball.Web.Model.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Paintball.Web.Manager
{
    public class OperationResult<TEntity> where TEntity : class
    {
        public OperationResult()
        {
            Result = false;
            SingleResult = null;
            MultipleResult = new List<TEntity>();
            Count = 0;
        }
        public bool Result { get; set; }
        public TEntity SingleResult { get; set; }
        public IEnumerable<TEntity> MultipleResult { get; set; }

        public int Count { get; set; }
    }

    public class UserPartial
    {
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [StringLength(128)]
        public string FirstName { get; set; }

        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(128)]
        public string MiddleName { get; set; }

        public bool CompanyOwner { get; set; }
        public bool CertificatesModify { get; set; }
        public bool CompanyModify { get; set; }
        public bool EquipmentModify { get; set; }
        public bool EventsModify { get; set; }
        public bool GameTypesModify { get; set; }
        public bool NewsModify { get; set; }
        public bool OperationsRead { get; set; }
        public bool OperationsModify { get; set; }
        public bool PlaygroundsModify { get; set; }
        public int Salary { get; set; }
    }

    public class CreateStaffViewModel
    {
        [StringLength(128)]
        public string FirstName { get; set; }

        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(128)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }


        public int? Salary { get; set; }
        
        public bool CertificatesModify { get; set; }
        public bool CompanyModify { get; set; }
        public bool EquipmentModify { get; set; }
        public bool EventsModify { get; set; }
        public bool GameTypesModify { get; set; }
        public bool NewsModify { get; set; }
        public bool OperationsRead { get; set; }
        public bool OperationsModify { get; set; }
        public bool PlaygroundsModify { get; set; }
    }

    public class UpdateStaffViewModel
    {
        public string UserName { get; set; }

        [StringLength(128)]
        public string FirstName { get; set; }

        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(128)]
        public string MiddleName { get; set; }

        public bool CertificatesModify { get; set; }
        public bool CompanyModify { get; set; }
        public bool EquipmentModify { get; set; }
        public bool EventsModify { get; set; }
        public bool GameTypesModify { get; set; }
        public bool NewsModify { get; set; }
        public bool OperationsRead { get; set; }
        public bool OperationsModify { get; set; }
        public bool PlaygroundsModify { get; set; }
    }

    public interface IManager : IDisposable
    {
        void SetUser(string Id);

        #region Repositories

        IdentityUser CurrentUser { get; set; }
        UserStore<IdentityUser> UserStore { get; set; }
        RoleStore<IdentityRole> RoleStore { get; set; }
        IRepository<Certificate, int> CertificatesRepository { get; set; }
        IRepository<Company, int> CompaniesRepository { get; set; }
        IRepository<Equipment, int> EquipmentsRepository { get; set; }
        IRepository<Event, int> EventsRepository { get; set; }
        IRepository<Game, int> GamesRepository { get; set; }
        IRepository<GameType, int> GameTypesRepository { get; set; }
        IRepository<News, int> NewsRepository { get; set; }
        IRepository<Operation, int> OperationsRepository { get; set; }
        IRepository<Playground, int> PlaygroundsRepository { get; set; }
        IRepository<DAL.Entities.Task, int> TasksRepository { get; set; }
        IRepository<EquipmentOrder, int> EquipmentOrdersRepository { get; set; }

        #endregion // Repositories

        #region Company

        System.Threading.Tasks.Task<OperationResult<Company>> GetCompanies(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<Company>> ReadCompany(int id);
        System.Threading.Tasks.Task<OperationResult<Company>> CreateCompany(Company company);
        System.Threading.Tasks.Task<OperationResult<Company>> UpdateCompany(Company company);
        System.Threading.Tasks.Task<OperationResult<Company>> DeleteCompany(int id);

        #endregion // Company

        #region Certificate

        System.Threading.Tasks.Task<OperationResult<Certificate>> GetCertificates(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<Certificate>> ReadCertificate(int Id);
        System.Threading.Tasks.Task<OperationResult<Certificate>> CreateCertificate(Certificate certificate);
        System.Threading.Tasks.Task<OperationResult<Certificate>> UpdateCertificate(Certificate certificate);
        System.Threading.Tasks.Task<OperationResult<Certificate>> DeleteCertificate(int id);

        #endregion // Certificate

        #region Equipment

        System.Threading.Tasks.Task<OperationResult<Equipment>> GetEquipment(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<Equipment>> ReadEquipment(int Id);
        System.Threading.Tasks.Task<OperationResult<Equipment>> CreateEquipment(Equipment certificate);
        System.Threading.Tasks.Task<OperationResult<Equipment>> UpdateEquipment(Equipment certificate);
        System.Threading.Tasks.Task<OperationResult<Equipment>> DeleteEquipment(int id);

        #endregion // Equipment

        #region EquipmentOrders

        System.Threading.Tasks.Task<OperationResult<EquipmentOrder>> GetEquipmentOrders(int gameId, int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<EquipmentOrder>> ReadEquipmentOrder(int Id);
        System.Threading.Tasks.Task<OperationResult<EquipmentOrder>> CreateEquipmentOrder(EquipmentOrder equipment);
        System.Threading.Tasks.Task<OperationResult<EquipmentOrder>> UpdateEquipmentOrder(EquipmentOrder equipment);
        System.Threading.Tasks.Task<OperationResult<EquipmentOrder>> DeleteEquipmentOrder(int id);

        #endregion // EquipmentOrders

        #region Event

        System.Threading.Tasks.Task<OperationResult<Event>> GetEvents(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<Event>> ReadEvent(int Id);
        System.Threading.Tasks.Task<OperationResult<Event>> CreateEvent(Event certificate);
        System.Threading.Tasks.Task<OperationResult<Event>> UpdateEvent(Event certificate);
        System.Threading.Tasks.Task<OperationResult<Event>> DeleteEvent(int id);

        #endregion // Event

        #region Game
        
        System.Threading.Tasks.Task<OperationResult<Game>> GetGames(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<Game>> GetGames(DateTime date);
        System.Threading.Tasks.Task<OperationResult<Game>> ReadGame(int Id);
        System.Threading.Tasks.Task<OperationResult<Game>> CreateGame(Game game);
        System.Threading.Tasks.Task<OperationResult<Game>> UpdateGame(Game game);
        System.Threading.Tasks.Task<OperationResult<Game>> DeleteGame(int id);

        #endregion // Game

        #region MyGame
        System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> GetMyGames(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> GetMyGames(DateTime date, int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> CreateMyGame(Game game);
        System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> ReadMyGame(int Id);
        System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> UpdateMyGame(Game game);
        System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> DeleteMyGame(int id);

        #endregion // MyGame

        #region Order

        System.Threading.Tasks.Task<OperationResult<OrderResponse>> GetCompanyForOrder(int id);

        #endregion // Order

        #region GameType

        System.Threading.Tasks.Task<OperationResult<GameType>> GetGameTypes(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<GameType>> ReadGameType(int Id);
        System.Threading.Tasks.Task<OperationResult<GameType>> CreateGameType(GameType gameType);
        System.Threading.Tasks.Task<OperationResult<GameType>> UpdateGameType(GameType gameType);
        System.Threading.Tasks.Task<OperationResult<GameType>> DeleteGameType(int id);

        #endregion // GameType

        #region News 

        System.Threading.Tasks.Task<OperationResult<News>> GetNews(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<News>> ReadNews(int Id);
        System.Threading.Tasks.Task<OperationResult<News>> CreateNews(News news);
        System.Threading.Tasks.Task<OperationResult<News>> UpdateNews(News news);
        System.Threading.Tasks.Task<OperationResult<News>> DeleteNews(int id);

        #endregion // News

        #region Operation

        System.Threading.Tasks.Task<OperationResult<Operation>> GetOperations(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<Operation>> ReadOperation(int Id);
        System.Threading.Tasks.Task<OperationResult<Operation>> CreateOperation(Operation operation);
        System.Threading.Tasks.Task<OperationResult<Operation>> UpdateOperation(Operation operation);
        System.Threading.Tasks.Task<OperationResult<Operation>> DeleteOperation(int id);

        #endregion // Operation

        #region Playground

        System.Threading.Tasks.Task<OperationResult<Playground>> GetPlaygrounds(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<Playground>> ReadPlayground(int Id);
        System.Threading.Tasks.Task<OperationResult<Playground>> CreatePlayground(Playground playground);
        System.Threading.Tasks.Task<OperationResult<Playground>> UpdatePlayground(Playground playground);
        System.Threading.Tasks.Task<OperationResult<Playground>> DeletePlayground(int id);

        #endregion // Playground

        #region Task
        System.Threading.Tasks.Task<OperationResult<Task>> GetTasks(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<Task>> GetTasks(Guid userId, int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<Task>> ReadTask(int Id);
        System.Threading.Tasks.Task<OperationResult<Task>> CreateTask(Task task);
        System.Threading.Tasks.Task<OperationResult<Task>> UpdateTask(Task task);
        System.Threading.Tasks.Task<OperationResult<Task>> DeleteTask(int id);

        #endregion // Task

        #region User

        System.Threading.Tasks.Task<OperationResult<UserPartial>> GetUsers(int pageSize, int pageNumber, bool descending);
        System.Threading.Tasks.Task<OperationResult<UserPartial>> ReadUser(string userName);
        System.Threading.Tasks.Task<OperationResult<UserPartial>> CreateUser(CreateStaffViewModel options);
        System.Threading.Tasks.Task<OperationResult<UserPartial>> UpdateUser(UpdateStaffViewModel user);
        System.Threading.Tasks.Task<OperationResult<UserPartial>> DeleteUser(string userName);

        #endregion // User
    }
}
