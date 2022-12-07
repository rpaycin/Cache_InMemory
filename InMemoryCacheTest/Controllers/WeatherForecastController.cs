using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace InMemoryCacheTest.Controllers
{
    //https://sefikcankanber.medium.com/asp-net-core-cache-kullan%C4%B1mlar%C4%B1-in-memory-cache-kullan%C4%B1m%C4%B1-34d54d91d3ce
    //startup.cs e services.AddMemoryCache(); ekliyoruz

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        const string CacheKey = "catalogKey";
        IMemoryCache _memoryCache;

        public WeatherForecastController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IEnumerable<Catalog> Get()
        {
            //Catalog listemize 2 adet kategori set ediyoruz
            List<Catalog> catList = null;

            //Burada değerin belirtilen key ile cache'de kontrolünü yapıyoruz
            if (!_memoryCache.TryGetValue(CacheKey, out catList))
            {
                catList = new List<Catalog> { new Catalog { Name = "Diş Macunu", Published = true }, new Catalog { Name = "Parfüm", Published = true } };

                //Burada cache için belirli ayarlamaları yapıyoruz.Cache süresi,önem derecesi gibi
                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                    Priority = CacheItemPriority.Normal
                };

                //Bu satırda belirlediğimiz key'e göre ve ayarladığımız cache özelliklerine göre kategorilerimizi in-memory olarak cache'liyoruz.
                _memoryCache.Set(CacheKey, catList, cacheExpOptions);
            }

            return catList;
        }

        [HttpGet("DeleteCache")]
        public void DeleteCache()
        {
            //Remove ile verilen key'e göre bulunan veriyi siliyoruz
            _memoryCache.Remove(CacheKey);
        }
    }

    public class Catalog
    {
        public string Name { get; set; }
        public bool Published { get; set; }
    }
}
