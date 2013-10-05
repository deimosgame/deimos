using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    class Camera : GameComponent
    {
        // Atributes
        private Vector3 CameraPosition;
        private Vector3 CameraRotation;
        private float   CameraSpeed;
        private Vector3 CameraLookAt;

		private Vector3 MouseRotationBuffer;
		private MouseState CurrentMouseState;
		private MouseState PreviousMouseState;
		private float MouseSpeed = 0.1f;
		private Boolean MouseInverted = true;


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
				return Matrix.CreateLookAt(CameraPosition, CameraLookAt, Vector3.Up);
			}
		}

		// Constructor
		public Camera(Game game, Vector3 position, Vector3 rotation, float speed)
			: base(game)
		{
			CameraSpeed = speed;

			// Setup projection matrix
			Projection = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.PiOver4,
				Game.GraphicsDevice.Viewport.AspectRatio,
				0.05f,
				1000.0f // Draw distance
			);

			// Set the camera position and rotation
			moveTo(position, rotation);


			PreviousMouseState = Mouse.GetState();
		}





		// Set camera position and rotation
		private void moveTo(Vector3 position, Vector3 rotation)
		{
			// Thanks to the properties set at the beginning, setting up these values will execute 
			// the code inside the property (i.e update our vectors)
			Position = position;
			Rotation = rotation;
		}

		// Update the look at vector
		private void updateLookAt()
		{
			// Build a rotation matrix
			Matrix rotationMatrix = Matrix.CreateRotationX(CameraRotation.X) * Matrix.CreateRotationY(CameraRotation.Y);
			// Build look at offset vector
			Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
			// Update our camera's look at vector
			CameraLookAt = CameraPosition + lookAtOffset;
		}

		// Methods that simulate movement
		private Vector3 previewMove(Vector3 amount)
		{
			// Create a rotate matrix
			Matrix rotate = Matrix.CreateRotationY(CameraRotation.Y);
			// Create a movement vector
			Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
			movement = Vector3.Transform(movement, rotate);
			// Return the value of camera position + movement vector
			return CameraPosition + movement;
		}

		// Method that actually move the camera
		private void move(Vector3 scale)
		{
			moveTo(previewMove(scale), Rotation);
		}

		// Update method, overriding the original one
		public override void Update(GameTime gameTime)
		{
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			CurrentMouseState = Mouse.GetState();

			// Let's get user inputs
			KeyboardState ks = Keyboard.GetState();

			// Handle basic key movement
			Vector3 moveVector = Vector3.Zero;
			if (ks.IsKeyDown(Keys.Z))
				moveVector.Z = 1;
			if (ks.IsKeyDown(Keys.S))
				moveVector.Z = -1;
			if (ks.IsKeyDown(Keys.Q))
				moveVector.X = 1;
			if (ks.IsKeyDown(Keys.D))
				moveVector.X = -1;

			if (moveVector != Vector3.Zero) // If we are actually moving (if the vector changed depending on the ifs)
			{
				// Normalize that vector so that we don't move faster diagonally
				moveVector.Normalize();

				// Now we add in move factor and speed
				moveVector *= dt * CameraSpeed;

				// Move camera!
				move(moveVector);
			}


			// Handle mouse movement
			float deltaX;
			float deltaY;
			if (CurrentMouseState != PreviousMouseState)
			{
				// Cache mouse location
				deltaX = CurrentMouseState.X - (Game.GraphicsDevice.Viewport.Width / 2); // We devide by 2 because mouse will be in the center
				deltaY = CurrentMouseState.Y - (Game.GraphicsDevice.Viewport.Height / 2);

				MouseRotationBuffer.X -= MouseSpeed * deltaX * dt;
				MouseRotationBuffer.Y -= MouseSpeed * deltaY * dt;

				// Limit the user so he can't do an unlimited movement with his mouse (like a 7683°)
				if(MouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
					MouseRotationBuffer.Y = MouseRotationBuffer.Y - (MouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
				if(MouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
					MouseRotationBuffer.Y = MouseRotationBuffer.Y - (MouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));

				float mouseInverted = (MouseInverted == true) ? 1 : -1;

				Rotation = new Vector3(
					mouseInverted * MathHelper.Clamp( 
						MouseRotationBuffer.Y, 
						MathHelper.ToRadians(-75.0f),
						MathHelper.ToRadians(75.0f)
					), 
					MathHelper.WrapAngle(MouseRotationBuffer.X), // This is so the camera isn't going fucking fast after some time 
																// (as we are increasing the speed with time)
					0
				);

				// Resetting them
				deltaX = 0;
				deltaY = 0;

			}

			// Putting the cursor in the middle of the screen
			Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);

			PreviousMouseState = CurrentMouseState;

			base.Update(gameTime);

		}
    }
}
