using System.Diagnostics;
using System.Text;
using Avangardum.OptimizationChallenges.GameOfLife.CSharp.Iterations;
using BenchmarkDotNet.Attributes;
using static Avangardum.OptimizationChallenges.GameOfLife.CSharp.Constants;

namespace Avangardum.OptimizationChallenges.GameOfLife.CSharp;

[MemoryDiagnoser]
public class Benchmarks
{
    [Params(100)]
    public int Size { get; set; }
    
    [Params(10000, 100000)]
    public int Generations { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        InputGenerator.GenerateInput(Size, Generations);
    }

    // [Benchmark]
    // public void RunIteration1() => Iteration1.Run();

    // [Benchmark]
    // public void RunIteration2() => Iteration2.Run();

    // [Benchmark]
    // public void RunIteration3() => Iteration3.Run();

    // [Benchmark]
    // public void RunIteration4() => Iteration4.Run();

    // [Benchmark]
    // public void RunIteration5() => Iteration5.Run();

    // [Benchmark]
    // public void RunIteration6() => Iteration6.Run();

    // [Benchmark]
    // public void RunIteration7() => Iteration7.Run();

    // [Benchmark]
    // public void RunIteration8() => Iteration8.Run();

    // [Benchmark]
    // public void RunIteration9() => Iteration9.Run();

    // [Benchmark]
    // public void RunIteration10() => Iteration10.Run();

    // [Benchmark]
    // public void RunIteration11() => Iteration11.Run();

    [Benchmark]
    public void RunIteration12() => Iteration12.Run();
    
    // [Benchmark]
    // public void RunIteration13() => Iteration13.Run();
    
    [Benchmark]
    public void RunIteration14() => Iteration14.Run();
}