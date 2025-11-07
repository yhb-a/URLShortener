using System.Diagnostics.Metrics;
using URLShortener.Repository;

namespace URLShortener
{
    public interface IGlobalCounter
    {
        void SetCounter(int latestId);
        void Increment();
        int GetCurrentCount();
    }
}
