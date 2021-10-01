using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Producer_API_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ProducerAPI.Services
{
    public class ConsumerService : IHostedService
    {
        
        private readonly string limitations_topic = "limitations";
        private ConsumerConfig cons_config;
        private static System.Timers.Timer timer;
        public ConsumerService()
        {
            cons_config = new ConsumerConfig
            {
                GroupId = "api-consumer-group",
                BootstrapServers = "kafka:9093",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
           
        }
        public async Task StartAsync(CancellationToken cancelattionToken)
        {
            using (var consumer = new ConsumerBuilder<string, string>(cons_config).Build())
            {
                consumer.Subscribe(limitations_topic);
               
                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(cancelToken.Token);
                        if (cr != null)
                        {
                            await UpdateRestrictions(cr);
                        }
                    }

                }
                catch (Exception e)
                {
                    consumer.Close();
                }
            }
        }
        private async Task UpdateRestrictions(ConsumeResult<string,string> new_restriction)
        {
            switch (new_restriction.Message.Key)
            {
                case "Length":
                    var length = Convert.ToInt32(new_restriction.Message.Value);
                    Restrictions.MessageLength = length;
                    break;

                case "Specific symbols":
                    var tmp = new_restriction.Message.Value.ToCharArray();
                    Restrictions.SpecificSymbols = tmp;
                    break;

                case "Step time":
                    var timestep = Convert.ToInt32(new_restriction.Message.Value);
                    Restrictions.StepTime = timestep;
                    Restrictions.IsTimeStepOver = false;

                    timer = new System.Timers.Timer(Restrictions.StepTime * 1000);
                    timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    timer.AutoReset = false;
                    timer.Enabled = true;
                    break;
            }
           
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Restrictions.IsTimeStepOver = true;
            timer.Close();
        }
        public Task StopAsync(CancellationToken cancelattionToken)
        {
            return Task.CompletedTask;
        }
    }
}
