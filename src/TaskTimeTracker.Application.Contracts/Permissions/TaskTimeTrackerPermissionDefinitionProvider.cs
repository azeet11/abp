using TaskTimeTracker.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace TaskTimeTracker.Permissions;

public class TaskTimeTrackerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(TaskTimeTrackerPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(TaskTimeTrackerPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TaskTimeTrackerResource>(name);
    }
}
