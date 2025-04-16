using TaskTimeTracker.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace TaskTimeTracker.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(TaskTimeTrackerEntityFrameworkCoreModule),
    typeof(TaskTimeTrackerApplicationContractsModule)
)]
public class TaskTimeTrackerDbMigratorModule : AbpModule
{
}
