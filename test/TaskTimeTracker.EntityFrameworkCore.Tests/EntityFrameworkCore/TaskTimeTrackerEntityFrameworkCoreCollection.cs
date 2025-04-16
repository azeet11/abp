using Xunit;

namespace TaskTimeTracker.EntityFrameworkCore;

[CollectionDefinition(TaskTimeTrackerTestConsts.CollectionDefinitionName)]
public class TaskTimeTrackerEntityFrameworkCoreCollection : ICollectionFixture<TaskTimeTrackerEntityFrameworkCoreFixture>
{

}
