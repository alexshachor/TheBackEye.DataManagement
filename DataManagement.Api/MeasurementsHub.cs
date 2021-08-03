using Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace DataManagement.Api
{

    public class MeasurementsHub : Hub
    {
        public MeasurementsHub() {}


       
        public async Task Send(MeasurementDto[] measurements, IClientProxy client, string connection)
        {
            await client.SendAsync("TransferMeasurements", measurements);
        }

        
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }

}