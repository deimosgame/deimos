using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Helpers
    {
        public BoundingBox BoundingBoxFromModel(CollidableModel.CollidableModel model)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); 
                        i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = new Vector3(
                            vertexData[i],
                            vertexData[i + 1],
                            vertexData[i + 2]
                        );

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box
            return new BoundingBox(min, max);
        }

        public BoundingBox PlayerBoundingBox(Vector3 dimension)
        {
            Vector3 bbTop = new Vector3(
                - (dimension.X / 2),
                - (dimension.Y / 3) * 2,
                - (dimension.Z / 2)
            );
            Vector3 bbBottom = new Vector3(
                (dimension.X / 2),
                (dimension.Y / 3),
                (dimension.Z / 2)
            );

            return new BoundingBox(
                bbTop,
                bbBottom
            );
        }

        public BoundingBox CreateBoundingBox(Vector3 dimension)
        {
            Vector3 bbTop = new Vector3(
                dimension.X,
                dimension.Y,
                dimension.Z
            );
            Vector3 bbBottom = new Vector3(
                0,
                0,
                0
            );

            return new BoundingBox(
                bbBottom,
                bbTop
            );
        }
    }
}
