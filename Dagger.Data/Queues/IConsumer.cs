using System;
using System.Threading.Tasks;

namespace Dagger.Data.Queues
{
    public interface IConsumer
    {
        Task Consume<T>(Func<T> onDequeue);     
    }
}