using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Validator
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Validator : StatelessService, IValidator
    {
        public Validator(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<string> Validate(DataModel model)
        {
            if (model == null)
            {
                Console.WriteLine("Model je null");
                return "Model je null";
            }

            if (model.Ime == null || model.Ime.Equals(string.Empty) || model.Prezime == null ||
                model.Prezime.Equals(string.Empty) || model.IdKnjige <= 0 || model.KolicinaKnjige <= 0 || model.BrojRacuna <= 0)
            {
                Console.WriteLine("Ulazne vrijednosti nisu u redu.");
                return "Ulazne vrijednosti nisu u redu.";
            }

            var fabricClient = new FabricClient();

            var serviceUri = new Uri("fabric:/CloudZadatak/TransactionCoordinator");
            var partitionList = await fabricClient.QueryManager.GetPartitionListAsync(serviceUri);
            ITransaction proxy = null;

            foreach (var partition in partitionList)
            {
                var partitionKey = partition.PartitionInformation as Int64RangePartitionInformation;

                if (partitionKey != null)
                {
                    var servicePartitionKey = new ServicePartitionKey(partitionKey.LowKey);

                    proxy = ServiceProxy.Create<ITransaction>(serviceUri, servicePartitionKey);
                    break;
                }
            }


            try
            {
                var prepareValue = await proxy.Prepare(model);

                if (prepareValue)
                {
                    return "Ok";
                }
                else
                {
                    return "Doslo je do greske pri pozivu funkcije Prepare";
                }
            }
            catch (Exception)
            {
                //await proxy.RollBack();
                return "Transaction failed";
            }

            /*try
            {
                var proxy = ServiceProxy.Create<Common.ITransaction>(new Uri("fabric:/CloudZadatak/TransactionCoordinator"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));

                return await proxy.Prepare(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }*/
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
