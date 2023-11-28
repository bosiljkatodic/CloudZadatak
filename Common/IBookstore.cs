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
    public interface IBookstore : IService
    {
        [OperationContract]
        Task ListAvailableItems();
        [OperationContract]
        Task EnlistPurchase(int bookId, int bookCount);
        [OperationContract]
        Task<double> GetItemPrice(int bookID);
    }
}
