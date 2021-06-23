using System.Collections.Generic;

namespace LRS.Rank
{
    public class BeOpRank: BaseRank<BaseRankData>
    {
        public delegate bool CheckGameCardFunc(EGameCard eGameCard);
        
        private CheckGameCardFunc m_checkGameCardFunc;
        
        public BeOpRank()
        {
            
        }

        public void Init(List<MatchData> datas, CheckGameCardFunc checkFunc = null,bool bSortTimes = true)
        {
            base.Init(datas, null,bSortTimes);
            m_checkGameCardFunc = checkFunc;
            foreach (var matchdata in datas)
            {
                foreach (var item in matchdata.playerBeOp)
                {
                    if (checkFunc != null &&  !m_checkGameCardFunc(item.Key) )
                    {
                        continue;
                    }

                    var list = matchdata.playerBeOp[item.Key];
                    if(list.Count == 0) continue;

                    var data = list[0];
                    
                    if (!m_dataDic.TryGetValue(data,out var scoreData))
                    {
                        var newData = new BaseRankData();
                        newData.Player = data;
                        m_dataDic[newData.Player] = newData;
                    }

                    m_dataDic[data].CompareValue  += 1;
                    m_dataDic[data].Count  += 1;
                }
            }

            foreach (var item in m_dataDic)
            {
                m_dataList.Add(item.Value);
            }
            
            Sort();
        }
        
        protected override void Sort()
        {
            // 降序
            m_dataList.Sort((x, y) => -x.CompareTo(y));
        }
        
    }
}