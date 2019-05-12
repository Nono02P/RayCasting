using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Raycasting
{
    public class Ray
    {
        #region Variables privées
        private Vector2? _intersection;
        #endregion Variables privées

        #region Propriétés
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; } = Vector2.UnitX;
        public Color Color { get; set; } = Color.White;
        public int Thickness { get; set; } = 1;
        public float Angle { get { return (float)Math.Atan2(Direction.Y, Direction.X); } set { Direction = new Vector2((float)Math.Cos(value), (float)Math.Sin(value)); } }
        public float Alpha { get; set; }
        #endregion Propriétés

        #region Constructeur
        public Ray(Vector2 pPosition)
        {
            Position = pPosition;
        }

        public Ray(Vector2 pPosition, float pAngle)
        {
            Position = pPosition;
            Direction = new Vector2((float)Math.Cos(pAngle) * 10, (float)Math.Sin(pAngle) * 10);
        }
        #endregion Constructeur

        public void LookAt(Vector2 pDirection)
        {
            Direction = Vector2.Normalize(pDirection - Position) * 10;
        }

        private Vector2? Intersection(Boundary pWall)
        {
            /*
            * t =   (x1-x3) * (y3-y4) - (y1-y3) * (x3-x4)
            *       _____________________________________
            *       (x1-x2) * (y3-y4) - (y1-y2) * (x3-x4)
            * 
            * u = - (x1-x2) * (y1-y3) - (y1-y2) * (x1-x3)
            *       ______________________________________
            *       (x1-x2) * (y3-y4) - (y1-y2) * (x3-x4)
            *       
            * If 0 < t < 1 and 0 < u < 1 then 2 lines intersection
            * If 0 < t < 1 and 0 < u then 1 lines intersect with 1 ray
            */
            float x1 = pWall.A.X;
            float y1 = pWall.A.Y;
            float x2 = pWall.B.X;
            float y2 = pWall.B.Y;
            float x3 = Position.X;
            float y3 = Position.Y;
            float x4 = Position.X + Direction.X;
            float y4 = Position.Y + Direction.Y; 
            float denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (denominator == 0)
            {
                return null;
            }
            else
            {
                float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denominator;
                float u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denominator;
                if (t > 0 && t < 1 && u > 0)
                    return new Vector2(x1 + t * (x2 - x1), y1 + t * (y2 - y1));
                else
                    return null;
            }
        }

        public void Update(GameTime gameTime, List<Boundary> pWalls)
        {
            _intersection = null;
            for (int i = 0; i < pWalls.Count; i++)
            {
                Boundary w = pWalls[i];
                Vector2? point = Intersection(w);
                if (point.HasValue)
                {
                    if (_intersection.HasValue)
                    {
                        Vector2 dif = point.Value - Position;
                        Vector2 dif2 = _intersection.Value - Position;
                        if (dif.Length() < dif2.Length())
                            _intersection = point;
                    }
                    else
                    {
                        _intersection = point;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //spriteBatch.DrawCircle(Position, Thickness + 2, 10, Color, Thickness + 2);
            spriteBatch.DrawLine(Position, Position + Direction, Color, Thickness);
            if (_intersection.HasValue)
            {
                spriteBatch.DrawLine(Position + Direction, _intersection.Value, Color.White * Alpha, Thickness);
                //spriteBatch.DrawCircle(_intersection.Value, Thickness + 2, 10, Color.Red, Thickness + 2);
            }
        }
    }
}