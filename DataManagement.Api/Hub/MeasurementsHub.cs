using Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace DataManagement.Api
{

    public class MeasurementsHub : Hub
    {
        public MeasurementsHub() { }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Connected");
        }


        public Task Send(MeasurementDto[] measurements) =>
            Clients.All.SendAsync("TransferMeasurements", measurements);


        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }

}