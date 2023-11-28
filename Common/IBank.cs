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
    public interface IBank : IService
    {
        [OperationContract]
        Task<bool> ListClients();
        [OperationContract]
        Task<bool> EnlistMoneyTransfer(int userID, double amount);
    }
}
