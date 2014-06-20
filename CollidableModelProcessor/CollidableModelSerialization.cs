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
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System.IO;
using System.ComponentModel;
using XNAnimationPipeline;

namespace CollidableModelProcessor
{
    [ContentTypeWriter]
    public class CollidableModelContentWriter : ContentTypeWriter<CollidableModelContent>
    {
        protected override void Write(ContentWriter output, CollidableModelContent value)
        {
           // just write the original model
            output.WriteObject<ModelContent>(value.model);

            // write the xna animation one
            output.WriteObject<SkinnedModelContent>(value.skinnedModel);

            // write collision data
            output.WriteObject<BspTreeContent>( value.collisionData );
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "CollidableModel.CollidableModel, CollidableModel_" + targetPlatform + ", " +
                 "Version=1.0.0.0, Culture=neutral";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "CollidableModel.CollidableModelReader, CollidableModel_" + targetPlatform + ", " +
                "Version=1.0.0.0, Culture=neutral";
        }
    }


    [ContentTypeWriter]
    public class BspTreeContentWriter : ContentTypeWriter<BspTreeContent>
    {
        protected override void Write(ContentWriter output, BspTreeContent value)
        {
            // write geometry data first
            output.WriteObject<ModelGeometryContent>( value.geometry );

            // call recursive write procedure
            WriteTree(output, value.root);
        }

        private void WriteTree(ContentWriter output, BspTreeContent.BspNodeContent node)
        {
            output.WriteObject<byte>((byte)node.type);
            if (node.type == BspTreeContent.BspNodeContent.NodeType.Leaf)
            {
                output.Write(node.faces.Count);
                foreach (int idx in node.faces)
                    output.Write( idx);

                return;
            }

            // write node data
            output.Write(node.separatingPlane.Normal.X);
            output.Write(node.separatingPlane.Normal.Y);
            output.Write(node.separatingPlane.Normal.Z);
            output.Write(node.separatingPlane.D);

            // recursively write both sides
            WriteTree(output, node.pos);
            WriteTree(output, node.neg);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "CollidableModel.BspTree, CollidableModel_" + targetPlatform + ", " +
                 "Version=1.0.0.0, Culture=neutral";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "CollidableModel.BspTreeReader, CollidableModel_" + targetPlatform + ", " +
                "Version=1.0.0.0, Culture=neutral";
        }
    }


    [ContentTypeWriter]
    public class ModelGeometryContentWriter : ContentTypeWriter<ModelGeometryContent>
    {
        protected override void Write(ContentWriter output, ModelGeometryContent geometry)
        {
            // vertices
            output.Write(geometry.vertices.Count);
            foreach (Vector3 vertex in geometry.vertices)
                output.Write(vertex);

            // normals
            output.Write(geometry.normals.Count);
            foreach (Vector3 normal in geometry.normals)
                output.Write(normal);

            // faces
            output.Write(geometry.faces.Count);
            foreach (ModelGeometryContent.FaceContent f in geometry.faces)
            {
                output.Write(f.v1);
                output.Write(f.v2);
                output.Write(f.v3);

                output.Write(f.n1);
                output.Write(f.n2);
                output.Write(f.n3);

                output.Write(f.faceNormal);
                output.WriteObject<BoundingSphere>(f.boundingSphere);
            }

            // bounding box
            output.WriteObject<BoundingBox>(geometry.boundingBox);

            // visualization mesh
            if (geometry.visualizationData != null)
            {
                output.Write(true);
                output.WriteObject<ModelContent>(geometry.visualizationData);
            }
            else
                output.Write(false);
    

        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "CollidableModel.ModelGeometry, CollidableModel_" + targetPlatform + ", " +
                "Version=1.0.0.0, Culture=neutral";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "CollidableModel.ModelGeometryReader, CollidableModel_" + targetPlatform + ", " +
                "Version=1.0.0.0, Culture=neutral";
        }
    }
}
