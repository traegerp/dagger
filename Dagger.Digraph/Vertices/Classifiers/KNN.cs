using System.Linq;
using System.Collections.Generic;
using Dagger.Utils.Math.Extensions;
using Dagger.Utils.Math;
using Mathematics = System.Math;

namespace Dagger.Digraph.Vertices.Classifiers
{

    /// <summary>
    /// Calculate K- Nearest Neighbors for non-parametric classification
    /// </summary>
    public class KNN
    {

        /// <summary>
        /// Ctor
        /// </summary>
        public KNN()
        {

        }

        /// <summary>
        /// Classify With KNN
        /// </summary>
        /// <param name="predictor"></param>
        /// <param name="neighbors"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Predictor<int> Classify(Predictor<int> predictor, IEnumerable<Neighbor<int>> neighbors, int min, int max)
        {            

            var totalClasses = (int)Mathematics.Ceiling(Mathematics.Sqrt(neighbors.GroupBy(x => x.Classifier).Count()));            

            var results = new List<Result<int>>();

            int k = totalClasses % 2 == 0 ? totalClasses + 1 : totalClasses; //k should always be odd

            //normalize data
            var data = neighbors.Select(x => new Neighbor<int>(){
                ExternalId = x.ExternalId,
                Classifier = x.Classifier,
                Features = x.Features.Normalize(max, min)
            });            

            predictor.Features = predictor.Features.Normalize(max, min);

            //calculate euclidean distance
            foreach(var item in data)
            {
                results.Add(
                    new Result<int>()
                    {
                        Value = Distance.CalculateEuclidean(predictor.Features.ToArray(), item.Features.ToArray()),
                        Neighbor = item
                    }
                );
                
            }

            //get nearest neighbors
            var topResults = results.OrderByDescending(x => x.Value).Take(k);

            //get majority vote by neighbors
            var classes = topResults.GroupBy(x => x.Neighbor.Classifier).OrderByDescending(x => x.Count());

            //set classifier prediction
            predictor.Prediction = classes.SingleOrDefault().SingleOrDefault().Neighbor.Classifier;

            return predictor;
        }

    }


    internal class Result<T>
    {
        public decimal Value {get; set;}
        public Neighbor<T> Neighbor {get; set;}
    }

    public class Predictor<T>:Neighbor<T>
    {
        public string Prediction {get; set;}
    }

    public class Neighbor<T>
    {
        public string ExternalId {get; set;}
        public string Classifier {get; set;}
        public IEnumerable<T> Features {get; set;}
    }

}