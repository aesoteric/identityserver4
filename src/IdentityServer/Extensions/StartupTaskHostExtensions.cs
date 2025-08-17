using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.Extensions
{
    public static class StartupTaskHostExtensions
    {
        public static async Task RunWithTasksAsync(this IHost host, CancellationToken cancellationToken = default)
        {
            var startupTasks = host.Services.GetServices<IStartupTask>();
            
            foreach (var startupTask in startupTasks)
            {
                await startupTask.ExecuteAsync(cancellationToken);
            }
            
            await host.RunAsync(cancellationToken);
        }
    }
}