using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace StressTesting
{
    [ServiceContract]
    interface ISortService
    {
        [OperationContract]
        int[] Sort(int[] array);
    }
}
