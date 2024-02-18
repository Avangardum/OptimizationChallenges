using Avangardum.OptimizationChallenges.GameOfLife.CSharp;
using Avangardum.OptimizationChallenges.GameOfLife.CSharp.Iterations;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchmarks>();

// InputGenerator.GenerateInput(100, 100000);
// Iteration1.Run();