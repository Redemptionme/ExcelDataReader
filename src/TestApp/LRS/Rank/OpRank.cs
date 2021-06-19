using System;
using System.Collections.Generic;

namespace LRS.Rank
{
    public class OpRank : BaseRank<BaseRankData>
    {
        private CheckPlayerFunc m_checkFunc;
        
        public OpRank()
        {
            
        }

        public override void Init(List<MatchData> datas, CheckPlayerFunc checkFunc = null,bool bSortTimes = true)
        {
            base.Init(datas, checkFunc,bSortTimes);
            m_checkFunc = checkFunc;
            foreach (var matchdata in datas)
            {
                foreach (var data in matchdata.playerDatas)
                {
                    if (checkFunc != null &&  !m_checkFunc(data) )
                    {
                        continue;
                    }
                    
                    if (!m_dataDic.TryGetValue(data.Player,out var scoreData))
                    {
                        var newData = new BaseRankData();
                        newData.Player = data.Player;
                        m_dataDic[newData.Player] = newData;
                    }

                    m_dataDic[data.Player].CompareValue  += data.OpScore;
                    m_dataDic[data.Player].Count  += 1;
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