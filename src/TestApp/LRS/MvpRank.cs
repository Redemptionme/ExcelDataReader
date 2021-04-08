using System;
using System.Collections.Generic;

namespace LRS
{
    public class MvpRankData : BaseRankData,IComparable<MvpRankData>
    {
        public int Times = 0;
        
        
        //ComparetTo:大于 1； 等于 0； 小于 -1；
        public int CompareTo(MvpRankData p)
        {
            return Times.CompareTo(p.Times);
        }
    } 
    public class MvpRank : BaseRank<MvpRankData>
    { 
        public MvpRank(List<MatchData> datas)
        {
            foreach (var matchdata in datas)
            {
                foreach (var data in matchdata.playerDatas)
                {
                    if (!m_dataDic.TryGetValue(data.Player,out MvpRankData scoreData))
                    {
                        var newData = new MvpRankData();
                        newData.Player = data.Player;
                        m_dataDic[newData.Player] = newData;
                    }

                    m_dataDic[data.Player].Times  += data.MvpScore;
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