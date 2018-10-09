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
        public async System.Threading.Tasks.Task<OperationResult<EquipmentOrder>> GetEquipmentOrders(int gameId, int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<EquipmentOrder>>(() =>
            {
                OperationResult<EquipmentOrder> result = new OperationResult<EquipmentOrder>();
                try
                {
                    Game game = GamesRepository.Read(gameId);
                    if(game != null)
                    {
                        if(game.CreatorId == CurrentUser.Id || IsInCompany(game.CompanyId) || UserStore.IsInRole(CurrentUser, RoleNames.Admin))
                        {
                            result.Count = EquipmentOrdersRepository.Count("GameId = @GameId", new { GameId = game.Id });
                            if (result.Count > 0)
                            {
                                result.MultipleResult = EquipmentOrdersRepository.Search("GameId = @GameId",
                                new { PageSize = pageSize, PageNumber = pageNumber, GameId = game.Id }, descending);
                            }
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
        public async System.Threading.Tasks.Task<OperationResult<EquipmentOrder>> ReadEquipmentOrder(int Id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<EquipmentOrder>>(() =>
            {
                OperationResult<EquipmentOrder> result = new OperationResult<EquipmentOrder>();
                try
                {
                    EquipmentOrder order = EquipmentOrdersRepository.Read(Id);
                    if(order != null)
                    {
                        Game game = GamesRepository.Read(order.GameId);
                        if(game != null)
                        {
                            if(game.CreatorId == CurrentUser.Id || IsInCompany(game.CompanyId) || UserStore.IsInRole(CurrentUser, RoleNames.Admin))
                            {
                                result.SingleResult = order;
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
        public async System.Threading.Tasks.Task<OperationResult<EquipmentOrder>> CreateEquipmentOrder(EquipmentOrder equipment)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<EquipmentOrder>>(() =>
            {
                OperationResult<EquipmentOrder> result = new OperationResult<EquipmentOrder>();
                try
                {
                    Game game = GamesRepository.Read(equipment.GameId);
                    if(game != null)
                    {
                        if(game.CreatorId == CurrentUser.Id || IsInCompany(game.CompanyId) || UserStore.IsInRole(CurrentUser, RoleNames.Admin))
                        {
                            EquipmentOrder created = EquipmentOrdersRepository.CreateOrUpdate(equipment);
                            if (created.Id > 0)
                            {
                                result.SingleResult = created;
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
        public async System.Threading.Tasks.Task<OperationResult<EquipmentOrder>> UpdateEquipmentOrder(EquipmentOrder equipment)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<EquipmentOrder>>(() =>
            {
                OperationResult<EquipmentOrder> result = new OperationResult<EquipmentOrder>();
                try
                {
                    Game game = GamesRepository.Read(equipment.GameId);
                    if (game != null)
                    {
                        if (game.CreatorId == CurrentUser.Id || IsInCompany(game.CompanyId) || UserStore.IsInRole(CurrentUser, RoleNames.Admin))
                        {
                            result.Result = EquipmentOrdersRepository.Update(equipment);
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
        public async System.Threading.Tasks.Task<OperationResult<EquipmentOrder>> DeleteEquipmentOrder(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<EquipmentOrder>>(() =>
            {
                OperationResult<EquipmentOrder> result = new OperationResult<EquipmentOrder>();
                try
                {
                    EquipmentOrder equipment = EquipmentOrdersRepository.Read(id);
                    if(equipment != null)
                    {
                        Game game = GamesRepository.Read(equipment.GameId);
                        if (game != null)
                        {
                            if (game.CreatorId == CurrentUser.Id || IsInCompany(game.CompanyId) || UserStore.IsInRole(CurrentUser, RoleNames.Admin))
                            {
                                result.Result = EquipmentOrdersRepository.Delete(id);
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
    }
}