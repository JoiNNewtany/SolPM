namespace SolPM.Core.ViewModels.Parameters
{
    public class PwdGenParams
    {
        public int Length { get; set; }
        public bool UpperCase { get; set; }
        public bool LowerCase { get; set; }
        public bool Numbers { get; set; }
        public bool Symbols { get; set; }
        public bool Consecutive { get; set; }
        public string Excluded { get; set; }
    }
}
