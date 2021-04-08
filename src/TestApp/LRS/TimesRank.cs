using System;
using System.Collections.Generic;

namespace LRS
{
    public class TimeRankData : BaseRankData,IComparable<TimeRankData>
    {
        public int Times = 0;
        
        
        //ComparetTo:大于 1； 等于 0； 小于 -1；
        public int CompareTo(TimeRankData p)
        {
            return Times.CompareTo(p.Times);
        }
    } 
    public class TimeRank : BaseRank<TimeRankData>
    { 
        public TimeRank(List<MatchData> datas)
        {
            foreach (var matchdata in datas)
            {
                foreach (var data in matchdata.playerDatas)
                {
                    if (!m_dataDic.TryGetValue(data.Player,out TimeRankData scoreData))
                    {
                        var newData = new TimeRankData();
                        newData.Player = data.Player;
                        m_dataDic[newData.Player] = newData;
                    }

                    m_dataDic[data.Player].Times  += 1;
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