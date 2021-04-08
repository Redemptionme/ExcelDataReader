using System;
using System.Collections.Generic;
using System.Data;
using ExcelDataReader.Log;

namespace LRS
{
    public static class ExcelDataReaderHelper
    {
        //private static Dictionary<string, List<List<string>>> g_excelData = new Dictionary<string, List<List<string>>>();
        private static DataSet g_dataSet;

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
                case "女巫": return EGameCard.Nv;
                case "猎人": return EGameCard.Lr;
                case "守卫": return EGameCard.Sw;
                case "猎魔人": return EGameCard.Lmr;
                case "白痴": return EGameCard.Bc;
                case "狼人": return EGameCard.Langr;
                case "狼王": return EGameCard.Lw;
                case "血月使徒": return EGameCard.Xyst;
                default: break;
            };

            return EGameCard.None;
        } 
        
        
        
        public static void HandleExcelData(DataSet dataSet)
        {
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

             
            DataMgr.Inst.Clear();
            foreach (var line in lines)
            {
                HandleMathData(tab, line);
            }

            var commonRank = new CommonRank(DataMgr.Inst.m_Datas);
            var timeRank = new TimeRank(DataMgr.Inst.m_Datas);
            var mvpRank = new MvpRank(DataMgr.Inst.m_Datas);
            var goodCampRank = new GoodCampRank(DataMgr.Inst.m_Datas);
            var badCampRank = new BadCampRank(DataMgr.Inst.m_Datas);
            


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
                    HHL.Common.Log.Inst.Print("分数算错");
                    throw new System.NotImplementedException();
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