using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiniMiner.Models
{
   internal record MinerProblemSet(int Difficulty, Block Block);

   internal class Block {
      [JsonProperty(Order = 1, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
      public object[] Data { get; init; } = null!;

      [JsonProperty(Order = 2, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
      public int? Nonce { get; init; }
   }
}
