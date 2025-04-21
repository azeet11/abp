namespace TaskTimeTracker.Permissions;

public static class TaskTimeTrackerPermissions
{
    public const string GroupName = "TaskTimeTracker";

    public static class Projects
    {
        public const string Default = GroupName + ".Project";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Tasks
    {
        public const string Default = GroupName + ".Tasks";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class TimeTracking
    {
        public const string Default = GroupName + ".TimeTracking";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Reporting
    {
        public const string Default = GroupName + ".Reporting";
        public const string View = Default + ".View";
        public const string Filter = Default + ".Filter";
    }
}
