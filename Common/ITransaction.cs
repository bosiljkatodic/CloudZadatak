using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ITransaction : IService
    {
        [OperationContract]
        Task<bool> Prepare(DataModel model);
        [OperationContract]
        Task Commit();
        [OperationContract]
        Task Rollback();
    }
}
