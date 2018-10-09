using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Paintball.Web.Manager
{
    public partial class PaintballManager : IManager
    {
        public async System.Threading.Tasks.Task<OperationResult<Equipment>> GetEquipment(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Equipment>>(() =>
            {
                OperationResult<Equipment> result = new OperationResult<Equipment>();
                try
                {
                    if (IsInCompany())
                    {
                        result.Count = EquipmentsRepository.Count("CompanyId = @CompanyId", new { CompanyId = CurrentUser.CompanyId });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = EquipmentsRepository.Search("CompanyId = @CompanyId",
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
        public async System.Threading.Tasks.Task<OperationResult<Equipment>> ReadEquipment(int Id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Equipment>>(() =>
            {
                OperationResult<Equipment> result = new OperationResult<Equipment>();
                try
                {
                    Equipment equipment = EquipmentsRepository.Read(Id);
                    if(equipment != null)
                    {
                        if (IsInCompany(equipment.CompanyId))
                        {
                            result.SingleResult = equipment;
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
        public async System.Threading.Tasks.Task<OperationResult<Equipment>> CreateEquipment(Equipment equipment)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Equipment>>(() =>
            {
                OperationResult<Equipment> result = new OperationResult<Equipment>();
                try
                {
                    if (IsInCompany())
                    {
                        equipment.CompanyId = CurrentUser.CompanyId.Value;
                        Equipment created = EquipmentsRepository.CreateOrUpdate(equipment);
                        if (created.Id > 0)
                        {
                            result.SingleResult = created;
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
        public async System.Threading.Tasks.Task<OperationResult<Equipment>> UpdateEquipment(Equipment equipment)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Equipment>>(() =>
            {
                OperationResult<Equipment> result = new OperationResult<Equipment>();
                try
                {
                    if (IsInCompany(equipment.CompanyId))
                    {
                        result.Result = EquipmentsRepository.Update(equipment);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<Equipment>> DeleteEquipment(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Equipment>>(() =>
            {
                OperationResult<Equipment> result = new OperationResult<Equipment>();
                try
                {
                    Equipment equipment = EquipmentsRepository.Read(id);
                    if(equipment != null)
                    {
                        if (IsInCompany(equipment.CompanyId))
                        {
                            result.Result = EquipmentsRepository.Delete(id);
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