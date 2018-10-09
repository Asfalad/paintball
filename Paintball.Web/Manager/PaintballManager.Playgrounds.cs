using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Paintball.DAL.Entities;

namespace Paintball.Web.Manager
{
    public partial class PaintballManager : IManager
    {
        public async System.Threading.Tasks.Task<OperationResult<Playground>> GetPlaygrounds(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Playground>>(() =>
            {
                OperationResult<Playground> result = new OperationResult<Playground>();
                try
                {
                    if (IsInCompany())
                    {
                        result.Count = PlaygroundsRepository.Count("CompanyId = @CompanyId", new { CompanyId = CurrentUser.CompanyId.Value });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = PlaygroundsRepository.Search("CompanyId = @CompanyId",
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
        public async System.Threading.Tasks.Task<OperationResult<Playground>> ReadPlayground(int Id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Playground>>(() =>
            {
                OperationResult<Playground> result = new OperationResult<Playground>();
                try
                {
                    Playground operation = PlaygroundsRepository.Read(Id);
                    if (operation != null)
                    {
                        if (IsInCompany(operation.CompanyId))
                        {
                            result.SingleResult = operation;
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
        public async System.Threading.Tasks.Task<OperationResult<Playground>> CreatePlayground(Playground playground)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Playground>>(() =>
            {
                OperationResult<Playground> result = new OperationResult<Playground>();
                try
                {
                    if (IsInCompany())
                    {
                        playground.CompanyId = CurrentUser.CompanyId.Value;
                        Playground newPlayground = PlaygroundsRepository.CreateOrUpdate(playground);
                        if (newPlayground.Id > 0)
                        {
                            result.SingleResult = newPlayground;
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
        public async System.Threading.Tasks.Task<OperationResult<Playground>> UpdatePlayground(Playground playground)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Playground>>(() =>
            {
                OperationResult<Playground> result = new OperationResult<Playground>();
                try
                {
                    if (IsInCompany(playground.CompanyId))
                    {
                        result.Result = PlaygroundsRepository.Update(playground);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<Playground>> DeletePlayground(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Playground>>(() =>
            {
                OperationResult<Playground> result = new OperationResult<Playground>();
                try
                {
                    Playground operation = PlaygroundsRepository.Read(id);
                    if (operation != null)
                    {
                        if (IsInCompany(operation.CompanyId))
                        {
                            result.Result = PlaygroundsRepository.Delete(id);
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