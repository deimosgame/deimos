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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using XNAnimationPipeline;

using TInput = Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent;
using TOutput = CollidableModelProcessor.CollidableModelContent;

namespace CollidableModelProcessor
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "CollidableModel")]
    public class CollidableModelProcessor : ContentProcessor<TInput, TOutput>
    {

        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            // prepare processor parameter. If you want to use your own model processor, prepare your custom parameter here
            OpaqueDataDictionary processorParameters = new OpaqueDataDictionary();
            OpaqueDataDictionary processorParameters2 = new OpaqueDataDictionary();
            // Convert the model. You can use your own model processor by specifying the processor name in the second parameter
            context.Logger.LogImportantMessage("CollidableModelProcessor: Converting model to XNA model format, using processor '{0}'", mModelProcessor);
            ModelContent convertedModel = context.Convert<NodeContent, ModelContent>(input, mModelProcessor, processorParameters);
            SkinnedModelContent skinnedModel = null;
            try
            {
                skinnedModel = context.Convert<NodeContent, SkinnedModelContent>(input, "SkinnedModelProcessor", processorParameters2);
            }
            catch (Exception)
            {
                // This is when XNAnimation can't be used
            }
            
            context.Logger.LogImportantMessage("CollidableModelProcessor: Done");

            // extract geometry data (optimized for the collision structure)
            context.Logger.LogImportantMessage("CollidableModelProcessor: Extracting geometry data");
            ModelGeometryContent geometry = new ModelGeometryContent(convertedModel);
            context.Logger.LogImportantMessage("CollidableModelProcessor: Done");

            // propose number of vertices per node value
            if (mNumVerticesPerBin == 0)
            {
                int maxAllowedTasks = 3600;
                int minAllowedVPB = 5;

                mNumVerticesPerBin =(int)(geometry.vertices.Count * 2 / maxAllowedTasks);
                if (mNumVerticesPerBin < minAllowedVPB)
                    mNumVerticesPerBin = minAllowedVPB;

                context.Logger.LogImportantMessage("CollidableModelProcessor: proposing NumVerticesPerBin = {0}", mNumVerticesPerBin);

            }

            // compute collision data
            context.Logger.LogImportantMessage("CollidableModelProcessor: Computing collision data with {0} vertices per bin", mNumVerticesPerBin );
            BspTreeContent collisionData = GenerateCollisionData(geometry, (uint)mNumVerticesPerBin);
            collisionData.printStatistics(context.Logger);
            context.Logger.LogImportantMessage("CollidableModelProcessor: Done" );

            if (mAddVisualizationData)
            {
                context.Logger.LogImportantMessage("CollidableModelProcessor: Adding visualization data");
                geometry.CreateVisualizationData(context, collisionData);
                context.Logger.LogImportantMessage("CollidableModelProcessor: Done");

            }
        
            CollidableModelContent collidableModel = new CollidableModelContent( convertedModel, skinnedModel, collisionData );
            return collidableModel;
        }

        [DllImport("GeometryHelper.dll")]
        public static extern bool ComputeCollisionData([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] float[] vertices, uint vertexArraySize, uint numVerticesPerBin, uint numThreads, out IntPtr result, out uint resultSize);
        
        [DllImport("GeometryHelper.dll")]
        public static extern void FreeCollisionData(IntPtr data);

        private BspTreeContent GenerateCollisionData(ModelGeometryContent geometry, uint numVerticesPerBin )
        {
            // convert vertices to flat vertex buffer next
            float[] vertexData = new float[geometry.vertices.Count * 3];
            uint idx = 0;
            foreach (Vector3 v in geometry.vertices)
            {
                vertexData[idx] = v.X;
                vertexData[idx + 1] = v.Y;
                vertexData[idx + 2] = v.Z;

                idx += 3;
            }

            // call geometryhelper dll to compute collision structure
            IntPtr resultPtr;
            uint resultSize;
            ComputeCollisionData(vertexData, (uint)geometry.vertices.Count * 3, numVerticesPerBin, mNumProcessingThreads, out resultPtr, out resultSize);

            // free vertex buffer
            vertexData = null;

            // convert result array to c# datatypes
            byte[] collData = new byte[resultSize];
            Marshal.Copy(resultPtr, collData, 0, (int)resultSize);

            // reconstruct bsp tree and free memory
            BspTreeContent bspTree = new BspTreeContent( collData, 0 );
            collData = null;
            FreeCollisionData(resultPtr);

            // extract model geometry and add to the tree
            bspTree.AddGeometry( geometry );
            return bspTree;
        }

        private string mModelProcessor = "ModelProcessor";
        [DisplayName("ModelProcessor")]
        [DefaultValue("ModelProcessor")]
        [Description("The processor used to convert the model to XNA format. Uses the standard Xna model processor 'ModelProcessor' by default")]
        public string ModelProcessor
        {
            get { return mModelProcessor; }
            set { mModelProcessor = value; }
        }

        private int mNumVerticesPerBin = 0;
        [DisplayName("NumVerticesPerBin")]
        [DefaultValue(0)]
        [Description("The number of vertices per collision data node. Usually, the smaller the value, the faster the collision detection will be. Processing however might become significantly slower. Use 0 to let the processor choose a reasonable tradeoff.")]
        public int NumVerticesPerBin
        {
            get { return mNumVerticesPerBin; }
            set { mNumVerticesPerBin = value; }
        }

        private bool mAddVisualizationData = false;
        [DisplayName("AddVisualizationData")]
        [DefaultValue(false)]
        [Description("If set to true, an additional copy of the collision data is generated for simple visualization. This can be very helpful for detecting problems, but creates a large data overhead. ")]
        public bool AddVisualizationData
        {
            get { return mAddVisualizationData; }
            set { mAddVisualizationData = value; }
        }

        private uint mNumProcessingThreads = 4;
        [DisplayName("NumProcessingThreads")]
        [DefaultValue(4)]
        [Description("The number of threads used for computing collision data. Set to 0 or 1 to disable multithreading.")]
        public uint NumProcessingThreads
        {
            get { return mNumProcessingThreads; }
            set { mNumProcessingThreads = value; }
        }
    }
}