﻿using System;
using System.Collections.Generic;

namespace LRS
{
    public class ScoreRankData : BaseRankData,IComparable<ScoreRankData>
    {
        public int Score = 0;
        
        
        //ComparetTo:大于 1； 等于 0； 小于 -1；
        public int CompareTo(ScoreRankData p)
        {
            return Score.CompareTo(p.Score);
        }
    } 
    public class CommonRank : BaseRank<ScoreRankData>
    { 
        public CommonRank(List<MatchData> datas)
        {
            foreach (var matchdata in datas)
            {
                foreach (var data in matchdata.playerDatas)
                {
                    if (!m_dataDic.TryGetValue(data.Player,out ScoreRankData scoreData))
                    {
                        var newData = new ScoreRankData();
                        newData.Player = data.Player;
                        m_dataDic[newData.Player] = newData;
                    }

                    m_dataDic[data.Player].Score  += data.TotalScore;
                }
            }

            foreach (var item in m_dataDic)
            {
                m_dataList.Add(item.Value);
            }
            
            // 降序
            m_dataList.Sort((x, y) => -x.CompareTo(y));
        }
 
    }
}