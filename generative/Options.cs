namespace generative
{
    sealed class Options
    {
        [Option("--seed")]
        public int Seed { get; set; }

        [Option("--hideseed")]
        public bool HideSeed { get; set; }
    }
}
