using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WebService
{
    [ServiceContract]
    public interface IWebService
    {
        [OperationContract]
        int[] BubbleSort(int[] inputArray);
    }
}
