﻿using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace BenchmarkDotNet.Samples.Intro
{
    [Config(typeof(Config))]
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    public class IntroGcMode
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.MediumRun.WithGcServer(true).WithGcForce(true).WithId("ServerForce"));
                Add(Job.MediumRun.WithGcServer(true).WithGcForce(false).WithId("Server"));
                Add(Job.MediumRun.WithGcServer(false).WithGcForce(true).WithId("Workstation"));
                Add(Job.MediumRun.WithGcServer(false).WithGcForce(false).WithId("WorkstationForce"));
#if !CORE
                Add(new Diagnostics.Windows.MemoryDiagnoser());
#endif
            }
        }

        [Benchmark(Description = "new byte[10KB]")]
        public byte[] Allocate()
        {
            return new byte[10000];
        }

        [Benchmark(Description = "stackalloc byte[10KB]")]
        public unsafe void AllocateWithStackalloc()
        {
            var array = stackalloc byte[10000];
            Consume(array);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe void Consume(byte* input)
        {
        }
    }
}