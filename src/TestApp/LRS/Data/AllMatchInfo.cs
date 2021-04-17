using System.Collections.Generic;

namespace LRS
{
    public class AllMatchInfo
    {
        public int TotalCount;
        public int GoodCampWinCount;
        
        public void SetData(List<MatchData> matchDatas)
        {
            TotalCount = 0;
            GoodCampWinCount = 0;
            foreach (var matchData in matchDatas)
            {
                TotalCount += 1;
                if (LrsHelper.IsGoodCard(matchData.playerDatas[0].Card))
                {
                    if (matchData.playerDatas[0].WinScore > 0)
                    {
                        GoodCampWinCount += 1;
                    }
                }
            }
        }
    }
}