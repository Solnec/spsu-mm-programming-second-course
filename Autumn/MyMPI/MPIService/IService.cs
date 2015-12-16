using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MPIService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        int[] Sort(int[] A);
    }
}
