using CollidableModel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class CollisionElement
    {
        protected Vector3 Dimensions;

        public enum CollisionType
        {
            Model,
            Sphere,
            Box
        }
        public enum ElementNature
        {
            Player,
            Bullet,
            Object,
            World
        }
        public CollisionType ElementType
        {
            get;
            set;
        }
        public ElementNature Nature = ElementNature.World;

        public LevelModel Model
        {
            get;
            set;
        }
        public virtual BoundingBox Box
        {
            get;
            set;
        }
        public BoundingSphere Sphere
        {
            get;
            set;
        }

        public Action<CollisionElement, DeimosGame> Event
        {
            get;
            set;
        }

        public CollisionElement()
        {
            Event = delegate(CollisionElement element, DeimosGame game) {};
        }

        public CollisionElement(Vector3 dimensions)
        {
            Event = delegate(CollisionElement element, DeimosGame game) {};
            Dimensions = dimensions;
        }

        public virtual BoundingBox GenerateBox(Vector3 position, Vector3 dimension)
        {
            Vector3 bbTop = new Vector3(
                position.X + (dimension.X / 2),
                position.Y + (dimension.Y / 2),
                position.Z + (dimension.Z / 2)
            );
            Vector3 bbBottom = new Vector3(
                position.X - (dimension.X / 2),
                position.Y - (dimension.Y / 2),
                position.Z - (dimension.Z / 2)
            );
            return new BoundingBox(
                bbBottom,
                bbTop
            );
        }



        public virtual bool PreCollisionBypass()
        {
            return false;
        }

        public virtual void CollisionEvent(CollisionElement element)
        {
            return;
        }

        public virtual bool FilterCollisionElement(CollisionElement element)
        {
            return false;
        }


        // Methods
        public Boolean CheckCollision(Vector3 position)
        {
            if (PreCollisionBypass())
            {
                return false;
            }

            // Creating the sphere of the camera for later collisions checks
            BoundingBox cameraBox = GenerateBox(position, Dimensions);
            Vector3 bbTop = cameraBox.Min;
            Vector3 bbBottom = cameraBox.Max;

            // And finally with our models collisions
            foreach (CollisionElement thisElement in
                GeneralFacade.SceneManager.CollisionManager.GetElements())
            {
                bool isCollision = false;
                switch (thisElement.ElementType)
                {
                    case CollisionElement.CollisionType.Box:
                        if (thisElement.Box.Contains(cameraBox) !=
                            ContainmentType.Disjoint)
                        {
                            isCollision = true;
                        }
                        break;
                    case CollisionElement.CollisionType.Sphere:
                        if (thisElement.Sphere.Contains(cameraBox) !=
                            ContainmentType.Disjoint)
                        {
                            isCollision = true;
                        }
                        break;
                    case CollisionElement.CollisionType.Model:
                        if (thisElement.Model.CollisionDetection ==
                            LevelModel.CollisionType.None)
                        {
                            continue;
                        }
                        var collidingFaces = new LinkedList<Face>();
                        var collisionPoints = new LinkedList<Vector3>();
                        // This method is used with pointer, so it does change 
                        // our above faces and points
                        thisElement.Model.CollisionModel.collisionData.collisions(
                            new BoundingBox(
                                (bbTop - thisElement.Model.Position) / thisElement.Model.Scale,
                                (bbBottom - thisElement.Model.Position) / thisElement.Model.Scale
                            ),
                            collidingFaces,
                            collisionPoints
                        );
                        if (collidingFaces.Count > 0 || collisionPoints.Count > 0)
                        {
                            isCollision = true;
                        }
                        break;
                }
                if (isCollision)
                {
                    if (this.FilterCollisionElement(thisElement) || thisElement.FilterCollisionElement(this))
                    {
                        continue;
                    }

                    thisElement.Event(this, GeneralFacade.Game);
                    thisElement.CollisionEvent(this);
                    this.CollisionEvent(thisElement);
                    return true;
                }
            }

            // If we're here, then no collision has been matched
            return false;
        }

        public ElementNature GetNature()
        {
            return Nature;
        }
    }
}
