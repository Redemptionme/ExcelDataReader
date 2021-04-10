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
        public string m_rankName;
        
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