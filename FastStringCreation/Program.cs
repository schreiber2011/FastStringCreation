using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace FastStringCreation
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchy>();
            // var benchy = new Benchy();
            // benchy.MaskStringCreate();
        }
        
        //  RUN at 16 Sep 2021
        // |            Method |      Mean |    Error |   StdDev |  Gen 0 | Allocated |
        // |------------------ |----------:|---------:|---------:|-------:|----------:|
        // |         MaskNaive | 207.46 ns | 3.365 ns | 2.983 ns | 0.0508 |     400 B |
        // | MaskStringBuilder |  78.51 ns | 1.245 ns | 1.104 ns | 0.0234 |     184 B |
        // |     MaskNewString |  44.97 ns | 0.550 ns | 0.429 ns | 0.0153 |     120 B |
        // |  MaskStringCreate |  19.61 ns | 0.387 ns | 0.323 ns | 0.0061 |      48 B |

    }

    [MemoryDiagnoser]
    public class Benchy
    {
        private const string ClearValue = "Password123!";

        [Benchmark]
        public string MaskNaive()
        {
            var firstChars = ClearValue[..3];  // Substring(0, 3);
            var length = ClearValue.Length - 3;

            for (var i = 0; i < length; i++)
            {
                firstChars += '*';
            }

            return firstChars;
        }
        
        [Benchmark]
        public string MaskStringBuilder()
        {
            var firstChars = ClearValue[..3];  // Substring(0, 3);
            var length = ClearValue.Length - 3;
            var stringBuilder = new StringBuilder(firstChars);

            for (var i = 0; i < length; i++)
            {
                stringBuilder.Append('*');
            }

            return stringBuilder.ToString();
        }
        
        [Benchmark]
        public string MaskNewString()
        {
            var firstChars = ClearValue[..3];  // Substring(0, 3);
            var length = ClearValue.Length - 3;
            var asterisks = new string('*', length);
            return firstChars + asterisks;
        }
        
        [Benchmark]
        public string MaskStringCreate()
        {
            return string.Create(ClearValue.Length, ClearValue, (span, value) =>
            {
                value.AsSpan().CopyTo(span);
                span[3..].Fill('*');
            });
        }
        
    }
}
