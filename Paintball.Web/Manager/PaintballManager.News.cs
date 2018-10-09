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
        public async System.Threading.Tasks.Task<OperationResult<News>> GetNews(int pageSize, int pageNumber, bool descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<News>>(() =>
            {
                OperationResult<News> result = new OperationResult<News>();
                try
                {
                    if (IsInCompany())
                    {
                        result.Count = NewsRepository.Count("CompanyId = @CompanyId", new { CompanyId = CurrentUser.CompanyId.Value });
                        if (result.Count > 0)
                        {
                            result.MultipleResult = NewsRepository.Search("CompanyId = @CompanyId",
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
        public async System.Threading.Tasks.Task<OperationResult<News>> ReadNews(int Id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<News>>(() =>
            {
                OperationResult<News> result = new OperationResult<News>();
                try
                {
                    News News = NewsRepository.Read(Id);
                    if (News != null)
                    {
                        if (IsInCompany(News.CompanyId))
                        {
                            result.SingleResult = News;
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
        public async System.Threading.Tasks.Task<OperationResult<News>> CreateNews(News news)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<News>>(() =>
            {
                OperationResult<News> result = new OperationResult<News>();
                try
                {
                    if (IsInCompany())
                    {
                        news.CompanyId = CurrentUser.CompanyId.Value;
                        News newNews = NewsRepository.CreateOrUpdate(news);
                        if (newNews.Id > 0)
                        {
                            result.SingleResult = newNews;
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
        public async System.Threading.Tasks.Task<OperationResult<News>> UpdateNews(News news)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<News>>(() =>
            {
                OperationResult<News> result = new OperationResult<News>();
                try
                {
                    if (IsInCompany(news.CompanyId))
                    {
                        result.Result = NewsRepository.Update(news);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Log(ex);
                }
                return result;
            });
        }
        public async System.Threading.Tasks.Task<OperationResult<News>> DeleteNews(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<OperationResult<News>>(() =>
            {
                OperationResult<News> result = new OperationResult<News>();
                try
                {
                    News news = NewsRepository.Read(id);
                    if (news != null)
                    {
                        if (IsInCompany(news.CompanyId))
                        {
                            result.Result = NewsRepository.Delete(id);
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