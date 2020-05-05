using System;
using System.Collections.Generic;
using System.Text;

namespace SolPM.Core.Interactions
{
    public class MessageInteraction
    {
        public string Message { get; set; }

        public MessageInteraction(string message)
        {
            Message = message;
        }
    }
}
