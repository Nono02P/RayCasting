using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Raycasting
{
    public class RayViewer
    {
        #region Variables privées
        private Vector2 _position;
        private float _angle;
        private float _alpha;
        private Color _color;
        #endregion Variables privées

        #region Propriétés
        public bool Draw3D { get; set; }
        public Rectangle Area3D { get; set; }
        public List<Ray> Rays { get; set; }
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Rays.ForEach(r => r.Position = value);
            }
        }

        public float Angle
        {
            get { return _angle; }
            set
            {
                float dif = value - _angle;
                _angle = value;
                Rays.ForEach(r => r.Angle += dif);
            }
        }

        public float Alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value;
                Rays.ForEach(r => r.Alpha = value);
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                Rays.ForEach(r => r.Color = value);
            }
        }
        #endregion Propriétés

        public RayViewer(Vector2 pPosition, int pAngleMin, int pAngleMax, float pAngleStep, Color pColor, float pAlpha)
        {
            Rays = new List<Ray>();
            Position = pPosition;
            for (float i = pAngleMin; i < pAngleMax; i+= pAngleStep)
            {
                Rays.Add(new Ray(this, Position, MathHelper.ToRadians(i)));
            }
            Color = pColor;
            Alpha = pAlpha;
        }

        /*private double MapValue(double a, double a0, double a1, double b0, double b1, bool pWithClamp = true)
        {
            double val = a;
            if (pWithClamp)
                val = MathHelper.Clamp((float)a, (float)a0, (float)a1);

            return b0 + (b1 - b0) * ((val - a0) / (a1 - a0));
        }*/

        public void Update(GameTime gameTime, List<Boundary> pWalls)
        {
            Rays.ForEach(r => r.Update(gameTime, pWalls));
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawCircle(Position, 2, 10, Color, 2);
            float nbRays = Rays.Count;
            for (int i = 0; i < Rays.Count; i++)
            {
                Ray r = Rays[i];
                r.Draw(spriteBatch, gameTime);
                if (Draw3D)
                {
                    float w = Area3D.Width / nbRays;
                    float brightness = (float)utils.MapValue(Math.Pow(r.ClosestDistance, 2), 0, Math.Pow(Area3D.Width, 2), 1, 0);// * .5f;
                    int h = (int)utils.MapValue(r.ClosestDistance, 0, Area3D.Width, Area3D.Height, 0);
                    Color col = r.WallColor * brightness;
                    spriteBatch.DrawRectangle(new Rectangle(Area3D.Width + (int)(i * w + w), (Area3D.Height - h) / 2, (int)w, h), new Color(col, 1f));
                    spriteBatch.FillRectangle(new Rectangle(Area3D.Width + (int)(i * w + w), (Area3D.Height - h) / 2, (int)w, h), new Color(col, 1f));
                }
            }
        }
    }
}
