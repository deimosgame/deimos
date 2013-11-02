using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
	public class Camera : GameComponent
	{
		// Atributes
		public Vector3 CameraPosition;
		private Vector3 CameraRotation;
		private float   CameraSpeed;
		public Vector3 CameraLookAt;
		public float AspectRatio;

		public Vector3 CameraOldPosition;
		private Vector3 CameraMovement = new Vector3(0, 0, 0);

		private Vector3 MouseRotationBuffer;
		private MouseState CurrentMouseState;
		private MouseState PreviousMouseState;
		private float MouseSpeed = 0.1f;
		private Boolean MouseInverted = false;


		// For testing purpose
		private Keys ForwardKey = Keys.Z;
		private Keys BackKey = Keys.S;
		private Keys LeftKey = Keys.Q;
		private Keys RightKey = Keys.D;


		// Properties
		public Vector3 Position
		{
			get 
			{ 
				return CameraPosition; 
			}

			set
			{
				CameraPosition = value;
				updateLookAt();
			}
		}

		public Vector3 Rotation
		{
			get
			{
				return CameraRotation;
			}

			set
			{
				CameraRotation = value;
				updateLookAt();
			}
		}

		public Matrix Projection
		{
			get;
			protected set;
		}

		public Matrix View
		{
			get
			{
				return Matrix.CreateLookAt(
					CameraPosition, 
					CameraLookAt, 
					Vector3.Up
				);
			}
		}

		public Vector3 ViewVector
		{
			get
			{
				Vector3 viewVector = Vector3.Transform(
					CameraPosition - CameraLookAt, 
					Matrix.CreateRotationY(0)
				);
				viewVector.Normalize();
				return viewVector;
			}
		}


		public BoundingFrustum Frustum
		{
			get
			{
				return new BoundingFrustum(View * Projection);
			}
		}

		// Constructor
		public Camera(Game game, Vector3 position, Vector3 rotation, float speed)
			: base(game)
		{
			CameraSpeed = speed;

			Collision.SetPlayerDimensions(1.2f, 2f, 2f);

			//Collision.AddCollisionBox(new Vector3(0, 0, 0), new Vector3(20, -1, 20)); // Adding the floor
			//Collision.AddCollisionBox(new Vector3(0, 0, 0), new Vector3(-1, 20, 20)); // Adding the collision at the right
			//Collision.AddCollisionBox(new Vector3(20, 0, 0), new Vector3(21, 20, 20)); // Adding the collision at the left
			//Collision.AddCollisionBox(new Vector3(0, 0, 0), new Vector3(20, 20, -1)); // behind
			//Collision.AddCollisionBox(new Vector3(0, 0, 20), new Vector3(20, 20, 21)); // In the front

			AspectRatio = game.GraphicsDevice.Viewport.AspectRatio;

			// Setup projection matrix
			Projection = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.PiOver4,
				AspectRatio,
				1.0f,
				1000.0f // Draw distance
			);

			// Set the camera position and rotation
			moveTo(position, rotation);



			PreviousMouseState = Mouse.GetState();
		}



		// Set camera position and rotation
		private void moveTo(Vector3 position, Vector3 rotation)
		{
			// Thanks to the properties set at the beginning, setting up these 
			// values will execute the code inside the property (i.e update our
			// vectors)
			CameraOldPosition = Position;

			Position = position;
			Rotation = rotation;
		}

		// Update the look at vector
		private void updateLookAt()
		{
			// Build a rotation matrix
			Matrix rotationMatrix = Matrix.CreateRotationX(CameraRotation.X) * 
									Matrix.CreateRotationY(CameraRotation.Y);
			// Build look at offset vector
			Vector3 lookAtOffset = Vector3.Transform(
				Vector3.UnitZ, 
				rotationMatrix
			);
			// Update our camera's look at vector
			CameraLookAt = CameraPosition + lookAtOffset;
		}

		// Methods that simulate movement
		private Vector3 previewMove(Vector3 amount, float dt)
		{
			// Create a rotate matrix
			Matrix rotate = Matrix.CreateRotationY(CameraRotation.Y);
			// Create a movement vector
			Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
			movement = Vector3.Transform(movement, rotate);
			// Return the value of camera position + movement vector

			// Testing for the UPCOMING position
			if (Collision.CheckCollision(CameraPosition + movement)) 
			{
				// Creating the new movement vector, which will make use 
				// able to have a smooth collision: being able to "slide" on 
				// the wall while colliding
				movement = new Vector3(
					Collision.CheckCollision(CameraPosition + 
								new Vector3(movement.X, 0, 0)) ? 0 : movement.X,
					Collision.CheckCollision(CameraPosition + 
								new Vector3(0, movement.Y, 0)) ? 0 : movement.Y,
					Collision.CheckCollision(CameraPosition + 
								new Vector3(0, 0, movement.Z)) ? 0 : movement.Z
				);
				return CameraPosition + movement;
			}
			else
			{
				// There isn't any collision, so we just move the user with 
				// the movement he wanted to do
				return CameraPosition + movement;
			}
		}

		// Method that actually moves the camera
		private void move(Vector3 scale, float dt)
		{
			moveTo(previewMove(scale, dt), Rotation);
		}

		// Update method, overriding the original one
		public override void Update(GameTime gameTime)
		{
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			// Getting Mouse state
			CurrentMouseState = Mouse.GetState();

			// Let's get user inputs
			KeyboardState ks = Keyboard.GetState();

			// Handle basic key movement
			// moveVector will be used to generate the movement vector used to
			// move the player in the world.
			Vector3 moveVector = Vector3.Zero;
			if (ks.IsKeyDown(ForwardKey))
			{
				moveVector.Z = 1;
			}
			if (ks.IsKeyDown(BackKey))
			{
				moveVector.Z = -1;
			}

			if (ks.IsKeyDown(LeftKey))
			{
				moveVector.X = 1;
			}
			if (ks.IsKeyDown(RightKey))
			{
				moveVector.X = -1;
			}

			if (ks.IsKeyDown(Keys.Up))
			{
				moveVector.Y = 1;
			}
			if (ks.IsKeyDown(Keys.Down))
			{
				moveVector.Y = -1;
			}

			// If we are actually moving (if the vector changed depending
			// on the ifs)
			if (moveVector != Vector3.Zero) 
			{
				// Normalize that vector so that we don't move faster diagonally
				moveVector.Normalize();

				// Now we add in move factor and speed
				moveVector *= dt * CameraSpeed;


				// Move camera!
				move(moveVector, dt);
			}




			// Handle mouse movement
			float deltaX;
			float deltaY;
			if (CurrentMouseState != PreviousMouseState)
			{
				// Cache mouse location
				// We devide by 2 because mouse will be in the center
				deltaX = CurrentMouseState.X 
					- (Game.GraphicsDevice.Viewport.Width / 2); 
				deltaY = CurrentMouseState.Y 
					- (Game.GraphicsDevice.Viewport.Height / 2);

				MouseRotationBuffer.X -= MouseSpeed * deltaX * dt;
				MouseRotationBuffer.Y -= MouseSpeed * deltaY * dt;

				// Limit the user so he can't do an unlimited movement with 
				// his mouse (like a 7683°)
				if (MouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
				{
					MouseRotationBuffer.Y = MouseRotationBuffer.Y -
						(MouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
				}
				if (MouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
				{
					MouseRotationBuffer.Y = MouseRotationBuffer.Y -
						(MouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
				}

				float mouseInverted = (MouseInverted == true) ? 1 : -1;

				Rotation = new Vector3(
					mouseInverted * MathHelper.Clamp(
						MouseRotationBuffer.Y,
						MathHelper.ToRadians(-75.0f),
						MathHelper.ToRadians(75.0f)
					),
					MathHelper.WrapAngle(MouseRotationBuffer.X), // This is so 
					// the camera isn't going really fast after some time 
					// (as we are increasing the speed with time)
					0
				);

				// Resetting them
				deltaX = 0;
				deltaY = 0;

			}

			// Putting the cursor in the middle of the screen
			Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2,
				Game.GraphicsDevice.Viewport.Height / 2);

			PreviousMouseState = CurrentMouseState;

			base.Update(gameTime);
		}
	}
}
