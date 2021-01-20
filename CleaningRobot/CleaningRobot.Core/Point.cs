namespace CleaningRobot.Core
{
    public class Point
    {
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public override int GetHashCode()
        {
            return this.X ^ this.Y;
        }

        public override bool Equals(object obj)
        {
            Point other = obj as Point;
            if (other == null) return false;
            return this.X == other.X && this.Y == other.Y;
        }
    }
}
