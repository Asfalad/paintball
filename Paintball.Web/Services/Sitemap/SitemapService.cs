namespace Paintball.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Boilerplate.Web.Mvc;
    using Boilerplate.Web.Mvc.Sitemap;
    using Paintball.Web.Constants;
    using DAL.Repositories;
    using DAL.Entities;

    /// <summary>
    /// Generates sitemap XML for the current site.
    /// </summary>
    public class SitemapService : SitemapGenerator, ISitemapService
    {
        #region Fields
        
        private readonly ICacheService cacheService;
        private readonly ILoggingService loggingService;
        private readonly UrlHelper urlHelper;
        private readonly IRepository<News, int> _newsRepository;
        private readonly IRepository<Event, int> _eventsRepository;
        private readonly IRepository<Company, int> _companiesRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapService" /> class.
        /// </summary>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <param name="urlHelper">The URL helper.</param>
        public SitemapService(
            ICacheService cacheService,
            ILoggingService loggingService,
            UrlHelper urlHelper,
            IRepository<News, int> newsRepository,
            IRepository<Event, int> eventsRepository,
            IRepository<Company, int> companiesRepository)
        {
            this.cacheService = cacheService;
            this.loggingService = loggingService;
            this.urlHelper = urlHelper;

            this._newsRepository = newsRepository;
            this._eventsRepository = eventsRepository;
            this._companiesRepository = companiesRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the sitemap XML for the current site. If an index of null is passed and there are more than 25,000 
        /// sitemap nodes, a sitemap index file is returned (A sitemap index file contains links to other sitemap files 
        /// and is a way of splitting up your sitemap into separate files). If an index is specified, a standard 
        /// sitemap is returned for the specified index parameter. See http://www.sitemaps.org/protocol.html
        /// </summary>
        /// <param name="index">The index of the sitemap to retrieve. <c>null</c> if you want to retrieve the root 
        /// sitemap or sitemap index document, depending on the number of sitemap nodes.</param>
        /// <returns>The sitemap XML for the current site or <c>null</c> if the sitemap index is out of range.</returns>
        public string GetSitemapXml(int? index = null)
        {
            // Here we are caching the entire set of sitemap documents. We cannot use OutputCacheAttribute because 
            // cache expiry could get out of sync if the number of sitemaps changes.
            List<string> sitemapDocuments = this.cacheService.GetOrAdd(
                CacheSetting.SitemapNodes.Key,
                () =>
                {
                    IReadOnlyCollection<SitemapNode> sitemapNodes = this.GetSitemapNodes();
                    return this.GetSitemapDocuments(sitemapNodes);
                },
                CacheSetting.SitemapNodes.SlidingExpiration);

            if (index.HasValue && ((index < 1) || (index.Value >= sitemapDocuments.Count)))
            {
                return null;
            }

            return sitemapDocuments[index.HasValue ? index.Value : 0];
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a collection of sitemap nodes for the current site.
        /// TODO: Add code here to create nodes to all your important sitemap URL's.
        /// You may want to do this from a database or in code.
        /// </summary>
        /// <returns>A collection of sitemap nodes for the current site.</returns>
        protected virtual IReadOnlyCollection<SitemapNode> GetSitemapNodes()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();

            nodes.Add(
                new SitemapNode(this.urlHelper.AbsoluteRouteUrl(HomeControllerRoute.GetIndex))
                {
                    Priority = 1
                });
            nodes.Add(
               new SitemapNode(this.urlHelper.AbsoluteRouteUrl(HomeControllerRoute.GetAbout))
               {
                   Priority = 0.9
               });
            nodes.Add(
               new SitemapNode(this.urlHelper.AbsoluteRouteUrl(EventsControllerRoute.GetIndex))
               {
                   Priority = 0.8
               });
            nodes.Add(
               new SitemapNode(this.urlHelper.AbsoluteRouteUrl(NewsControllerRoute.GetIndex))
               {
                   Priority = 0.8
               });
            nodes.Add(
               new SitemapNode(this.urlHelper.AbsoluteRouteUrl(CompaniesControllerRoute.GetIndex))
               {
                   Priority = 0.8
               });

            foreach(int newsId in _newsRepository.GetAllIds())
            {
                nodes.Add(new SitemapNode(this.urlHelper.AbsoluteRouteUrl(NewsControllerRoute.GetRead, new { id = newsId }))
                {
                    Frequency = SitemapFrequency.Daily,
                    LastModified = DateTime.Now,
                    Priority = 0.8
                });
            }

            foreach (int eventId in _eventsRepository.GetAllIds())
            {
                nodes.Add(new SitemapNode(this.urlHelper.AbsoluteRouteUrl(EventsControllerRoute.GetRead, new { id = eventId }))
                {
                    Frequency = SitemapFrequency.Daily,
                    LastModified = DateTime.Now,
                    Priority = 0.8
                });
            }

            foreach (int companyId in _companiesRepository.GetAllIds())
            {
                nodes.Add(new SitemapNode(this.urlHelper.AbsoluteRouteUrl(CompaniesControllerRoute.GetRead, new { id = companyId }))
                {
                    Frequency = SitemapFrequency.Daily,
                    LastModified = DateTime.Now,
                    Priority = 0.8
                });
            }

            // An example of how to add many pages into your sitemap.
            // foreach (int productId in myProductRepository.GetProductIds())
            // {
            //     nodes.Add(
            //        new SitemapNode(this.urlHelper.AbsoluteRouteUrl(ProductControllerRoute.GetProduct, new { id = productId }))
            //        {
            //            Frequency = SitemapFrequency.Weekly,
            //            LastModified = DateTime.Now,
            //            Priority = 0.8
            //        });
            // }

            return nodes;
        }

        protected override string GetSitemapUrl(int index)
        {
            return this.urlHelper.AbsoluteRouteUrl(HomeControllerRoute.GetSitemapXml).TrimEnd('/') + "?index=" + index;
        }

        protected override void LogWarning(Exception exception)
        {
            this.loggingService.Log(exception);
        }

        #endregion
    }
}