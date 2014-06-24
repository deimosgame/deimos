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
        protected Vector2 Dimensions;

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
            World,
            SecretWall
        }
        public CollisionType ElementType
        {
            get;
            set;
        }
        public ElementNature Nature = ElementNature.World;

        public byte Owner = 0xFF;

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

        public CollisionElement(Vector2 dimensions)
        {
            Event = delegate(CollisionElement element, DeimosGame game) {};
            Dimensions = dimensions;
        }

        public virtual List<BoundingSphere> GenerateSphere(Vector3 position, Vector2 dimension)
        {
            List<BoundingSphere> l = new List<BoundingSphere>();
            l.Add(new BoundingSphere(position, Math.Max(dimension.X, dimension.Y)));
            return l;
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
            List<BoundingSphere> listSpheres = GenerateSphere(position, Dimensions);

            // And finally with our models collisions
            for (int i = 0; i < GeneralFacade.SceneManager.CollisionManager.GetElements().Count(); i++)
            {
                CollisionElement thisElement = 
                    GeneralFacade.SceneManager.CollisionManager.GetElements().ElementAt(i);

                bool isCollision = false;

                for (int j = 0; j < listSpheres.Count; j++)
                {
                    var sphere = listSpheres.ElementAt(j);

                    switch (thisElement.ElementType)
                    {
                        case CollisionElement.CollisionType.Box:
                            if (thisElement.Box.Contains(sphere) !=
                                ContainmentType.Disjoint)
                            {
                                isCollision = true;
                            }
                            break;
                        case CollisionElement.CollisionType.Sphere:
                            if (thisElement.Sphere.Contains(sphere) !=
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
                            BoundingSphere newSphere = new BoundingSphere((sphere.Center - thisElement.Model.Position) / thisElement.Model.Scale,
                                    sphere.Radius / thisElement.Model.Scale);
                            if (thisElement.Model.CollisionModel.collisionData.collisions(
                                newSphere
                            ))
                            {
                                isCollision = true;
                            }
                            break;
                    }
                    if (isCollision
                        && !this.FilterCollisionElement(thisElement) && !thisElement.FilterCollisionElement(this))
                    {
                        break;
                    }
                    else
                    {
                        isCollision = false;
                    }
                }

                if (isCollision)
                {
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
