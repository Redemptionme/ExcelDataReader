using System.Collections.Generic;
using HHL.Common;
using LRS.Rank;

namespace LRS
{
    public class DataMgr:Singleton<DataMgr>
    {
        // 一切数据集合
        public List<MatchData> m_Datas = new List<MatchData>();
        
        // 各榜数据
        public ScoreRank m_scoreRank = new ScoreRank(); 
        public TimeDataRank m_timeRank = new TimeDataRank();
        public MvpRank m_mvpRank = new MvpRank();
        public GodRank m_godRank = new GodRank(); 
        public ScoreRank m_goodCampRank = new ScoreRank(); 
        public ScoreRank m_badCampRank = new ScoreRank(); 
        public OpRank m_yyjRank = new OpRank(); 
        public OpRank m_nwRank = new OpRank(); 
        public OpRank m_lrRank = new OpRank(); 
        public OpRank m_lmrRank = new OpRank(); 
        public OpRank m_swRank = new OpRank(); 
        public DayScoreRank m_noSkillRank = new DayScoreRank();
        
        public DayScoreRank m_DayScoreRank = new DayScoreRank();
        public Dictionary<EGameCard, WinRateRank> m_winRateRankDic = new Dictionary<EGameCard, WinRateRank>();
        
        public Dictionary<PlayerInfo, List<PlayerData>> m_playerDataSystem = new Dictionary<PlayerInfo, List<PlayerData>>();

        public Dictionary<int, string> m_psdDic = new Dictionary<int, string>();
        
        // 游戏总数据
        public AllMatchInfo m_AllMatchInfo = new AllMatchInfo();
        
        // 特殊被操作数据
        public BeOpRank m_yyjBeOpRank = new BeOpRank();
        public BeOpRank m_langrBeOpRank = new BeOpRank();
        public BeOpRank m_nwBeOpRank = new BeOpRank();

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

        public void GenMatchRank()
        {
            GenMatchInfo();
            GenAwardRank();
            GenNoAwardRank();
        }
 
        public void GenAwardRank()
        {
            m_timeRank.Init(m_Datas,null,false);
            m_scoreRank.Init(m_Datas,null,true);
            LrsHelper.PrintRank("积分榜","排名","工号","姓名","积分","对应场次",m_scoreRank);
            LrsHelper.PrintRank("次数排行版","排名","工号","姓名","次数","对应场次",m_timeRank);

            m_mvpRank.Init(m_Datas);
            LrsHelper.PrintRank("MVP榜","排名","工号","姓名","MVP","对应场次",m_mvpRank);
            
            m_godRank.Init(m_Datas);
            //LrsHelper.PrintRank("上帝版","排名","工号","姓名","次数","总场",m_godRank);
            
            m_goodCampRank.Init(m_Datas, playerData =>
            {
                return LrsHelper.IsGoodCard(playerData.Card);
            });
            LrsHelper.PrintRank("正义领袖榜","排名","工号","姓名","积分","对应场次",m_goodCampRank);
            
            m_badCampRank.Init(m_Datas, playerData =>
            {
                return LrsHelper.IsBadCard(playerData.Card);;
            });
            LrsHelper.PrintRank("狼王榜","排名","工号","姓名","积分","对应场次",m_badCampRank);

            m_yyjRank.Init(m_Datas, player =>
            {
                return player.Card == EGameCard.Yyj;
            });
            LrsHelper.PrintRank("预言家榜","排名","工号","姓名","操作分","对应场次",m_yyjRank);
            
            m_nwRank.Init(m_Datas, player =>
            {
                return player.Card == EGameCard.Nw;
            });
            LrsHelper.PrintRank("女巫榜","排名","工号","姓名","操作分","对应场次",m_nwRank);
            
            m_lrRank.Init(m_Datas, player =>
            {
                return player.Card == EGameCard.Lr;
            });
            LrsHelper.PrintRank("猎人榜","排名","工号","姓名","操作分","对应场次",m_lrRank);
           
            m_lmrRank.Init(m_Datas, player =>
            {
                return player.Card == EGameCard.Lmr;
            });
            LrsHelper.PrintRank("猎魔人榜","排名","工号","姓名","操作分","对应场次",m_lmrRank);
            
            m_swRank.Init(m_Datas, player =>
            {
                return player.Card == EGameCard.Sw;
            });
            LrsHelper.PrintRank("守卫榜","排名","工号","姓名","操作分","对应场次",m_swRank);
            
            m_noSkillRank.Init(m_Datas, player =>
            {
                return player.Card == EGameCard.Cm || player.Card == EGameCard.Bc;
            });
            LrsHelper.PrintRank("徒手抓狼榜","排名","工号","姓名","投票分","对应场次",m_noSkillRank);

        }

        private void GenMatchInfo()
        {
            var lastInfo = m_Datas[m_Datas.Count - 1].Info;
            var Day = lastInfo.Year + "-" + lastInfo.Month.ToString("D2") + "-" + lastInfo.Day.ToString("D2") + "-周" + LrsHelper.OneBitNumberToChinese(lastInfo.WeekDay);
            LrsHelper.PrintRankCsv("!==========截至 " + Day);
            m_AllMatchInfo.SetData(m_Datas);
            LrsHelper.PrintRankCsv("本次比赛共计 " + m_AllMatchInfo.TotalCount + "场 其中好人获胜 " + m_AllMatchInfo.GoodCampWinCount + "场 好人胜率 " + m_AllMatchInfo.GoodCampWinCount * 100 / m_AllMatchInfo.TotalCount + "%" );
            LrsHelper.PrintRankCsv("" );
        }
        private void GenNoAwardRank()
        {
         LrsHelper.PrintRankCsv("!==========奖励无关数据================================");
         
         m_DayScoreRank.Init(m_Datas);
         LrsHelper.PrintRank("投票榜","排名","工号","姓名","投票分","总场",m_DayScoreRank);
        
         for (int i = (int)EGameCard.None; i <= (int)EGameCard.FG; i++)
         {
             EGameCard eCard = (EGameCard)i;
             if (!LrsHelper.IsVaild(eCard)) continue;
             
             if (m_winRateRankDic.ContainsKey(eCard)) continue;

             var cardRateRank = new WinRateRank();
             cardRateRank.Init(m_Datas, player =>
             {
                 return LrsHelper.IsContain(player.Card,eCard);
             });
             
             m_winRateRankDic.Add(eCard,cardRateRank);
             
             if (cardRateRank.m_dataList.Count == 0 || cardRateRank.m_dataList[0].Count == 0) continue;

             var cardName = LrsHelper.GetCardName(eCard);
             
             LrsHelper.PrintRateRank(cardName + "胜率榜","排名","工号","姓名","百分比","对应场次",cardRateRank);
         } 
         
         m_yyjBeOpRank.Init(m_Datas, gameCard =>
             {
                 return gameCard == EGameCard.Yyj;
             }
         );
         LrsHelper.PrintRank("首验榜","排名","工号","姓名","次数","对应场次",m_yyjBeOpRank);
         
         m_langrBeOpRank.Init(m_Datas, gameCard =>
             {
                 return gameCard == EGameCard.Langr;
             }
         );
         LrsHelper.PrintRank("首刀榜","排名","工号","姓名","次数","对应场次",m_langrBeOpRank);
         
         m_nwBeOpRank.Init(m_Datas, gameCard =>
             {
                 return gameCard == EGameCard.Nw;
             }
         );
         LrsHelper.PrintRank("吃毒榜","排名","工号","姓名","次数","对应场次",m_nwBeOpRank);
         
        }

        public void GenDataSystem()
        {
            // 处理玩家分数系统，也要搞个整体比赛数量，狼获胜情况
            foreach (var matchData in DataMgr.Inst.m_Datas)
            {
                foreach (var playerData in matchData.playerDatas)
                {
                    if (!m_playerDataSystem.ContainsKey(playerData.Player))
                    {
                        m_playerDataSystem[playerData.Player] = new List<PlayerData>();
                    }
                    
                    m_playerDataSystem[playerData.Player].Add(playerData);   
                }
            }
            
            LrsHelper.PrintSystemData(m_playerDataSystem); 
        }
    }
}