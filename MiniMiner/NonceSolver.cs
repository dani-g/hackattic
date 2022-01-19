using MiniMiner.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MiniMiner
{
   internal class NonceSolver
   {
      private const int MAX_VALUE = 100000;
      public static int GetNonceForProblem(MinerProblemSet problemSet)
      {
         using var sha256Crypt = SHA256.Create();
         for(var i = 1; i < MAX_VALUE; i++)
         {
            var testBlock = new Block { Nonce = i, Data = problemSet.Block.Data };
            var serialized = JsonConvert.SerializeObject(testBlock, Formatting.None);
            var bytes = Encoding.UTF8.GetBytes(serialized);
            var hash = sha256Crypt.ComputeHash(bytes);
            if (ValidateHash(hash, problemSet.Difficulty))
            {
               return i;
            }
         }
         return -1;
      }
      

      private static bool ValidateHash(byte[] hash, int numberOfZeroBits)
      {
         var numberBytesNeededToCheck  = (int)Math.Ceiling(numberOfZeroBits / 8D);
         string binary = string.Empty;
         for (var i = 0; i < hash.Length; i++)
         {
            binary += Convert.ToString(hash[i], 2).PadLeft(8, '0');
         }
         return binary.StartsWith(string.Empty.PadLeft(numberOfZeroBits, '0'));
      }
   }
}
