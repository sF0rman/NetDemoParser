#nullable enable
using System;
using System.Collections.Generic;
using DemoParserNET.models;
using ProtoBuf;

namespace DemoParserNET.models
{
    public enum NetMsg
    {
        NetNop = 0,
        NetDisconnect = 1,
        NetFile = 2,
        NetSplitScreenUser = 3,
        NetTick = 4,
        NetStringCmd = 5,
        NetSetConVar = 6,
        NetSignonState = 7
    }

    public enum ESplitScreenMessageType
    {
        // allow alias = true
        MSG_SPLITSCREEN_ADDUSER = 0,
        MSG_SPLITSCREEN_REMOVEUSER = 1,
        MSG_SPLITSCREEN_TYPE_BITS = 1
    };

    public enum SvcMsg
    {
        SvcServerInfo = 8,
        SvcSendTable = 9,
        SvcClassInfo = 10,
        SvcSetPause = 11,
        SvcCreateStringTable = 12,
        SvcUpdateStringTable = 13,
        SvcVoiceInit = 14,
        SvcVoiceData = 15,
        SvcPrint = 16,
        SvcSounds = 17,
        SvcSetView = 18,
        SvcFixAngle = 19,
        SvcCrosshairAngle = 20,
        SvcBSPDecal = 21,
        SvcSplitScreen = 22,
        SvcUserMessage = 23,
        SvcEntityMessage = 24,
        SvcGameEvent = 25,
        SvcPacketEntities = 26,
        SvcTempEntities = 27,
        SvcPrefetch = 28,
        SvcMenu = 29,
        SvcGameEventList = 30,
        SvcGetCvarValue = 31,
        SvcPaintmapData = 33,
        SvcCmdKeyValues = 34
    }

    public class NetMessageParser
    {
        public static bool Continue(int type)
        {
            switch (type)
            {
                case 7:
                    return false;
                default:
                    return true;
            }
        }

        public static NetMessage ParseMessage(int type, byte[] buffer)
        {
            if (type <= 7)
            {
                switch ((NetMsg) type)
                {
                    case NetMsg.NetNop:
                        return new CNetMsgNop().Decode<CNetMsgNop>(buffer);
                    case NetMsg.NetDisconnect:
                        return new CNetMsgDisconnect().Decode<CNetMsgDisconnect>(buffer);
                    case NetMsg.NetFile:
                        return new CNetMsgFile().Decode<CNetMsgFile>(buffer);
                    case NetMsg.NetSplitScreenUser:
                        return new CNetMsgSplitScreenUser().Decode<CNetMsgSplitScreenUser>(buffer);
                    case NetMsg.NetTick:
                        return new CNetMsgTick().Decode<CNetMsgTick>(buffer);
                    case NetMsg.NetStringCmd:
                        return new CNetMsgStringCmd().Decode<CNetMsgStringCmd>(buffer);
                    case NetMsg.NetSetConVar:
                        return new CNetMsgSetConVar().Decode<CNetMsgSetConVar>(buffer);
                    case NetMsg.NetSignonState:
                        return new CNetMsgSignonState().Decode<CNetMsgSignonState>(buffer);
                    default:
                        return null;
                }
            }

            // else
            switch ((SvcMsg) type)
            {
                case SvcMsg.SvcServerInfo:
                    return new CSvcMsgServerInfo().Decode<CSvcMsgServerInfo>(buffer);
                case SvcMsg.SvcSendTable:
                    return new CSvcMsgSendTable().Decode<CSvcMsgSendTable>(buffer);
                case SvcMsg.SvcClassInfo:
                    return new CSvcMsgClassInfo().Decode<CSvcMsgClassInfo>(buffer);
                case SvcMsg.SvcSetPause:
                    return new CSvcMsgSetPause().Decode<CSvcMsgSetPause>(buffer);
                case SvcMsg.SvcCreateStringTable:
                    return new CSvcMsgCreateStringTable().Decode<CSvcMsgCreateStringTable>(buffer);
                case SvcMsg.SvcUpdateStringTable:
                    return new CSvcMsgUpdateStringTable().Decode<CSvcMsgUpdateStringTable>(buffer);
                case SvcMsg.SvcVoiceInit:
                    return new CSvcMsgVoiceInit().Decode<CSvcMsgVoiceInit>(buffer);
                case SvcMsg.SvcVoiceData:
                    return new CSvcMsgVoiceData().Decode<CSvcMsgVoiceData>(buffer);
                case SvcMsg.SvcPrint:
                    return new CSvcMsgPrint().Decode<CSvcMsgPrint>(buffer);
                case SvcMsg.SvcSounds:
                    return new CSvcMsgSounds().Decode<CSvcMsgSounds>(buffer);
                case SvcMsg.SvcSetView:
                    return new CSvcMsgSetView().Decode<CSvcMsgSetView>(buffer);
                case SvcMsg.SvcFixAngle:
                    return new CSvcMsgFixAngle().Decode<CSvcMsgFixAngle>(buffer);
                case SvcMsg.SvcCrosshairAngle:
                    return new CSvcMsgCrosshairAngle().Decode<CSvcMsgCrosshairAngle>(buffer);
                case SvcMsg.SvcBSPDecal:
                    return new CSvcMsgBspDecal().Decode<CSvcMsgBspDecal>(buffer);
                case SvcMsg.SvcSplitScreen:
                    return new CSvcMsgSplitScreen().Decode<CSvcMsgSplitScreen>(buffer);
                case SvcMsg.SvcUserMessage:
                    return new CSvcMsgUserMessage().Decode<CSvcMsgUserMessage>(buffer);
                case SvcMsg.SvcEntityMessage:
                    return new CSvcMsgEntityMessage().Decode<CSvcMsgEntityMessage>(buffer);
                case SvcMsg.SvcGameEvent:
                    return new CSvcMsgGameEvent().Decode<CSvcMsgGameEvent>(buffer);
                case SvcMsg.SvcPacketEntities:
                    return new CSvcMsgPacketEntities().Decode<CSvcMsgPacketEntities>(buffer);
                case SvcMsg.SvcTempEntities:
                    return new CSvcMsgTempEntities().Decode<CSvcMsgTempEntities>(buffer);
                case SvcMsg.SvcPrefetch:
                    return new CSvcMsgPrefetch().Decode<CSvcMsgPrefetch>(buffer);
                case SvcMsg.SvcMenu:
                    return new CSvcMsgMenu().Decode<CSvcMsgMenu>(buffer);
                case SvcMsg.SvcGameEventList:
                    return new CSvcMsgGameEventList().Decode<CSvcMsgGameEventList>(buffer);
                case SvcMsg.SvcGetCvarValue:
                    return new CSvcMsgGetCVarValue().Decode<CSvcMsgGetCVarValue>(buffer);
                case SvcMsg.SvcPaintmapData:
                    return new CSvcMsgPaintMapData().Decode<CSvcMsgPaintMapData>(buffer);
                case SvcMsg.SvcCmdKeyValues:
                    return new CSvcMsgCmdKeyValues().Decode<CSvcMsgCmdKeyValues>(buffer);
                default:
                    return null;
            }
        }
    }

    // Common Types
    [ProtoContract]
    public class CMsgVector
    {
        [ProtoMember(1)] public float? X { get; set; }
        [ProtoMember(2)] public float? Y { get; set; }
        [ProtoMember(3)] public float? Z { get; set; }
    }

    [ProtoContract]
    public class CMsgVector2D
    {
        [ProtoMember(1)] public float? X { get; set; }
        [ProtoMember(2)] public float? Y { get; set; }
    }

    [ProtoContract]
    public class CMsgQAngle
    {
        [ProtoMember(1)] public float? X { get; set; }
        [ProtoMember(2)] public float? Y { get; set; }
        [ProtoMember(3)] public float? Z { get; set; }
    }

    [ProtoContract]
    public class CMsgRgba
    {
        [ProtoMember(1)] public float? R { get; set; }
        [ProtoMember(2)] public float? G { get; set; }
        [ProtoMember(3)] public float? B { get; set; }
        [ProtoMember(4)] public float? A { get; set; }
    }

    // NET Messages
    public abstract class NetMessage
    {
        public abstract int NetMsgType { get; }
        public T Decode<T>(Span<byte> stream)
        {
            return Serializer.Deserialize<T>(stream);
        }
    }

    // NetMsg 0
    [ProtoContract]
    public class CNetMsgNop : NetMessage
    {
        public override int NetMsgType => 0;
    }

    // NetMsg 1
    [ProtoContract]
    public class CNetMsgDisconnect : NetMessage
    {
        public override int NetMsgType => 1;
        [ProtoMember(1)] public string Text { get; set; }
    }

    // NetMsg 2
    [ProtoContract]
    public class CNetMsgFile : NetMessage
    {
        public override int NetMsgType { get; }  = 2;
        [ProtoMember(1)] public int? TransferId { get; set; }
        [ProtoMember(2)] public string? FileName { get; set; }
        [ProtoMember(3)] public bool? IsReplayDemoFile { get; set; }
        [ProtoMember(4)] public bool? Deny { get; set; }
    }

    // NetMsg 3
    [ProtoContract]
    public class CNetMsgSplitScreenUser : NetMessage
    {
        public override int NetMsgType => 3;
        [ProtoMember(1)] public string? Text { get; set; }
    }

    // NetMsg 4
    [ProtoContract]
    public class CNetMsgTick : NetMessage
    {
        public override int NetMsgType => 4;
        [ProtoMember(1)] public int? Tick { get; set; }
        [ProtoMember(2)] public int? HostFrameTime { get; set; }
        [ProtoMember(3)] public int? HostFrameTimeStdDeviation { get; set; }
    }

    // NetMsg 5
    [ProtoContract]
    public class CNetMsgStringCmd : NetMessage
    {
        public override int NetMsgType => 5;
        [ProtoMember(1)] public string? Command { get; set; }
    }

    // NetMsg 6
    [ProtoContract]
    [ProtoInclude(1, typeof(CMsgCVars))]
    public class CNetMsgSetConVar : NetMessage
    {
        public override int NetMsgType => 6;
        [ProtoMember(1)] public CMsgCVars Convars { get; set; }
    }

    // Start CVars


    [ProtoContract]
    [ProtoInclude(1, typeof(CVar))]
    public class CMsgCVars
    {
        [ProtoContract]
        public class CVar
        {
            [ProtoMember(1)] public string? Name { get; set; }
            [ProtoMember(2)] public string? Value { get; set; }
        }

        [ProtoMember(1)] public CVar[] Cvars { get; set; }
    }
    // END CVars

    // NetMsg 7
    [ProtoContract]
    public class CNetMsgSignonState : NetMessage
    {
        public override int NetMsgType => 7;
        [ProtoMember(1)] public uint? SignOnState { get; set; }
        [ProtoMember(2)] public uint? SpawnCount { get; set; }
        [ProtoMember(3)] public uint? NumServerPlayers { get; set; }
        [ProtoMember(4)] public string[] PlayerNetworkIds { get; set; }
        [ProtoMember(5)] public string? MapName { get; set; }
    }


    // SvcMsg 8
    [ProtoContract]
    public class CSvcMsgServerInfo : NetMessage
    {
        public override int NetMsgType => 8;
        [ProtoMember(1)] public int? Protocol { get; set; }
        [ProtoMember(2)] public int? ServerCount { get; set; }
        [ProtoMember(3)] public bool? IsDedicated { get; set; }
        [ProtoMember(4)] public bool? IsOfficialValveServer { get; set; }
        [ProtoMember(5)] public bool? IsHltv { get; set; }
        [ProtoMember(6)] public bool? IsReplay { get; set; }
        [ProtoMember(7)] public int? COs { get; set; }
        [ProtoMember(8)] public uint? MapCrc { get; set; }
        [ProtoMember(9)] public uint? ClientCrc { get; set; }
        [ProtoMember(10)] public uint? StringTableCrc { get; set; }
        [ProtoMember(11)] public int? MaxClients { get; set; }
        [ProtoMember(12)] public int? MaxClasses { get; set; }
        [ProtoMember(13)] public int? PlayerSlot { get; set; }
        [ProtoMember(14)] public float? TickInterval { get; set; }
        [ProtoMember(15)] public string? GameDir { get; set; }
        [ProtoMember(16)] public string? MapName { get; set; }
        [ProtoMember(17)] public string? MapGroupName { get; set; }
        [ProtoMember(18)] public string? SkyName { get; set; }
        [ProtoMember(19)] public string? HostName { get; set; }
        [ProtoMember(20)] public string? PublicIp { get; set; }
        [ProtoMember(21)] public bool? IsRedirectingToProxyReay { get; set; }
        [ProtoMember(22)] public long? UgcMapId { get; set; }
    }

    // SvcMsg 9
    [ProtoContract]
    [ProtoInclude(4, typeof(SendPropT))]
    public class CSvcMsgSendTable : NetMessage
    {
        public override int NetMsgType => 9;
        [ProtoContract]
        public class SendPropT
        {
            [ProtoMember(1)] public int? Type { get; set; }
            [ProtoMember(2)] public string? VarName { get; set; }
            [ProtoMember(3)] public int? Flags { get; set; }
            [ProtoMember(4)] public int? Priority { get; set; }
            [ProtoMember(5)] public string? DtName { get; set; }
            [ProtoMember(6)] public int? NumElements { get; set; }
            [ProtoMember(7)] public float? LowValue { get; set; }
            [ProtoMember(8)] public float? HighValue { get; set; }
            [ProtoMember(9)] public int NumBits { get; set; }
        }

        [ProtoMember(1)] public bool? IsEnd { get; set; }
        [ProtoMember(2)] public string? NetTableName { get; set; }
        [ProtoMember(3)] public bool? NeedsDecoder { get; set; }
        [ProtoMember(4)] public SendPropT[] Props { get; set; }
    }

    // CSvcMsg 10
    [ProtoContract]
    [ProtoInclude(2, typeof(ClassT))]
    public class CSvcMsgClassInfo : NetMessage
    {
        public override int NetMsgType => 10;
        [ProtoContract]
        public class ClassT
        {
            [ProtoMember(1)] public int? ClassId { get; set; }
            [ProtoMember(2)] public string? DataTableName { get; set; }
            [ProtoMember(3)] public string? ClassName { get; set; }
        }

        [ProtoMember(1)] public bool? CreateOnClient { get; set; }
        [ProtoMember(2)] public ClassT Classes { get; set; }
    }

    // CSvcMsg 11
    [ProtoContract]
    public class CSvcMsgSetPause : NetMessage
    {
        public override int NetMsgType => 11;
        [ProtoMember(1)] public bool? Paused { get; set; }
    }

    // CSvcMsg 12
    [ProtoContract]
    public class CSvcMsgCreateStringTable : NetMessage
    {
        public override int NetMsgType => 12;
        [ProtoMember(1)] public string? Name { get; set; }
        [ProtoMember(2)] public int? MaxEntries { get; set; }
        [ProtoMember(3)] public int? NumEntries { get; set; }
        [ProtoMember(4)] public bool? UserDataFixedSize { get; set; }
        [ProtoMember(5)] public int? UserDataSize { get; set; }
        [ProtoMember(6)] public int? UserDataSizeBits { get; set; }
        [ProtoMember(7)] public int? Flags { get; set; }
        [ProtoMember(8)] public byte[]? StringData { get; set; } // ByteString
    }

    // CSvcMsg 13
    [ProtoContract]
    public class CSvcMsgUpdateStringTable : NetMessage
    {
        public override int NetMsgType => 13;
        [ProtoMember(1)] public int? TableId { get; set; }
        [ProtoMember(2)] public int? NumChangedEntries { get; set; }
        [ProtoMember(3)] public byte[]? StringData { get; set; } // ByteString
    }

    // CSvcMsg 14
    [ProtoContract]
    public class CSvcMsgVoiceInit : NetMessage
    {
        public override int NetMsgType => 14;
        [ProtoMember(1)] public int? Quality { get; set; }
        [ProtoMember(2)] public string? Codec { get; set; }
    }

    // CSvcMsg 15
    [ProtoContract]
    public class CSvcMsgVoiceData : NetMessage
    {
        public override int NetMsgType => 15;
        [ProtoMember(1)] public int? Client { get; set; }
        [ProtoMember(2)] public bool? Proximity { get; set; }
        [ProtoMember(3)] public ulong? XUid { get; set; }
        [ProtoMember(4)] public int? AudibleMask { get; set; }
        [ProtoMember(5)] public byte[]? VoiceData { get; set; } // ByteString
    }

    // CSvcMsg 16
    [ProtoContract]
    public class CSvcMsgPrint : NetMessage
    {
        public override int NetMsgType => 16;
        [ProtoMember(1)] public string? Text { get; set; }
    }

    // CSvcMsg 17
    [ProtoContract]
    [ProtoInclude(2, typeof(SoundDataT))]
    public class CSvcMsgSounds : NetMessage
    {
        public override int NetMsgType => 17;
        [ProtoMember(1)] public bool? ReliableSound { get; set; }
        [ProtoMember(2)] public SoundDataT[] Sounds { get; set; }
    }

    [ProtoContract]
    public class SoundDataT
    {
        [ProtoMember(1)] public int? OriginX { get; set; }
        [ProtoMember(2)] public int? OriginY { get; set; }
        [ProtoMember(3)] public int? OriginZ { get; set; }
        [ProtoMember(4)] public uint? Volume { get; set; }
        [ProtoMember(5)] public float? DelayValue { get; set; }
        [ProtoMember(6)] public int? SequenceNumber { get; set; }
        [ProtoMember(7)] public int? EntityIndex { get; set; }
        [ProtoMember(8)] public int? Channel { get; set; }
        [ProtoMember(9)] public int? Pitch { get; set; }
        [ProtoMember(10)] public int? Flags { get; set; }
        [ProtoMember(11)] public uint? SoundNum { get; set; }
        [ProtoMember(12)] public uint? SoundNumHandle { get; set; }
        [ProtoMember(13)] public int? SpeakerEntity { get; set; }
        [ProtoMember(14)] public int? RandomSeed { get; set; }
        [ProtoMember(15)] public int? SoundLevel { get; set; }
        [ProtoMember(16)] public bool? IsSentence { get; set; }
        [ProtoMember(17)] public bool? IsAmbient { get; set; }
    }

    // CSvcMsg 18
    [ProtoContract]
    public class CSvcMsgSetView : NetMessage
    {
        public override int NetMsgType => 18;
        [ProtoMember(1)] public int? EntityIndex { get; set; }
    }

    // CSvcMsg 19
    [ProtoContract]
    [ProtoInclude(2, typeof(CMsgQAngle))]
    public class CSvcMsgFixAngle : NetMessage
    {
        public override int NetMsgType => 19;
        [ProtoMember(1)] public bool? Relative { get; set; }
        [ProtoMember(2)] public CMsgQAngle? Angle { get; set; }
    }

    // CSvcMsg 20
    [ProtoContract]
    [ProtoInclude(1, typeof(CMsgQAngle))]
    public class CSvcMsgCrosshairAngle : NetMessage
    {
        public override int NetMsgType => 20;
        [ProtoMember(1)] public CMsgQAngle? Angle { get; set; }
    }

    // CSvcMsg 21
    [ProtoContract]
    [ProtoInclude(1, typeof(CMsgVector))]
    public class CSvcMsgBspDecal : NetMessage
    {
        public override int NetMsgType => 21;
        [ProtoMember(1)] public CMsgVector? Pos { get; set; }
        [ProtoMember(2)] public int? DecalTextureIndex { get; set; }
        [ProtoMember(3)] public int? EntityIndex { get; set; }
        [ProtoMember(4)] public int? ModelIndex { get; set; }
        [ProtoMember(5)] public bool? LowPriority { get; set; }
    }

    // CSvcMsg 22 
    [ProtoContract]
    [ProtoInclude(1, typeof(ESplitScreenMessageType))]
    public class CSvcMsgSplitScreen : NetMessage
    {
        public override int NetMsgType => 22;
        [ProtoMember(1)] public ESplitScreenMessageType? Type { get; set; }
        [ProtoMember(2)] public int? Slot { get; set; }
        [ProtoMember(3)] public int? PlayerIndex { get; set; }
    }

    // CSvcMsg 23
    [ProtoContract]
    public class CSvcMsgUserMessage : NetMessage
    {
        public override int NetMsgType => 23;
        [ProtoMember(1)] public int? MessageType { get; set; }
        [ProtoMember(2)] public byte[]? MessageData { get; set; } // ByteString
    }

    // CSvcMsg 24
    [ProtoContract]
    public class CSvcMsgEntityMessage : NetMessage
    {
        public override int NetMsgType => 24;
        [ProtoMember(1)] public int? EntityIndex { get; set; }
        [ProtoMember(2)] public int? ClassId { get; set; }
        [ProtoMember(3)] public byte[]? EntityData { get; set; }
    }

    // CSvcMsg 25
    [ProtoContract]
    [ProtoInclude(3, typeof(KeyT))]
    public class CSvcMsgGameEvent : NetMessage
    {
        public override int NetMsgType => 25;
        [ProtoContract]
        public class KeyT
        {
            [ProtoMember(1)] public int? Type { get; set; }
            [ProtoMember(2)] public string? ValueString { get; set; }
            [ProtoMember(3)] public float? ValueFloat { get; set; }
            [ProtoMember(4)] public int? ValueLong { get; set; }
            [ProtoMember(5)] public int? ValueShort { get; set; }
            [ProtoMember(6)] public int? ValueByte { get; set; }
            [ProtoMember(7)] public bool? ValueBool { get; set; }
            [ProtoMember(8)] public ulong? ValueUInt64 { get; set; }
            [ProtoMember(9)] public byte[]? ValueWString { get; set; } // ByteString
        }

        [ProtoMember(1)] public string? EventName { get; set; }
        [ProtoMember(2)] public int EventId { get; set; }
        [ProtoMember(3)] public List<KeyT>? Keys { get; set; }
    }

    // CSvcMsg 26
    [ProtoContract]
    public class CSvcMsgPacketEntities : NetMessage
    {
        public override int NetMsgType => 26;
        [ProtoMember(1)] public int? MaxEntries { get; set; }
        [ProtoMember(2)] public int? UpdatedEntries { get; set; }
        [ProtoMember(3)] public bool IsDelta { get; set; }
        [ProtoMember(4)] public bool? UpdateBaseline { get; set; }
        [ProtoMember(5)] public int? Baseline { get; set; }
        [ProtoMember(6)] public int? DeltaFrom { get; set; }
        [ProtoMember(7)] public byte[]? EntityData { get; set; } // ByteString
    }

    // CSvcMsg 27
    [ProtoContract]
    public class CSvcMsgTempEntities : NetMessage
    {
        public override int NetMsgType => 27;
        [ProtoMember(1)] public bool? Reliable { get; set; }
        [ProtoMember(2)] public int? NumEntries { get; set; }
        [ProtoMember(3)] public byte[]? EntityData { get; set; } // ByteString
    }

    // CSvcMsg 28
    [ProtoContract]
    public class CSvcMsgPrefetch : NetMessage
    {
        public override int NetMsgType => 28;
        [ProtoMember(1)] public int? SoundIndex { get; set; }
    }

    // CSvcMsg 29
    [ProtoContract]
    public class CSvcMsgMenu : NetMessage
    {
        public override int NetMsgType => 29;
        [ProtoMember(1)] public int? DialogType { get; set; }
        [ProtoMember(2)] public byte[]? MenuKeyValues { get; set; } // ByteString
    }

    // CSvcMsg 30
    [ProtoContract]
    [ProtoInclude(1, typeof(DescriptorT))]
    public class CSvcMsgGameEventList : NetMessage
    {
        public override int NetMsgType => 30;
        [ProtoContract]
        public class KeyT
        {
            [ProtoMember(1)] public int? Type { get; set; }
            [ProtoMember(2)] public string? Name { get; set; }
        }

        [ProtoContract]
        [ProtoInclude(3, typeof(KeyT))]
        public class DescriptorT
        {
            [ProtoMember(1)] public int EventId { get; set; }
            [ProtoMember(2)] public string? Name { get; set; }
            [ProtoMember(3)] public KeyT[]? Keys { get; set; }
        }

        [ProtoMember(1)] public DescriptorT[] Descriptors { get; set; }
    }

    // CSvcMsg 31
    [ProtoContract]
    public class CSvcMsgGetCVarValue : NetMessage
    {
        public override int NetMsgType => 31;
        [ProtoMember(1)] public int? Cookie { get; set; }
        [ProtoMember(2)] public string? CvarName { get; set; }
    }

    // CSvcMsg 33
    [ProtoContract]
    public class CSvcMsgPaintMapData : NetMessage
    {
        public override int NetMsgType => 33;
        [ProtoMember(1)] public byte[] PaintMap { get; set; } // ByteString
    }

    // CSvcMsg 34 
    [ProtoContract]
    public class CSvcMsgCmdKeyValues : NetMessage
    {
        public override int NetMsgType => 34;
        [ProtoMember(1)] public byte[]? KeyValues { get; set; }
    }
}