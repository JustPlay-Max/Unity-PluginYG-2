using System;

namespace YG
{
    public partial interface IPlatformsYG2
    {
        public long ServerTime()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}