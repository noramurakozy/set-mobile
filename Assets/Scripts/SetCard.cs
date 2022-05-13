namespace DefaultNamespace
{
    public class SetCard
    {
        public Fill Fill { get; set; }
        public Color Color { get; set; }
        public Number Number { get; set; }
        public Shape Shape { get; set; }
        
        public int Index { get; set; }

        public SetCard(Fill f, Color c, Number n, Shape s, int index)
        {
            Fill = f;
            Color = c;
            Number = n;
            Shape = s;
            Index = index;
        }
    }
}