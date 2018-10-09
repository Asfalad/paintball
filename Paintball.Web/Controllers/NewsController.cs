using Paintball.DAL.Entities;
using Paintball.DAL.Repositories;
using Paintball.Web.Constants;
using Paintball.Web.Model;
using Paintball.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Paintball.Web.Controllers
{
    public class NewsController : Controller
    {
        private IRepository<News, int> _newsRepository;
        private IRepository<Event, int> _eventsRepository;
        private ILoggingService _loggingService;
        public NewsController(IRepository<News, int> newsRepository, 
            IRepository<Event, int> eventsRepository,
            ILoggingService loggingService)
        {
            _eventsRepository = eventsRepository;
            _newsRepository = newsRepository;
            _loggingService = loggingService;
        }
        // GET: News
        [Route("news", Name = NewsControllerRoute.GetIndex)]
        public async System.Threading.Tasks.Task<ActionResult> Index(int? pageNumber, int? pageSize, bool? descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<ActionResult>(() =>
            {
                try
                {
                    int _pageNumber = 1;
                    int _pageSize = 10;
                    bool _descending = true;

                    if (pageNumber.HasValue)
                        _pageNumber = pageNumber.Value;

                    if (pageSize.HasValue)
                        _pageSize = pageSize.Value;

                    if (descending.HasValue)
                        _descending = descending.Value;

                    int newsCount = _newsRepository.Count();

                    NewsGetAllViewModel vm = new NewsGetAllViewModel
                    {
                        PageCount = (int)Math.Ceiling((double)(newsCount / _pageSize)),
                        CurrentPage = _pageNumber,
                        News = _newsRepository.GetAll(_pageSize, _pageNumber, _descending),
                        TopEvents = _eventsRepository.GetAll(5, 1, true)
                    };

                    return View(vm);
                }
                catch (Exception ex)
                {
                    _loggingService.Log(ex);
                    return RedirectToRoute(ErrorControllerRoute.GetNotFound);
                }
            });
        }

        [Route("news/{id}", Name = NewsControllerRoute.GetRead)]
        public async System.Threading.Tasks.Task<ActionResult> Read(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<ActionResult>(() =>
            {
                try
                {
                    News news = _newsRepository.Read(id);
                    if (news != null)
                    {
                        NewsReadViewModel vm = new NewsReadViewModel
                        {
                            CurrentNews = news,
                            TopEvents = _eventsRepository.GetAll(5, 1, true)
                        };
                        if (vm.CurrentNews != null)
                        {
                            return View(vm);
                        }
                        else
                        {
                            return RedirectToRoute(ErrorControllerRoute.GetNotFound);
                        }
                    }
                    else
                    {
                        return RedirectToRoute(ErrorControllerRoute.GetNotFound);
                    }
                }
                catch (Exception ex)
                {
                    _loggingService.Log(ex);
                    return RedirectToRoute(ErrorControllerRoute.GetNotFound);
                }
            });
        }
    }
}