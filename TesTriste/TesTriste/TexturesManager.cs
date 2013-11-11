using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TesTriste
{
    public static class TexturesManager
    {
        public static Texture2D GameBackground,
                                GameOver,
                                Piece,
                                PieceBlocked,
                                Wait,
                                Error;

        public static SpriteFont Trebuchet18;

        public static void LoadContent(ContentManager content)
        {
            GameBackground = content.Load<Texture2D>("GameBackground");
            GameOver = content.Load<Texture2D>("GameOver");
            Piece = content.Load<Texture2D>("Piece");
            PieceBlocked = content.Load<Texture2D>("PieceBlocked");
            Trebuchet18 = content.Load<SpriteFont>("Trebuchet18");
        }

    }
}
