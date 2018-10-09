using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paintball.Web.Constants
{
    public class AccountControllerRoute
    {
        public const string GetLogin = ControllerName.Account + "GetLogin";

        public const string PostLogin = ControllerName.Account + "PostLogin";

        public const string GetRegister = ControllerName.Account + "GetRegister";

        public const string PostRegister = ControllerName.Account + "PostRegister";

        public const string PostLogOff = ControllerName.Account + "PostLogOff";
    }
}