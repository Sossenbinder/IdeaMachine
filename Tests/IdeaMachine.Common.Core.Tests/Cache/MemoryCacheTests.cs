using System.Collections.Generic;
using IdeaMachine.Common.Core.Cache.Implementations;
using NUnit.Framework;

namespace IdeaMachine.Common.Core.Tests.Cache
{
    [TestFixture]
    public class MemoryCacheTests
    {
	    private MemoryCache<string, TestItem> _cache = null!;

	    [SetUp]
	    protected void SetUp()
	    {
			_cache = new MemoryCache<string, TestItem>();
	    }

		[Test]
	    public void CacheGetShouldWorkForHappyPath()
	    {
		    const string cacheKey = "Key_1";

		    var originalItem = new TestItem(5);
		    _cache.Set(cacheKey, originalItem);

		    var resolvedItem = _cache.Get(cacheKey);

		    Assert.AreEqual(originalItem, resolvedItem);
		}

	    [Test]
	    public void CacheGetShouldReturnEmptyIfNotSet()
	    {
		    const string cacheKey = "Key_1";
			
		    var cache = new MemoryCache<string, TestItem>();

		    Assert.Throws<KeyNotFoundException>(() => cache.Get(cacheKey));
		}

	    [Test]
	    public void CacheGetShouldWorkWithGetOrAdd()
	    {
		    const string cacheKey = "Key_1";

		    var originalItem = new TestItem(5);
		    var cache = new MemoryCache<string, TestItem>();

		    var resolvedItem = cache.GetOrAdd(cacheKey, _ => originalItem);

		    Assert.AreEqual(originalItem, resolvedItem);
		}

	    [Test]
	    public void CacheGetShouldWorkWithGetOrAddForCreate()
	    {
		    const string cacheKey = "Key_1";

		    var originalItem = new TestItem(5);
		    var cache = new MemoryCache<string, TestItem>();
			cache.Set(cacheKey, originalItem);

		    var resolvedItem = cache.GetOrAdd(cacheKey, _ => new TestItem(9));

		    Assert.AreEqual(originalItem, resolvedItem);
	    }
	}
}
