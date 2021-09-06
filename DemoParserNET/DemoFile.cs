using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using DemoParserNET.models;
using DemoParserNET.util;
using Microsoft.VisualBasic.CompilerServices;

namespace DemoParserNET
{
    public class DemoHeader
    {
        public string magic { get; init; }
        public int protocol { get; init; }
        public int networkProtocol { get; init; }
        public string serverName { get; init; }
        public string clientName { get; init; }
        public string mapName { get; init; }
        public string gameDirectory { get; init; }
        public float playbackTime { get; init; }
        public int playbackTicks { get; init; }
        public int playbackFrames { get; init; }
        public int signOnLength { get; init; }
    }

    public class DemoFile
    {
        private DemoHeader _demoHeader;
        private readonly GameEvents _gameEvents;
        private readonly Dictionary<int, BaseEntity> _entities;
        private readonly List<DataTables> _dataTables;
        private readonly StringTables _stringTables;
        private readonly UserMessages _userMessages;
        private readonly ConVars _conVars;
        private ImmutableDictionary<int, object> _frame;

        private BinaryReader _demoBuffer;
        private int _serverClassBits = 0;
        private int _currentTick = 0;
        private bool _isDemoFinished = false;

        public event EventHandler<NetMessage> MessageEvent;
        public static event EventHandler<int> TickStart;
        public static event EventHandler<int> TickEnd;
        public static event EventHandler<bool> DemoEnd;

        public DemoFile(string path)
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                _demoBuffer = new BinaryReader(stream);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Unable to open file: " + path);
            }
            finally
            {
                // No point in creating Models unless we have successfully opened 
                _gameEvents = new GameEvents();
                _entities = new Dictionary<int, BaseEntity>();
                _dataTables = new List<DataTables>();
                _stringTables = new StringTables();
                _userMessages = new UserMessages();
                _conVars = new ConVars();

                DemoReader.CSvcPacketEntitiesEvent += HandlePacketEntities;
            }
        }

        enum Message : byte
        {
            signon = 1,
            packet = 2,
            synctick = 3,
            consolecmd = 4,
            usercmd = 5,
            datatables = 6,
            stop = 7,
            customdata = 8,
            stringtables = 9,
            lastcmd = stringtables
        }

        /// <summary>
        /// Reads the CSGO Demo Header
        /// </summary>
        /// <param name="buffer">BinaryReader representation of DEMO file</param>
        /// <returns>Header object</returns>
        public void ReadHeader()
        {
            _demoHeader = new DemoHeader()
            {
                magic = new string(_demoBuffer.ReadChars(8)),
                protocol = _demoBuffer.ReadInt32(),
                networkProtocol = _demoBuffer.ReadInt32(),
                serverName = new string(_demoBuffer.ReadChars(260)).Replace("\0", String.Empty),
                clientName = new string(_demoBuffer.ReadChars(260)).Replace("\0", String.Empty),
                mapName = new string(_demoBuffer.ReadChars(260)).Replace("\0", String.Empty),
                gameDirectory = new string(_demoBuffer.ReadChars(260)).Replace("\0", String.Empty),
                playbackTime = _demoBuffer.ReadSingle(),
                playbackTicks = _demoBuffer.ReadInt32(),
                playbackFrames = _demoBuffer.ReadInt32(),
                signOnLength = _demoBuffer.ReadInt32()
            };

            Console.WriteLine("----------------");
            Console.WriteLine($"Reading {_demoHeader.clientName} on map {_demoHeader.mapName}.");
            Console.WriteLine($"Demo Length {_demoHeader.playbackTime} (Ticks: {_demoHeader.playbackTicks})");
            Console.WriteLine("----------------");
        }

        public void ParseDemo()
        {
            while (!_isDemoFinished)
            {
                byte cmd = _demoBuffer.ReadByte();
                int tick = _demoBuffer.ReadInt32();
                byte playerSlot = _demoBuffer.ReadByte();

                if (_currentTick != tick)
                {
                    TickEnd?.Invoke(this, _currentTick);
                    this._currentTick = tick;
                    TickStart?.Invoke(this, _currentTick);
                }

                switch ((Message) cmd)
                {
                    case Message.signon:
                    case Message.packet:
                        HandlePacket();
                        break;
                    case Message.synctick:
                        break;
                    case Message.consolecmd:
                        HandleDataChunk();
                        break;
                    case Message.usercmd:
                        HandleUserCmd();
                        break;
                    case Message.datatables:
                        HandleDataTables();
                        break;
                    case Message.stringtables:
                        HandleStringTables();
                        break;
                    case Message.stop:
                        Console.WriteLine("Demo STOP");
                        TickEnd?.Invoke(this, _currentTick);
                        DemoEnd?.Invoke(this, true); // bool marks completed . False for incomplete demo.
                        _isDemoFinished = true;
                        break;
                    default:
                        Console.WriteLine("Unrecognized Command");
                        break;
                }
            }
        }

        public void HandlePacket()
        {
            // Skip CMD Info (there are 152 0x00 characters before next piece of information)
            byte[] cmdInfo = _demoBuffer.ReadBytes(152);

            // Skip Sequence Info
            int seqInfo1 = _demoBuffer.ReadInt32();
            int seqInfo2 = _demoBuffer.ReadInt32();

            int chunkSize = _demoBuffer.ReadInt32();
            long chunkEnd = _demoBuffer.BaseStream.Position + chunkSize;
            while (_demoBuffer.BaseStream.Position < chunkEnd)
            {
                int cmd = ProtoReaderUtil.ReadVarInt(_demoBuffer);
                int size = ProtoReaderUtil.ReadVarInt(_demoBuffer);
                if (NetMessageParser.Continue(cmd))
                {
                    NetMessage message = NetMessageParser.ParseMessage(cmd, _demoBuffer.ReadBytes(size));
                    // Emit Message
                    MessageEvent?.Invoke(this, message);
                }
                else
                {
                    _demoBuffer.ReadBytes(size);
                }
            }
        }

        public void HandleDataTables()
        {
            int chunkSize = _demoBuffer.ReadInt32();
            long chunkEnd = _demoBuffer.BaseStream.Position + chunkSize;

            while (true)
            {
                int cmd = ProtoReaderUtil.ReadVarInt(_demoBuffer);
                int size = ProtoReaderUtil.ReadVarInt(_demoBuffer);
                CSvcMsgSendTable message =
                    (CSvcMsgSendTable) NetMessageParser.ParseMessage(cmd, _demoBuffer.ReadBytes(size));

                if (message.IsEnd.HasValue && message.IsEnd.Value)
                {
                    break;
                }

                _dataTables.Add(new DataTables()
                {
                    IsEnd = message.IsEnd,
                    NeedsDecoder = message.NeedsDecoder,
                    NetTableName = message.NetTableName,
                    Props = message.Props
                });
            }
            
            short serverClasses = _demoBuffer.ReadInt16();
            _serverClassBits = (int) Math.Ceiling(Math.Log2(serverClasses));
            for (int i = 0; i < serverClasses; i++)
            {
                short classId = _demoBuffer.ReadInt16();
                string name = ProtoReaderUtil.ReadNullTerminatedString(_demoBuffer).Trim('\0');
                string dtName = ProtoReaderUtil.ReadNullTerminatedString(_demoBuffer).Trim('\0');

                // Find DataTable
                DataTables dataTables = FindDataTableByName(dtName);
                Console.Write("");
                // Create and Add ServerClasses to List
            }
        }

        public void HandleStringTables()
        {
        }

        public void HandleDataChunk()
        {
        }

        public void HandleUserCmd()
        {
        }

        public void HandlePacketEntities(object sender, CSvcMsgPacketEntities message)
        {
            if (!message.IsDelta)
            {
                _entities.Clear();
            }

            ReadPacketEntities(sender, message);
        }

        private void ReadPacketEntities(object sender, CSvcMsgPacketEntities message)
        {
            int entityIndex = -1;
            BitReader reader = new BitReader(new MemoryStream(message.EntityData));

            for (int i = 0; i < message.UpdatedEntries; ++i)
            {
                // We need to read 6 bits
                entityIndex += 1 + reader.ReadBits(6);

                if (reader.ReadBit() ?? false)
                {
                    // remove(entityIndex)
                    if (reader.ReadBit() ?? false)
                    {
                        // remove(entityIndex);
                    }
                }
                else if (reader.ReadBit() ?? false)
                {
                    int classId = reader.ReadBits(_serverClassBits);
                    int serialNum = reader.ReadBits(10);
                }
                else
                {
                }
            }
        }

        private DataTables FindDataTableByName(string name)
        {
            return _dataTables.Find(table => table.NetTableName.Equals(name));
        }
    }
}