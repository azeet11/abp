using TaskTimeTracker.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace TaskTimeTracker.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class TaskTimeTrackerController : AbpControllerBase
{
    protected TaskTimeTrackerController()
    {
        LocalizationResource = typeof(TaskTimeTrackerResource);
    }
}
