namespace ConsoleTextRedactor.ColorLineOutput
{
    public static class ColoredLine
    {
        public struct Buffer
        {
            public ConsoleColor? fgBuffer;
            public ConsoleColor? bgBuffer;
        }

        public static void SetBufferColors(this Buffer bfr, ConsoleColor colorfg, ConsoleColor colorbg)
        {
            bfr.fgBuffer = colorfg;
            bfr.bgBuffer = colorbg;
        }

        public static void ResetColors(this Buffer bfr)
        {
            bfr.fgBuffer = default;
            bfr.bgBuffer = default;
        }
    }
}