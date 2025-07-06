namespace Banan
{
    // Represents a game map/level, including layout, display, and item placement
    public class Level
    {
        // Array of strings representing the map layout
        protected string[] levelVisuals = new string[]
        {
            "######################################",
            "#....................................#",
            "#...........................###################",
            "#............................................>#           ##############",
            "#..........................#####.....##########           #............#",
            "#...###.....####.....###...#.........#                    #............#",
            "#..........................#.........#################    ########.....########",
            "#...###.....####.....###...#..........................#   #............#......#",
            "#..........................#..........................#   #......#.....#......#",
            "#...###.....####.....###...#.........#############....#   #......#.....#......#",
            "#..........................#.........#           #....#   #......#.....#......#",
            "#.....######################.........#           #....############.....###....#",
            "#..............#.....................#           #............................#",
            "#..............#.....................#           #............................#",
            "#####################################            #............................#",
            "                                                 ################....##########",
            "                                                                #....#",
            "                                                                #....#",
            "                                                                #....#",
            "                                                                #....#",
            "                                                    #############....#############",
            "                                                    #............................#",
            "                                                    #....###.....####.....###....#",
            "                                                    #............................#",
            "                                                    #....###.....####.....###....#",
            "                                                    #............................#",
            "                                                    #....###.....####.....###....#",
            "                                                    ##############################",
        };

        // 2D list of cells representing the map
        public List<List<Cell>> levelData { get; private set; } = new List<List<Cell>>();

        // Constructor: initializes the level data from the layout
        public Level()
        {
            levelData = new List<List<Cell>>();
            InitializeLevelData();
        }

        // Converts levelVisuals to levelData (list of cells)
        public void InitializeLevelData()
        {
            levelData.Clear();
            for (int y = 0; y < levelVisuals.Length; y++)
            {
                List<Cell> row = new List<Cell>();
                for (int x = 0; x < levelVisuals[y].Length; x++)
                {
                    row.Add(new Cell(levelVisuals[y][x]));
                }
                levelData.Add(row);
            }
        }

        public void Display()
        {
            for (int y = 0; y < levelData.Count; y++)
            {
                Console.SetCursorPosition(0, y);
                foreach (Cell cell in levelData[y])
                {
                    cell.Display();
                }
                Console.WriteLine();
            }
        }

        public char GetCellVisuals(int x, int y)
        {
            if (y < 0 || y >= levelData.Count) return ' ';
            if (x < 0 || x >= levelData[y].Count) return ' ';
            return levelData[y][x].visuals;
        }

        public void SetCell(int x, int y, char value)
        {
            if (y < 0 || y >= levelData.Count) return;
            if (x < 0 || x >= levelData[y].Count) return;
            levelData[y][x].visuals = value;
        }

        public int GetHeight() => levelData.Count;
        public int GetRowWidth(int rowNumber) => levelData[rowNumber].Count;
        public int GetWidth() => levelData.Count > 0 ? levelData[0].Count : 0;

        public void RedrawCell(Point pos)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write(GetCellVisuals(pos.x, pos.y));
        }

        public void PlaceRandomItem(char symbol, int count)
        {
            Random rnd = new Random();
            List<Point> emptyPositions = new();

            for (int y = 0; y < GetHeight(); y++)
            {
                for (int x = 0; x < GetRowWidth(y); x++)
                {
                    if (GetCellVisuals(x, y) == '.')
                        emptyPositions.Add(new Point(x, y));
                }
            }

            for (int i = 0; i < count && emptyPositions.Count > 0; i++)
            {
                int index = rnd.Next(emptyPositions.Count);
                Point pos = emptyPositions[index];
                SetCell(pos.x, pos.y, symbol);
                emptyPositions.RemoveAt(index);
            }
        }

        public Point GetRandomEmptyPosition()
        {
            Random rnd = new Random();
            List<Point> emptyPositions = new();

            for (int y = 0; y < GetHeight(); y++)
            {
                for (int x = 0; x < GetRowWidth(y); x++)
                {
                    if (GetCellVisuals(x, y) == '.')
                        emptyPositions.Add(new Point(x, y));
                }
            }

            return emptyPositions[rnd.Next(emptyPositions.Count)];
        }
    }
}
