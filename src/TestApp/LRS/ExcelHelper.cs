using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms.VisualStyles;
using ExcelDataReader.Log;
using LRS.Rank;

namespace LRS
{
    public static class ExcelDataReaderHelper
    {
        //private static Dictionary<string, List<List<string>>> g_excelData = new Dictionary<string, List<List<string>>>();
        private static DataSet g_dataSet;
        public static string g_systemDataFileName;
        public static string g_rankFileName;


        public static void PrintRank(string rankName, string headName1, string headName2, string headName3,
            string headName4,string headName5,BaseRank<BaseRankData> rank)
        {
            PrintRankCsv(rankName);
            PrintRankCsv(headName1 + "," + headName2+ "," + headName3+ "," + headName4  + "," + headName5);
            for (int i = 0; i < rank.m_dataList.Count; i++)
            {
                var data = rank.m_dataList[i];
                PrintRankCsv((i+1) + "," + data.Player.WorkNum + "," + data.Player.Name + "," + data.CompareValue + "," + data.allTimes);
            }
            PrintRankCsv("");
            
            
        }
        
        public static void PrintRateRank(string rankName, string headName1, string headName2, string headName3,
            string headName4,string headName5,BaseRank<RateData> rank)
        {
            PrintRankCsv(rankName);
            PrintRankCsv(headName1 + "," + headName2+ "," + headName3+ "," + headName4 + "," + headName5);
            for (int i = 0; i < rank.m_dataList.Count; i++)
            {
                var data = rank.m_dataList[i];
                var perNum = (float)data.CompareValue / 100.0f;
                
                PrintRankCsv((i+1) + "," + data.Player.WorkNum + "," + data.Player.Name + "," + perNum + "," + data.allTimes);
            }
            PrintRankCsv("");
        }
        
        public static void PrintRankCsv(string str, bool append = true)
        {
            WriteFile(g_rankFileName, str, append);
        }
         
        public static void PrintSystemData(string str, bool append = true)
        {
            WriteFile(g_systemDataFileName, str, append);
        }

        public static void WriteFile(string fileName, string str, bool append = true)
        {
            using (var sw = new StreamWriter(fileName, append))
            {
                sw.WriteLine(str);
            }
        }

        public static int stringToint(object obj)
        {
            string str = obj.ToString();
            if (str.Length == Decimal.Zero)
            {
                return 0;
            }

            return int.Parse(str);
        }
        public static EGameCard GetCard(string str)
        {
            switch(str)
            {
                case "村民": return EGameCard.Cm;
                case "预言家": return EGameCard.Yyj;
                case "女巫": return EGameCard.Nw;
                case "猎人": return EGameCard.Lr;
                case "守卫": return EGameCard.Sw;
                case "猎魔人": return EGameCard.Lmr;
                case "白痴": return EGameCard.Bc;
                case "狼人": return EGameCard.Langr;
                case "狼王": return EGameCard.Lw;
                case "血月使徒": return EGameCard.Xyst;
                default: break;
            };
            throw new System.NotImplementedException(str + "该卡牌不符合名字标准");
            return EGameCard.None;
        }

        public static string GetCardName(EGameCard eType)
        {
            switch (eType)
            {
                case EGameCard.Langr:
                    return "狼人";
                case EGameCard.Lw:
                    return "狼王";
                case EGameCard.Lx:
                    return "狼兄";
                case EGameCard.Ld:
                    return "狼弟";
                case EGameCard.Xyst:
                    return "血月使徒";
                case EGameCard.Cm:
                    return "村民";
                case EGameCard.Yyj:
                    return "预言家";
                case EGameCard.Nw:
                    return "女巫";
                case EGameCard.Lr:
                    return "猎人";
                case EGameCard.Bc:
                    return "白痴";
                case EGameCard.Sw:
                    return "守卫";
                case EGameCard.Hssr:
                    return "黑市商人";
                case EGameCard.Lmr:
                    return "猎魔人";
                case EGameCard.FG:
                    return "法官";
            }

            return "错误卡牌";
        }
        
        
        
        public static void HandleExcelData(DataSet dataSet)
        {
            DataMgr.Inst.Clear();
            g_dataSet = dataSet;
            //g_excelData.Clear();
            foreach (var table in dataSet.Tables)
            {
                var tab = table as DataTable;
                if (!tab.TableName.Contains("2021-"))
                {
                    continue;
                }

                //g_excelData.Add(tableName,new List<List<string>>());
                HandleTabData(tab);
            }

            CalRank();
            CalExRank();
            GenDataSystem();
        }

        private static void GenDataSystem()
        {
            
            
            
        }
        
        private static void CalRank()
        {
            var scoreRank = new ScoreRank(DataMgr.Inst.m_Datas);
            ExcelDataReaderHelper.PrintRank("积分榜","排名","工号","姓名","积分","总场",scoreRank);
            var timeRank = new TimeDataRank(DataMgr.Inst.m_Datas);
            ExcelDataReaderHelper.PrintRank("次数排行版","排名","工号","姓名","次数","总场",timeRank);
            
            var mvpRank = new MvpRank(DataMgr.Inst.m_Datas);
            ExcelDataReaderHelper.PrintRank("MVP榜","排名","工号","姓名","MVP","总场",mvpRank);
            var godRank = new GodRank(DataMgr.Inst.m_Datas);
            ExcelDataReaderHelper.PrintRank("上帝版","排名","工号","姓名","次数","总场",godRank);
            var goodCampRank = new ScoreRank(DataMgr.Inst.m_Datas, playerData =>
            {
                return playerData.Card > EGameCard.GoodCardBegin && playerData.Card < EGameCard.GoodCardEnd;
            });
            ExcelDataReaderHelper.PrintRank("正义领袖榜","排名","工号","姓名","积分","总场",goodCampRank);
            
            var badCampRank = new ScoreRank(DataMgr.Inst.m_Datas, player =>
            {
                return player.Card > EGameCard.BadCardBegin && player.Card < EGameCard.BadCardEnd;
            });
            ExcelDataReaderHelper.PrintRank("狼王榜","排名","工号","姓名","积分","总场",badCampRank);

            var yyjRank = new OpRank(DataMgr.Inst.m_Datas, player =>
            {
                return player.Card == EGameCard.Yyj;
            });
            ExcelDataReaderHelper.PrintRank("预言家榜","排名","工号","姓名","操作分","总场",yyjRank);
            
            var nwRank = new OpRank(DataMgr.Inst.m_Datas, player =>
            {
                return player.Card == EGameCard.Nw;
            });
            ExcelDataReaderHelper.PrintRank("女巫榜","排名","工号","姓名","操作分","总场",nwRank);
            
            var lmrRank = new OpRank(DataMgr.Inst.m_Datas, player =>
            {
                return player.Card == EGameCard.Lmr;
            });
            ExcelDataReaderHelper.PrintRank("猎魔人榜","排名","工号","姓名","操作分","总场",lmrRank);
            var swRank = new OpRank(DataMgr.Inst.m_Datas, player =>
            {
                return player.Card == EGameCard.Sw;
            });
            ExcelDataReaderHelper.PrintRank("守卫榜","排名","工号","姓名","操作分","总场",swRank);
            var noSkillRank = new DayScoreRank(DataMgr.Inst.m_Datas, player =>
            {
                return player.Card == EGameCard.Cm || player.Card == EGameCard.Bc;
            });
            ExcelDataReaderHelper.PrintRank("徒手抓狼榜","排名","工号","姓名","投票分","总场",noSkillRank);
        }
        
        private static void CalExRank()
        {
            ExcelDataReaderHelper.PrintRankCsv("==========奖励无关数据================================");
            
            var dayScoreRank = new DayScoreRank(DataMgr.Inst.m_Datas);
            ExcelDataReaderHelper.PrintRank("投票榜","排名","工号","姓名","投票分","总场",dayScoreRank);
            
            var rateRank = new RateRank(DataMgr.Inst.m_Datas);
            ExcelDataReaderHelper.PrintRateRank("胜率榜","排名","工号","姓名","百分比","总场",rateRank);
            
            var goodRateRank = new RateRank(DataMgr.Inst.m_Datas, player =>
            {
                return player.Card > EGameCard.GoodCardBegin && player.Card < EGameCard.GoodCardEnd;
            });
            ExcelDataReaderHelper.PrintRateRank("正义阵营胜率榜","排名","工号","姓名","百分比","总场",goodRateRank);
            
            var badRateRank = new RateRank(DataMgr.Inst.m_Datas, player =>
            {
                return player.Card > EGameCard.BadCardBegin && player.Card < EGameCard.BadCardEnd;
            });
            ExcelDataReaderHelper.PrintRateRank("邪恶秩序胜率榜","排名","工号","姓名","百分比","总场",badRateRank);

            for (int i = (int)EGameCard.None; i <= (int)EGameCard.FG; i++)
            {
                EGameCard eCard = (EGameCard)i;
                if (eCard == EGameCard.None || eCard == EGameCard.FG || eCard == EGameCard.GoodCardBegin || eCard == EGameCard.GoodCardEnd || eCard == EGameCard.BadCardBegin || eCard == EGameCard.BadCardEnd)
                {
                    continue;
                }
                var cardRateRank = new RateRank(DataMgr.Inst.m_Datas, player =>
                {
                    return player.Card == eCard;
                });
                if (cardRateRank.m_dataList.Count == 0 || cardRateRank.m_dataList[0].allTimes == 0)
                {
                    continue;
                }

                var cardName = GetCardName(eCard);
                
                ExcelDataReaderHelper.PrintRateRank(cardName + "胜率榜","排名","工号","姓名","百分比","总场",cardRateRank);
            } 
        }

        public static void HandleTabData(DataTable tab)
        {
            // string[] tabNames = tab.TableName.Split('-'); 
            //
            // //var y = tab.Columns.Count;
            // var rowsCount = tab.Rows.Count;
            // const int columnNum = 'N' - 'A' + 1;
            //
            // for (int i = 0; i < rowsCount; i++)
            // {
            //     //g_excelData[tabName][i] = new List<string>();
            //     for (int j = 0; j < columnNum; j++)
            //     {
            //         var str = tab.Rows[i][j].ToString();
            //         //g_excelData[tabName][i].Add(str);
            //     }    
            // }
            
            // 第一次扫描先确定3盘的标记号
            List<int> lines = new List<int>();
            
            for (int i = 0; i < tab.Rows.Count; i++)
            {
                var str = tab.Rows[i][0].ToString();
                if (str.Contains("局法"))
                {
                    lines.Add(i);
                }
            }
            
            foreach (var line in lines)
            {
                HandleMathData(tab, line);
            }
        }

        public static void HandleMathData(DataTable tab,int starline)
        {
            MatchInfo info = new MatchInfo();
            string str = tab.Rows[starline][0].ToString();
            var curTime = stringToint(str[1]);

            // 解析出第一行法官和板子
            PlayerInfo god = new PlayerInfo();
            var godWorkNum = stringToint(tab.Rows[starline][1]);
            var godName = tab.Rows[starline][2].ToString();
            var banzi = tab.Rows[starline][3].ToString();
            god.SetData(godWorkNum,godName);
            // 生成比赛数据
            info.SetData(tab.TableName,curTime,god,banzi);


            MatchData matchData = new MatchData();
            matchData.Info = info;
            
            // 12位玩家数据
            for (int i = 0; i < BaseInfoHelper.PlayerNum; i++)
            {
                int curLine = starline + i + 2;
                PlayerData playerData = new PlayerData();
                
                var gameNum = stringToint(tab.Rows[curLine][0]);
                PlayerInfo playerInfo = new PlayerInfo();
                var worknum = stringToint(tab.Rows[curLine][1]);
                
                var playerName = tab.Rows[curLine][2].ToString();
                playerInfo.SetData(worknum,playerName);
                
                
                var gameCard = ExcelDataReaderHelper.GetCard(tab.Rows[curLine][3].ToString());
                
                var day1Score = stringToint(tab.Rows[curLine][4]);
                var day2Score = stringToint(tab.Rows[curLine][5]);
                var day3Score = stringToint(tab.Rows[curLine][6]);
                var day4Score = stringToint(tab.Rows[curLine][7]);
                var day5Score = stringToint(tab.Rows[curLine][8]);
                var dayScore = day1Score + day2Score + day3Score + day4Score + day5Score;
                
                var winScore = stringToint(tab.Rows[curLine][9]);
                var extendScore = stringToint(tab.Rows[curLine][10]);
                var totalScore = stringToint(tab.Rows[curLine][11]);
                
                if (totalScore != winScore + dayScore)
                {
                    throw new System.NotImplementedException( info.AllDay+ " " + curLine +"行"+ playerInfo.Name + "总分数算错");
                }
                
                var opScore = stringToint(tab.Rows[curLine][12]);
                var mvpScore = stringToint(tab.Rows[curLine][13]);
                
                playerData.SetData(playerInfo,gameNum,gameCard,dayScore,winScore,extendScore,opScore,mvpScore);
                matchData.playerDatas.Add(playerData);
            } 
            
            DataMgr.Inst.AddData(matchData);
        }
    }
}