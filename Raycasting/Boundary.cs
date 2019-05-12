using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raycasting
{
    public class Boundary
    {
        #region Variables privées

        #endregion

        #region Propriétés
        public Vector2 A { get; set; }
        public Vector2 B { get; set; }
        public Color Color { get; set; } = Color.White;
        public int Thickness { get; set; } = 1;
        #endregion

        #region Constructeur
        public Boundary(Vector2 pA, Vector2 pB)
        {
            A = pA;
            B = pB;
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawLine(A, B, Color, Thickness);
        }
    }
}
