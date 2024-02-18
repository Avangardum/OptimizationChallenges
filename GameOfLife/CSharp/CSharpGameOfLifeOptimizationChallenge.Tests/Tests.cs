using System.Reflection;
using FluentAssertions;
using Xunit.Abstractions;
using static Avangardum.OptimizationChallenges.GameOfLife.CSharp.Constants;

namespace Avangardum.OptimizationChallenges.GameOfLife.CSharp.Tests;

public sealed class Tests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Tests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GliderGlides8Generations()
    {
        TestConfiguration
        (
                """
                8
                0000000000
                0010000000
                0001000000
                0111000000
                0000000000
                0000000000
                0000000000
                """, 
                """
                0000000000
                0000000000
                0000000000
                0000100000
                0000010000
                0001110000
                0000000000
                """
        );
    }
    
    [Fact]
    public void GliderGlides9Generations()
    {
        TestConfiguration
        (
            """
            9
            0000000000
            0010000000
            0001000000
            0111000000
            0000000000
            0000000000
            0000000000
            """, 
            """
            0000000000
            0000000000
            0000000000
            0000000000
            0001010000
            0000110000
            0000100000
            """
        );
    }
    
    private int ForAllIterations(Action<Action, string> action)
    {
        var iterations = Assembly.Load("CSharpGameOfLifeOptimizationChallenge").GetTypes()
            .Where(t => t.Name.StartsWith("Iteration")).ToList();
        foreach (var iteration in iterations)
        {
            Action runIteration = () => iteration.GetMethod("Run")!.Invoke(null, null);
            action(runIteration, iteration.Name);
        }
        return iterations.Count;
    }

    private void TestConfiguration(string input, string expectedOutput)
    {
        var iterations = ForAllIterations((run, name) =>
        {
            File.WriteAllText(InputPath, input);
            run();
            var actualOutput = File.ReadAllText(OutputPath);
            actualOutput.Should().Be(expectedOutput, $"test should pass in {name}");
        });
        _testOutputHelper.WriteLine($"Ran {iterations} iterations.");
    }
}