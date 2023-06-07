global using NUnit.Framework;

#if NCRUNCH
#else
  [assembly: Parallelizable(ParallelScope.All)]
  [assembly: LevelOfParallelism(16)]
#endif

