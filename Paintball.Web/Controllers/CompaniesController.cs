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
    public class CompaniesController : Controller
    {
        IRepository<Company, int> _companiesRepository;
        IRepository<GameType, int> _gameTypesRepository;
        IRepository<Playground, int> _playgroundsRepository;
        IRepository<News, int> _newsRepository;
        ILoggingService _loggingService;
        public CompaniesController(
            IRepository<Company, int> companiesRepository,
            IRepository<GameType, int> gameTypesRepository,
            IRepository<Playground, int> playgroundsRepository,
            IRepository<News, int> newsRepository,
            ILoggingService loggingService
            )
        {
            _loggingService = loggingService;
            _companiesRepository = companiesRepository;
            _gameTypesRepository = gameTypesRepository;
            _playgroundsRepository = playgroundsRepository;
            _newsRepository = newsRepository;
        }
        // GET: Companies
        [Route("companies", Name = CompaniesControllerRoute.GetIndex)]
        public async System.Threading.Tasks.Task<ActionResult> Index(int? pageNumber, int? pageSize, bool? descending)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<ActionResult>(() =>
            {
                try
                {
                    int _pageNumber = 1;
                    int _pageSize = 30;
                    bool _descending = true;

                    if (pageNumber.HasValue)
                        _pageNumber = pageNumber.Value;

                    if (pageSize.HasValue)
                        _pageSize = pageSize.Value;

                    if (descending.HasValue)
                        _descending = descending.Value;

                    int companiesCount = _companiesRepository.Count();

                    CompaniesGetAllViewModel vm = new CompaniesGetAllViewModel
                    {
                        CurrentPage = _pageNumber,
                        PageCount = (int)Math.Ceiling((double)(companiesCount / _pageSize)),
                        CompaniesCount = companiesCount,
                        Companies = _companiesRepository.GetAll(_pageSize, _pageNumber, _descending),
                        TopNews = _newsRepository.GetAll(5, 1, true)
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

        [Route("companies/{id}", Name = CompaniesControllerRoute.GetRead)]
        public async System.Threading.Tasks.Task<ActionResult> Read(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<ActionResult>(() =>
            {
                try
                {
                    Company company = _companiesRepository.Read(id);
                    if (company != null)
                    {
                        CompanyReadViewModel vm = new CompanyReadViewModel
                        {
                            Company = _companiesRepository.Read(id),
                            GameTypes = _gameTypesRepository.Search("CompanyId = @CompanyId",
                            new
                            {
                                PageSize = 1000,
                                PageNumber = 1,
                                CompanyId = id
                            }),
                            Playgrounds = _playgroundsRepository.Search("CompanyId = @CompanyId",
                            new
                            {
                                PageSize = 1000,
                                PageNumber = 1,
                                CompanyId = id
                            }),
                            TopNews = _newsRepository.GetAll(5, 1, true)
                        };
                        if (vm.Company != null)
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