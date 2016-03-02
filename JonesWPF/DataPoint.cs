namespace JonesWPF
{
    class DataPoint
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Temperature { get; set; }
        public int Time { get; set; }
        public int Density { get; set; }
        public int RockType { get; set; }
        public int WaterContent { get; set; }
        public int Viscosity { get; set; }
        public int RelativeDeformation { get; set; }

        public DataPoint()
        { }
        public DataPoint(int id, int temperature, int time, int x, int y)
        {
            Id = id;
            Temperature = temperature;
            Time = time;
            X = x;
            Y = y;
        }
    }
}

