using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Raycasting
{
    public class RayViewer
    {
        #region Variables privées
        private Vector2 _position;
        private float _angle;
        private float _alpha;
        #endregion Variables privées

        #region Propriétés
        public List<Ray> Rays { get; set; }
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                for (int i = 0; i < Rays.Count; i++)
                {
                    Ray r = Rays[i];
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
                for (int i = 0; i < Rays.Count; i++)
                {
                    Ray r = Rays[i];
                    r.Angle += dif;
                }
                _angle = value;
            }
        }

        public float Alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value;
                for (int i = 0; i < Rays.Count; i++)
                {
                    Ray r = Rays[i];
                    r.Alpha = value;
                }
            }
        }
        #endregion Propriétés

        public RayViewer(Vector2 pPosition, int pAngleMin, int pAngleMax, float pAngleStep, float pAlpha)
        {
            Rays = new List<Ray>();
            Position = pPosition;
            for (float i = pAngleMin; i < pAngleMax; i+= pAngleStep)
            {
                Rays.Add(new Ray(Position, MathHelper.ToRadians(i)));
            }
            Alpha = pAlpha;
        }

        public void Update(GameTime gameTime, List<Boundary> pWalls)
        {
            for (int i = 0; i < Rays.Count; i++)
            {
                Ray r = Rays[i];
                r.Update(gameTime, pWalls);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < Rays.Count; i++)
            {
                Ray r = Rays[i];
                r.Draw(spriteBatch, gameTime);
            }
        }
    }
}
