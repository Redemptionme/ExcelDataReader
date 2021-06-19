using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using LRS.Rank;

namespace LRS
{
    public static partial class LrsHelper
    {
        //private static Dictionary<string, List<List<string>>> g_excelData = new Dictionary<string, List<List<string>>>();
        private static DataSet g_dataSet;
        private static DataSet g_dataSet2;
        public static string g_systemDataFileName;
        public static string g_rankFileName;

        public static void PrintRank(string rankName, string headName1, string headName2, string headName3,
            string headName4,string headName5,BaseRank<BaseRankData> rank)
        {
            PrintRankCsv(rankName);
            PrintRankCsv(headName1 + "," + headName2+ "," + headName3+ "," + headName4  + "," + headName5 + "," + "游戏总场");
            
            for (int i = 0; i < rank.m_dataList.Count; i++)
            {
                var data = rank.m_dataList[i];
                int totalTimes = DataMgr.Inst.m_timeRank.GetTotalTimes(data.Player);
                PrintRankCsv((i+1) + "," + data.Player.WorkNum + "," + data.Player.Name + "," + data.CompareValue + "," + data.allTimes +"," + totalTimes);
            }
            PrintRankCsv("");
        }
        
        public static void PrintRateRank(string rankName, string headName1, string headName2, string headName3,
            string headName4,string headName5,BaseRank<RateRankData> rank)
        {
            PrintRankCsv(rankName);
            PrintRankCsv(headName1 + "," + headName2+ "," + headName3+ "," + headName4 + "," + headName5);
            for (int i = 0; i < rank.m_dataList.Count; i++)
            {
                var data = rank.m_dataList[i];

                PrintRankCsv((i+1) + "," + data.Player.WorkNum + "," + data.Player.Name + "," + (data.RateData.Rate * 100).ToString("F2") + "%," + data.allTimes);
            }
            PrintRankCsv("");
        }
        
        public static void PrintRankCsv(string str, bool append = true)
        {
            WriteFile(g_rankFileName, str, append);
        }

        public static void PrintSystemData(Dictionary<PlayerInfo, List<PlayerData>> dic)
        {
            PrintSystemDataCsv("", false);
            string str = "工号,密码";
            str += ",概况,A类积分赛,B类参与奖,C类高阶称号,D类称号";
            
            for (int i = (int)EGameCard.None; i < (int)EGameCard.FG; i++)
            {
                EGameCard eCard = (EGameCard)i;
                if (!IsVaild(eCard)) continue;
                 
                str += "," + GetCardName(eCard); //+ "胜率|总盘|积分|操作分";
            }
            
            PrintSystemDataCsv(str, false);

            foreach (var item in dic)
            {
                var dataSystem = new DataSystem(item.Value);

                if (DataMgr.Inst.m_psdDic.TryGetValue(item.Key.WorkNum,out var psd))
                { 
                    dataSystem.Psd = psd;    
                }
                else
                {
                    dataSystem.Psd = "88888888";
                }

                str = item.Key.WorkNum + "," + dataSystem.Psd;
                str += "," + item.Key.Name
                           + " 参与 " + dataSystem.CardScoreDic[EGameCard.AllCard].WinRate.AllValue + " 次"
                           + " 总积分 " + dataSystem.CardScoreDic[EGameCard.AllCard].Score + " 分"
                           + " MVP " + dataSystem.MvpScore + " 次"
                           + " 总胜率 " + (dataSystem.CardScoreDic[EGameCard.AllCard].WinRate.Rate * 100).ToString("F2") +
                           " %";
                           

                str += "," + DataMgr.Inst.m_scoreRank.PrintPlayerRank(item.Key);
                str += "," + DataMgr.Inst.m_timeRank.PrintPlayerRank(item.Key);
                
                str += ",【号令天下Mvp】 " + DataMgr.Inst.m_mvpRank.PrintPlayerRank(item.Key) 
                    + "  【正义领袖】 " +DataMgr.Inst.m_goodCampRank.PrintPlayerRank(item.Key) 
                    + "  【狼王榜】 " + DataMgr.Inst.m_badCampRank.PrintPlayerRank(item.Key);
                    
                str += ",【预言家】: " + DataMgr.Inst.m_yyjRank.PrintPlayerRank(item.Key)
                    + " 【女巫】:" + DataMgr.Inst.m_nwRank.PrintPlayerRank(item.Key)
                    + " 【猎人】:" + DataMgr.Inst.m_lrRank.PrintPlayerRank(item.Key)
                    + " 【猎魔人】:" + DataMgr.Inst.m_lmrRank.PrintPlayerRank(item.Key)
                    + " 【守卫】:" + DataMgr.Inst.m_swRank.PrintPlayerRank(item.Key)
                    + " 【徒手抓狼】:" + DataMgr.Inst.m_noSkillRank.PrintPlayerRank(item.Key);
                
                // 胜率数据集合
                for (int i = (int)EGameCard.None; i < (int)EGameCard.FG; i++)
                {
                    EGameCard eCard = (EGameCard)i;
                    if (!IsVaild(eCard)) continue;

                    if (dataSystem.CardScoreDic.TryGetValue(eCard,out var data))
                    {
                        str += ",胜率: " + (data.WinRate.Rate * 100).ToString("F2");
                        str += "%  总盘数: " + data.WinRate.AllValue;
                        str += "  积分: " + data.Score;
                        if (LrsHelper.IsGodCard(eCard))
                        {
                            // 神
                            str += "  操作分: " + data.OpScore;    
                        }
                    }
                    else
                    {
                        str += ",未拿过";
                    } 
                }
                
                PrintSystemDataCsv(str, true);
            }
            
        }
        public static void PrintSystemDataCsv(string str, bool append = true)
        {
            WriteFile(g_systemDataFileName, str, append);
        }

        public static void WriteFile(string fileName, string str, bool append = true)
        {
            using (var sw = new StreamWriter(fileName, append,Encoding.UTF8))
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

        public static bool IsCard(EGameCard eGameCard)
        {
            return IsGoodCard(eGameCard) || IsBadCard(eGameCard);
        }

        public static bool IsVaild(EGameCard eGameCard)
        {
            return IsCard(eGameCard) || eGameCard == EGameCard.AllCard || eGameCard == EGameCard.GoodCamp ||
                   eGameCard == EGameCard.BadCamp;
        }
        
        public static bool IsGoodCard(EGameCard eGameCard)
        {
            return eGameCard > EGameCard.GoodCardBegin && eGameCard < EGameCard.GoodCardEnd;
        }

        public static bool IsBadCard(EGameCard eGameCard)
        {
            return eGameCard > EGameCard.BadCardBegin && eGameCard < EGameCard.BadCardEnd;
        }

        public static bool IsGodCard(EGameCard eGameCard)
        {
            return IsGoodCard(eGameCard) && !IsCmCard(eGameCard);
        }

        public static bool IsCmCard(EGameCard eGameCard)
        {
            return eGameCard == EGameCard.Cm;
        }
        

        public static bool IsContain(EGameCard eGameCard1, EGameCard eGameCard2)
        {
            if (eGameCard2 == EGameCard.AllCard)
            {
                return IsCard(eGameCard1);
            }
            if (eGameCard2 == EGameCard.GoodCamp)
            {
                return IsGoodCard(eGameCard1);
            }

            if (eGameCard2 == EGameCard.BadCamp)
            {
                return IsBadCard(eGameCard1);
            }

            return eGameCard1 == eGameCard2;
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
                case EGameCard.AllCard:
                    return "全部身份";
                case EGameCard.GoodCamp:
                    return "好人阵营";
                case EGameCard.BadCamp:
                    return "邪恶秩序";
            }

            return "错误卡牌";
        }
        
        //数字1-9转换为中文数字
        public static string OneBitNumberToChinese(string num){
            string numStr = "123456789";
            string chineseStr = "一二三四五六七八九";
            string result = "";
            int numIndex=numStr.IndexOf(num);
            if(numIndex>-1){
                result=chineseStr.Substring(numIndex,1);
            }
            return result;
        }
        
        //数字1-9转换为中文数字
        public static string OneBitNumberToChinese(int num){
            return OneBitNumberToChinese(num.ToString());
        }
        
        

    }
}