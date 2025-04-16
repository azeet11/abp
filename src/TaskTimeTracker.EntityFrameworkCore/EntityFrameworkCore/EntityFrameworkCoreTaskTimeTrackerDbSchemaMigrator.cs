using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskTimeTracker.Data;
using Volo.Abp.DependencyInjection;

namespace TaskTimeTracker.EntityFrameworkCore;

public class EntityFrameworkCoreTaskTimeTrackerDbSchemaMigrator
    : ITaskTimeTrackerDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreTaskTimeTrackerDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the TaskTimeTrackerDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<TaskTimeTrackerDbContext>()
            .Database
            .MigrateAsync();
    }
}
