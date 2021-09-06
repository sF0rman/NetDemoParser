using System;
using System.Collections.Generic;
using System.Linq;
using DemoParserNET.Definitions;

namespace DemoParserNET.models
{
    public class GameEvents
    {
        public Dictionary<int, GameEvent> gameEventList = new Dictionary<int, GameEvent>();

        public GameEvents()
        {
            DemoReader.CSvcGameEventListEvent += HandleGameEventList;
        }

        private void HandleGameEventList(object sender, CSvcMsgGameEventList message)
        {
            foreach (var descriptor in message.Descriptors)
            {
                gameEventList.Add(descriptor.EventId, new GameEvent()
                {
                    id = descriptor.EventId,
                    name = descriptor.Name,
                    keyNames = descriptor.Keys != null ? descriptor.Keys.Select(t => t.Name).ToArray() : new string[1]{""}
                });
            }
        }
    }
}