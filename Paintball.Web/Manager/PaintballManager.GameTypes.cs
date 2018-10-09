using System;
using Paintball.DAL.Entities;

namespace Paintball.Web.Manager
{
    public partial class PaintballManager : IManager
    {
        public async System.Threading.Tasks.Task<OperationResult<GameType>> GetGameTypes(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<GameType>>(() =>
            {
                OperationResult<GameType> result = new OperationResult<GameType>();
                try
                {
                    if (IsInCompany())
                    {
                        result.Count = GameTypesRepository.Count("CompanyId = @CompanyId", new { CompanyId = CurrentUser.CompanyId.Value });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = GameTypesRepository.Search("CompanyId = @CompanyId",
                            new { PageSize = pageSize, PageNumber = pageNumber, CompanyId = CurrentUser.CompanyId.Value }, descending);
                        }
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
        public async System.Threading.Tasks.Task<OperationResult<GameType>> ReadGameType(int Id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                OperationResult<GameType> result = new OperationResult<GameType>();
                try
                {
                    GameType gameType = GameTypesRepository.Read(Id);
                    if (gameType != null)
                    {
                        if (IsInCompany(gameType.CompanyId))
                        {
                            result.SingleResult = gameType;
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
        public async System.Threading.Tasks.Task<OperationResult<GameType>> CreateGameType(GameType gameType)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<GameType>>(() =>
            {
                OperationResult<GameType> result = new OperationResult<GameType>();
                try
                {
                    if (IsInCompany())
                    {
                        gameType.CompanyId = CurrentUser.CompanyId.Value;
                        GameType newGameType = GameTypesRepository.CreateOrUpdate(gameType);
                        if (newGameType.Id > 0)
                        {
                            result.SingleResult = newGameType;
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
        public async System.Threading.Tasks.Task<OperationResult<GameType>> UpdateGameType(GameType gameType)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<GameType>>(() =>
            {
                OperationResult<GameType> result = new OperationResult<GameType>();
                try
                {
                    if (IsInCompany(gameType.CompanyId))
                    {
                        result.Result = GameTypesRepository.Update(gameType);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<GameType>> DeleteGameType(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<GameType>>(() =>
            {
                OperationResult<GameType> result = new OperationResult<GameType>();
                try
                {
                    GameType gameType = GameTypesRepository.Read(id);
                    if (gameType != null)
                    {
                        if (IsInCompany(gameType.CompanyId))
                        {
                            result.Result = GameTypesRepository.Delete(id);
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