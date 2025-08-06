namespace YG
{
    public partial interface IPlatformsYG2
    {
        void SetLeaderboard(string nameLB, int score, string extraData) { }
        void GetLeaderboard(string nameLB, int quantityTop, int quantityAround, string photoSizeLB) { }
    }
}