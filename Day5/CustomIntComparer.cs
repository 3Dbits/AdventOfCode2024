namespace Day5
{
    public class CustomIntComparer(Dictionary<int, List<int>> orderRules) : IComparer<int>
    {
        private readonly Dictionary<int, List<int>> _orderRules = orderRules;

        public int Compare(int x, int y)
        {
            // If x has rules about what can come after it and y is in x's allowed following numbers
            if (_orderRules.TryGetValue(x, out var xRules) && xRules.Contains(y))
            {
                return -1; // x should come before y
            }

            // If y has rules about what can come after it and x is in y's allowed following numbers
            if (_orderRules.TryGetValue(y, out var yRules) && yRules.Contains(x))
            {
                return 1; // x should come after y
            }

            // If no specific rules, maintain original order
            return 0;
        }
    }
}
