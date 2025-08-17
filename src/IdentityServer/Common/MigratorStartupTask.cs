using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Data;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Common
{
    public class MigratorStartupTask : IStartupTask
    {
        private readonly IServiceProvider _serviceProvider;
        
        public MigratorStartupTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using var scope = _serviceProvider.CreateScope();
            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                
            await initialiser.InitialiseAsync();
            await initialiser.SeedAsync();
        }
    }
}