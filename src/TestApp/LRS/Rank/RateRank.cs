using System.Collections.Generic;

namespace LRS.Rank
{
    public class RateData : BaseRankData
    {
        public int winTimes;
    }
    
    public class RateRank : BaseRank<RateData>
    {
        private CheckPlayerFunc m_checkFunc;
        
        public RateRank(List<MatchData> datas,CheckPlayerFunc checkFunc = null)
        {
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
                        var newData = new RateData();
                        newData.Player = data.Player;
                        m_dataDic[newData.Player] = newData;
                    }

                    m_dataDic[data.Player].allTimes  += 1;
                    m_dataDic[data.Player].winTimes  += data.WinScore > 0 ? 1 : 0;
                    if (m_dataDic[data.Player].winTimes != 0)
                    {
                        m_dataDic[data.Player].CompareValue =
                            m_dataDic[data.Player].winTimes * 10000 / m_dataDic[data.Player].allTimes;    
                    }
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