using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Raycasting
{
    public class Particle
    {
        #region Variables privées
        private List<Ray> _rays { get; set; }
        private Vector2 _position;
        private float _angle;
        #endregion Variables privées

        #region Propriétés
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                for (int i = 0; i < _rays.Count; i++)
                {
                    Ray r = _rays[i];
                    r.Position = value;
                }
            }
        }
        public float Angle
        {
            get { return _angle; }
            set
            {
                float dif = value - _angle;
                for (int i = 0; i < _rays.Count; i++)
                {
                    Ray r = _rays[i];
                    r.Angle += dif;
                }
                _angle = value;
            }
        }
        #endregion Propriétés

        public Particle(Vector2 pPosition, int pAngleMin, int pAngleMax, int pAngleStep)
        {
            _rays = new List<Ray>();
            Position = pPosition;
            for (int i = pAngleMin; i < pAngleMax; i+= pAngleStep)
            {
                _rays.Add(new Ray(Position, MathHelper.ToRadians(i)));
            }
        }

        public void Update(GameTime gameTime, List<Boundary> pWalls)
        {
            for (int i = 0; i < _rays.Count; i++)
            {
                Ray r = _rays[i];
                r.Update(gameTime, pWalls);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < _rays.Count; i++)
            {
                Ray r = _rays[i];
                r.Draw(spriteBatch, gameTime);
            }
        }
    }
}
