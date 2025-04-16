using TaskTimeTracker.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace TaskTimeTracker.Web.Pages;

public abstract class TaskTimeTrackerPageModel : AbpPageModel
{
    protected TaskTimeTrackerPageModel()
    {
        LocalizationResourceType = typeof(TaskTimeTrackerResource);
    }
}
