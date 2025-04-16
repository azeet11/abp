using Microsoft.AspNetCore.Builder;
using TaskTimeTracker;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("TaskTimeTracker.Web.csproj"); 
await builder.RunAbpModuleAsync<TaskTimeTrackerWebTestModule>(applicationName: "TaskTimeTracker.Web");

public partial class Program
{
}
