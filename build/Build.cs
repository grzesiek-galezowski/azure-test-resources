using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

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

  Target Clean => _ => _
    .Before(Restore)
    .Executes(() =>
    {
      DotNetTasks.DotNetClean(s => s.SetConfiguration(Configuration.Debug));
      DotNetTasks.DotNetClean(s => s.SetConfiguration(Configuration.Release));
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
        .EnableNoRestore());
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
        .EnableIncludeSymbols());
    });
}