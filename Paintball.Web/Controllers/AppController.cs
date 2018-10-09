using Paintball.Web.Constants;
using System.Web.Mvc;

namespace Paintball.Web.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        [AllowAnonymous]
        // GET: App
        [Route("app", Name = AppControllerRoute.GetIndex)]
        public ActionResult Index()
        {
            return View(AppControllerAction.Index);
        }

        [AllowAnonymous]
        [Route("app/login", Name = AppControllerRoute.GetLogin)]
        public ActionResult Login()
        {
            return View(AppControllerAction.Login);
        }

        [AllowAnonymous]
        [Route("app/register", Name = AppControllerRoute.GetRegister)]
        public ActionResult Register()
        {
            return View(AppControllerAction.Register);
        }

        [Route("app/index", Name = AppControllerRoute.GetView)]
        public new ActionResult View()
        {
            return View(AppControllerAction.View);
        }

        [Route("app/error404", Name = AppControllerRoute.GetError404)]
        public ActionResult Error404()
        {
            return View(AppControllerAction.Error404);
        }

        [Authorize(Roles = "Admin")]
        [Route("app/admin", Name = AppControllerRoute.GetAdmin)]
        public ActionResult Admin()
        {
            return View(AppControllerAction.Admin);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/company", Name = AppControllerRoute.GetCompany)]
        public ActionResult Company()
        {
            return View(AppControllerAction.Company);
        }

        [Authorize(Roles = "Admin")]
        [Route("app/companies", Name = AppControllerRoute.GetCompanies)]
        public ActionResult Companies()
        {
            return View(AppControllerAction.Companies);
        }

        [Route("app/company/create", Name = AppControllerRoute.GetCompanyCreate)]
        public ActionResult CompanyCreate()
        {
            return View(AppControllerAction.CompanyCreate);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/company/read", Name = AppControllerRoute.GetCompanyRead)]
        public ActionResult CompanyRead()
        {
            return View(AppControllerAction.CompanyRead);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/company/modify", Name = AppControllerRoute.GetCompanyModify)]
        public ActionResult CompanyModify()
        {
            return View(AppControllerAction.CompanyModify);
        }

        [Authorize(Roles = "CompanyOwner")]
        [Route("app/company/delete", Name = AppControllerRoute.GetCompanyDelete)]
        public ActionResult CompanyDelete()
        {
            return View(AppControllerAction.CompanyDelete);
        }

        [Route("app/company/order", Name = AppControllerRoute.GetCompanyOrder)]
        public ActionResult CompanyOrder()
        {
            return View(AppControllerAction.CompanyOrder);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/gametypes", Name = AppControllerRoute.GetGameTypes)]
        public ActionResult GameTypes()
        {
            return View(AppControllerAction.GameTypes);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/gametype/read", Name = AppControllerRoute.GetGameTypeRead)]
        public ActionResult GameTypeRead()
        {
            return View(AppControllerAction.GameTypeRead);
        }

        [Authorize]
        [Route("app/order", Name = AppControllerRoute.GetOrder)]
        public ActionResult Order()
        {
            return View(AppControllerAction.Order);
        }

        [Authorize(Roles = "CompanyOwner, GameTypesModify")]
        [Route("app/gametype/modify", Name = AppControllerRoute.GetGameTypeModify)]
        public ActionResult GameTypeModify()
        {
            return View(AppControllerAction.GameTypeModify);
        }

        [Authorize(Roles = "CompanyOwner, GameTypesModify")]
        [Route("app/gametype/delete", Name = AppControllerRoute.GetGameTypeDelete)]
        public ActionResult GameTypeDelete()
        {
            return View(AppControllerAction.GameTypeDelete);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/playgrounds", Name = AppControllerRoute.GetPlaygrounds)]
        public ActionResult Playgrounds()
        {
            return View(AppControllerAction.Playgrounds);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/playground/read", Name = AppControllerRoute.GetPlaygroundRead)]
        public ActionResult PlaygroundRead()
        {
            return View(AppControllerAction.PlaygroundRead);
        }

        [Authorize(Roles = "CompanyOwner, PlaygroundsModify")]
        [Route("app/playground/modify", Name = AppControllerRoute.GetPlaygroundModify)]
        public ActionResult PlaygroundModify()
        {
            return View(AppControllerAction.PlaygroundModify);
        }

        [Authorize(Roles = "CompanyOwner, PlaygroundsModify")]
        [Route("app/playground/delete", Name = AppControllerRoute.GetPlaygroundDelete)]
        public ActionResult PlaygroundDelete()
        {
            return View(AppControllerAction.PlaygroundDelete);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/events", Name = AppControllerRoute.GetEvents)]
        public ActionResult Events()
        {
            return View(AppControllerAction.Events);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/event/read", Name = AppControllerRoute.GetEventRead)]
        public ActionResult EventRead()
        {
            return View(AppControllerAction.EventRead);
        }

        [Authorize(Roles = "CompanyOwner, EventsModify")]
        [Route("app/event/modify", Name = AppControllerRoute.GetEventModify)]
        public ActionResult EventModify()
        {
            return View(AppControllerAction.EventModify);
        }

        [Authorize(Roles = "CompanyOwner, EventsModify")]
        [Route("app/event/delete", Name =AppControllerRoute.GetEventDelete)]
        public ActionResult EventDelete()
        {
            return View(AppControllerAction.EventDelete);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/news", Name = AppControllerRoute.GetNews)]
        public ActionResult News()
        {
            return View(AppControllerAction.News);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/news/read", Name = AppControllerRoute.GetNewsRead)]
        public ActionResult NewsRead()
        {
            return View(AppControllerAction.NewsRead);
        }

        [Authorize(Roles = "CompanyOwner, NewsModify")]
        [Route("app/news/modify", Name = AppControllerRoute.GetNewsModify)]
        public ActionResult NewsModify()
        {
            return View(AppControllerAction.NewsModify);
        }

        [Authorize(Roles = "CompanyOwner, NewsModify")]
        [Route("app/news/delete", Name = AppControllerRoute.GetNewsDelete)]
        public ActionResult NewsDelete()
        {
            return View(AppControllerAction.NewsDelete);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/certificates", Name = AppControllerRoute.GetCertificates)]
        public ActionResult Certificates()
        {
            return View(AppControllerAction.Certificates);
        }

        [Authorize(Roles = "CompanyOwner, CertificatesModify")]
        [Route("app/certificate/modify", Name = AppControllerRoute.GetCertificateModify)]
        public ActionResult CertificateModify()
        {
            return View(AppControllerAction.CertificateModify);
        }

        [Authorize(Roles = "CompanyOwner, CertificatesModify")]
        [Route("app/certificate/delete", Name = AppControllerRoute.GetCertificateDelete)]
        public ActionResult CertificateDelete()
        {
            return View(AppControllerAction.CertificateDelete);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/storage", Name = AppControllerRoute.GetStorage)]
        public ActionResult Storage()
        {
            return View(AppControllerAction.Storage);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/storage/read", Name = AppControllerRoute.GetStorageItemRead)]
        public ActionResult StorageItemRead()
        {
            return View(AppControllerAction.StorageItemRead);
        }

        [Authorize(Roles = "CompanyOwner, EquipmentModify")]
        [Route("app/storage/modify", Name = AppControllerRoute.GetStorageItemModify)]
        public ActionResult StorageItemModify()
        {
            return View(AppControllerAction.StorageItemModify);
        }

        [Authorize(Roles = "CompanyOwner, EquipmentModify")]
        [Route("app/storage/delete", Name = AppControllerRoute.GetStorageItemDelete)]
        public ActionResult StorageItemDelete()
        {
            return View(AppControllerAction.StorageItemDelete);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/operations", Name = AppControllerRoute.GetOperations)]
        public ActionResult Operations()
        {
            return View(AppControllerAction.Operations);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/operation/read", Name = AppControllerRoute.GetOperationRead)]
        public ActionResult OperationRead()
        {
            return View(AppControllerAction.OperationRead);
        }

        [Authorize(Roles = "CompanyOwner, OperationsModify")]
        [Route("app/operation/modify", Name = AppControllerRoute.GetOperationModify)]
        public ActionResult OperationModify()
        {
            return View(AppControllerAction.OperationModify);
        }

        [Authorize(Roles = "CompanyOwner, OperationsModify")]
        [Route("app/operation/delete", Name = AppControllerRoute.GetOperationDelete)]
        public ActionResult OperationDelete()
        {
            return View(AppControllerAction.OperationDelete);
        }

        [Route("app/games", Name = AppControllerRoute.GetGames)]
        public ActionResult Games()
        {
            return View(AppControllerAction.Games);
        }

        [Route("app/game/read", Name = AppControllerRoute.GetGameRead)]
        public ActionResult GameRead()
        {
            return View(AppControllerAction.GameRead);
        }
        
        [Route("app/game/modify", Name = AppControllerRoute.GetGameModify)]
        public ActionResult GameModify()
        {
            return View(AppControllerAction.GameModify);
        }
        
        [Route("app/game/delete", Name = AppControllerRoute.GetGameDelete)]
        public ActionResult GameDelete()
        {
            return View(AppControllerAction.GameDelete);
        }

        [Route("app/mygames", Name = AppControllerRoute.GetMyGames)]
        public ActionResult MyGames()
        {
            return View(AppControllerAction.MyGames);
        }

        [Route("app/mygame/modify", Name = AppControllerRoute.GetMyGamesModify)]
        public ActionResult MyGameModify()
        {
            return View(AppControllerAction.MyGameModify);
        }

        [Route("app/mygame/delete", Name = AppControllerRoute.GetMyGamesDelete)]
        public ActionResult MyGameDelete()
        {
            return View(AppControllerAction.MyGameDelete);
        }

        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("app/tasks", Name = AppControllerRoute.GetTasks)]
        public ActionResult Tasks()
        {
            return View(AppControllerAction.Tasks);
        }

        [Authorize(Roles = "CompanyOwner")]
        [Route("app/staff", Name = AppControllerRoute.GetStaff)]
        public ActionResult Staff()
        {
            return View(AppControllerAction.Staff);
        }

        [Authorize(Roles = "CompanyOwner")]
        [Route("app/staff/read", Name = AppControllerRoute.GetStaffRead)]
        public ActionResult StaffRead()
        {
            return View(AppControllerAction.StaffRead);
        }

        [Authorize(Roles = "CompanyOwner")]
        [Route("app/staff/modify", Name = AppControllerRoute.GetStaffModify)]
        public ActionResult StaffModify()
        {
            return View(AppControllerAction.StaffModify);
        }

        [Authorize(Roles = "CompanyOwner")]
        [Route("app/staff/delete", Name = AppControllerRoute.GetStaffDelete)]
        public ActionResult StaffDelete()
        {
            return View(AppControllerAction.StaffDelete);
        }

        [Authorize(Roles = "CompanyOwner")]
        [Route("app/staff/register", Name = AppControllerRoute.GetStaffRegister)]
        public ActionResult StaffRegister()
        {
            return View(AppControllerAction.StaffRegister);
        }
    }
}