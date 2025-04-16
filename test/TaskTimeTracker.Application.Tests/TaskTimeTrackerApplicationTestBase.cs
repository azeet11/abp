using Volo.Abp.Modularity;

namespace TaskTimeTracker;

public abstract class TaskTimeTrackerApplicationTestBase<TStartupModule> : TaskTimeTrackerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
