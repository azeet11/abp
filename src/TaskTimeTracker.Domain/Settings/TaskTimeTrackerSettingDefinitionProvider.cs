using Volo.Abp.Settings;

namespace TaskTimeTracker.Settings;

public class TaskTimeTrackerSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(TaskTimeTrackerSettings.MySetting1));
    }
}
