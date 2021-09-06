using System;
using System.Collections.Generic;
using System.Net.Mime;
using DemoParserNET.models;

namespace DemoParserNET.models
{
    enum EventKeyType
    {
        STRING = 1,
        FLOAT,
        LONG,
        SHORT,
        BYTE,
        BOOL,
        UINT64,
        WSTRING
    }

    public class GameEvent
    {
        public string name { get; init; }
        public int id { get; init; }
        public string[] keyNames { get; init; }

        public class Event
        {
            public string Name { get; set; }
            public Dictionary<string, string> Value { get; set; } = new();
        }

        public Event MessageToObject(CSvcMsgGameEvent message)
        {
            if (message.EventId != this.id)
            {
                throw new Exception("Game Event ID Mismatch");
            }

            if (message.Keys == null)
            {
                throw new Exception("No Event Keys");
            }

            Event gameEvent = new Event();
            for (int i = 0; i < message.Keys.Count; i++)
            {
                CSvcMsgGameEvent.KeyT key = message.Keys[i];
                gameEvent.Name = name;
                switch ((EventKeyType) key.Type)
                {
                    case EventKeyType.STRING:
                        gameEvent.Value.Add(keyNames[i], key.ValueString);
                        break;
                    case EventKeyType.FLOAT:
                        gameEvent.Value.Add(keyNames[i], key.ValueFloat.ToString());
                        break;
                    case EventKeyType.LONG:
                        gameEvent.Value.Add(keyNames[i], key.ValueLong.ToString());
                        break;
                    case EventKeyType.SHORT:
                        gameEvent.Value.Add(keyNames[i], key.ValueShort.ToString());
                        break;
                    case EventKeyType.BYTE:
                        gameEvent.Value.Add(keyNames[i], key.ValueByte.ToString());
                        break;
                    case EventKeyType.BOOL:
                        gameEvent.Value.Add(keyNames[i], key.ValueBool.ToString());
                        break;
                    case EventKeyType.UINT64:
                        gameEvent.Value.Add(keyNames[i], key.ValueUInt64.ToString());
                        break;
                    case EventKeyType.WSTRING:
                        gameEvent.Value.Add(keyNames[i], System.Text.Encoding.Default.GetString(key.ValueWString));
                        break;
                }
            }

            return gameEvent;
        }
    }
}