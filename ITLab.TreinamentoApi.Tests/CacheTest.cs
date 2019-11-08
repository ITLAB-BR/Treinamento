using ITLab.Treinamento.Api.Core.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace ITLab.Treinamento.Api.Tests

{
    [TestClass]
    public class CacheTest
    {
        private CacheWrapper cacheWrapper = new CacheWrapper();
        private int cacheTimeSeconds = 3;

        private string cacheCallbackOnRemoveAuxiliar;

        [TestCategory("CacheWrapper"), Priority(0), TestMethod]
        public void PutInCache()
        {
            string cacheKeyName = "PutInCache";
            string cacheContentTest = "PutInCacheContent";

            cacheWrapper.AddToMyCache(cacheKeyName, cacheContentTest, CachePriority.Default, cacheTimeSeconds);
            Assert.AreEqual(CacheWrapper.GetCachedItem(cacheKeyName), cacheContentTest);
        }

        [TestCategory("CacheWrapper"), Priority(1), TestMethod]
        public void CacheExpiration()
        {
            string cacheKeyName = "CacheExpiration";
            string cacheContentTest = "CacheExpirationContent";

            cacheWrapper.AddToMyCache(cacheKeyName, cacheContentTest, CachePriority.Default, cacheTimeSeconds);

            Thread.Sleep(cacheTimeSeconds * 1000);

            Assert.IsNull(CacheWrapper.GetCachedItem(cacheKeyName));
        }

        [TestCategory("CacheWrapper"), Priority(2), TestMethod]
        public void RemoveCache()
        {
            string cacheKeyName = "RemoveCache";
            string cacheContentTest = "RemoveCacheContent";

            cacheWrapper.AddToMyCache(cacheKeyName, cacheContentTest, CachePriority.Default, cacheTimeSeconds, cacheCallbackOnRemove);
            CacheWrapper.RemoveCachedItem(cacheKeyName);

            Assert.AreEqual(cacheCallbackOnRemoveAuxiliar, "TESTEOK");
        }

        [TestCategory("CacheWrapper"), Priority(3), TestMethod]
        public void CacheCallBack()
        {
            string cacheKeyName = "CacheCallBack";
            string cacheContentTest = "CacheCallBackContent";

            cacheWrapper.AddToMyCache(cacheKeyName, cacheContentTest, CachePriority.Default, cacheTimeSeconds);
            CacheWrapper.RemoveCachedItem(cacheKeyName);

            Assert.IsNull(CacheWrapper.GetCachedItem(cacheKeyName));
        }

        private void cacheCallbackOnRemove()
        {
            cacheCallbackOnRemoveAuxiliar = "TESTEOK";
        }
    }
}
