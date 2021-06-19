using System;
using System.Collections.Generic;

namespace LRS.Rank
{ 
    public class MvpRank : BaseRank<BaseRankData>
    { 
        public MvpRank()
        {
            
        }

        public override void Init(List<MatchData> datas, CheckPlayerFunc checkFunc = null,bool bSortTimes = true)
        {
            base.Init(datas, checkFunc,bSortTimes);
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
                    m_dataDic[data.Player].allTimes  += 1;
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