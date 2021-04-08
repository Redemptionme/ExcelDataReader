using System;
using System.Collections.Generic;

namespace LRS
{
    public class GoodCampRankData : BaseRankData,IComparable<GoodCampRankData>
    {
        public int Score = 0;
        
        
        //ComparetTo:大于 1； 等于 0； 小于 -1；
        public int CompareTo(GoodCampRankData p)
        {
            return Score.CompareTo(p.Score);
        }
    } 
    public class GoodCampRank : BaseRank<GoodCampRankData>
    { 
        public GoodCampRank(List<MatchData> datas)
        {
            foreach (var matchdata in datas)
            {
                foreach (var data in matchdata.playerDatas)
                {
                    bool bIn = data.Card > EGameCard.BadCardEnd && data.Card < EGameCard.GoodCardEnd; 
                    if (!bIn) continue;
                    
                    if (!m_dataDic.TryGetValue(data.Player,out GoodCampRankData scoreData))
                    {
                        var newData = new GoodCampRankData();
                        newData.Player = data.Player;
                        m_dataDic[newData.Player] = newData;
                    }

                    m_dataDic[data.Player].Score  += data.TotalScore;
                }
            }

            foreach (var item in m_dataDic)
            {
                m_dataList.Add(item.Value);
            }
            
            // 降序
            m_dataList.Sort((x, y) => -x.CompareTo(y));
        }
 
    }
}