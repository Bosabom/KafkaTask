using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Threading;
namespace Consumer_console_app_
{
    public class Consumer
    {
        private readonly string limitations_topic = "limitations";
        private ConsumerConfig consumer_config;
        private ProducerConfig producer_config;

        public Consumer()
        {
            consumer_config=new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",    
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            producer_config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
        }
        public void ConsumeMessages()
        {
            using (var consumer = new ConsumerBuilder<string, string>(consumer_config).Build())
            {
                consumer.Subscribe("messages");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = consumer.Consume(cts.Token);
                            Console.WriteLine($"Consumed message: Key: '{cr.Message.Key}' , Value: '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    consumer.Close();
                }
            }
        }

        public async void ProduceLimitations(string limitationKey, string limitationValue)
        {
            
            using (var producer = new ProducerBuilder<string, string>(producer_config).Build())
            {
                try
                {
                    var dr = await producer.ProduceAsync(limitations_topic, new Message<string, string> { Key = limitationKey, Value = limitationValue });
                    Console.WriteLine($"Your limitation ({limitationKey} = {limitationValue}) was send to broker successfully.");
                }
                catch (Exception e)
                {

                    Console.WriteLine($"Error{e.Message}");
                }
            }
        }
    }
}
