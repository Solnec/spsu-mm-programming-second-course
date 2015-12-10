using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MyWCFService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
       int[] Sort(int[] A);
    }
}
