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
        public async System.Threading.Tasks.Task<OperationResult<Operation>> GetOperations(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Operation>>(() =>
            {
                OperationResult<Operation> result = new OperationResult<Operation>();
                try
                {
                    if (IsInCompany())
                    {
                        result.Count = OperationsRepository.Count("CompanyId = @CompanyId", new { CompanyId = CurrentUser.CompanyId.Value });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = OperationsRepository.Search("CompanyId = @CompanyId",
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
        public async System.Threading.Tasks.Task<OperationResult<Operation>> ReadOperation(int Id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Operation>>(() =>
            {
                OperationResult<Operation> result = new OperationResult<Operation>();
                try
                {
                    Operation operation = OperationsRepository.Read(Id);
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
        public async System.Threading.Tasks.Task<OperationResult<Operation>> CreateOperation(Operation operation)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Operation>>(() =>
            {
                OperationResult<Operation> result = new OperationResult<Operation>();
                try
                {
                    if (IsInCompany())
                    {
                        operation.CompanyId = CurrentUser.CompanyId.Value;
                        Operation newOperation = OperationsRepository.CreateOrUpdate(operation);
                        if (newOperation.Id > 0)
                        {
                            result.SingleResult = newOperation;
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
        public async System.Threading.Tasks.Task<OperationResult<Operation>> UpdateOperation(Operation operation)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Operation>>(() =>
            {
                OperationResult<Operation> result = new OperationResult<Operation>();
                try
                {
                    if (IsInCompany(operation.CompanyId))
                    {
                        result.Result = OperationsRepository.Update(operation);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<Operation>> DeleteOperation(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Operation>>(() =>
            {
                OperationResult<Operation> result = new OperationResult<Operation>();
                try
                {
                    Operation operation = OperationsRepository.Read(id);
                    if (operation != null)
                    {
                        if (IsInCompany(operation.CompanyId))
                        {
                            result.Result = OperationsRepository.Delete(id);
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