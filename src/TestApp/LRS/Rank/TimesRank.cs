using System;
using System.Collections.Generic;

namespace LRS.Rank
{ 
    public class TimeDataRank : BaseRank<BaseRankData>
    { 
        public TimeDataRank(List<MatchData> datas)
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

                    m_dataDic[data.Player].CompareValue  += 1;
                    m_dataDic[data.Player].allTimes  += 1;
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