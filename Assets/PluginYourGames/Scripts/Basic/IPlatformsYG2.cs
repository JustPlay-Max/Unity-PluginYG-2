using UnityEngine;

namespace YG
{
    public partial interface IPlatformsYG2
    {
        void InitAwake()
        {
            if (YG2.infoYG.Basic.syncInitSDK)
                YG2.SyncInitialization();
        }
        void InitStart() { }
        void InitComplete() { }
        void Message(string message) => Debug.Log(message);
    }

    public partial class PlatformYG2 : IPlatformsYG2 { }
    public partial class PlatformYG2NoRealization : IPlatformsYG2 { }
}