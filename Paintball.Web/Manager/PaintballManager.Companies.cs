using Paintball.DAL.Entities;
using Paintball.Web.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Paintball.Web.Manager
{
    public partial class PaintballManager : IManager
    {
        public async System.Threading.Tasks.Task<OperationResult<Company>> GetCompanies(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Company>>(() =>
            {
                OperationResult<Company> result = new OperationResult<Company>();
                try
                {
                    result.Count = CompaniesRepository.Count();
                    if (result.Count > 0)
                    {
                        result.MultipleResult = CompaniesRepository.GetAll(pageSize, pageNumber, descending);
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
        public async System.Threading.Tasks.Task<OperationResult<Company>> ReadCompany(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Company>>(() =>
            {
                OperationResult<Company> result = new OperationResult<Company>();
                try
                {
                    Company company = CompaniesRepository.Read(id);
                    if (company != null)
                    {
                        result.SingleResult = company;
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

        public async System.Threading.Tasks.Task<OperationResult<Company>> CreateCompany(Company company)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Company>>(() =>
            {
                OperationResult<Company> result = new OperationResult<Company>();
                try
                {
                    company.OwnerId = CurrentUser.Id;
                    Company created = CompaniesRepository.CreateOrUpdate(company);
                    if (created.Id > 0)
                    {
                        UserStore.AddToRoleAsync(CurrentUser, RoleNames.CompanyOwner);
                        result.SingleResult = created;
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

        public async System.Threading.Tasks.Task<OperationResult<Company>> UpdateCompany(Company company)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Company>>(() =>
            {
                OperationResult<Company> result = new OperationResult<Company>();
                try
                {
                    if (IsInCompany(company.Id))
                    {
                        result.Result = CompaniesRepository.Update(company);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<Company>> DeleteCompany(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Company>>(() =>
            {
                OperationResult<Company> result = new OperationResult<Company>();
                try
                {
                    if (IsInCompany(id))
                    {
                        Company company = CompaniesRepository.Read(id);
                        if (company != null)
                        {
                            if (company.OwnerId == CurrentUser.Id)
                            {
                                result.Result = CompaniesRepository.Delete(id);
                                if (result.Result)
                                {
                                    UserStore.RemoveFromRoleAsync(CurrentUser, RoleNames.CompanyOwner);
                                }
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