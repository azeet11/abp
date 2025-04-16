using TaskTimeTracker.Samples;
using Xunit;

namespace TaskTimeTracker.EntityFrameworkCore.Domains;

[Collection(TaskTimeTrackerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<TaskTimeTrackerEntityFrameworkCoreTestModule>
{

}
