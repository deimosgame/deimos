using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class CollisionManager
    {
        List<CollisionElement> CollisionElements =
            new List<CollisionElement>();

        public void AddCollisionBox(Vector3 coords1, Vector3 coords2,
            Action<CollisionElement, DeimosGame> onCollision)
        {
            CollisionElement element = new CollisionElement();
            element.Box = new BoundingBox(coords1, coords2);
            element.ElementType = CollisionElement.CollisionType.Box;
            element.Event = onCollision;
            CollisionElements.Add(element);
        }
        // Adding a box directly helps for the ModelManager class, as we're
        // Creating a box directly from its methods when loading a model
        public void AddCollisionBoxDirectly(BoundingBox box,
            Action<CollisionElement, DeimosGame> onCollision)
        {
            CollisionElement element = new CollisionElement();
            element.Box = box;
            element.ElementType = CollisionElement.CollisionType.Box;
            element.Event = onCollision;
            CollisionElements.Add(element);
        }

        public void AddCollisionSphere(Vector3 coords, float radius,
            Action<CollisionElement, DeimosGame> onCollision)
        {
            CollisionElement element = new CollisionElement();
            element.Sphere = new BoundingSphere(coords, radius);
            element.ElementType = CollisionElement.CollisionType.Sphere;
            element.Event = onCollision;
            CollisionElements.Add(element);
        }
        // Same here
        public void AddCollisionSphereDirectly(BoundingSphere sphere,
            Action<CollisionElement, DeimosGame> onCollision)
        {
            CollisionElement element = new CollisionElement();
            element.Sphere = sphere;
            element.ElementType = CollisionElement.CollisionType.Sphere;
            element.Event = onCollision;
            CollisionElements.Add(element);
        }

        public void AddLevelModel(LevelModel model,
            Action<CollisionElement, DeimosGame> onCollision)
        {
            CollisionElement element = new CollisionElement();
            element.Model = model;
            element.ElementType = CollisionElement.CollisionType.Model;
            element.Event = onCollision;
            CollisionElements.Add(element);
        }

        public List<CollisionElement> GetElements()
        {
            return CollisionElements;
        }
    }
}
