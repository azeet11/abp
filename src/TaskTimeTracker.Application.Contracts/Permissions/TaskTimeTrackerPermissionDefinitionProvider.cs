using TaskTimeTracker.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace TaskTimeTracker.Permissions;

public class TaskTimeTrackerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(TaskTimeTrackerPermissions.GroupName);

        // Define Project permissions
        var projectPermission = group.AddPermission(TaskTimeTrackerPermissions.Projects.Default, L("Permission:Projects"));
        projectPermission.AddChild(TaskTimeTrackerPermissions.Projects.Create, L("Permission:Create"));
        projectPermission.AddChild(TaskTimeTrackerPermissions.Projects.Update, L("Permission:Update"));
        projectPermission.AddChild(TaskTimeTrackerPermissions.Projects.Delete, L("Permission:Delete"));

        // Define Task permissions
        var taskPermission = group.AddPermission(TaskTimeTrackerPermissions.Tasks.Default, L("Permission:Tasks"));
        taskPermission.AddChild(TaskTimeTrackerPermissions.Tasks.Create, L("Permission:Create"));
        taskPermission.AddChild(TaskTimeTrackerPermissions.Tasks.Update, L("Permission:Update"));
        taskPermission.AddChild(TaskTimeTrackerPermissions.Tasks.Delete, L("Permission:Delete"));

        // Define Time Tracking permissions
        var timeTrackingPermission = group.AddPermission(TaskTimeTrackerPermissions.TimeTracking.Default, L("Permission:TimeTracking"));
        timeTrackingPermission.AddChild(TaskTimeTrackerPermissions.TimeTracking.Create, L("Permission:Create"));
        timeTrackingPermission.AddChild(TaskTimeTrackerPermissions.TimeTracking.Update, L("Permission:Update"));
        timeTrackingPermission.AddChild(TaskTimeTrackerPermissions.TimeTracking.Delete, L("Permission:Delete"));

        // Define Reporting permissions
        var reportingPermission = group.AddPermission(TaskTimeTrackerPermissions.Reporting.Default, L("Permission:Reporting"));
        reportingPermission.AddChild(TaskTimeTrackerPermissions.Reporting.View, L("Permission:View"));
        reportingPermission.AddChild(TaskTimeTrackerPermissions.Reporting.Filter, L("Permission:Filter"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TaskTimeTrackerResource>(name);
    }
}
