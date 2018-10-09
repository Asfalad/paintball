using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Paintball.DAL.Entities;
using Paintball.Web.Constants;

namespace Paintball.Web.Manager
{
    public partial class PaintballManager : IManager
    {
        public async System.Threading.Tasks.Task<OperationResult<UserPartial>> GetUsers(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<UserPartial>>(() =>
            {
                OperationResult<UserPartial> result = new OperationResult<UserPartial>();
                try
                {
                    if (IsInCompany())
                    {
                        List<IdentityUser> users = UserStore.Users.Where(u => u.CompanyId == CurrentUser.CompanyId).ToList();
                        List<UserPartial> partials = new List<UserPartial>();
                        foreach(var user in users)
                        {
                            partials.Add(new UserPartial
                            {
                                FirstName = user.Profile.FirstName,
                                LastName = user.Profile.LastName,
                                MiddleName = user.Profile.MiddleName,
                                UserName = user.UserName
                            });
                        }
                        result.MultipleResult = partials;
                        result.Count = partials.Count;
                        result.Result = true;
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<UserPartial>> ReadUser(string userName)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<UserPartial>>(() =>
            {
                OperationResult<UserPartial> result = new OperationResult<UserPartial>();
                try
                {
                    IdentityUser user = UserStore.FindByName(userName);
                    if (user != null)
                    {
                        if (IsInCompany(user.CompanyId))
                        {
                            List<string> roles = new List<string>(UserStore.GetRoles(user));
                            UserPartial partial = new UserPartial
                            {
                                FirstName = user.Profile.FirstName,
                                LastName = user.Profile.LastName,
                                MiddleName = user.Profile.MiddleName,
                                UserName = user.UserName,
                                CompanyOwner = roles.Contains(RoleNames.CompanyOwner),
                                CertificatesModify = roles.Contains(RoleNames.CertificatesModify),
                                CompanyModify = roles.Contains(RoleNames.CompanyModify),
                                EquipmentModify = roles.Contains(RoleNames.EquipmentModify),
                                EventsModify = roles.Contains(RoleNames.EventsModify),
                                GameTypesModify = roles.Contains(RoleNames.GameTypesModify),
                                NewsModify = roles.Contains(RoleNames.NewsModify),
                                OperationsModify = roles.Contains(RoleNames.OperationsModify),
                                OperationsRead = roles.Contains(RoleNames.OperationsRead),
                                PlaygroundsModify = roles.Contains(RoleNames.PlaygroundsModify)
                            };
                            result.SingleResult = partial;
                            result.Result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<UserPartial>> CreateUser(CreateStaffViewModel options)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<UserPartial>>(() =>
            {
                OperationResult<UserPartial> result = new OperationResult<UserPartial>();
                try
                {
                    if (options != null)
                    {
                        if (IsInCompany())
                        {
                            IdentityUser user = new IdentityUser()
                            {
                                Email = options.Email,
                                UserName = options.Email,
                                CompanyId = CurrentUser.CompanyId,
                                Salary = options.Salary
                            };
                            UserStore.Create(user);
                            user = UserStore.FindByName(options.Email);
                            
                            if(user != null)
                            {
                                user.Profile.FirstName = options.FirstName;
                                user.Profile.LastName = options.LastName;
                                user.Profile.MiddleName = options.MiddleName;
                                UserStore.Update(user);

                                if (options.CertificatesModify)
                                {
                                    UserStore.AddToRole(user, RoleNames.CertificatesModify);
                                }
                                if(options.CompanyModify)
                                {
                                    UserStore.AddToRole(user, RoleNames.CompanyModify);
                                }
                                if (options.EquipmentModify)
                                {
                                    UserStore.AddToRole(user, RoleNames.EquipmentModify);
                                }
                                if(options.EventsModify)
                                {
                                    UserStore.AddToRole(user, RoleNames.EventsModify);
                                }
                                if(options.GameTypesModify)
                                {
                                    UserStore.AddToRole(user, RoleNames.GameTypesModify);
                                }
                                if(options.NewsModify)
                                {
                                    UserStore.AddToRole(user, RoleNames.NewsModify);
                                }
                                if(options.OperationsModify)
                                {
                                    UserStore.AddToRole(user, RoleNames.OperationsModify);
                                }
                                if(options.OperationsRead)
                                {
                                    UserStore.AddToRole(user, RoleNames.OperationsRead);
                                }
                                if(options.PlaygroundsModify)
                                {
                                    UserStore.AddToRole(user, RoleNames.PlaygroundsModify);
                                }
                                List<string> roles = new List<string>(UserStore.GetRoles(user));
                                UserPartial partial = new UserPartial
                                {
                                    FirstName = user.Profile.FirstName,
                                    LastName = user.Profile.LastName,
                                    MiddleName = user.Profile.MiddleName,
                                    UserName = user.UserName,
                                    CompanyOwner = roles.Contains(RoleNames.CompanyOwner),
                                    CertificatesModify = roles.Contains(RoleNames.CertificatesModify),
                                    CompanyModify = roles.Contains(RoleNames.CompanyModify),
                                    EquipmentModify = roles.Contains(RoleNames.EquipmentModify),
                                    EventsModify = roles.Contains(RoleNames.EventsModify),
                                    GameTypesModify = roles.Contains(RoleNames.GameTypesModify),
                                    NewsModify = roles.Contains(RoleNames.NewsModify),
                                    OperationsModify = roles.Contains(RoleNames.OperationsModify),
                                    OperationsRead = roles.Contains(RoleNames.OperationsRead),
                                    PlaygroundsModify = roles.Contains(RoleNames.PlaygroundsModify)
                                };
                                result.SingleResult = partial;
                                result.Result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<UserPartial>> UpdateUser(UpdateStaffViewModel user)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<UserPartial>>(() =>
            {
                OperationResult<UserPartial> result = new OperationResult<UserPartial>();
                try
                {
                    if (IsInCompany())
                    {
                        if(user != null)
                        {
                            var userFromDb = UserStore.FindByName(user.UserName);

                            if (userFromDb != null)
                            {
                                userFromDb.Profile.FirstName = user.FirstName;
                                userFromDb.Profile.LastName = user.LastName;
                                userFromDb.Profile.MiddleName = user.MiddleName;

                                UserStore.Update(userFromDb);

                                if (user.CertificatesModify)
                                {
                                    UserStore.AddToRole(userFromDb, RoleNames.CertificatesModify);
                                }
                                else
                                {
                                    UserStore.RemoveFromRole(userFromDb, RoleNames.CertificatesModify);
                                }
                                if (user.CompanyModify)
                                {
                                    UserStore.AddToRole(userFromDb, RoleNames.CompanyModify);
                                }else
                                {
                                    UserStore.RemoveFromRole(userFromDb, RoleNames.CompanyModify);
                                }
                                if (user.EquipmentModify)
                                {
                                    UserStore.AddToRole(userFromDb, RoleNames.EquipmentModify);
                                }else
                                {
                                    UserStore.RemoveFromRole(userFromDb, RoleNames.EquipmentModify);
                                }
                                if (user.EventsModify)
                                {
                                    UserStore.AddToRole(userFromDb, RoleNames.EventsModify);
                                }else
                                {
                                    UserStore.RemoveFromRole(userFromDb, RoleNames.EventsModify);
                                }
                                if (user.GameTypesModify)
                                {
                                    UserStore.AddToRole(userFromDb, RoleNames.GameTypesModify);
                                }else
                                {
                                    UserStore.RemoveFromRole(userFromDb, RoleNames.GameTypesModify);
                                }
                                if (user.NewsModify)
                                {
                                    UserStore.AddToRole(userFromDb, RoleNames.NewsModify);
                                }else
                                {
                                    UserStore.RemoveFromRole(userFromDb, RoleNames.NewsModify);
                                }
                                if (user.OperationsModify)
                                {
                                    UserStore.AddToRole(userFromDb, RoleNames.OperationsModify);
                                }else
                                {
                                    UserStore.RemoveFromRole(userFromDb, RoleNames.OperationsModify);
                                }
                                if (user.OperationsRead)
                                {
                                    UserStore.AddToRole(userFromDb, RoleNames.OperationsRead);
                                }else
                                {
                                    UserStore.RemoveFromRole(userFromDb, RoleNames.OperationsRead);
                                }
                                if (user.PlaygroundsModify)
                                {
                                    UserStore.AddToRole(userFromDb, RoleNames.PlaygroundsModify);
                                }else
                                {
                                    UserStore.RemoveFromRole(userFromDb, RoleNames.PlaygroundsModify);
                                }

                                List<string> roles = new List<string>(UserStore.GetRoles(userFromDb));
                                UserPartial partial = new UserPartial
                                {
                                    FirstName = userFromDb.Profile.FirstName,
                                    LastName = userFromDb.Profile.LastName,
                                    MiddleName = userFromDb.Profile.MiddleName,
                                    UserName = userFromDb.UserName,
                                    CompanyOwner = roles.Contains(RoleNames.CompanyOwner),
                                    CertificatesModify = roles.Contains(RoleNames.CertificatesModify),
                                    CompanyModify = roles.Contains(RoleNames.CompanyModify),
                                    EquipmentModify = roles.Contains(RoleNames.EquipmentModify),
                                    EventsModify = roles.Contains(RoleNames.EventsModify),
                                    GameTypesModify = roles.Contains(RoleNames.GameTypesModify),
                                    NewsModify = roles.Contains(RoleNames.NewsModify),
                                    OperationsModify = roles.Contains(RoleNames.OperationsModify),
                                    OperationsRead = roles.Contains(RoleNames.OperationsRead),
                                    PlaygroundsModify = roles.Contains(RoleNames.PlaygroundsModify)
                                };
                                result.SingleResult = partial;
                                result.Result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<UserPartial>> DeleteUser(string userName)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<UserPartial>>(() =>
            {
                OperationResult<UserPartial> result = new OperationResult<UserPartial>();
                try
                {
                    if (IsInCompany())
                    {
                        var user = UserStore.FindByName(userName);
                        if (user != null)
                        {
                            UserStore.RemoveFromRole(user, RoleNames.CertificatesModify);
                            UserStore.RemoveFromRole(user, RoleNames.CompanyModify);
                            UserStore.RemoveFromRole(user, RoleNames.CompanyStaff);
                            UserStore.RemoveFromRole(user, RoleNames.EquipmentModify);
                            UserStore.RemoveFromRole(user, RoleNames.EventsModify);
                            UserStore.RemoveFromRole(user, RoleNames.GameTypesModify);
                            UserStore.RemoveFromRole(user, RoleNames.NewsModify);
                            UserStore.RemoveFromRole(user, RoleNames.OperationsModify);
                            UserStore.RemoveFromRole(user, RoleNames.OperationsRead);
                            UserStore.RemoveFromRole(user, RoleNames.PlaygroundsModify);
                            UserStore.AddToRole(user, RoleNames.User);
                            UserStore.RemoveFromCompany(user);

                            List<string> roles = new List<string>(UserStore.GetRoles(user));
                            UserPartial partial = new UserPartial
                            {
                                FirstName = user.Profile.FirstName,
                                LastName = user.Profile.LastName,
                                MiddleName = user.Profile.MiddleName,
                                UserName = user.UserName,
                                CompanyOwner = roles.Contains(RoleNames.CompanyOwner),
                                CertificatesModify = roles.Contains(RoleNames.CertificatesModify),
                                CompanyModify = roles.Contains(RoleNames.CompanyModify),
                                EquipmentModify = roles.Contains(RoleNames.EquipmentModify),
                                EventsModify = roles.Contains(RoleNames.EventsModify),
                                GameTypesModify = roles.Contains(RoleNames.GameTypesModify),
                                NewsModify = roles.Contains(RoleNames.NewsModify),
                                OperationsModify = roles.Contains(RoleNames.OperationsModify),
                                OperationsRead = roles.Contains(RoleNames.OperationsRead),
                                PlaygroundsModify = roles.Contains(RoleNames.PlaygroundsModify)
                            };
                            result.SingleResult = partial;
                            result.Result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
            return result;
        });
       }
    }
}