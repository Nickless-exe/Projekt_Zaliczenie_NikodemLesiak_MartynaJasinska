using System;
using System.Collections.Generic;

namespace Banan
{
    public class Cell
    {
        public char visuals;
        public Cell(char visuals) { this.visuals = visuals; }
        public void Display() => Console.Write(visuals);
    }
}
