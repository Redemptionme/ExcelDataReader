using System.Collections.Generic;

namespace LRS.Rank
{
    public class RateRankData : BaseRankData
    {
        public RateData RateData = new RateData();
    }
    
    public class WinRateRank : BaseRank<RateRankData>
    {
        private CheckPlayerFunc m_checkFunc;
        
        public WinRateRank()
        {
            
        }

        public override void Init(List<MatchData> datas, CheckPlayerFunc checkFunc = null)
        {
            base.Init(datas, checkFunc);
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
                        var newData = new RateRankData();
                        newData.Player = data.Player;
                        m_dataDic[newData.Player] = newData;
                    }
                    
                    m_dataDic[data.Player].RateData.WinValue  += data.WinScore > 0 ? 1 : 0;
                    m_dataDic[data.Player].RateData.AllValue  += 1;
                    m_dataDic[data.Player].RateData.CalRate();
                    m_dataDic[data.Player].allTimes  += 1;
                    m_dataDic[data.Player].CompareValue  = (int)(m_dataDic[data.Player].RateData.Rate * 10000);
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