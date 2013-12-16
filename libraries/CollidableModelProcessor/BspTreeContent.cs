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
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using System.Linq;

namespace CollidableModelProcessor
{
    public class BspTreeContent
    {
        public class BspNodeContent
        {
            public enum NodeType
            {
                Leaf = 0,
                InternalNode = 1
            };

            public BspNodeContent(Plane plane)
            {
                separatingPlane = plane;
                type = BspNodeContent.NodeType.InternalNode;
            }

            public BspNodeContent(NodeType nodeType)
            {
                type = nodeType;
            }

            public BspNodeContent pos, neg;
            public Plane separatingPlane;
            public List<int> faces = new List<int>();

            public NodeType type;
        }

        // the input data       
        private byte[] mTreeData;

        private BspNodeContent mRoot;
        public BspNodeContent root
        {
            get { return mRoot; }
        }

        private ModelGeometryContent mGeometry;
        public ModelGeometryContent geometry
        {
            get { return mGeometry; }
        }

        public BspTreeContent(byte[] treeData, int startIndex)
        {
            mTreeData = treeData;

            int startPosition = startIndex;
            BspNodeContent root = null;
            buildTree(ref root, ref startPosition);

            mRoot = root;
        }

        public void AddGeometry(ModelGeometryContent geometry)
        {
            mGeometry = geometry;

            for (int i = 0; i < geometry.faces.Count; i++)
                insertFace(i);
        }

        private void buildTree(ref BspNodeContent curNode, ref int curPosition)
        {
            // read node type first
            byte b = mTreeData[curPosition];
            curPosition++;

            // did we read a leaf?
            if (b == 0)
            {
                curNode = new BspNodeContent(BspNodeContent.NodeType.Leaf);
                return;
            }

            // internal node, read plane data
            float x = BitConverter.ToSingle(mTreeData, curPosition);
            curPosition += sizeof(Single);

            float y = BitConverter.ToSingle(mTreeData, curPosition);
            curPosition += sizeof(Single);

            float z = BitConverter.ToSingle(mTreeData, curPosition);
            curPosition += sizeof(Single);

            float d = BitConverter.ToSingle(mTreeData, curPosition);
            curPosition += sizeof(Single);

            // create internal node
            Plane separatingPlane = new Plane(x, y, z, d);
            curNode = new BspNodeContent(separatingPlane);

            // recurse to left/right
            buildTree(ref curNode.pos, ref curPosition);
            buildTree(ref curNode.neg, ref curPosition);

        }

        private void insertFace(int faceIdx)
        {
            ModelGeometryContent.FaceContent f = geometry.faces[faceIdx];

            Vector3 pos1 = mGeometry.vertices[f.v1];
            Vector3 pos2 = mGeometry.vertices[f.v2];
            Vector3 pos3 = mGeometry.vertices[f.v3];

            // create a list of nodes we have to process on our way to the leafs of the tree
            LinkedList<BspNodeContent> toProcess = new LinkedList<BspNodeContent>();
            toProcess.AddFirst(mRoot);

            // as long as we have nodes to process
            while (toProcess.Count > 0)
            {
                // take first node
                BspNodeContent curNode = toProcess.First.Value;
                toProcess.RemoveFirst();

                if (curNode.type == BspNodeContent.NodeType.Leaf)
                {
                    curNode.faces.Add(faceIdx);
                }
                // not leaf? propagate face to correct child node. (both nodes in case of intersection)
                else
                {
                    // compute side each triangle vertex lies on
                    float side1 = Vector3.Dot(curNode.separatingPlane.Normal, pos1) + curNode.separatingPlane.D;
                    float side2 = Vector3.Dot(curNode.separatingPlane.Normal, pos2) + curNode.separatingPlane.D;
                    float side3 = Vector3.Dot(curNode.separatingPlane.Normal, pos3) + curNode.separatingPlane.D;


                    if (side1 > 0 && side2 > 0 && side3 > 0)
                    {
                        // all points on positive side? propagate to positive side
                        toProcess.AddLast(curNode.pos);
                    }
                    else if (side1 <= 0 && side2 <= 0 && side3 <= 0)
                    {
                        // all points on negative side?  propagate to negative side
                        toProcess.AddLast(curNode.neg);
                    }
                    else
                    {
                        // no luck, triangle intersects plane, we have to propagate it on both sides
                        toProcess.AddLast(curNode.pos);
                        toProcess.AddLast(curNode.neg);
                    }
                }
            }
        }

        public void forEachNode( BspNodeContent node, Action<BspNodeContent> action)
        {
            if (node !=null)
            {
                action(node);
                forEachNode(node.pos, action);
                forEachNode(node.neg, action);
            }
        }

        public void forEachLeaf(BspNodeContent node, Action<BspNodeContent> action)
        {
            forEachNode(node, n =>
                {
                    if (n.type == BspNodeContent.NodeType.Leaf)
                        action(n);
                });
        }

        public void printStatistics( ContentBuildLogger logger )
        {
            logger.LogImportantMessage("---- Collision data: node statistics: -----");
            int numNodes = 0;
            int numLeaves = 0;
            int numFaces = 0;

            int maxFacesPerNode = 0;
            int minFacesPerNode = int.MaxValue;

            double meanSquared = 0.0;

            forEachNode(mRoot, node =>
                {
                    numNodes++;

                    if (node.type == BspNodeContent.NodeType.Leaf)
                    {
                        numLeaves++;
                        numFaces += node.faces.Count;

                        maxFacesPerNode = Math.Max(maxFacesPerNode, node.faces.Count);
                        minFacesPerNode = Math.Min(minFacesPerNode, node.faces.Count);

                        meanSquared += node.faces.Count * node.faces.Count;
                    }
                });

            double mean = (double)numFaces / numLeaves;
            double variance  = Math.Sqrt(meanSquared / numLeaves - mean * mean);

            logger.LogImportantMessage("Min number of triangles:\t{0}", minFacesPerNode);
            logger.LogImportantMessage("Max number of triangles:\t{0}", maxFacesPerNode);
            logger.LogImportantMessage("On average we have {0} +- {1} triangles per node", mean, variance);

            {
                int histSize = 10;
                int histVisualizationBins = 50;
                int[] histogram = new int[histSize];
                int[] bins = new int[histSize];

                for (int i = 1; i <= histSize; i++)
                    bins[i - 1] = (int)Math.Ceiling(minFacesPerNode + (maxFacesPerNode - minFacesPerNode) * ((float)i) / histSize);
                bins[histSize - 1] = maxFacesPerNode;

                forEachLeaf(mRoot, leaf =>
                    {
                        
                        for (int i = 0; i < bins.Length; i++)
                        {
                            if (leaf.faces.Count <= bins[i])
                            {
                                histogram[i]++;
                                break;
                            }
                        }
                    });


                // print histogram to console
                int nodesToStars = (int)Math.Round(((float)numLeaves) / histVisualizationBins);
                logger.LogImportantMessage("Node distribution (Number of nodes with x triangles, one * equals {0} nodes):", nodesToStars);

                for (int i = 0; i < histSize; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{0} - {1}:\t", i == 0 ? minFacesPerNode : bins[i - 1], bins[i]);

                    float fraction = histogram[i] / (float)numLeaves;
                    var s = System.Linq.Enumerable.Range(0, histVisualizationBins).Select(k => k < fraction * histVisualizationBins ? '*' : ' ');
                    sb.Append(s.ToArray());

                    sb.AppendFormat("\t{0}\t= {1}%", histogram[i], fraction * 100.0f);
                    logger.LogImportantMessage(sb.ToString());
                }
            }

            logger.LogImportantMessage("On average each triangles appears {0} times", numFaces / (float)geometry.faces.Count);
            logger.LogImportantMessage("---------------------------");
        }
    }
}
