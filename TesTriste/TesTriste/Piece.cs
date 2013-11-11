using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TesTriste
{
    public class Piece
    {
        public Cell[] Cells { get; set; }

        private readonly Plate _plate;

        public Piece(Plate plate)
        {
            _plate = plate;
            Random random = new Random();
            Cells = new Cell[3];
            switch (random.Next(6))
            {
                case 0:
                    Cells[0] = new Cell(4, 0);
                    Cells[1] = new Cell(5, 0);
                    Cells[2] = new Cell(6, 0);
                    break;
                case 1:
                    Cells[0] = new Cell(4, 0);
                    Cells[1] = new Cell(4, 1);
                    Cells[2] = new Cell(4, 2);
                    break;
                case 2:
                    Cells[0] = new Cell(4, 0);
                    Cells[1] = new Cell(4, 1);
                    Cells[2] = new Cell(5, 1);
                    break;
                case 3:
                    Cells[0] = new Cell(5, 0);
                    Cells[1] = new Cell(5, 1);
                    Cells[2] = new Cell(4, 1);
                    break;
                case 4:
                    Cells[0] = new Cell(4, 0);
                    Cells[1] = new Cell(5, 0);
                    Cells[2] = new Cell(4, 1);
                    break;
                case 5:
                    Cells[0] = new Cell(4, 0);
                    Cells[1] = new Cell(5, 0);
                    Cells[2] = new Cell(5, 1);
                    break;
            }

            Print();
        }

        public void Draw(int x, int y, SpriteBatch spriteBatch)
        {
            int minX = Cells[0].X, maxX = Cells[0].X, minY = Cells[0].Y, maxY = Cells[0].Y;
            foreach (var cell in Cells)
            {
                if (cell.X < minX)
                    minX = cell.X;
                if (cell.X > maxX)
                    maxX = cell.X;
                if (cell.Y < minY)
                    minY = cell.Y;
                if (cell.Y > maxY)
                    maxY = cell.Y;
            }
            int startX = x - (maxX-minX + 1)*11;
            int startY = y - (maxY-minY + 1)*11;
            foreach (var cell in Cells)
                spriteBatch.Draw(TexturesManager.Piece, new Vector2(startX + (cell.X - minX)*22, startY + (cell.Y - minY)*22),
                                 Color.White);
        }

        public bool CanGoDown()
        {
            return Cells.All(cell => cell.Y + 1 <= 21 && _plate.Cells[cell.X, cell.Y + 1] != 1);
        }

        public void GoDown()
        {
            Clear();
            for (int i = 0; i < 3; i++)
                Cells[i] = new Cell(Cells[i].X, (byte) (Cells[i].Y + 1));
            Print();
        }

        public void GoLeft()
        {
            if (Cells.Any(cell => cell.X - 1 < 0 || _plate.Cells[cell.X - 1, cell.Y] == 1))
                return;
            Clear();
            for (int i = 0; i < 3; i++)
                Cells[i] = new Cell((byte) (Cells[i].X - 1), Cells[i].Y);
            Print();
        }

        public void GoRight()
        {
            if (Cells.Any(cell => cell.X + 1 > 9 || _plate.Cells[cell.X + 1, cell.Y] == 1))
                return;
            Clear();
            for (int i = 0; i < 3; i++)
                Cells[i] = new Cell((byte)(Cells[i].X + 1), Cells[i].Y);
            Print();
        }

        public void Drop()
        {
            while (CanGoDown())
                GoDown();
        }

        public void Burn()
        {
            foreach (var cell in Cells)
                _plate.Cells[cell.X, cell.Y] = 1;
        }

        public void PutUp()
        {
            int minY = Cells[0].Y;
            foreach (var cell in Cells)
                if (cell.Y < minY)
                    minY = cell.Y;
            while (minY > 0)
            {
                for (int i = 0; i < 3; i++)
                    Cells[i] = new Cell(Cells[i].X, (byte)(Cells[i].Y - 1));
                minY--;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < 3; i++)
                _plate.Cells[Cells[i].X, Cells[i].Y] = 0;
        }

        public void Print()
        {
            foreach (var cell in Cells)
                _plate.Cells[cell.X, cell.Y] = 2;
        }

        public class Cell
        {
            public byte X { get; set; }
            public byte Y { get; set; }

            public Cell(byte x, byte y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
