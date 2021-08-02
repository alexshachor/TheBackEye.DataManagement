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

    public class StreamHub : Hub
    {
        public static IDictionary<string, TimerManager> clientConnections =
            new Dictionary<string, TimerManager>();
        private static string[] newDataArray;

        public StreamHub() {}

        //public async void UpdateParameters(int interval, int volume, bool live = false, bool updateAll = true)
        //{
        //    newDataArray = new string[] {"a","b","c" };
        //    var connection = Context.ConnectionId;
        //    var clients = Clients;

        //    if (live)
        //    {
        //        if (!clientConnections.ContainsKey(connection))
        //        {
        //            clientConnections.Add(connection, new TimerManager(async () =>
        //            {
        //                var client = clients.Client(connection);
        //                await Send(newDataArray, client, connection);
        //            }, interval));
        //        }
        //        else
        //        {
        //            clientConnections[connection].Stop();
        //            clientConnections[connection] = new TimerManager(async () =>
        //            {
        //                var client = clients.Client(connection);
        //                await Send(newDataArray, client, connection);
        //            }, interval);
        //        }
        //    }
        //    else
        //    {
        //        var client = clients.Client(connection);
        //        await Send(newDataArray, client, connection);
        //    }
        //}

       
        public async Task Send(MeasurementDto[] measurements, IClientProxy client, string connection)
        {
            await client.SendAsync("TransferMeasurements", measurements);
        }

        public void StopTimer()
        {
            TimerManager timerManager;
            clientConnections.TryGetValue(Context.ConnectionId, out timerManager);
            if (timerManager != null)
            {
                timerManager.Stop();
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            StopTimer();
            clientConnections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }

}