using System;
using System.IO;
using DemoParserNET.models;
using DemoParserNET.util;

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
        private readonly Entities _entities;
        private readonly StringTables _stringTables;
        private readonly UserMessages _userMessages;
        private readonly ConVars _conVars;

        private BinaryReader _demoBuffer;
        private int currentTick = 0;
        private bool isDemoFinished = false;

        public event EventHandler<NetMessage> MessageEvent;
        public event EventHandler<int> TickStart;
        public event EventHandler<int> TickEnd;
        public event EventHandler<bool> DemoEnd;

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
                _entities = new Entities();
                _stringTables = new StringTables();
                _userMessages = new UserMessages();
                _conVars = new ConVars();
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
            while (!isDemoFinished)
            {
                byte cmd = _demoBuffer.ReadByte();
                int tick = _demoBuffer.ReadInt32();
                byte playerSlot = _demoBuffer.ReadByte();

                if (currentTick != tick)
                {
                    TickEnd?.Invoke(this, currentTick);
                    this.currentTick = tick;
                    TickStart?.Invoke(this, currentTick);
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
                        TickEnd?.Invoke(this, currentTick);
                        DemoEnd?.Invoke(this, true); // bool marks completed . False for incomplete demo.
                        isDemoFinished = true;
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

                // Add Datatables to List
            }

            short serverClasses = _demoBuffer.ReadInt16();
            for (int i = 0; i < serverClasses; i++)
            {
                short classId = _demoBuffer.ReadInt16();
                string name = ProtoReaderUtil.ReadNullTerminatedString(_demoBuffer);
                string dtName = ProtoReaderUtil.ReadNullTerminatedString(_demoBuffer);

                // Find DataTable
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
    }
}