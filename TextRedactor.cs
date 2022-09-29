namespace ConsoleTextRedactor;

public class TextRedacting
{
    (int x, int y) cursor;
    public struct Window
    {
        public (int X, int Y) Size;

        public Window((int x, int y) size)
        {
            Size = size;
        }
    }

    public List<string> BeginRedact(List<string> list, Window window)
    {
        #region Variables
        ConsoleKeyInfo key;
        int prevline; // КОСТЫЛЬ
        List<string> text = new List<string>();
        # endregion

        cursor = (0, 0);
        if (list.Count != 0) text = list;

        if (text.Count > 0)
        {
            foreach (var e in text) Console.WriteLine(e);
            cursor = (text[^1].Length, text.Count - 1);
            Console.SetCursorPosition(cursor.x, cursor.y);
        }



        while (true)
        {

            key = Console.ReadKey();

            prevline = cursor.y; // КОСТЫЛЬ

            Console.SetWindowSize(window.Size.X, window.Size.Y);
            Console.SetBufferSize(window.Size.X, window.Size.Y);

            if (key.Modifiers != 0 && key.Modifiers == ConsoleModifiers.Control)
            { }

            else
                switch (key.Key)
                {

                    case ConsoleKey.Enter:
                        if (list.Count < Console.BufferHeight - 3)
                        {
                            MakeNewLine(text, cursor.y);
                            cursor = (0, cursor.y + 1);
                            RenderFrom(text, fromLine: cursor.y);
                        }
                        break;

                    case ConsoleKey.Backspace:
                        if (text[cursor.y].Length == 0 && cursor.y > 0)
                        {
                            text.RemoveAt(cursor.y);
                            cursor = (cursor.x, cursor.y - 1);
                            RenderFrom(text, cursor.y);
                            cursor.x = (cursor.x < text[cursor.y].Length) ? text[cursor.y].Length : cursor.x;
                            break;
                        }

                        if (cursor.x > 0)
                        {
                            RemoveChar(ref text, cursor);
                            cursor = (cursor.x - 1, cursor.y);
                        }
                        break;

                    // UP _ DOWN
                    case ConsoleKey.UpArrow:
                        ArrUp(text);
                        break;

                    case ConsoleKey.DownArrow:
                        ArrDown(text);
                        break;

                    // LEFT _ RIGHT
                    case ConsoleKey.LeftArrow:
                        ArrLeft(text);
                        break;

                    case ConsoleKey.RightArrow:
                        ArrRight(text);
                        break;

                    // DEFAULT
                    case ConsoleKey.Escape:
                        break;

                    default:
                        cursor.x++;
                        if (cursor.x >= window.Size.X)
                        {
                            MakeNewLine(text, cursor.y);
                            cursor = (1, cursor.y + 1);
                            RenderFrom(text, fromLine: cursor.y);
                        }
                        text[cursor.y] = text[cursor.y].Insert(cursor.x - 1, $"{key.KeyChar}");
                        break;
                }
            
            if (key.Key == ConsoleKey.Escape) break;
            RenderLine(text[cursor.y], cursor.y);
            //RenderLine(text[prevline], prevline); // КОСТЫЛЬ
            Console.SetCursorPosition(cursor.x, cursor.y);
        }
        return text;
    }

    public void ArrRight(List<string> list)
    {
        // if u can go right -> go right
        if (cursor.x + 1 <= list[cursor.y].Length)
            cursor = (cursor.x + 1, cursor.y);

        // if cursor char == line length  and u have line below -> go next line
        else if (cursor.x + 1 + 1 > list[cursor.y].Length && cursor.y + 1 < list.Count)
            cursor = (0, cursor.y + 1);
    }

    public void ArrLeft(List<string> list)
    {
        if (cursor.x - 1 >= 0)
            cursor = (cursor.x - 1, cursor.y);
        else if (cursor.y > 0) cursor = (list[cursor.y - 1].Length, cursor.y - 1);
    }

    public void ArrDown(List<string> list)
    {
        if (cursor.y < list.Count - 1) cursor.y++;
        cursor.x = cursor.x > list[cursor.y].Length ?
            list[cursor.y].Length : cursor.x;
    }

    public void ArrUp(List<string> list)
    {
        if (cursor.y > 0) cursor.y--;
        cursor.x = cursor.x > list[cursor.y].Length ?
            list[cursor.y].Length : cursor.x;
    }

    public void MakeNewLine(List<string> list, int linePos)
    {
        list.Insert(linePos + 1, "");
    }

    public void RenderFrom(List<string> list, int fromLine)
    {
        Console.SetCursorPosition(cursor.x, cursor.y);
        for (int i = fromLine; i < list.Count; i++)
            Console.WriteLine(list[i]);
    }

    public void RenderLine(string line, int linePos)
    {
        // clear line from input data
        Console.SetCursorPosition(0, linePos);
        for (int charUnitPos = 0; charUnitPos < Console.BufferWidth; charUnitPos++)
            Console.Write(' ');

        // render line
        Console.SetCursorPosition(0, linePos);
        for (int charUnitPos = 0; charUnitPos < line.Length; charUnitPos++)
            Console.Write(line[charUnitPos]);
    }

    public void RemoveChar(ref List<string> list, (int X, int Y) cursorPos)
    {
        Console.SetCursorPosition(cursorPos.X - 1, cursorPos.Y);
        Console.Write(' ');
        list[cursorPos.Y] = list[cursorPos.Y].Remove(cursorPos.X - 1, 1);
    }
}
