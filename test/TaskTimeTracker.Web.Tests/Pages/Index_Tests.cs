using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace TaskTimeTracker.Pages;

[Collection(TaskTimeTrackerTestConsts.CollectionDefinitionName)]
public class Index_Tests : TaskTimeTrackerWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
