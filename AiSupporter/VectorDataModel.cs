using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiSupporter
{
    public class VectorDataModel
    {
        [VectorStoreKey]
        [TextSearchResultName]
        public required string Id { get; set; }

        [VectorStoreData]
        [TextSearchResultValue]
        public required string Content { get; set; }

        [VectorStoreVector(Dimensions: 3072, DistanceFunction = DistanceFunction.CosineSimilarity)]
        public ReadOnlyMemory<float> EmbeddingContent { get; set; }
    }
}
