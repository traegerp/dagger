using System.Collections.Generic;
using System.Threading.Tasks;
using Dagger.Digraph.Graphes;

namespace Dagger.Digraph
{
    public interface IProcessor
    {
        Task Run(IEnumerable<dynamic> data,  Graphes.Graph graph);
    }

    /// <summary>
    /// Proceses a graph
    /// </summary>
    public class Processor:IProcessor
    {
        
        public Processor()
        {

        }

        public async Task Run(IEnumerable<dynamic> data, Graphes.Graph graph)
        {
            throw new System.NotImplementedException();
        }
    }
}