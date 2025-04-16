using TaskTimeTracker.Samples;
using Xunit;

namespace TaskTimeTracker.EntityFrameworkCore.Applications;

[Collection(TaskTimeTrackerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<TaskTimeTrackerEntityFrameworkCoreTestModule>
{

}
