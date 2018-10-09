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
        public async System.Threading.Tasks.Task<OperationResult<Event>> GetEvents(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Event>>(() =>
            {
                OperationResult<Event> result = new OperationResult<Event>();
                try
                {
                    if (IsInCompany())
                    {
                        result.Count = EventsRepository.Count("CompanyId = @CompanyId", new { CompanyId = CurrentUser.CompanyId.Value });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = EventsRepository.Search("CompanyId = @CompanyId",
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
        public async System.Threading.Tasks.Task<OperationResult<Event>> ReadEvent(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Event>>(() =>
            {
                OperationResult<Event> result = new OperationResult<Event>();
                try
                {
                    Event ev = EventsRepository.Read(id);
                    if(ev != null)
                    {
                        if (IsInCompany(ev.CompanyId))
                        {
                            result.SingleResult = ev;
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
        public async System.Threading.Tasks.Task<OperationResult<Event>> CreateEvent(Event ev)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Event>>(() =>
            {
                OperationResult<Event> result = new OperationResult<Event>();
                try
                {
                    if (IsInCompany())
                    {
                        ev.CompanyId = CurrentUser.CompanyId.Value;
                        Event newEvent = EventsRepository.CreateOrUpdate(ev);
                        if (newEvent.Id > 0)
                        {
                            result.SingleResult = newEvent;
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
        public async System.Threading.Tasks.Task<OperationResult<Event>> UpdateEvent(Event ev)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Event>>(() =>
            {
                OperationResult<Event> result = new OperationResult<Event>();
                try
                {
                    if (IsInCompany(ev.CompanyId))
                    {
                        result.Result = EventsRepository.Update(ev);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<Event>> DeleteEvent(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Event>>(() =>
            {
                OperationResult<Event> result = new OperationResult<Event>();
                try
                {
                    Event ev = EventsRepository.Read(id);
                    if(ev != null)
                    {
                        if (IsInCompany(ev.CompanyId))
                        {
                            result.Result = EventsRepository.Delete(id);
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