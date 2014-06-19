using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CollidableModel;


namespace Deimos
{
    class PlayerCollision : CollisionElement
    {
        // Attributes
        public override BoundingBox Box
        {
            get { return GenerateBox(DisplayFacade.Camera.Position, DisplayFacade.Camera.Rotation); }
        }

        // Constructor
        public PlayerCollision(float playerHeight, float playerWidth)
            : base(new Vector2(playerWidth, playerHeight))
        {
            Nature = ElementNature.Player;
        }

        private BoundingBox GenerateBox(Vector3 position, Vector3 dimension)
        {
            Vector3 bbTop = new Vector3(
                position.X + (dimension.X / 2),
                position.Y + (dimension.Y / 5) * 2,
                position.Z + (dimension.Z / 2)
            );
            Vector3 bbBottom = new Vector3(
                position.X - (dimension.X / 2),
                position.Y - (dimension.Y / 5) * 3,
                position.Z - (dimension.Z / 2)
            );
            return new BoundingBox(
                bbBottom,
                bbTop
            );
        }

        public override List<BoundingSphere> GenerateSphere(Vector3 position, Vector2 dimension)
        {
            List<BoundingSphere> l = new List<BoundingSphere>();

            float height = dimension.Y;
            float width = dimension.X;

            l.Add(new BoundingSphere(
                new Vector3(position.X, position.Y - (height / 5), position.Z),
                width / 2
            ));
            l.Add(new BoundingSphere(
                new Vector3(position.X, position.Y - 2 * (height / 5), position.Z),
                width / 2
            ));
            l.Add(new BoundingSphere(
                new Vector3(position.X, position.Y - 3 * (height / 5), position.Z),
                width / 2
            ));
            l.Add(new BoundingSphere(
                new Vector3(position.X, position.Y - 4 * (height / 5), position.Z),
                width / 2
            ));
            l.Add(new BoundingSphere(
                new Vector3(position.X, position.Y + (height / 5), position.Z),
                width / 2
            ));

            return l;
        }

        public override void CollisionEvent(CollisionElement element)
        {
            return;
        }

        public override bool FilterCollisionElement(CollisionElement element)
        {
            if (element == this)
            {
                return true;
            }

            return false;
        }

        public override bool PreCollisionBypass()
        {
            if (GeneralFacade.Game.CurrentPlayingState == DeimosGame.PlayingStates.NoClip)
            {
                return true;
            }

            return false;
        }

    }
}
