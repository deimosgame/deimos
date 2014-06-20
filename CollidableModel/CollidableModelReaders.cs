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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.ComponentModel;
using CollidableModel;
using XNAnimation;

namespace CollidableModel
{
    public class CollidableModelReader : ContentTypeReader<CollidableModel>
    {
        protected override CollidableModel Read(ContentReader input, CollidableModel existingInstance)
        {
            Model model = input.ReadObject<Model>();
            SkinnedModel skinnedModel = input.ReadObject<SkinnedModel>();
            BspTree tree = input.ReadObject<BspTree>();

            return new CollidableModel( model, skinnedModel, tree );
        }
    }

    public class BspTreeReader : ContentTypeReader<BspTree>
    {
        protected override BspTree Read(ContentReader input, BspTree existingInstance)
        {
            // read geometry first
            ModelGeometry geometry = input.ReadObject<ModelGeometry>();

            // now read tree
            BspNode root = null;
            ReadTree( ref root, input, geometry );

            return new BspTree( root, geometry );
        }

        private void ReadTree( ref BspNode node, ContentReader input, ModelGeometry geometry )
        {
            byte type = input.ReadObject<byte>();
            if (type == 0)
            {
                // read list of faces
                int numFaces = input.ReadInt32();
                int[] faces = new int[numFaces];
                for (int i = 0; i < numFaces; i++)
                {
                    int idx = input.ReadInt32();
                    Face f = geometry.faces[idx];
                    f.boundingSphere = new BoundingSphere( f.boundingSphere.Center, f.boundingSphere.Radius );
                    f.faceNormal = new Vector3( f.faceNormal.X, f.faceNormal.Y, f.faceNormal.Z );
                    faces[i]= idx ;
                }

                // create new leaf node
                node = new BspNode();
                node.faces = faces;
                node.type = 0;

                return;
            }
            else
            {
                Single x = input.ReadSingle();
                Single y = input.ReadSingle();
                Single z = input.ReadSingle();
                Single d = input.ReadSingle();

                node = new BspNode();
                node.separatingPlane = new Plane(x, y, z, d);
                node.type = 1;

                ReadTree(ref node.pos, input, geometry);
                ReadTree(ref node.neg, input, geometry);
            }
        }
    }

    public class ModelGeometryReader : ContentTypeReader<ModelGeometry>
    {
        protected override ModelGeometry Read(ContentReader input, ModelGeometry existingInstance)
        {
            // vertices
            int numVertices = input.ReadInt32();
            Vector3[] vertices = new Vector3[numVertices];
            for (int i = 0; i < numVertices; i++)
                vertices[i] = input.ReadVector3();

            // normals
            int numNormals = input.ReadInt32();
            Vector3[] normals = new Vector3[numNormals];
            for (int i = 0; i < numNormals; i++)
                normals[i] = input.ReadVector3();

            // faces
            int numFaces = input.ReadInt32();
            Face[] faces = new Face[numFaces];
            for (int i = 0; i < numFaces; i++)
            {
                int v1 = input.ReadInt32();
                int v2 = input.ReadInt32();
                int v3 = input.ReadInt32();

                int n1 = input.ReadInt32();
                int n2 = input.ReadInt32();
                int n3 = input.ReadInt32();

                faces[i] = new Face(v1, v2, v3, n1, n2, n3);
                faces[i].faceNormal = input.ReadVector3();
                faces[i].boundingSphere = input.ReadObject<BoundingSphere>();
            }

            // read bounding box
            BoundingBox bb = input.ReadObject<BoundingBox>();

            // read visualization mesh
            bool visualization = input.ReadBoolean();
            Model visualizationData = null;
            if (visualization)
                visualizationData = input.ReadObject<Model>();

            return new ModelGeometry(vertices, normals, faces, bb, visualizationData);
        }
    }
}
