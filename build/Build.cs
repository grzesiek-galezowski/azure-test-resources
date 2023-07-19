using System.IO;
using GlobExpressions;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

[GitHubActions(
  "continuous",
  GitHubActionsImage.UbuntuLatest,
  On = new[] { GitHubActionsTrigger.Push },
  InvokedTargets = new[] { nameof(Compile) })]
class Build : NukeBuild
{
  public static int Main () => Execute<Build>(x => x.Test);

  readonly Configuration Configuration = Configuration.Release;

  [Solution]
  readonly Solution Solution;

  AbsolutePath ArtifactsDirectory => Solution.Directory / "Nugets";

  const string VersionPrefix = "0.2.0";

  Target Clean => _ => _
    .Before(Restore)
    .Executes(() =>
    {
      DotNetTasks.DotNetClean(s => s.SetConfiguration(Configuration.Debug));
      DotNetTasks.DotNetClean(s => s.SetConfiguration(Configuration.Release));
      if (Directory.Exists(ArtifactsDirectory))
      {
        Directory.Delete(ArtifactsDirectory, true);
      }
    });

  Target Restore => _ => _
    .Executes(() =>
    {
      DotNetTasks.DotNetRestore(s => s.SetProjectFile(Solution));
    });

  Target Compile => _ => _
    .DependsOn(Restore)
    .Executes(() =>
    {
      DotNetTasks.DotNetBuild(s => s
        .SetProjectFile(Solution)
        .SetConfiguration(Configuration.Release)
        .EnableNoRestore()
        .SetVersionPrefix(VersionPrefix));
    });

  Target Test => _ => _
    .DependsOn(Compile)
    .Executes(() =>
    {
      DotNetTasks.DotNetTest(s => s
        .SetProjectFile(Solution)
        .SetConfiguration(Configuration.Release)
        .EnableNoRestore());
    });

  Target Pack => _ => _
    .DependsOn(Compile)
    .Executes(() =>
    {
      DotNetTasks.DotNetPack(s => s
        .SetProject(Solution)
        .SetConfiguration(Configuration.Release)
        .EnableNoRestore()
        .EnableNoBuild()
        .EnableIncludeSymbols()
        .SetVersionPrefix(VersionPrefix)
        .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
        .SetOutputDirectory(ArtifactsDirectory));
    });

  Target Push => _ => _
    .DependsOn(Pack)
    .Executes(() =>
    {
      ArtifactsDirectory.GlobFiles($"*{VersionPrefix}.nupkg", $"*{VersionPrefix}.snupkg").ForEach(path =>
      {
        DotNetTasks.DotNetNuGetPush(s => s.SetTargetPath(path)
          .SetSource("https://api.nuget.org/v3/index.json"));
      });
    });
}