using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paintball.Web.Constants
{
    public static class AppControllerRoute
    {
        public const string GetError404 = ControllerName.App + "GetError404";
        public const string GetIndex = ControllerName.App + "GetIndex";
        public const string GetLogin = ControllerName.App + "GetLogin";
        public const string GetRegister = ControllerName.App + "GetRegister";

        public const string GetAdmin = ControllerName.App + "GetAdmin";

        public const string GetView = ControllerName.App + "GetView";

        public const string GetOrder = ControllerName.App + "GetOrder";

        public const string GetCompany = ControllerName.App + "GetCompany";
        public const string GetCompanies = ControllerName.App + "GetCompanies";
        public const string GetCompanyRead = ControllerName.App + "GetCompanyRead";
        public const string GetCompanyCreate = ControllerName.App + "GetCompanyCreate";
        public const string GetCompanyModify = ControllerName.App + "GetCompanyModify";
        public const string GetCompanyDelete = ControllerName.App + "GetCompanyDelete";
        public const string GetCompanyOrder = ControllerName.App + "GetCompanyOrder";

        public const string GetGameTypes = ControllerName.App + "GetGamesTypes";
        public const string GetGameTypeRead = ControllerName.App + "GetGameTypeRead";
        public const string GetGameTypeModify = ControllerName.App + "GetGameTypeModify";
        public const string GetGameTypeDelete = ControllerName.App + "GetGameTypeDelete";

        public const string GetPlaygrounds = ControllerName.App + "GetPlaygrounds";
        public const string GetPlaygroundRead = ControllerName.App + "GetPlaygroundRead";
        public const string GetPlaygroundModify = ControllerName.App + "GetPlaygroundModify";
        public const string GetPlaygroundDelete = ControllerName.App + "GetPlaygroundDelete";

        public const string GetEvents = ControllerName.App + "GetEvents";
        public const string GetEventRead = ControllerName.App + "GetEventRead";
        public const string GetEventModify = ControllerName.App + "GetEventModify";
        public const string GetEventDelete = ControllerName.App + "GetEventDelete";

        public const string GetNews = ControllerName.App + "GetNews";
        public const string GetNewsRead = ControllerName.App + "GetNewsRead";
        public const string GetNewsModify = ControllerName.App + "GetNewsModify";
        public const string GetNewsDelete = ControllerName.App + "GetNewsDelete";

        public const string GetCertificates = ControllerName.App + "GetCertificates";
        public const string GetCertificateModify = ControllerName.App + "GetCertificateModify";
        public const string GetCertificateDelete = ControllerName.App + "GetCertificateDelete";

        public const string GetStorage = ControllerName.App + "GetStorage";
        public const string GetStorageItemRead = ControllerName.App + "GetStorageItemRead";
        public const string GetStorageItemModify = ControllerName.App + "GetStorageItemModify";
        public const string GetStorageItemDelete = ControllerName.App + "GetStorageItemDelete";

        public const string GetOperations = ControllerName.App + "GetOperations";
        public const string GetOperationRead = ControllerName.App + "GetOperationRead";
        public const string GetOperationModify = ControllerName.App + "GetOperationModify";
        public const string GetOperationDelete = ControllerName.App + "GetOperationDelete";
        
        public const string GetGames = ControllerName.App + "GetGames";
        public const string GetGameRead = ControllerName.App + "GetGameRead";
        public const string GetGameModify = ControllerName.App + "GetGameModify";
        public const string GetGameDelete = ControllerName.App + "GetGameDelete";

        public const string GetMyGames = ControllerName.App + "GetMyGames";
        public const string GetMyGamesModify = ControllerName.App + "GetMyGamesModify";
        public const string GetMyGamesDelete = ControllerName.App + "GetMyGamesDelete";

        public const string GetTasks = ControllerName.App + "GetTasks";

        public const string GetStaff = ControllerName.App + "GetStaff";
        public const string GetStaffRead = ControllerName.App + "GetStaffRead";
        public const string GetStaffModify = ControllerName.App + "GetStaffModify";
        public const string GetStaffRegister = ControllerName.App + "GetStaffRegister";
        public const string GetStaffDelete = ControllerName.App + "GetStaffDelete";
    }
}