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
        public async System.Threading.Tasks.Task<OperationResult<Game>> GetGames(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Game>>(() =>
            {
                OperationResult<Game> result = new OperationResult<Game>();
                try
                {
                    if (IsInCompany())
                    {
                        result.Count = GamesRepository.Count("CompanyId = @CompanyId", new { CompanyId = CurrentUser.CompanyId.Value });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = GamesRepository.Search("CompanyId = @CompanyId",
                            new { PageSize = pageSize, PageNumber = pageNumber, CompanyId = CurrentUser.CompanyId.Value }, descending);
                        }
                        result.Result = true;
                    }
                    else
                    {
                        result.Count = GamesRepository.Count("CreatorId = @CreatorId", new { CreatorId = CurrentUser.Id });
                        if(result.Count > 0)
                        {
                            result.MultipleResult = GamesRepository.Search("CreatorId = @CreatorId",
                                new { PageSize = pageSize, PageNumber = pageNumber, CreatorId = CurrentUser.Id }, descending);
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

        public async System.Threading.Tasks.Task<OperationResult<Game>> GetGames(DateTime date)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Game>>(() =>
            {
                OperationResult<Game> result = new OperationResult<Game>();
                try
                {
                    DateTime startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    DateTime endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    if (IsInCompany())
                    {
                        result.Count = GamesRepository.Count("CompanyId = @CompanyId AND BeginDate BETWEEN @StartDate AND @EndDate", 
                            new { CompanyId = CurrentUser.CompanyId, StartDate = startDate, EndDate = endDate });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = GamesRepository.Search("CompanyId = @CompanyId AND BeginDate BETWEEN @StartDate AND @EndDate",
                            new { PageSize = 100, PageNumber = 1, CompanyId = CurrentUser.CompanyId, StartDate = startDate, EndDate = endDate });
                        }
                        result.Result = true;
                    }
                    else
                    {
                        result.Count = GamesRepository.Count("CreatorId = @CreatorId AND BeginDate BETWEEN @StartDate AND @EndDate",
                            new { CreatorId = CurrentUser.Id, StartDate = startDate, EndDate = endDate });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = GamesRepository.Search("CreatorId = @CreatorId AND BeginDate BETWEEN @StartDate AND @EndDate",
                            new { PageSize = 100, PageNumber = 1, CreatorId = CurrentUser.Id, StartDate = startDate, EndDate = endDate });
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
        public async System.Threading.Tasks.Task<OperationResult<Game>> ReadGame(int Id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Game>>(() =>
            {
                OperationResult<Game> result = new OperationResult<Game>();
                try
                {
                    Game game = GamesRepository.Read(Id);
                    if(game != null)
                    {
                        if (IsInCompany(game.CompanyId))
                        {
                            result.SingleResult = game;
                            result.Result = true;
                        }
                        else if (game.CreatorId == CurrentUser.Id)
                        {
                            result.SingleResult = game;
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
        public async System.Threading.Tasks.Task<OperationResult<Game>> CreateGame(Game game)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Game>>(() =>
            {
                OperationResult<Game> result = new OperationResult<Game>();
                try
                {
                    if (IsInCompany())
                    {
                        game.CompanyId = CurrentUser.CompanyId.Value;
                        game.CreatorId = CurrentUser.Id;
                        Game newGame = GamesRepository.CreateOrUpdate(game);
                        if (newGame.Id > 0)
                        {
                            result.SingleResult = newGame;
                            result.Result = true;
                        }
                    }
                    else
                    {
                        game.CreatorId = CurrentUser.Id;
                        Game newGame = GamesRepository.CreateOrUpdate(game);
                        if(newGame.Id > 0)
                        {
                            result.SingleResult = newGame;
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
        public async System.Threading.Tasks.Task<OperationResult<Game>> UpdateGame(Game game)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Game>>(() =>
            {
                OperationResult<Game> result = new OperationResult<Game>();
                try
                {
                    if (IsInCompany(game.CompanyId) || game.CreatorId == CurrentUser.Id || UserStore.IsInRole(CurrentUser, RoleNames.Admin))
                    {
                        result.Result = GamesRepository.Update(game);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<Game>> DeleteGame(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Game>>(() =>
            {
                OperationResult<Game> result = new OperationResult<Game>();
                try
                {
                    Game game = GamesRepository.Read(id);
                    if (game != null)
                    {
                        if (IsInCompany(game.CompanyId) || game.CreatorId == CurrentUser.Id || UserStore.IsInRole(CurrentUser, RoleNames.Admin))
                        {
                            result.Result = GamesRepository.Delete(id);
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