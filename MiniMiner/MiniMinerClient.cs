using MiniMiner.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMiner
{
   internal class MiniMinerClient
   {
      private readonly HttpClient _httpClient;
      public MiniMinerClient(HttpClient client)
      {
         _httpClient = client;
      }
      private struct Endpoints
      {
         public static readonly string GET_PROBLEM_SET = "https://hackattic.com/challenges/mini_miner/problem?access_token=8e473f1dbf696fbc";
         public static readonly string SOLVE = "https://hackattic.com/challenges/mini_miner/solve?access_token=8e473f1dbf696fbc";
      }
      public async Task<MinerProblemSet> GetProblemSet(CancellationToken cancellationToken)
      {
         var problemSetResult = await _httpClient.GetAsync(Endpoints.GET_PROBLEM_SET, cancellationToken);
         problemSetResult.EnsureSuccessStatusCode();
         var content = await problemSetResult.Content.ReadAsStringAsync(cancellationToken);
         return JsonConvert.DeserializeObject<MinerProblemSet>(content)  ?? throw new Exception("Failed to deserialize result");
      }

      public async Task<SolveResult> Solve(int nonce, CancellationToken cancellationToken)
      {
         var serializerSettings = new JsonSerializerSettings {
            Formatting = Formatting.None,
            ContractResolver = new DefaultContractResolver {
               NamingStrategy = new CamelCaseNamingStrategy()
            }
         };
         var bodyContent = JsonConvert.SerializeObject(new SolveSet(nonce), serializerSettings);
         var solveResult = await _httpClient.PostAsync(Endpoints.SOLVE, new StringContent(bodyContent), cancellationToken);
         var content = await solveResult.Content.ReadAsStringAsync(cancellationToken);
         
         return JsonConvert.DeserializeObject<SolveResult>(content) ?? throw new Exception("Failed to deserialize result");
      }
   }
}
