
namespace GraphicFiltersWF
{
    static class ConvolutionMatrix
    {
        public static int GetX3x3(int i, int j)
        {
            return _offsets3x3[i, j].XOffset;
        }
        public static int GetY3x3(int i, int j)
        {
            return _offsets3x3[i, j].YOffset;
        }
        public static int GetX5x5(int i, int j)
        {
            return _offsets5x5[i, j].XOffset;
        }
        public static int GetY5x5(int i, int j)
        {
            return _offsets5x5[i, j].YOffset;
        }

        private static  Move[,] _offsets3x3 =
        {
            {new Move(-1,-1), new Move( 0,-1), new Move( 1,-1)}, 
            {new Move(-1, 0), new Move( 0, 0), new Move( 1, 0)}, 
            {new Move(-1, 1), new Move( 0, 1), new Move( 1, 1)} 
        };

        private static Move[,] _offsets5x5 =
        {
            {new Move(-2,-2), new Move(-1,-2), new Move(0,-2), new Move(1,-2), new Move(2,-2)},
            {new Move(-2,-1), new Move(-1,-1), new Move(0,-1), new Move(1,-1), new Move(2,-1)},
            {new Move(-2, 0), new Move(-1, 0), new Move(0, 0), new Move(1, 0), new Move(2, 0)},
            {new Move(-2, 1), new Move(-1, 1), new Move(0, 1), new Move(1, 1), new Move(2, 1)},
            {new Move(-2, 2), new Move(-1, 2), new Move(0, 2), new Move(1, 2), new Move(2, 2)}
        };
        private struct Move
        {
            public readonly int XOffset;
            public readonly int YOffset;

            public Move(int x, int y)
            {
                XOffset = x;
                YOffset = y;
            }
        }
    }
}
