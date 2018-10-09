using Paintball.DAL.Entities;
using Paintball.Web.Model.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paintball.Web.Manager
{
    public partial class PaintballManager : IManager
    {
        public async System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> GetMyGames(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<MyGameReponseSingle>>(() =>
            {
                OperationResult<MyGameReponseSingle> result = new OperationResult<MyGameReponseSingle>();
                try
                {
                    result.Count = GamesRepository.Count("CreatorId = @CreatorId", new { CreatorId = CurrentUser.Id });
                    if (result.Count > 0)
                    {
                        List<MyGameReponseSingle> response = new List<MyGameReponseSingle>();
                        var games = GamesRepository.Search("CreatorId = @CreatorId", new { PageSize = pageSize, PageNumber = pageNumber, CreatorId = CurrentUser.Id }, descending);
                        foreach(var game in games)
                        {
                            MyGameReponseSingle single = new MyGameReponseSingle();
                            single.Game = game;
                            single.Playground = PlaygroundsRepository.Read(game.Playground);
                            single.GameType = GameTypesRepository.Read(game.GameType);
                            response.Add(single);
                        }
                        result.MultipleResult = response;
                    }
                    result.Result = true;
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }

        public async System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> GetMyGames(DateTime date, int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<MyGameReponseSingle>>(() =>
            {
                OperationResult<MyGameReponseSingle> result = new OperationResult<MyGameReponseSingle>();
                try
                {
                    DateTime startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    DateTime endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    
                    result.Count = GamesRepository.Count("CreatorId = @CreatorId AND BeginDate BETWEEN @StartDate AND @EndDate",
                        new { CreatorId = CurrentUser.Id, StartDate = startDate, EndDate = endDate });
                    if (result.Count > 0)
                    {
                        List<MyGameReponseSingle> response = new List<MyGameReponseSingle>();
                        var games = GamesRepository.Search("CreatorId = @CreatorId AND BeginDate BETWEEN @StartDate AND @EndDate",
                        new { PageSize = pageSize, PageNumber = pageNumber, CreatorId = CurrentUser.Id, StartDate = startDate, EndDate = endDate });
                        foreach (var game in games)
                        {
                            MyGameReponseSingle single = new MyGameReponseSingle();
                            single.Game = game;
                            single.Playground = PlaygroundsRepository.Read(game.Playground);
                            single.GameType = GameTypesRepository.Read(game.GameType);
                            response.Add(single);
                        }
                        result.MultipleResult = response;
                    }
                    result.Result = true;
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }

        public async System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> CreateMyGame(Game game)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<MyGameReponseSingle>>(() =>
            {
                OperationResult<MyGameReponseSingle> result = new OperationResult<MyGameReponseSingle>();
                try
                {
                    game.CreatorId = CurrentUser.Id;
                    Game newGame = GamesRepository.CreateOrUpdate(game);
                    if (newGame.Id > 0)
                    {
                        newGame.CreatorId = Guid.Empty;
                        MyGameReponseSingle single = new MyGameReponseSingle();

                        single.Playground = PlaygroundsRepository.Read(game.Playground);
                        single.GameType = GameTypesRepository.Read(game.GameType);
                        single.Game = newGame;
                        single.Orders = EquipmentOrdersRepository.Search("GameId = @GameId",
                            new { PageSize = 1, PageNumber = 200, GameId = game.Id }, true);

                        result.SingleResult = single;

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

        public async System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> ReadMyGame(int Id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<MyGameReponseSingle>>(() =>
            {
                OperationResult<MyGameReponseSingle> result = new OperationResult<MyGameReponseSingle>();
                try
                {
                    Game game = GamesRepository.Read(Id);
                    if (game != null)
                    {
                        if (game.CreatorId == CurrentUser.Id)
                        {
                            game.CreatorId = Guid.Empty;
                            MyGameReponseSingle single = new MyGameReponseSingle();

                            single.Playground = PlaygroundsRepository.Read(game.Playground);
                            single.GameType = GameTypesRepository.Read(game.GameType);
                            single.Game = game;
                            single.Orders = EquipmentOrdersRepository.Search("GameId = @GameId",
                                new { PageSize = 200, PageNumber = 1, GameId = Id }, true);

                            result.SingleResult = single;
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
        
        public async System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> UpdateMyGame(Game game)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<MyGameReponseSingle>>(() =>
            {
                OperationResult<MyGameReponseSingle> result = new OperationResult<MyGameReponseSingle>();
                try
                {
                    var dbGame = GamesRepository.Read(game.Id);
                    if (dbGame.CreatorId == CurrentUser.Id)
                    {
                        game.CreatorId = dbGame.CreatorId;
                        result.Result = GamesRepository.Update(game);

                        if (result.Result)
                        {
                            game.CreatorId = Guid.Empty;
                            MyGameReponseSingle single = new MyGameReponseSingle();

                            single.Playground = PlaygroundsRepository.Read(game.Playground);
                            single.GameType = GameTypesRepository.Read(game.GameType);
                            single.Game = game;
                            single.Orders = EquipmentOrdersRepository.Search("GameId = @GameId",
                                new { PageSize = 1, PageNumber = 200, GameId = game.Id }, true);

                            result.SingleResult = single;
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
        public async System.Threading.Tasks.Task<OperationResult<MyGameReponseSingle>> DeleteMyGame(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<MyGameReponseSingle>>(() =>
            {
                OperationResult<MyGameReponseSingle> result = new OperationResult<MyGameReponseSingle>();
                try
                {
                    Game game = GamesRepository.Read(id);
                    if (game != null)
                    {
                        if (game.CreatorId == CurrentUser.Id)
                        {
                            game.CreatorId = Guid.Empty;
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