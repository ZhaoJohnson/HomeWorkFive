using FiveCommonLayer;
using FiveDataLayer.DbModel;
using FiveDataLayer.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveBusinessLayer
{
    public abstract class WarmService<T> : IWarmService
        where T : class
    {
        public WarmService(ShowService _ShowService, Stock _Stock)
        {
            this.DbService = _ShowService;
            this.Stock = _Stock;
        }

        protected Stock Stock;
        protected ShowService DbService;

        public virtual string GetUrlData(string url, string encoding)
        {
            return HttpHelper.DownloadCommodity(url, encoding);
        }

        public abstract T SaveProperty(T t);

        public void Dispose()
        {
        }
    }
}