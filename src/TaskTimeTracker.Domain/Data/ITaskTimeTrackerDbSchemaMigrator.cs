using System.Threading.Tasks;

namespace TaskTimeTracker.Data;

public interface ITaskTimeTrackerDbSchemaMigrator
{
    Task MigrateAsync();
}
