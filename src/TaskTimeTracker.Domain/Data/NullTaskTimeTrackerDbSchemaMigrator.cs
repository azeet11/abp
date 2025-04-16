using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace TaskTimeTracker.Data;

/* This is used if database provider does't define
 * ITaskTimeTrackerDbSchemaMigrator implementation.
 */
public class NullTaskTimeTrackerDbSchemaMigrator : ITaskTimeTrackerDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
