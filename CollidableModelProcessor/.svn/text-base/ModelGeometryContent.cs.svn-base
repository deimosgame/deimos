//--------------------------------------------------------------------------------------//
//                                                                                      //
//    __  __  __    __    __  ___   __   ___  __    ___    __  __  __  ___  ___  __     //
//   / _)/  \(  )  (  )  (  )(   \ (  ) (  ,)(  )  (  _)  (  \/  )/  \(   \(  _)(  )    //
//  ( (_( () ))(__  )(__  )(  ) ) )/__\  ) ,\ )(__  ) _)   )    (( () )) ) )) _) )(__   //
//   \__)\__/(____)(____)(__)(___/(_)(_)(___/(____)(___)  (_/\/\_)\__/(___/(___)(____)  //
//                                                                                      //
//                                                                                      //
//       Copyright by Theodor Mader, 2011                                               //
//			www.theomader.com/public/Projects.html                                      //
//--------------------------------------------------------------------------------------//


using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using System.Linq;

namespace CollidableModelProcessor
{
    public class ModelGeometryContent
    {
        public class FaceContent : IComparable<FaceContent>
        {
            public int v1;
            public int v2;
            public int v3;

            public int n1;
            public int n2;
            public int n3;

            public Vector3 faceNormal;
            public BoundingSphere boundingSphere;

            private uint ID;
            private static uint mNextID = 0;

            public FaceContent(int vertex1, int vertex2, int vertex3, int normal1, int normal2, int normal3)
            {
                v1 = vertex1;
                v2 = vertex2;
                v3 = vertex3;

                n1 = normal1;
                n2 = normal2;
                n3 = normal3;

                ID = mNextID;
                mNextID++;
            }

            public int CompareTo(FaceContent other)
            {
                return ID.CompareTo(other.ID);
            }
        }


        private List<Vector3> mVertices = new List<Vector3>();
        public List<Vector3> vertices
        {
            get { return mVertices; }
        }

        private List<Vector3> mNormals = new List<Vector3>();
        public List<Vector3> normals
        {
            get { return mNormals; }
        }

        private List<FaceContent> mFaces = new List<FaceContent>();
        public List<FaceContent> faces
        {
            get { return mFaces; }
        }

        private BoundingBox mBoundingBox;
        public BoundingBox boundingBox
        { 
            get { return mBoundingBox; } 
        }

        private ModelContent mVisualizationData = null;
        public ModelContent visualizationData
        {
            get { return mVisualizationData; }
        }

        public ModelGeometryContent(ModelContent model)
        {
            Dictionary<Vector3, int> uniquePositions = new Dictionary<Vector3, int>();
            Dictionary<Vector3, int> uniqueNormals = new Dictionary<Vector3, int>();

            mVertices.Clear();
            mNormals.Clear();
            mFaces.Clear();

            foreach (ModelMeshContent meshContent in model.Meshes)
            {
                MeshContent sourceMesh = meshContent.SourceMesh;
                Matrix absoluteTransform = sourceMesh.AbsoluteTransform;
                Matrix inverseTransposeTransform = Matrix.Transpose( Matrix.Invert( absoluteTransform ) );

                // foreach geometry batch of the mesh
                foreach (GeometryContent geometry in sourceMesh.Geometry)
                {
                    IEnumerable<Vector3> normalStream = null;

                    // see if the model contains a normal channel
                    foreach (VertexChannel channel in geometry.Vertices.Channels)
                    {
                        if (channel.Name.StartsWith("Normal"))
                        {
                            normalStream = channel.ReadConvertedContent<Vector3>();
                            break;
                        }
                    }

                    // convert normals stream to list
                    List<Vector3> batchNormals = null;

                    // copy normals to array and transform by mesh part transform
                    if (normalStream != null)
                    {
                        batchNormals = new List<Vector3>();
                        foreach (Vector3 n in normalStream)
                            batchNormals.Add(Vector3.TransformNormal(n, inverseTransposeTransform));
                    }

                    // now read faces and get rid of duplicate vertices/normals
                    for (int i = 0; i < geometry.Indices.Count; i += 3)
                    {
                        int[] vidx = new int[3];
                        int[] nidx = new int[3] { -1, -1, -1 };

                        for (int j = 0; j < 3; j++)
                        {
                            int idx = geometry.Indices[i + j];
                            Vector3 v = geometry.Parent.Positions[geometry.Vertices.PositionIndices[idx]];
                            v = Vector3.Transform(v, absoluteTransform);

                            if (!uniquePositions.TryGetValue(v, out vidx[j]))
                            {
                                vidx[j] = mVertices.Count;
                                uniquePositions.Add(v, vidx[j]);
                                mVertices.Add(v );
                            }

                            if (batchNormals != null)
                            {
                                Vector3 n = batchNormals[geometry.Indices[i + j]];
                                if (!uniqueNormals.TryGetValue(n, out nidx[j]))
                                {
                                    nidx[j] = mNormals.Count;
                                    uniqueNormals.Add(n, nidx[j]);
                                    mNormals.Add(n);
                                }
                            }
                        }

                        // add face to list of faces
                        FaceContent face = new FaceContent(vidx[0], vidx[1], vidx[2], nidx[0], nidx[1], nidx[2]);
                        mFaces.Add(face);
                    }
                }
            }

            // compute bounding sphere for each face, and also face normals
            foreach (FaceContent f in faces)
            {
                Vector3 center = vertices[f.v1] + vertices[f.v2] + vertices[f.v3];
                center /= 3;

                float radius = (center - vertices[f.v1]).Length();
                float dist2 = (center - vertices[f.v2]).Length();
                if (radius < dist2) radius = dist2;

                float dist3 = (center - vertices[f.v3]).Length();
                if (radius < dist3) radius = dist3;

                f.boundingSphere = new BoundingSphere(center, radius);

                // compute face normal
                f.faceNormal = Vector3.Cross(
                    vertices[f.v2] - vertices[f.v1],
                    vertices[f.v3] - vertices[f.v1]
                );
                f.faceNormal.Normalize();

                // make sure we get valid normal for every face: replace invalid normals by face normals
                if (f.n1 == -1)
                {
                    if (!uniqueNormals.TryGetValue(f.faceNormal, out f.n1 ))
                    {
                        f.n1 = mNormals.Count;
                        uniqueNormals.Add(f.faceNormal, f.n1);
                        normals.Add(f.faceNormal);
                    }
                }

                if (f.n2 == -1)
                {
                    if (!uniqueNormals.TryGetValue(f.faceNormal, out f.n2))
                    {
                        f.n2 = mNormals.Count;
                        uniqueNormals.Add(f.faceNormal, f.n2);
                        normals.Add(f.faceNormal);
                    }
                }

                if (f.n3 == -1)
                {
                    if (!uniqueNormals.TryGetValue(f.faceNormal, out f.n2))
                    {
                        f.n2 = mNormals.Count;
                        uniqueNormals.Add(f.faceNormal, f.n2);
                        normals.Add(f.faceNormal);
                    }
                }
            }

            // last step: compute bounding box
            mBoundingBox = calcBoundingBox();
        }

        public void CreateVisualizationData( ContentProcessorContext context, BspTreeContent bspTree )
        {
            // create a new mesh builder for building the collision data model
            MeshBuilder builder = MeshBuilder.StartMesh( "CollisionData" );

            // count number of times each face appears in of tree
            uint[] faceCounts = new uint[mFaces.Count];
            bspTree.forEachLeaf(bspTree.root, leaf =>
                {
                    foreach (var face in leaf.faces)
                    {
                        faceCounts[face]++;
                    }
                    
                });
            uint maxFaceCount = faceCounts.Max();

            // add vertex positions to the mesh
            foreach (Vector3 v in mVertices)
                builder.CreatePosition(v);

            int normalChannelIndex = builder.CreateVertexChannel<Vector3>(VertexChannelNames.Normal());
            int colorChannelIndex = builder.CreateVertexChannel<Vector4>(VertexChannelNames.Color(0));

            // set basic material content to use for all faces
            BasicMaterialContent basicMaterial = new BasicMaterialContent();
            basicMaterial.VertexColorEnabled = true;
            builder.SetMaterial(basicMaterial);

            var good = new Vector4(0, 1, 0, 1);
            var bad = new Vector4(1, 0, 0, 1);
            // now run through all faces and add normal and vertex index index information
            for( int faceIndex = 0; faceIndex < mFaces.Count; ++faceIndex )
            {
                var face = mFaces[faceIndex];
                var color = Vector4.Lerp(good, bad, faceCounts[faceIndex] / ((float)maxFaceCount));
              //  var color = new Vector4( faceCounts[faceIndex] / ((float)maxFaceCount));

                builder.SetVertexChannelData(normalChannelIndex, mNormals[face.n1]);
                builder.SetVertexChannelData(colorChannelIndex, color);
                builder.AddTriangleVertex(face.v1);

                builder.SetVertexChannelData(normalChannelIndex, mNormals[face.n2]);
                builder.SetVertexChannelData(colorChannelIndex, color);
                builder.AddTriangleVertex(face.v2);

                builder.SetVertexChannelData(normalChannelIndex, mNormals[face.n3]);
                builder.SetVertexChannelData(colorChannelIndex, color);
                builder.AddTriangleVertex(face.v3);
            }

            // and voila.. finished is our mesh.. nice work xna!
            MeshContent mesh = builder.FinishMesh();
            NodeContent root = new NodeContent();
            root.Children.Add( mesh );
            ModelContent model = context.Convert<NodeContent, ModelContent>(root, "ModelProcessor");
            mVisualizationData = model;

        }

        // calculates the min/max coordinates in x, y and z direction
        private BoundingBox calcBoundingBox()
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (Vector3 v in mVertices)
            {
                if (v.X < min.X) min.X = v.X;
                else if (v.X > max.X) max.X = v.X;

                if (v.Y < min.Y) min.Y = v.Y;
                else if (v.Y > max.Y) max.Y = v.Y;

                if (v.Z < min.Z) min.Z = v.Z;
                else if (v.Z > max.Z) max.Z = v.Z;
            }

            return new BoundingBox(min, max);
        }
	
    }
}
