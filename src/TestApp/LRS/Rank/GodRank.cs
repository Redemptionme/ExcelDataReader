using System;
using System.Collections.Generic;
using LRS;

namespace LRS.Rank
{ 
    public class GodRank : BaseRank<BaseRankData>
    { 
        public GodRank()
        {
            
        }

        public override void Init(List<MatchData> datas, CheckPlayerFunc checkFunc = null)
        {
            base.Init(datas, checkFunc);
            foreach (var matchdata in datas)
            {
                if (!m_dataDic.TryGetValue(matchdata.Info.God,out var timeRankData))
                {
                    var newData = new BaseRankData();
                    newData.Player = matchdata.Info.God;
                    m_dataDic[newData.Player] = newData;
                }

                m_dataDic[matchdata.Info.God].CompareValue  += 1;
                m_dataDic[matchdata.Info.God].allTimes  += 1;
            }

            
            foreach (var item in m_dataDic)
            {
                m_dataList.Add(item.Value);
            }
            
            Sort();
        }
    }
}