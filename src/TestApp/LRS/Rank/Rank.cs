using System.Collections.Generic;

namespace LRS.Rank
{
    public delegate bool CheckPlayerFunc(PlayerData playerData);
    
    public class BaseRankData
    {
        public PlayerInfo Player;
        public int CompareValue = 0;
        public int allTimes;
        
        //ComparetTo:大于 1； 等于 0； 小于 -1；
        public int CompareTo(BaseRankData p)
        {
            return CompareValue.CompareTo(p.CompareValue);
        }
    }
    public class BaseRank<T> where T: BaseRankData
    {
        public List<T> m_dataList = new List<T>();
        public Dictionary<PlayerInfo, T> m_dataDic = new Dictionary<PlayerInfo, T>();

        public virtual void Init(List<MatchData> datas, CheckPlayerFunc checkFunc = null)
        {
            m_dataDic.Clear();
            m_dataDic.Clear();
        }

        public string PrintPlayerRank(PlayerInfo player)
        {
            int rankNum = GetPlayerRank(player);
            if (rankNum == 0)
            {
                return "未拿过";  
            }
            else
            {
                T e = GetPlayerData(player);
                return e.CompareValue + "分排" + rankNum + "名   ";
            }
        }
        public int GetPlayerRank(PlayerInfo player)
        {
            for (int i = 0; i < m_dataList.Count; i++)
            {
                if (player == m_dataList[i].Player)
                {
                    return i + 1;
                }
            }

            return 0;
        }
        
        public T GetPlayerData(PlayerInfo player)
        {
            if (m_dataDic.TryGetValue(player,out T value))
            {
                return value;
            }

            return null;
        }

        protected virtual void Sort()
        {
            // 降序
            //m_dataList.Sort((x, y) => -x.CompareTo(y));
            m_dataList.Sort((x, y) => -x.CompareTo(y) * 2 + -x.allTimes.CompareTo(y.allTimes));
        }
        
        public void Clear()
        {
            m_dataList.Clear();
            m_dataDic.Clear();
        }
    }
}