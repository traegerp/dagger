using System.Collections.Generic;
using System.Threading.Tasks;
using Dagger.Digraph.Graphs;

namespace Dagger.Digraph
{
    public interface IProcessor
    {
        Task Run(IEnumerable<dynamic> data,  Graph graph);
    }

    /// <summary>
    /// Proceses a graph
    /// </summary>
    public class Processor:IProcessor
    {
        
        public Processor()
        {

        }

        public async Task Run(IEnumerable<dynamic> data, Graph graph)
        {
            throw new System.NotImplementedException();
        }
    }
}