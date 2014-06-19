//--------------------------------------------------------------------------------------//
//                                                                                      //
//    __  __  __    __    __  ___   __   ___  __    ___    __  __  __  ___  ___  __     //
//   / _)/  \(  )  (  )  (  )(   \ (  ) (  ,)(  )  (  _)  (  \/  )/  \(   \(  _)(  )    //
//  ( (_( () ))(__  )(__  )(  ) ) )/__\  ) ,\ )(__  ) _)   )    (( () )) ) )) _) )(__   //
//   \__)\__/(____)(____)(__)(___/(_)(_)(___/(____)(___)  (_/\/\_)\__/(___/(___)(____)  //
//                                                                                      //
//                                                                                      //
//       Copyright by Theodor Mader, 2009                                               //
//			www.theomader.com/public/Projects.html                                      //
//--------------------------------------------------------------------------------------//


using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace CollidableModel
{
    public class Face : IComparable<Face>
    {
        private int vertexID1;
        public int v1 { get { return vertexID1; } }

        private int vertexID2;
        public int v2 { get { return vertexID2; } }

        private int vertexID3;
        public int v3 { get { return vertexID3; } }

        private int normalID1;
        public int n1 { get { return normalID1; } }

        private int normalID2;
        public int n2 { get { return normalID2; } }

        private int normalID3;
        public int n3 { get { return normalID3; } }

        private Vector3 mFaceNormal;
        public Vector3 faceNormal { 
            get { return mFaceNormal; }
            set { mFaceNormal = value; }
        }

        public BoundingSphere boundingSphere;

        private uint ID;
        private static uint mNextID = 0;

        public Face( int vertex1, int vertex2, int vertex3, int normal1, int normal2, int normal3 )
        {
            vertexID1 = vertex1;
            vertexID2 = vertex2;
            vertexID3 = vertex3;

            normalID1 = normal1;
            normalID2 = normal2;
            normalID3 = normal3;

            ID = mNextID;
            mNextID++;
         }

        public int CompareTo( Face other)
        {
            return ID.CompareTo( other.ID );
        }
    }


    /// <summary>
    /// LevelGeometry holds the geometry referenced by the bsp tree. Following the definition of
    /// .obj files, it stores vertices and normals in arrays, and triangles reference into these arrays
    /// For debugging purposes, the class also allows the creation of a vertex and index buffer, and 
    /// provides a shader that can be used to draw the geometry.
    /// </summary>
    public class ModelGeometry
    {
        private Vector3[] mVertices;
        public Vector3[] vertices
        {
            get { return mVertices; }
        }

        private Vector3[] mNormals;
        public Vector3[] normals
        {
            get { return mNormals; }
        }

        private Face[] mFaces;
        public Face[] faces
        {
            get { return mFaces; }
        }

        private BoundingBox mBoundingBox;
        public BoundingBox boundingBox
        {
            get { return mBoundingBox; }
        }

        public Model mVisualizationData = null;
        public Model visualizationData
        {
            get { return mVisualizationData; }
        }

        public ModelGeometry(Vector3[] vertices, Vector3[] normals, Face[] faces, BoundingBox boundingBox, Model visualizationData )
        {
            mVertices = vertices;
            mNormals = normals;
            mFaces = faces;
            mBoundingBox = boundingBox;
            mVisualizationData = visualizationData;
        }
    }
}
