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
        BadCardBegin,
        Langr,//狼人
        Lw,//狼王
        Lx,//狼兄
        Ld,//狼弟    
        Xyst, //血月使徒
        BadCardEnd, // 坏身份结束-----------
        GoodCardBegin,
        Cm,// 村民
        Yyj,// 预言家
        Nw,//女巫
        Lr,//猎人
        Bc,//白痴    
        Sw,//守卫
        Hssr,//黑商商人
        Lmr, //猎魔人
        GoodCardEnd,// 好身份结束-----------
        
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

        public void SetData(PlayerInfo info,int num,EGameCard card, int dayScore, int winScore,int extendScore, int opScore,  int mvpScore)
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
        }
    }

    public class MatchData
    {
        public MatchInfo Info;
        public List<PlayerData> playerDatas = new List<PlayerData>();
    }
    
    
    
    
    
    
    
    
}