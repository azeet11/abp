using Volo.Abp.Modularity;

namespace TaskTimeTracker;

[DependsOn(
    typeof(TaskTimeTrackerDomainModule),
    typeof(TaskTimeTrackerTestBaseModule)
)]
public class TaskTimeTrackerDomainTestModule : AbpModule
{

}
