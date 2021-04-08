using System.Collections.Generic;

namespace LRS
{
    public class BaseRankData
    {
        public PlayerInfo Player;
    }
    public class BaseRank<T> where T: BaseRankData
    {
        public List<T> m_dataList = new List<T>();
        public Dictionary<PlayerInfo, T> m_dataDic = new Dictionary<PlayerInfo, T>();

        public T GetPlayerData(PlayerInfo player)
        {
            if (m_dataDic.TryGetValue(player,out T value))
            {
                return value;
            }

            return null;
        }
        
        public void Clear()
        {
            m_dataList.Clear();
            m_dataDic.Clear();
        }
    }
}