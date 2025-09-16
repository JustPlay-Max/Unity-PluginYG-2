using System;

namespace YG
{
    [Serializable]
    public class BonusData
    {
        public string description;
        public string id;
        public string imageURI;
        public string title;
    }

    [Serializable]
    public class PlayerBonusData : BonusData
    {
        public long receivedAt;
        public string token;
    }

    [Serializable]
    public class ConsumptionResponse
    {
        public string token;
        public string signature;
        public bool isSignedResponse;
    }

    [Serializable]
    public class PlayerBonusesResponse
    {
        public PlayerBonusData[] bonuses;
        public string signature;
        public bool isSignedResponse;
    }

    [Serializable]
    public class BonusCatalogResponse
    {
        public BonusData[] bonuses;
    }

    [Serializable]
    public class BonusCatalogWrapper
    {
        public BonusData[] bonuses;
    }

    [Serializable]
    public class PlayerBonusesWrapper
    {
        public PlayerBonusData[] bonuses;
    }
}