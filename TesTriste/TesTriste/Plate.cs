using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TesTriste
{
    public class Plate
    {
        public byte[,] Cells { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 22; y++)
                    if (Cells[x, y] == 2)
                        spriteBatch.Draw(TexturesManager.Piece,
                                         new Vector2(20 + x*22, 76 + y*22),
                                         Color.White);

                    else if (Cells[x, y] == 1)
                        spriteBatch.Draw(TexturesManager.PieceBlocked,
                                         new Vector2(20 + x*22, 76 + y*22),
                                         Color.White);
        }

        public bool IsLineFull(int y)
        {
            for (int x = 0; x < 10; x++)
                if (Cells[x, y] != 1)
                    return false;
            return true;
        }

        public void EmptyLine(int line)
        {
            for (int x = 0; x < 10; x++)
                Cells[x, line] = 0;
            for (int y = line; y > 0; y--)
                for (int x = 0; x < 10; x++)
                    Cells[x, y] = Cells[x, y - 1];
            for (int x = 0; x < 10; x++)
                Cells[x, 0] = 0;
        }
    }
}