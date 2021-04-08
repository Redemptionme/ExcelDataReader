
using System;
using System.Collections.Generic;

namespace HHL.Common
{
    public partial class Log
    {
        // 写文件单行字符长度,stringbuilder长度现在默认是200,所以别超过200
        private int _fileTxtLen = 120;
        private bool _allMsg = true;
        // private readonly List<MsgType> m_MsgList = new List<MsgType>();
        
         public enum eLogType
         {
             eLog = 0,
             eWarning,
             eErro,
             eRootNetwork,
         }
         
         public enum eLogOut
         {
             eNone = 0,
             eFile = 1,
             eUnity = 1<<1,
         }

         private void InitExtend()
         {
             _filePath = System.Environment.CurrentDirectory + "\\Assets\\Scripts\\Game\\HHL" + "\\HHL.log";
             
             _typeList.Add(eLogType.eLog);
             _typeList.Add(eLogType.eWarning);
             _typeList.Add(eLogType.eErro);
             //_typeList.Add(eLogType.eRootNetwork);

             _outType = (uint) eLogOut.eFile | (uint) eLogOut.eUnity;
             
             InitMsgListen();
             
             var strFu = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
             Print("", eLogType.eLog, false);
             Print("-----HHL Log----------" + strFu + "--------------");
         }
         private void InitMsgListen()
         {
             // InitMsgExplore();
             
         }

         // private void InitMsgExplore()
         // {
         //     AddListenMsgType(MsgType.KMsgGs2ClexploreReply);
         //     AddListenMsgType(MsgType.KMsgGs2ClexploreUpdateVillageNotice);
         //     AddListenMsgType(MsgType.KMsgGs2ClexploreUpdateCaveNotice);
         //     AddListenMsgType(MsgType.KMsgGs2ClexploreUpdateBuildingNotice);
         //     AddListenMsgType(MsgType.KMsgCl2GsexploreVisitReply);
         //     AddListenMsgType(MsgType.KMsgGs2ClexploreAwardReply);
         //     AddListenMsgType(MsgType.KMsgGs2ClexploreBatchAwardReply);
         //     AddListenMsgType(MsgType.KMsgGs2ClmistOpenBuildingMistReply);
         //     AddListenMsgType(MsgType.KMsgGs2ClmistCleanNotice);
         //     AddListenMsgType(MsgType.KMsgGs2ClmistOpenNotice);
         //     AddListenMsgType(MsgType.KMsgGs2ClscoutCommandUpdateNotice);
         //
         //     AddListenMsgType(MsgType.KMsgCl2GsmistOpenBuildingMistRequest);
         //     AddListenMsgType(MsgType.KMsgCl2GsexploreBatchAwardRequest);
         //     AddListenMsgType(MsgType.KMsgCl2GsexploreAwardRequest);
         //     AddListenMsgType(MsgType.KMsgCl2GsexploreVisitRequest);
         // }
         //
         // public void AddListenMsgType(MsgType eType)
         // {
         //     m_MsgList.Add(eType);
         // }
         
         // public void PrintMsg(IMessage msg, bool bSend = false)
         // {
         //     if (msg == null) return;
         //     var msgType = (MsgType) MsgMap.GetIdByType(msg.GetType());
         //
         //     if (_allMsg || m_MsgList.Contains(msgType))
         //     {
         //         if (bSend)
         //             Print("Send " + (int) msgType + " ==>" + msgType + " " + msg, eLogType.eRootNetwork);
         //         else
         //             Print("Recv " + (int) msgType + " ==>" + msgType + " " + msg, eLogType.eRootNetwork);
         //     }
         // }
         
    }
}