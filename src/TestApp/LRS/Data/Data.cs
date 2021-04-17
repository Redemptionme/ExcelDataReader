using System;
using System.Collections.Generic;

namespace LRS
{
    public static class BaseInfoHelper
    {
        public static int PlayerNum = 12; // 玩家人数
    } 
    
    public enum EGameCard {  
        None = -1, 
        AllCard,
        BadCamp,
        GoodCamp,
        BadCardBegin,
        Langr,//狼人
        Lw,//狼王
        Xyst, //血月使徒
        BadCardEnd, // 坏身份结束-----------
        Lx,//狼兄
        Ld,//狼弟
        GoodCardBegin,
        Cm,// 村民
        Yyj,// 预言家
        Nw,//女巫
        Lr,//猎人
        Bc,//白痴    
        Sw,//守卫
        Lmr, //猎魔人
        GoodCardEnd,// 好身份结束-----------
        Hssr,//黑商商人
        
        FG, // 法官
    };
    
    public class MatchInfo
    {
        public int AllDay = 0;
        public int Year = 0;
        public int Month = 0;
        public int Day = 0;
        public int WeekDay = 0;
        
        public int CurTime = 0;
        public string Banzi;
        public PlayerInfo God; // 法官上帝

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var _class = (MatchInfo)obj;
            return this.GetHashCode() == _class.GetHashCode();
        }

        public override int GetHashCode()
        {
            return AllDay + 100000000 * CurTime;
        }


        public void SetData(string tableName,int curTime,PlayerInfo god,string banzi)
        {
            string[] tabNames = tableName.Split('-');
            Year = int.Parse(tabNames[0]);
            Month = int.Parse(tabNames[1]);
            Day = int.Parse(tabNames[2]);
            AllDay = Year * 10000 + Month * 100 + Day;

            string weekday = tabNames[3];
            WeekDay = weekday switch
            {
                "周一" => 1,
                "周二" => 2,
                "周三" => 3,
                "周四" => 4,
                "周五" => 5,
                "周六" => 6,
                "周七" => 7,
                _ => WeekDay
            };

            CurTime = curTime;
            God = god;
            Banzi = banzi;
        }
    }
    
    

    // 玩家信息
    public class PlayerInfo
    {
        public int WorkNum;
        public string Name;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var _class = (PlayerInfo)obj;
            return this.GetHashCode() == _class.GetHashCode();
        }

        public static bool operator !=(PlayerInfo lhs, PlayerInfo rhs)
        {
            return !lhs.Equals(rhs);
        }
        public static bool operator == (PlayerInfo lhs, PlayerInfo rhs)
        {
            return lhs.Equals(rhs);
        }

        public override int GetHashCode()
        {
            return WorkNum;
        }
        
        public void SetData(int workNum, string name)
        {
            WorkNum = workNum;
            Name = name;
        }
    }
    
    // 玩家数据
    public class PlayerData
    {
        public PlayerInfo Player;
        public EGameCard Card;
        public int GameNum;
        public int DayScore;
        public int WinScore;
        public int ExtendScore;
        public int TotalScore;
        public int OpScore;
        public int MvpScore;
        public MatchInfo MatchInfo;

        public void SetData(PlayerInfo info,int num,EGameCard card, int dayScore, int winScore,int extendScore, int opScore,  int mvpScore,MatchInfo matchinfo)
        {
            Player = info;
            GameNum = num;
            Card = card;
            
            DayScore = dayScore;
            WinScore = winScore;
            ExtendScore = extendScore;
            TotalScore = DayScore + WinScore + ExtendScore;
            OpScore = opScore;
            MvpScore = mvpScore;

            MatchInfo = matchinfo;
        }
    }

    public class RateData
    {
        public int WinValue;
        public int AllValue;
        public float Rate;

        public void CalRate()
        {
            if (AllValue == 0) Rate = 0;

            Rate = (float)WinValue / (float)AllValue;
        }
    }
    

    public class MatchData
    {
        public MatchInfo Info;
        public List<PlayerData> playerDatas = new List<PlayerData>();
    }

    public class DataSystemCardData
    {
        public EGameCard CardType;
        public int Score;
        public int OpScore;

        public RateData WinRate = new RateData();
    }
    
    
    // 个人数据中心
    public class DataSystem
    {
        public int MvpScore;
        public Dictionary<EGameCard, DataSystemCardData> CardScoreDic = new Dictionary<EGameCard, DataSystemCardData>();
        public string Psd;
        
        public DataSystem(List<PlayerData> list)
        {
            CardScoreDic[EGameCard.AllCard] = new DataSystemCardData();
            CardScoreDic[EGameCard.GoodCamp] = new DataSystemCardData();
            CardScoreDic[EGameCard.BadCamp] = new DataSystemCardData();

            foreach (var playerData in list)
            {
                CardScoreDic[EGameCard.AllCard].Score += playerData.TotalScore;
                CardScoreDic[EGameCard.AllCard].WinRate.AllValue += 1;
                CardScoreDic[EGameCard.AllCard].WinRate.WinValue += playerData.WinScore > 0 ? 1 : 0;
                CardScoreDic[EGameCard.AllCard].WinRate.CalRate();
                CardScoreDic[EGameCard.AllCard].OpScore += playerData.OpScore;
                MvpScore += playerData.MvpScore;

                if (!CardScoreDic.ContainsKey(playerData.Card))
                {
                    CardScoreDic[playerData.Card] = new DataSystemCardData();
                }

                CardScoreDic[playerData.Card].Score = playerData.TotalScore;
                CardScoreDic[playerData.Card].OpScore = playerData.OpScore;
                CardScoreDic[playerData.Card].WinRate.AllValue += 1;
                CardScoreDic[playerData.Card].WinRate.WinValue += playerData.WinScore > 0 ? 1 : 0;
                CardScoreDic[playerData.Card].WinRate.CalRate();
                

                if (LrsHelper.IsGoodCard(playerData.Card))
                {
                    CardScoreDic[EGameCard.GoodCamp].Score += playerData.TotalScore;
                    CardScoreDic[EGameCard.GoodCamp].OpScore += playerData.OpScore;
                    CardScoreDic[EGameCard.GoodCamp].WinRate.WinValue += playerData.WinScore > 0 ? 1 : 0;
                    CardScoreDic[EGameCard.GoodCamp].WinRate.AllValue += 1;
                    CardScoreDic[EGameCard.GoodCamp].WinRate.CalRate();                    
                }
                else if (LrsHelper.IsBadCard(playerData.Card))
                {
                    CardScoreDic[EGameCard.BadCamp].Score += playerData.TotalScore;
                    CardScoreDic[EGameCard.BadCamp].OpScore += playerData.OpScore;
                    CardScoreDic[EGameCard.BadCamp].WinRate.WinValue += playerData.WinScore > 0 ? 1 : 0;
                    CardScoreDic[EGameCard.BadCamp].WinRate.AllValue += 1;
                    CardScoreDic[EGameCard.BadCamp].WinRate.CalRate();
                } 

            }
        }
    }
    
    
    
    
    
    
    
}