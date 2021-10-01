using Confluent.Kafka;
using Producer_API_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer_API_.Interfaces
{
    public interface IProducerService
    {
       
        public Task<Message<string, string>> SendToKafka(MessageModel messageModel);
        public bool CheckMessageOnSpecificSymbols(string new_message);
    }
}
