using System.Collections.Generic;
using HHL.Common;

namespace LRS
{
    public class DataMgr:Singleton<DataMgr>
    {
        public List<MatchData> m_Datas = new List<MatchData>();
        
        public override void Init()
        {
            
        }

        public override void Dispose()
        {
            
        }

        public void Clear()
        {
            m_Datas.Clear();
        }

        public void AddData(MatchData data)
        {
            m_Datas.Add(data);
        }


        public void Print()
        {
            
        }
    }
}