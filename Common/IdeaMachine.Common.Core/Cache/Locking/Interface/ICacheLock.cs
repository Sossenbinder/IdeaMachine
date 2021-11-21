using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Cache.Locking.Interface
{
    /// <summary>
    /// Represents a cache lock. This might be a semaphore, or a cache dependent thing like a distributed lock
    /// </summary>
    public interface ICacheLock
    {
	    Task Lock();

	    ValueTask Release();
    }
}
