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
    public class EventsController : Controller
    {
        private IRepository<News, int> _newsRepository;
        private IRepository<Event, int> _eventsRepository;
        private IRepository<Game, int> _gamesRepository;
        private IRepository<Company, int> _companiesRepository;
        private ILoggingService _loggingService;

        public EventsController(IRepository<News, int> newsRepository,
            IRepository<Event, int> eventsRepository,
            IRepository<Game, int> gamesRepository,
            IRepository<Company, int> companiesRepository,
            ILoggingService loggingService)
        {
            _newsRepository = newsRepository;
            _eventsRepository = eventsRepository;
            _gamesRepository = gamesRepository;
            _companiesRepository = companiesRepository;

            _loggingService = loggingService;
        }

        // GET: Events
        [Route("events", Name = EventsControllerRoute.GetIndex)]
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

                    int eventsCount = _eventsRepository.Count();

                    EventsGetAllViewModel vm = new EventsGetAllViewModel
                    {
                        PageCount = (int)Math.Ceiling((double)(eventsCount / _pageSize)),
                        CurrentPage = _pageNumber,
                        Events = new List<EventModel>(),
                        TopNews = _newsRepository.GetAll(5, 1, true)
                    };

                    IEnumerable<Event> events = _eventsRepository.GetAll(_pageSize, _pageNumber, _descending);
                    foreach (var e in events)
                    {
                        EventModel em = new EventModel();
                        em.Event = e;
                        em.Game = _gamesRepository.Read(e.GameId);
                        em.Company = _companiesRepository.Read(em.Game.CompanyId);
                        vm.Events.Add(em);
                    }
                    return View(vm);
                }
                catch (Exception ex)
                {
                    _loggingService.Log(ex);
                    return RedirectToRoute(ErrorControllerRoute.GetNotFound);
                }
            });
        }

        [Route("events/{id}", Name = EventsControllerRoute.GetRead)]
        public async System.Threading.Tasks.Task<ActionResult> Read(int id)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<ActionResult>(() =>
            {
                try
                {

                    Event ev = _eventsRepository.Read(id);
                    if (ev != null)
                    {
                        Game game = _gamesRepository.Read(ev.GameId);
                        if (game != null)
                        {
                            EventsReadViewModel vm = new EventsReadViewModel
                            {
                                CurrentEvent = new EventModel
                                {
                                    Event = ev,
                                    Game = _gamesRepository.Read(ev.GameId),
                                    Company = _companiesRepository.Read(game.CompanyId)
                                },
                                TopNews = _newsRepository.GetAll(5, 1, true)
                            };
                            return View(vm);
                        }
                    }
                    return RedirectToRoute(ErrorControllerRoute.GetNotFound);
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