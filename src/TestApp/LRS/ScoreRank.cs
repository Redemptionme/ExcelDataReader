using System.Collections.Generic;

namespace LRS
{
    public class ScoreRank : Rank
    {
        public Dictionary<PlayerInfo, int> m_data = new Dictionary<PlayerInfo, int>();

        public ScoreRank(List<MatchData> datas)
        {
            foreach (var matchdata in datas)
            {
                foreach (var data in matchdata.playerDatas)
                {
                    if (!m_data.TryGetValue(data.Player,out int curScore))
                    {
                        m_data[data.Player] = 0;
                    }

                    m_data[data.Player] += data.TotalScore;
                }
            }
            
        }


    }
}