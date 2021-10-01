using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Producer_API_.Interfaces;
using Producer_API_.Models;

namespace Producer_API_.Services
{
    public class ProducerService : IProducerService
    {
        private readonly string messages_topic = "messages";
       
        private ProducerConfig prod_config;

        public ProducerService()
        {
             prod_config=new ProducerConfig
             { BootstrapServers = "kafka:9093" };
        } 
        public async Task<Message<string, string>> SendToKafka(MessageModel message)
        {

            using (var producer = new ProducerBuilder<string, string>(prod_config).Build())
            {
                try
                {   //validation(message length and wrong symbols)
                    if (message.Value.Length < Restrictions.MessageLength && CheckMessageOnSpecificSymbols(message.Value))
                    {
                        var dr = await producer.ProduceAsync(messages_topic, new Message<string, string> { Key = message.Key, Value = message.Value });
                        return dr.Message;
                    }
                    else
                    {
                        return new Message<string, string> { Key = "Validation error", Value = "Incorrect data. Please, try again!" };
                    }

                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public bool CheckMessageOnSpecificSymbols(string new_message)
        {
            foreach (var ch in Restrictions.SpecificSymbols)
            {
                if (new_message.Contains(ch))
                    return false;
            }
            return true;
        }

    }
}

