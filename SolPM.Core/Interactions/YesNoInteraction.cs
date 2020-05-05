using System;

namespace SolPM.Core.Interactions
{
    public class YesNoInteraction
    {
        public Action<bool> YesNoCallback { get; set; }
        public string Message { get; set; }
    }
}