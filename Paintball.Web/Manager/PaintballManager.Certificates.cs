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
        public async System.Threading.Tasks.Task<OperationResult<Certificate>> GetCertificates(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Certificate>>(() =>
            {
                OperationResult<Certificate> result = new OperationResult<Certificate>();
                try
                {
                    if(IsInCompany())
                    {
                        result.Count = CertificatesRepository.Count("CompanyId = @CompanyId", new { CompanyId = CurrentUser.CompanyId.Value });
                        if(result.Count > 0)
                        {
                            result.MultipleResult = CertificatesRepository.Search("CompanyId = @CompanyId",
                            new { PageSize = pageSize, PageNumber = pageNumber, CompanyId = CurrentUser.CompanyId.Value }, descending);
                        }
                        result.Result = true;
                    }
                }
                catch(Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }

        public async System.Threading.Tasks.Task<OperationResult<Certificate>> ReadCertificate(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Certificate>>(() =>
            {
                OperationResult<Certificate> result = new OperationResult<Certificate>();
                try
                {
                    Certificate certificate = CertificatesRepository.Read(id);
                    if(certificate != null)
                    {
                        if (IsInCompany(certificate.CompanyId))
                        {
                            result.SingleResult = certificate;
                            result.Result = true;
                        }
                    }
                }
                catch(Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }

        public async System.Threading.Tasks.Task<OperationResult<Certificate>> CreateCertificate(Certificate certificate)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Certificate>>(() =>
            {
                OperationResult<Certificate> result = new OperationResult<Certificate>();
                try
                {
                    if (IsInCompany())
                    {
                        certificate.CompanyId = CurrentUser.CompanyId.Value;
                        Certificate created = CertificatesRepository.CreateOrUpdate(certificate);
                        if (created.Id > 0)
                        {
                            result.SingleResult = created;
                            result.Result = true;
                        }
                    }
                }
                catch(Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }

        public async System.Threading.Tasks.Task<OperationResult<Certificate>> UpdateCertificate(Certificate certificate)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Certificate>>(() =>
            {
                OperationResult<Certificate> result = new OperationResult<Certificate>();
                try
                {
                    if (IsInCompany(certificate.CompanyId))
                    {
                        result.Result = CertificatesRepository.Update(certificate);
                    }
                }
                catch(Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }

        public async System.Threading.Tasks.Task<OperationResult<Certificate>> DeleteCertificate(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<Certificate>>(() =>
            {
                OperationResult<Certificate> result = new OperationResult<Certificate>();
                try
                {
                    Certificate certificate = CertificatesRepository.Read(id);
                    if (certificate != null)
                    {
                        if (IsInCompany(certificate.CompanyId))
                        {
                            result.Result = CertificatesRepository.Delete(id);
                        }
                    }
                }
                catch(Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
    }
}