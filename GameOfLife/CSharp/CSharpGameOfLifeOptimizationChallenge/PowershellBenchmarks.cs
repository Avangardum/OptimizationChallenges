using System.Diagnostics;
using BenchmarkDotNet.Attributes;

namespace Avangardum.OptimizationChallenges.GameOfLife.CSharp;

[IterationCount(10)]
public class PowershellBenchmarks
{
    [Benchmark]
    public void RunPublishedCsJitIteration12()
    {
        var process = Process.Start("powershell.exe", """ cd \"C:\Users\yuryk\Desktop\Optimization Challenge Executables\" ; ./CsJitIteration12.exe """);
        process.WaitForExit();
    }
    
    [Benchmark]
    public void RunPublishedCsAotIteration12()
    {
        var process = Process.Start("powershell.exe", """ cd \"C:\Users\yuryk\Desktop\Optimization Challenge Executables\" ; ./CsAotIteration12.exe """);
        process.WaitForExit();
    }
}