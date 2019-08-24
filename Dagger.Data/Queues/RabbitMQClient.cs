using System;
using System.Threading.Tasks;

namespace Dagger.Data.Queues
{
    public class RabbitMQClient : IConsumer
    {

        public RabbitMQClient()
        {

        }

        public async Task Consume<T>(Func<T, Task> onDequeue)
        {
            
        }
    }
}