using System;
using DemoParserNET.models;

namespace DemoParserNET
{
    public static class DemoReader
    {
        // NetMsgEvents
        public static event EventHandler<CNetMsgNop> NetNopEvent;
        public static event EventHandler<CNetMsgDisconnect> NetDisconnectEvent;
        public static event EventHandler<CNetMsgFile> NetFileEvent;
        public static event EventHandler<CNetMsgSplitScreenUser> NetSplitScreenUserEvent;
        public static event EventHandler<CNetMsgTick> NetTickEvent;
        public static event EventHandler<CNetMsgStringCmd> NetStringCmdEvent;
        public static event EventHandler<CNetMsgSetConVar> NetSetConVarEvent;
        public static event EventHandler<CNetMsgSignonState> NetSignOnStateEvent;

        // CsvcMsgEvents
        public static event EventHandler<CSvcMsgServerInfo> CSvcServerInfoEvent;
        public static event EventHandler<CSvcMsgSendTable> CSvcSendTableEvent;
        public static event EventHandler<CSvcMsgClassInfo> CSvcClassInfoEvent;
        public static event EventHandler<CSvcMsgSetPause> CSvcSetPauseEvent;
        public static event EventHandler<CSvcMsgCreateStringTable> CSvcCreateStringTableEvent;
        public static event EventHandler<CSvcMsgUpdateStringTable> CSvcUpdateStringTableEvent;
        public static event EventHandler<CSvcMsgVoiceInit> CSvcVoiceInitEvent;
        public static event EventHandler<CSvcMsgVoiceData> CSvcVoiceDataEvent;
        public static event EventHandler<CSvcMsgPrint> CSvcPrintEventEvent;
        public static event EventHandler<CSvcMsgSounds> CSvcSoundsEventEvent;
        public static event EventHandler<CSvcMsgSetView> CSvcSetViewEvent;
        public static event EventHandler<CSvcMsgFixAngle> CSvcFixAngleEvent;
        public static event EventHandler<CSvcMsgCrosshairAngle> CSvcCrosshairAngleEvent;
        public static event EventHandler<CSvcMsgBspDecal> CSvcBspDecalEvent;
        public static event EventHandler<CSvcMsgSplitScreen> CSvcSplitScreenEvent;
        public static event EventHandler<CSvcMsgUserMessage> CSvcUserMessageEvent;
        public static event EventHandler<CSvcMsgEntityMessage> CSvcEntityMessageEvent;
        public static event EventHandler<CSvcMsgGameEvent> CSvcGameEventEvent;
        public static event EventHandler<CSvcMsgPacketEntities> CSvcPacketEntitiesEvent;
        public static event EventHandler<CSvcMsgTempEntities> CSvcTempEntitiesEvent;
        public static event EventHandler<CSvcMsgPrefetch> CSvcPrefetchEvent;
        public static event EventHandler<CSvcMsgMenu> CSvcMenuEvent;
        public static event EventHandler<CSvcMsgGameEventList> CSvcGameEventListEvent;
        public static event EventHandler<CSvcMsgGetCVarValue> CSvcGetCVarValueEvent;
        public static event EventHandler<CSvcMsgPaintMapData> CSvcPaintMapDataEvent;
        public static event EventHandler<CSvcMsgCmdKeyValues> CSvcCmdKeyValuesEvent;

        /// <summary>
        /// Begins parsing a demo based on file path
        /// </summary>
        /// <param name="path">Filepath to .dem file</param>
        public static void parseDemo(string path)
        {
            DemoFile demo = new DemoFile(path);
            demo.MessageEvent += DemoOnMessageEvent;

            demo.ReadHeader();
            demo.ParseDemo();
        }

        /// <summary>
        /// Passes on MessageEvent as the correct event type.
        /// </summary>
        /// <param name="sender">DemoFile Object that invoked the MessageEvent</param>
        /// <param name="arg">NetMessage Object containing the message</param>
        private static void DemoOnMessageEvent(object sender, NetMessage arg)
        {
            if (arg.NetMsgType <= 7)
            {
                switch ((NetMsg) arg.NetMsgType)
                {
                    case NetMsg.NetNop:
                        CNetMsgNop netMsgNop = (CNetMsgNop) arg;
                        NetNopEvent?.Invoke(sender, netMsgNop);
                        break;
                    case NetMsg.NetDisconnect:
                        CNetMsgDisconnect netMsgDisconnect = (CNetMsgDisconnect) arg;
                        NetDisconnectEvent?.Invoke(sender, netMsgDisconnect);
                        break;
                    case NetMsg.NetFile:
                        CNetMsgFile netMsgFile = (CNetMsgFile) arg;
                        NetFileEvent?.Invoke(sender, netMsgFile);
                        break;
                    case NetMsg.NetSplitScreenUser:
                        CNetMsgSplitScreenUser netMsgSplitScreenUser = (CNetMsgSplitScreenUser) arg;
                        NetSplitScreenUserEvent?.Invoke(sender, netMsgSplitScreenUser);
                        break;
                    case NetMsg.NetTick:
                        CNetMsgTick netMsgTick = (CNetMsgTick) arg;
                        NetTickEvent?.Invoke(sender, netMsgTick);
                        break;
                    case NetMsg.NetStringCmd:
                        CNetMsgStringCmd netMsgStringCmd = (CNetMsgStringCmd) arg;
                        NetStringCmdEvent?.Invoke(sender, netMsgStringCmd);
                        break;
                    case NetMsg.NetSetConVar:
                        CNetMsgSetConVar netMsgSetConVar = (CNetMsgSetConVar) arg;
                        NetSetConVarEvent?.Invoke(sender, netMsgSetConVar);
                        break;
                    case NetMsg.NetSignonState:
                        CNetMsgSignonState netMsgSignonState = (CNetMsgSignonState) arg;
                        NetSignOnStateEvent?.Invoke(sender, netMsgSignonState);
                        break;
                }
            }

            switch ((SvcMsg) arg.NetMsgType)
            {
                case SvcMsg.SvcServerInfo:
                    CSvcMsgServerInfo cSvcMsgServerInfo = (CSvcMsgServerInfo) arg;
                    CSvcServerInfoEvent?.Invoke(sender, cSvcMsgServerInfo);
                    break;
                case SvcMsg.SvcSendTable:
                    CSvcMsgSendTable cSvcMsgSendTable = (CSvcMsgSendTable) arg;
                    CSvcSendTableEvent?.Invoke(sender, cSvcMsgSendTable);
                    break;
                case SvcMsg.SvcClassInfo:
                    CSvcMsgClassInfo cSvcMsgClassInfo = (CSvcMsgClassInfo) arg;
                    CSvcClassInfoEvent?.Invoke(sender, cSvcMsgClassInfo);
                    break;
                case SvcMsg.SvcSetPause:
                    CSvcMsgSetPause cSvcMsgSetPause = (CSvcMsgSetPause) arg;
                    CSvcSetPauseEvent?.Invoke(sender, cSvcMsgSetPause);
                    break;
                case SvcMsg.SvcCreateStringTable:
                    CSvcMsgCreateStringTable cSvcMsgCreateStringTable = (CSvcMsgCreateStringTable) arg;
                    CSvcCreateStringTableEvent?.Invoke(sender, cSvcMsgCreateStringTable);
                    break;
                case SvcMsg.SvcUpdateStringTable:
                    CSvcMsgUpdateStringTable cSvcMsgUpdateStringTable = (CSvcMsgUpdateStringTable) arg;
                    CSvcUpdateStringTableEvent?.Invoke(sender, cSvcMsgUpdateStringTable);
                    break;
                case SvcMsg.SvcVoiceInit:
                    CSvcMsgVoiceInit cSvcMsgVoiceInit = (CSvcMsgVoiceInit) arg;
                    CSvcVoiceInitEvent?.Invoke(sender, cSvcMsgVoiceInit);
                    break;
                case SvcMsg.SvcVoiceData:
                    CSvcMsgVoiceData cSvcMsgVoiceData = (CSvcMsgVoiceData) arg;
                    CSvcVoiceDataEvent?.Invoke(sender, cSvcMsgVoiceData);
                    break;
                case SvcMsg.SvcPrint:
                    CSvcMsgPrint cSvcMsgPrint = (CSvcMsgPrint) arg;
                    CSvcPrintEventEvent?.Invoke(sender, cSvcMsgPrint);
                    break;
                case SvcMsg.SvcSounds:
                    CSvcMsgSounds cSvcMsgSounds = (CSvcMsgSounds) arg;
                    CSvcSoundsEventEvent?.Invoke(sender, cSvcMsgSounds);
                    break;
                case SvcMsg.SvcSetView:
                    CSvcMsgSetView cSvcMsgSetView = (CSvcMsgSetView) arg;
                    CSvcSetViewEvent?.Invoke(sender, cSvcMsgSetView);
                    break;
                case SvcMsg.SvcFixAngle:
                    CSvcMsgFixAngle cSvcMsgFixAngle = (CSvcMsgFixAngle) arg;
                    CSvcFixAngleEvent?.Invoke(sender, cSvcMsgFixAngle);
                    break;
                case SvcMsg.SvcCrosshairAngle:
                    CSvcMsgCrosshairAngle cSvcMsgCrosshairAngle = (CSvcMsgCrosshairAngle) arg;
                    CSvcCrosshairAngleEvent?.Invoke(sender, cSvcMsgCrosshairAngle);
                    break;
                case SvcMsg.SvcBSPDecal:
                    CSvcMsgBspDecal cSvcMsgBspDecal = (CSvcMsgBspDecal) arg;
                    CSvcBspDecalEvent?.Invoke(sender, cSvcMsgBspDecal);
                    break;
                case SvcMsg.SvcSplitScreen:
                    CSvcMsgSplitScreen cSvcMsgSplitScreen = (CSvcMsgSplitScreen) arg;
                    CSvcSplitScreenEvent?.Invoke(sender, cSvcMsgSplitScreen);
                    break;
                case SvcMsg.SvcUserMessage:
                    CSvcMsgUserMessage cSvcMsgUserMessage = (CSvcMsgUserMessage) arg;
                    CSvcUserMessageEvent?.Invoke(sender, cSvcMsgUserMessage);
                    break;
                case SvcMsg.SvcEntityMessage:
                    CSvcMsgEntityMessage cSvcMsgEntityMessage = (CSvcMsgEntityMessage) arg;
                    CSvcEntityMessageEvent?.Invoke(sender, cSvcMsgEntityMessage);
                    break;
                case SvcMsg.SvcGameEvent:
                    CSvcMsgGameEvent cSvcMsgGameEvent = (CSvcMsgGameEvent) arg;
                    CSvcGameEventEvent?.Invoke(sender, cSvcMsgGameEvent);
                    break;
                case SvcMsg.SvcPacketEntities:
                    CSvcMsgPacketEntities cSvcMsgPacketEntities = (CSvcMsgPacketEntities) arg;
                    CSvcPacketEntitiesEvent?.Invoke(sender, cSvcMsgPacketEntities);
                    break;
                case SvcMsg.SvcTempEntities:
                    CSvcMsgTempEntities cSvcMsgTempEntities = (CSvcMsgTempEntities) arg;
                    CSvcTempEntitiesEvent?.Invoke(sender, cSvcMsgTempEntities);
                    break;
                case SvcMsg.SvcPrefetch:
                    CSvcMsgPrefetch cSvcMsgPrefetch = (CSvcMsgPrefetch) arg;
                    CSvcPrefetchEvent?.Invoke(sender, cSvcMsgPrefetch);
                    break;
                case SvcMsg.SvcMenu:
                    CSvcMsgMenu cSvcMsgMenu = (CSvcMsgMenu) arg;
                    CSvcMenuEvent?.Invoke(sender, cSvcMsgMenu);
                    break;
                case SvcMsg.SvcGameEventList:
                    CSvcMsgGameEventList cSvcMsgGameEventList = (CSvcMsgGameEventList) arg;
                    CSvcGameEventListEvent?.Invoke(sender, cSvcMsgGameEventList);
                    break;
                case SvcMsg.SvcGetCvarValue:
                    CSvcMsgGetCVarValue cSvcMsgGetCVarValue = (CSvcMsgGetCVarValue) arg;
                    CSvcGetCVarValueEvent?.Invoke(sender, cSvcMsgGetCVarValue);
                    break;
                case SvcMsg.SvcPaintmapData:
                    CSvcMsgPaintMapData cSvcMsgPaintMapData = (CSvcMsgPaintMapData) arg;
                    CSvcPaintMapDataEvent?.Invoke(sender, cSvcMsgPaintMapData);
                    break;
                case SvcMsg.SvcCmdKeyValues:
                    CSvcMsgCmdKeyValues cSvcMsgCmdKeyValues = (CSvcMsgCmdKeyValues) arg;
                    CSvcCmdKeyValuesEvent?.Invoke(sender, cSvcMsgCmdKeyValues);
                    break;
            }
        }
    }
}