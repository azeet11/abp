using TaskTimeTracker.Localization;
using Volo.Abp.Application.Services;

namespace TaskTimeTracker;

/* Inherit your application services from this class.
 */
public abstract class TaskTimeTrackerAppService : ApplicationService
{
    protected TaskTimeTrackerAppService()
    {
        LocalizationResource = typeof(TaskTimeTrackerResource);
    }
}
