using Volo.Abp.Modularity;

namespace TaskTimeTracker;

/* Inherit from this class for your domain layer tests. */
public abstract class TaskTimeTrackerDomainTestBase<TStartupModule> : TaskTimeTrackerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
