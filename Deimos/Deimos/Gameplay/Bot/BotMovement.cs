using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class BotMovement
    {
        DeimosGame Game;
        Bot ThisBot;

        Vector3 Acceleration;
        Vector3 MoveVector;

        public BotMovement(DeimosGame game, Bot thisBot)
        {
            Game = game;
            ThisBot = thisBot;
        }

        public Vector3 GetMovementVector(float dt)
        {
            ThisBot.Physics.ApplyGravity(dt);
            return ((new Vector3(0, 1, 0)) * ThisBot.Physics.GetAcceleration());
        }

        // Set camera position and rotation
        public void MoveTo(Vector3 position, Vector3 rotation)
        {
            ThisBot.Position = position;
            ThisBot.Rotation = rotation;
        }

        // Methods that simulate movement
        private Vector3 PreviewMove(Vector3 movement, float dt)
        {
            // Create a rotate matrix
            Matrix rotate = Matrix.CreateRotationY(ThisBot.Rotation.Y);
            // Create a movement vector
            Vector3 movementGravity = new Vector3(0, movement.Y, 0);
            movement = Vector3.Transform(movement, rotate);
            movementGravity = Vector3.Transform(movementGravity, rotate);
            // Return the value of camera position + movement vector

            // Testing for the UPCOMING position
            if (Game.SceneManager.Collision.CheckCollision(ThisBot.Position + movement))
            {
                if (Game.SceneManager.Collision.CheckCollision(ThisBot.Position + movementGravity))
                {
                    if (ThisBot.Physics.GravityState == BotPhysics.PhysicalState.Falling)
                    {
                        movement.Y = -GetNearFloorDistance(ThisBot.Position + new Vector3(movement.X, 0, movement.Z), 0.1f);
                        if (movement.Y > 0)
                        {
                            movement.Y = 0;
                        }
                    }
                    // Hit floor or ceiling
                    ThisBot.Physics.StabilizeGravity();
                    //Game.ThisPlayerPhysics.Reset(LocalPlayerPhysics.AccelerationDirection.Y);
                }
                else if (ThisBot.Physics.GravityState == BotPhysics.PhysicalState.Walking &&
                    !Game.SceneManager.Collision.CheckCollision(
                        ThisBot.Position + new Vector3(movement.X, 2, movement.Z)))
                {
                    movement.Y = GetNearFloorDistance(ThisBot.Position + new Vector3(movement.X, 2, movement.Z), 0.1f);
                }
                // Creating the new movement vector, which will make us 
                // able to have a smooth collision: being able to "slide" on 
                // the wall while colliding
                movement.X = Game.SceneManager.Collision.CheckCollision(
                                    ThisBot.Position +
                                    new Vector3(movement.X, 0, 0)
                                ) ? 0 : movement.X;
                movement.Y = Game.SceneManager.Collision.CheckCollision(
                                    ThisBot.Position +
                                    new Vector3(0, movement.Y, 0)
                                ) ? 0 : movement.Y;
                movement.Z = Game.SceneManager.Collision.CheckCollision(
                                    ThisBot.Position +
                                    new Vector3(0, 0, movement.Z)
                             ) ? 0 : movement.Z;
                return ThisBot.Position + movement;
            }
            else
            {
                // There isn't any collision, so we just move the user with 
                // the movement he wanted to do
                return ThisBot.Position + movement;
            }
        }

        public void Move(Vector3 scale, float dt)
        {
            MoveTo(PreviewMove(scale, dt), ThisBot.Rotation);
        }

        // Movement handling
        public void HandleMovement(float dt)
        {
            //GetMouseMovement(dt);

            Acceleration = GetMovementVector(dt);
            MoveVector = Acceleration * dt * ThisBot.Speed;
            Move(MoveVector, dt);
        }

        private float GetNearFloor(Vector3 pos, float increment)
        {
            while (!Game.SceneManager.Collision.CheckCollision(pos))
            {
                pos.Y -= increment;
            }
            return pos.Y + increment;
        }

        private float GetNearFloorDistance(Vector3 pos, float increment)
        {
            float x = GetNearFloor(pos, increment);
            return pos.Y - x;
        }

    }
}
