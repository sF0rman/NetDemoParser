using DemoParserNET.models;

namespace DemoParserNET.Definitions
{
    public class GameEvent
    {
        public string name { get; init; }
        public int id { get; init; }
        public string[] keyNames { get; init; }
    }
}