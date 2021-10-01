using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Producer_API_.Models;
using Producer_API_.Interfaces;

namespace Producer_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {

        private IProducerService prod_service;
        public ProducerController(IProducerService service)
        {
            prod_service = service;
        }
  
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MessageModel message)
        {
            
            if(!Restrictions.IsTimeStepOver)
            {
                throw new ArgumentException($"Time limit for future requests isn't over!");
            }
           
            var send_message = await prod_service.SendToKafka(message);
            return Created(string.Empty, send_message);

        }

    }
}
