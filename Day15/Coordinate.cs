namespace Day15
{
    public struct Coordinate(long x, long y)
    {
        public long X { get; set; } = x;
        public long Y { get; set; } = y;

        public override bool Equals(object obj) =>
            obj is Coordinate coordinate &&
                   X == coordinate.X &&
                   Y == coordinate.Y;

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public override string ToString() => $"({X}, {Y})";

        public static bool operator ==(Coordinate left, Coordinate right) =>
            left.Equals(right);

        public static bool operator !=(Coordinate left, Coordinate right) =>
            !(left == right);
    }
}
