using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
namespace WCFService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        int[,] MatrixMultiplication(int[,] m1, int[,] m2);
    }
}
