using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using TaskTimeTracker.Localization;

namespace TaskTimeTracker.Web;

[Dependency(ReplaceServices = true)]
public class TaskTimeTrackerBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<TaskTimeTrackerResource> _localizer;

    public TaskTimeTrackerBrandingProvider(IStringLocalizer<TaskTimeTrackerResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
