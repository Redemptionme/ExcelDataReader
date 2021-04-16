using System.Collections.Generic;
using System.Data;

namespace LRS
{
    public static partial class LrsHelper
    {
        public static void TestHandleTabData(DataTable tab)
        {
            string[] tabNames = tab.TableName.Split('-'); 
            
            //var y = tab.Columns.Count;
            var rowsCount = tab.Rows.Count;
            const int columnNum = 'N' - 'A' + 1;
            
            for (int i = 0; i < rowsCount; i++)
            {
                //g_excelData[tabName][i] = new List<string>();
                for (int j = 0; j < columnNum; j++)
                {
                    var str = tab.Rows[i][j].ToString();
                    //g_excelData[tabName][i].Add(str);
                }    
            }
        }
        
        public static void HandleTabData(DataTable tab)
        {   
            // 第一次扫描先确定3盘的标记号
            List<int> lines = new List<int>();
            
            for (int i = 0; i < tab.Rows.Count; i++)
            {
                var str = tab.Rows[i][0].ToString();
                if (str.Contains("局法"))
                {
                    // 判定没身份,说明没玩
                    string firstRole = tab.Rows[i + 2][3].ToString(); 
                    if (firstRole.Length == 0) continue;
                    
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
                
                
                var gameCard = LrsHelper.GetCard(tab.Rows[curLine][3].ToString());
                
                var day1Score = stringToint(tab.Rows[curLine][4]);
                var day2Score = stringToint(tab.Rows[curLine][5]);
                var day3Score = stringToint(tab.Rows[curLine][6]);
                var day4Score = stringToint(tab.Rows[curLine][7]);
                var day5Score = stringToint(tab.Rows[curLine][8]);
                var dayScore = day1Score + day2Score + day3Score + day4Score + day5Score;
                
                var winScore = stringToint(tab.Rows[curLine][9]);
                var extendScore = stringToint(tab.Rows[curLine][10]);
                var totalScore = stringToint(tab.Rows[curLine][11]);
                
                if (totalScore != winScore + dayScore + extendScore)
                {
                    throw new System.NotImplementedException( info.AllDay+ " " + curLine +"行"+ playerInfo.Name + "总分数算错");
                }
                
                var opScore = stringToint(tab.Rows[curLine][12]);
                var mvpScore = stringToint(tab.Rows[curLine][13]);
                
                playerData.SetData(playerInfo,gameNum,gameCard,dayScore,winScore,extendScore,opScore,mvpScore,info);
                matchData.playerDatas.Add(playerData);
            } 
            
            DataMgr.Inst.AddData(matchData);
        }
    }
}