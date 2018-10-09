using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Paintball.DAL.Entities;

namespace Paintball.Web.Manager
{
    public partial class PaintballManager : IManager
    {
        public async System.Threading.Tasks.Task<OperationResult<Task>> GetTasks(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Task>>(() =>
            {
                OperationResult<Task> result = new OperationResult<Task>();
                try
                {
                    if (IsInCompany())
                    {
                        result.Count = TasksRepository.Count("CompanyId = @CompanyId", new { CompanyId = CurrentUser.CompanyId });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = TasksRepository.Search("CompanyId = @CompanyId",
                            new { PageSize = pageSize, PageNumber = pageNumber, CompanyId = CurrentUser.CompanyId }, descending);
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

        public async System.Threading.Tasks.Task<OperationResult<Task>> GetTasks(Guid userId, int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Task>>(() =>
            {
                OperationResult<Task> result = new OperationResult<Task>();
                try
                {
                    if (IsInCompany())
                    {
                        result.Count = TasksRepository.Count("CompanyId = @CompanyId AND StaffId = @StaffId", 
                            new { CompanyId = CurrentUser.CompanyId, StaffId = userId });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = TasksRepository.Search("CompanyId = @CompanyId AND StaffId = @StaffId",
                            new { PageSize = pageSize, PageNumber = pageNumber, CompanyId = CurrentUser.CompanyId, StaffId = userId }, descending);
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

        public async System.Threading.Tasks.Task<OperationResult<Task>> ReadTask(int Id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Task>>(() =>
            {
                OperationResult<Task> result = new OperationResult<Task>();
                try
                {
                    Task task = TasksRepository.Read(Id);
                    if (task != null)
                    {
                        if (IsInCompany(task.CompanyId))
                        {
                            result.SingleResult = task;
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
        public async System.Threading.Tasks.Task<OperationResult<Task>> CreateTask(Task task)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Task>>(() =>
            {
                OperationResult<Task> result = new OperationResult<Task>();
                try
                {
                    if (IsInCompany())
                    {
                        task.CompanyId = CurrentUser.CompanyId.Value;
                        if(task.StaffId == Guid.Empty)
                        {
                            task.StaffId = CurrentUser.Id;
                        }
                        Task newTask = TasksRepository.CreateOrUpdate(task);
                        if (newTask.Id > 0)
                        {
                            result.SingleResult = newTask;
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
        public async System.Threading.Tasks.Task<OperationResult<Task>> UpdateTask(Task task)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Task>>(() =>
            {
                OperationResult<Task> result = new OperationResult<Task>();
                try
                {
                    if (IsInCompany(task.CompanyId))
                    {
                        result.Result = TasksRepository.Update(task);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<Task>> DeleteTask(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Task>>(() =>
            {
                OperationResult<Task> result = new OperationResult<Task>();
                try
                {
                    Task task = TasksRepository.Read(id);
                    if (task != null)
                    {
                        if (IsInCompany(task.CompanyId))
                        {
                            result.Result = TasksRepository.Delete(id);
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