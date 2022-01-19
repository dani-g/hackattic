// See https://aka.ms/new-console-template for more information

using MiniMiner;
using Newtonsoft.Json;

using HttpClient httpClient = new();
var miniMinerClient = new MiniMinerClient(httpClient);

var problemSet = await miniMinerClient.GetProblemSet(CancellationToken.None);
Console.WriteLine("GOT PROBLEM SET WITH DIFFICULTY: " + problemSet.Difficulty);
Console.WriteLine("-------------------------------------------------------------");

// solve
var nonce = NonceSolver.GetNonceForProblem(problemSet);
if(nonce == -1)
{
   Console.WriteLine("Failed to find nonce");
   return;
}
Console.WriteLine(nonce);
Console.WriteLine();

// send response
var result = await miniMinerClient.Solve(nonce, CancellationToken.None);
Console.WriteLine(result.Result);
Console.WriteLine(result.Rejected);
