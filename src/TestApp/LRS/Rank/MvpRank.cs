using System;
using System.Collections.Generic;

namespace LRS.Rank
{ 
    public class MvpRank : BaseRank<BaseRankData>
    { 
        public MvpRank(List<MatchData> datas)
        {
            foreach (var matchdata in datas)
            {
                foreach (var data in matchdata.playerDatas)
                {
                    if (!m_dataDic.TryGetValue(data.Player,out var scoreData))
                    {
                        var newData = new BaseRankData();
                        newData.Player = data.Player;
                        m_dataDic[newData.Player] = newData;
                    }

                    m_dataDic[data.Player].CompareValue  += data.MvpScore;
                }
            }
            foreach (var item in m_dataDic)
            {
                m_dataList.Add(item.Value);
            }
            
            Sort();
        }
 
    }
}