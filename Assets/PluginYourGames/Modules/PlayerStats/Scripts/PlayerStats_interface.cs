using System.Collections.Generic;
using YG.Utils;

namespace YG
{
    public partial interface IPlatformsYG2
    {
        private const string KEY_STATES = "PlayerStatsYG";

        void InitStats() => YG2.ReceiveStatsJson(LocalStorage.GetKey(KEY_STATES));

        void LoadStats() => YG2.ReceiveStatsJson(LocalStorage.GetKey(KEY_STATES));

        void SetStats(Dictionary<string, int> stats) => LocalStorage.SetKey(KEY_STATES, JsonYG.SerializeDictionary(YG2.GetAllStats()));

        // Platforms without a stats service use local storage, as in the existing overload.
        void SetStats(Dictionary<string, int> stats, System.Action<bool> completed)
            => completed(LocalStorage.TrySetKey(KEY_STATES, JsonYG.SerializeDictionary(stats)));

        void IncrementStat(string key, int value, System.Action<bool, int> completed)
        {
            var snapshot = new Dictionary<string, int>(YG2.GetAllStats());
            snapshot.TryGetValue(key, out int current);
            int result = checked(current + value);
            snapshot[key] = result;
            completed(LocalStorage.TrySetKey(KEY_STATES, JsonYG.SerializeDictionary(snapshot)), result);
        }
    }
}
