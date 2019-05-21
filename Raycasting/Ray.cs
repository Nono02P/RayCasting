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
        public RayViewer Parent { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; } = Vector2.UnitX;
        public Color Color { get; set; } = Color.White;
        public float ClosestDistance { get; private set; }
        public Color WallColor { get; set; }
        public int Thickness { get; set; } = 1;
        public float Angle { get { return (float)Math.Atan2(Direction.Y, Direction.X); } set { Direction = new Vector2((float)Math.Cos(value), (float)Math.Sin(value)); } }
        public float Alpha { get; set; }
        #endregion Propriétés

        #region Constructeur
        public Ray(RayViewer pParent, Vector2 pPosition)
        {
            Parent = pParent;
            Position = pPosition;
        }

        public Ray(RayViewer pParent, Vector2 pPosition, float pAngle)
        {
            Parent = pParent;
            Position = pPosition;
            Direction = new Vector2((float)Math.Cos(pAngle) * 10, (float)Math.Sin(pAngle) * 10);
        }
        #endregion Constructeur

        public void LookAt(Vector2 pDirection)
        {
            Direction = Vector2.Normalize(pDirection - Position);
        }

        private Vector2? Intersection(Boundary pWall)
        {
            /* t => wall
            * t =   (x1-x3) * (y3-y4) - (y1-y3) * (x3-x4)
            *       _____________________________________
            *       (x1-x2) * (y3-y4) - (y1-y2) * (x3-x4)
            * 
            * u => ray
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
                if (t >= 0 && t <= 1 && u >= 0)
                    return new Vector2(x1 + t * (x2 - x1), y1 + t * (y2 - y1));
                else
                    return null;
            }
        }

        public void Update(GameTime gameTime, List<Boundary> pWalls)
        {
            _intersection = null;
            ClosestDistance = float.PositiveInfinity;
            for (int i = 0; i < pWalls.Count; i++)
            {
                Boundary w = pWalls[i];
                Vector2? point = Intersection(w);
                if (point.HasValue)
                {
                    if (_intersection.HasValue)
                    {
                        Vector2 dif = point.Value - Position;
                        float distance = dif.Length();
                        Vector2 closestDif = _intersection.Value - Position;
                        if (distance < closestDif.Length())
                        {
                            _intersection = point;
                            distance *= (float)Math.Cos(Angle - Parent.Angle);
                            ClosestDistance = distance;
                            WallColor = w.Color;
                        }
                    }
                    else
                    {
                        _intersection = point;
                        Vector2 dif = point.Value - Position;
                        WallColor = w.Color;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (_intersection.HasValue)
                spriteBatch.DrawLine(Position + Direction, _intersection.Value, Color * Alpha, Thickness);
        }
    }
}