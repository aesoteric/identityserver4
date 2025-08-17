using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Common
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}