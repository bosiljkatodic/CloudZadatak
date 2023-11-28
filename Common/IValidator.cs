using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    public interface IValidator : IService
    {
        [OperationContract]
        Task<string> Validate(DataModel model);
    }
}
