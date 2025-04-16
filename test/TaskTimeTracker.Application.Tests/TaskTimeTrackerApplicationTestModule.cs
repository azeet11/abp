using Volo.Abp.Modularity;

namespace TaskTimeTracker;

[DependsOn(
    typeof(TaskTimeTrackerApplicationModule),
    typeof(TaskTimeTrackerDomainTestModule)
)]
public class TaskTimeTrackerApplicationTestModule : AbpModule
{

}
