namespace AsianOptions.ViewModel
{
    public class MainWindowViewModel
    {
        public double InitialPrice { get; set; }
        public double ExercisePrice { get; set; }
        public double UpGrowth { get; set; }
        public double DownGrowth { get; set; }
        public double InterestRate { get; set; }
        public long Periods { get; set; }
        public long Simulations { get; set; }

        public MainWindowViewModel()
        {
            InitialPrice = 30.0;
            ExercisePrice = 30.0;
            UpGrowth = 1.4;
            DownGrowth = 0.8;
            InterestRate = 1.08;
            Periods = 30;
            Simulations = 5_000_000;
        }
    }
}
