using System.Formats.Asn1;
using URLShortener.Repository;

namespace URLShortener
{
    public class GlobalCounter : IGlobalCounter
    {
        private int counter = 1;

        public void SetCounter(int latestId)
        {
            this.counter = latestId + 1;
        }

        public void Increment()
        {
            this.counter = counter + 1;
        }

        public int GetCurrentCount()
        {
            return this.counter;
        }
    }
}
