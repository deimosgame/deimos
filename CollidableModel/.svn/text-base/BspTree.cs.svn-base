// ========================================================================================== //
//                      ____                             _                                    //
//                     |  _ \                           | |                                   //
//                     | |_) | ___  _   _ _ __   ___ ___| |                                   //
//                     |  _ < / _ \| | | | '_ \ / __/ _ \ |                                   //
//                     | |_) | (_) | |_| | | | | (_|  __/_|                                   //
//                     |____/ \___/ \__,_|_| |_|\___\___(_)                                   //
//                                                                                            //
// ========================================================================================== //
// Developed and implemented by:                                                              //
//     Theodor Mader                                                                          //
//                                                                                            //
// Part of Portfolio projects, www.theomader.com                                              //
//                                                                                            //
// Copyright 2008. All rights reserved.                                                       //
// ========================================================================================== //


#region File Description
//-----------------------------------------------------------------------------//
// Class: BspTree.cs
// Author: Theodor Mader
//                                                            
// 
//-----------------------------------------------------------------------------//
#endregion


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace CollidableModel
{
    public class BspNode
    {
        public BspNode pos, neg;
        public Plane separatingPlane;
        public int[] faces;

        public byte type;
    }

    public class BspTree
    {
        private BspNode mRoot;  // root node

        // A separate copy of the level geometry
        private ModelGeometry mGeometry = null;    
        public ModelGeometry geometry
        {
            get { return mGeometry; }
        }
        public int numTrianglesChecked;
        public int numNodesReached;

        public BspTree(BspNode root, ModelGeometry collisionGeometry)
        {
            mRoot = root;
            mGeometry = collisionGeometry;
        }
   
        /// <summary>
        /// Computes all triangles colliding with the given bounding sphere
        /// </summary>
        /// <param name="b">Bounding sphere to be checked for collision</param>
        /// <param name="collidingFaces">Colliding faces are added to this list (may contain duplicates!)</param>
        /// <param name="collisionPoints">For each colliding face, the exact collision points is added to this list</param>
        public void collisions(BoundingSphere b, LinkedList<Face> collidingFaces, LinkedList<Vector3> collisionPoints)
        {
            LinkedList<BspNode> toProcess = new LinkedList<BspNode>();
            toProcess.AddLast(mRoot);

            while (toProcess.Count > 0)
            {
                BspNode curNode = toProcess.First.Value;
                toProcess.RemoveFirst();

                if (curNode.type == 0 ) // is BspLeaf)
                {
                    numNodesReached++;
                    numTrianglesChecked += curNode.faces.Length;

                  //  BspLeaf leaf = curNode as BspLeaf;
                    nodeCollisions(b, curNode, collidingFaces, collisionPoints);
                }
                else
                {
                  //  BspInternalNode internalNode = curNode as BspInternalNode;

                    // propagate bounding sphere down the tree
                    PlaneIntersectionType side = curNode.separatingPlane.Intersects(b);

                    if (side == PlaneIntersectionType.Back) toProcess.AddLast(curNode.neg);
                    else if (side == PlaneIntersectionType.Front) toProcess.AddLast(curNode.pos);
                    else
                    {
                        toProcess.AddLast(curNode.pos);
                        toProcess.AddLast(curNode.neg);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the raw number of triangles actually checked for collision 
        /// (all triangles in all nodes that are reached, note: some triangles might be contained in several nodes)
        /// </summary>
        /// <param name="b"></param>
        /// <param name="result"></param>
        public void checkedFaces(BoundingSphere b, LinkedList<Face> result)
        {
            LinkedList<BspNode> toProcess = new LinkedList<BspNode>();
            toProcess.AddLast(mRoot);

            while (toProcess.Count > 0)
            {
                BspNode curNode = toProcess.First.Value;
                toProcess.RemoveFirst();

                if( curNode.type == 0 )
                {
                    foreach (int f in curNode.faces)
                        result.AddLast(mGeometry.faces[f]);

                }
                else
                {
                    PlaneIntersectionType side = curNode.separatingPlane.Intersects(b);

                    if (side == PlaneIntersectionType.Back) toProcess.AddLast(curNode.neg);
                    else if (side == PlaneIntersectionType.Front) toProcess.AddLast(curNode.pos);
                    else
                    {
                        toProcess.AddLast(curNode.pos);
                        toProcess.AddLast(curNode.neg);
                    }
                }
            }
        }

        /// <summary>
        ///  Adds all triangles of node which intersect b to list result, returns exact intersection points in 
        /// intersectionPoints. the function closestPointInTriangle() is used for computing triangle-sphere
        /// intersections.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="node"></param>
        /// <param name="result"></param>
        private void nodeCollisions(BoundingSphere b, BspNode node, LinkedList<Face> result, LinkedList<Vector3> intersectionPoints)
        {
            float squaredBoundingSphereRadius = b.Radius * b.Radius;

            foreach (int i in node.faces)
            {
                Face face = mGeometry.faces[i];
                Vector3 pos1, pos2, pos3;
                pos1 = mGeometry.vertices[face.v1];
                pos2 = mGeometry.vertices[face.v2];
                pos3 = mGeometry.vertices[face.v3];

                // check collision with triangle bounding sphere first
                if (b.Intersects(face.boundingSphere) == false)
                    continue;

                Vector3 closestPoint = closestPointInTriangle(b.Center, pos1, pos2, pos3);
                float squaredDist = (closestPoint - b.Center).LengthSquared();
                if (squaredDist <= squaredBoundingSphereRadius)
                {
                    result.AddLast(face);
                    intersectionPoints.AddLast(closestPoint);
                }

            }
        }

        /// <summary>
        /// Computes the distance between point and the triangle (v0, v1, v2). 
        /// Code taken from http://www.geometrictools.com/LibFoundation/Distance/Distance.html
        /// </summary>
        /// <param name="point"></param>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private Vector3 closestPointInTriangle(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
        {
            {
                //  Vector3<Real> kDiff = m_rkTriangle.V[0] - m_rkVector;
                Vector3 kDiff = v0 - point;
                Vector3 kEdge0 = v1 - v0;
                Vector3 kEdge1 = v2 - v0;

                double fA00 = kEdge0.LengthSquared();

                double fA01 = Vector3.Dot(kEdge0, kEdge1);
                double fA11 = kEdge1.LengthSquared();
                double fB0 = Vector3.Dot(kDiff, kEdge0);
                double fB1 = Vector3.Dot(kDiff, kEdge1);
                double fC = kDiff.LengthSquared();
                double fDet = Math.Abs(fA00 * fA11 - fA01 * fA01);
                double fS = fA01 * fB1 - fA11 * fB0;
                double fT = fA01 * fB0 - fA00 * fB1;
                double fSqrDistance;

                if (fS + fT <= fDet)
                {
                    if (fS < 0.0)
                    {
                        if (fT < 0.0)  // region 4
                        {
                            if (fB0 < 0.0)
                            {
                                fT = 0.0;
                                if (-fB0 >= fA00)
                                {
                                    fS = 1.0;
                                    fSqrDistance = fA00 + 2.0 * fB0 + fC;
                                }
                                else
                                {
                                    fS = -fB0 / fA00;
                                    fSqrDistance = fB0 * fS + fC;
                                }
                            }
                            else
                            {
                                fS = 0.0;
                                if (fB1 >= 0.0)
                                {
                                    fT = 0.0;
                                    fSqrDistance = fC;
                                }
                                else if (-fB1 >= fA11)
                                {
                                    fT = 1.0;
                                    fSqrDistance = fA11 + 2.0 * fB1 + fC;
                                }
                                else
                                {
                                    fT = -fB1 / fA11;
                                    fSqrDistance = fB1 * fT + fC;
                                }
                            }
                        }
                        else  // region 3
                        {
                            fS = 0.0;
                            if (fB1 >= 0.0)
                            {
                                fT = 0.0;
                                fSqrDistance = fC;
                            }
                            else if (-fB1 >= fA11)
                            {
                                fT = 1.0;
                                fSqrDistance = fA11 + 2.0 * fB1 + fC;
                            }
                            else
                            {
                                fT = -fB1 / fA11;
                                fSqrDistance = fB1 * fT + fC;
                            }
                        }
                    }
                    else if (fT < 0.0)  // region 5
                    {
                        fT = 0.0;
                        if (fB0 >= 0.0)
                        {
                            fS = 0.0;
                            fSqrDistance = fC;
                        }
                        else if (-fB0 >= fA00)
                        {
                            fS = 1.0;
                            fSqrDistance = fA00 + 2.0 * fB0 + fC;
                        }
                        else
                        {
                            fS = -fB0 / fA00;
                            fSqrDistance = fB0 * fS + fC;
                        }
                    }
                    else  // region 0
                    {
                        // minimum at interior point
                        double fInvDet = 1.0 / fDet;
                        fS *= fInvDet;
                        fT *= fInvDet;
                        fSqrDistance = fS * (fA00 * fS + fA01 * fT + 2.0 * fB0) +
                            fT * (fA01 * fS + fA11 * fT + 2.0 * fB1) + fC;
                    }
                }
                else
                {
                    double fTmp0, fTmp1, fNumer, fDenom;

                    if (fS < 0.0)  // region 2
                    {
                        fTmp0 = fA01 + fB0;
                        fTmp1 = fA11 + fB1;
                        if (fTmp1 > fTmp0)
                        {
                            fNumer = fTmp1 - fTmp0;
                            fDenom = fA00 - 2.0f * fA01 + fA11;
                            if (fNumer >= fDenom)
                            {
                                fS = 1.0;
                                fT = 0.0;
                                fSqrDistance = fA00 + 2.0 * fB0 + fC;
                            }
                            else
                            {
                                fS = fNumer / fDenom;
                                fT = 1.0 - fS;
                                fSqrDistance = fS * (fA00 * fS + fA01 * fT + 2.0f * fB0) +
                                    fT * (fA01 * fS + fA11 * fT + 2.0) * fB1 + fC;
                            }
                        }
                        else
                        {
                            fS = 0.0;
                            if (fTmp1 <= 0.0)
                            {
                                fT = 1.0;
                                fSqrDistance = fA11 + 2.0 * fB1 + fC;
                            }
                            else if (fB1 >= 0.0)
                            {
                                fT = 0.0;
                                fSqrDistance = fC;
                            }
                            else
                            {
                                fT = -fB1 / fA11;
                                fSqrDistance = fB1 * fT + fC;
                            }
                        }
                    }
                    else if (fT < 0.0)  // region 6
                    {
                        fTmp0 = fA01 + fB1;
                        fTmp1 = fA00 + fB0;
                        if (fTmp1 > fTmp0)
                        {
                            fNumer = fTmp1 - fTmp0;
                            fDenom = fA00 - 2.0 * fA01 + fA11;
                            if (fNumer >= fDenom)
                            {
                                fT = 1.0;
                                fS = 0.0;
                                fSqrDistance = fA11 + 2.0 * fB1 + fC;
                            }
                            else
                            {
                                fT = fNumer / fDenom;
                                fS = 1.0 - fT;
                                fSqrDistance = fS * (fA00 * fS + fA01 * fT + 2.0 * fB0) +
                                    fT * (fA01 * fS + fA11 * fT + 2.0 * fB1) + fC;
                            }
                        }
                        else
                        {
                            fT = 0.0;
                            if (fTmp1 <= 0.0)
                            {
                                fS = 1.0;
                                fSqrDistance = fA00 + 2.0 * fB0 + fC;
                            }
                            else if (fB0 >= 0.0)
                            {
                                fS = 0.0;
                                fSqrDistance = fC;
                            }
                            else
                            {
                                fS = -fB0 / fA00;
                                fSqrDistance = fB0 * fS + fC;
                            }
                        }
                    }
                    else  // region 1
                    {
                        fNumer = fA11 + fB1 - fA01 - fB0;
                        if (fNumer <= 0.0)
                        {
                            fS = 0.0;
                            fT = 1.0;
                            fSqrDistance = fA11 + 2.0 * fB1 + fC;
                        }
                        else
                        {
                            fDenom = fA00 - 2.0f * fA01 + fA11;
                            if (fNumer >= fDenom)
                            {
                                fS = 1.0;
                                fT = 0.0;
                                fSqrDistance = fA00 + 2.0 * fB0 + fC;
                            }
                            else
                            {
                                fS = fNumer / fDenom;
                                fT = 1.0 - fS;
                                fSqrDistance = fS * (fA00 * fS + fA01 * fT + 2.0 * fB0) +
                                    fT * (fA01 * fS + fA11 * fT + 2.0 * fB1) + fC;
                            }
                        }
                    }
                }

                // account for numerical round-off error
                if (fSqrDistance < 0.0)
                {
                    fSqrDistance = 0.0;
                }

                //  m_kClosestPoint0 = m_rkVector;
                return (v0 + ((float)fS) * kEdge0 + ((float)fT) * kEdge1);
            }
        }
    
        /// <summary>
        /// Returns true if ptTest lies within the triangle defined by pt1, pt2, pt3
        /// Code taken from http://www.blackpawn.com/texts/pointinpoly/default.html
        /// </summary>
        /// <param name="f"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        private bool pointInTriangle(Vector3 pt1, Vector3 pt2, Vector3 pt3, Vector3 ptTest)
        {
            // Compute vectors        
            Vector3 v0 = pt3 - pt1;
            Vector3 v1 = pt2 - pt1;
            Vector3 v2 = ptTest - pt1;

            // Compute dot products
            float dot00 = Vector3.Dot(v0, v0);
            float dot01 = Vector3.Dot(v0, v1);
            float dot02 = Vector3.Dot(v0, v2);
            float dot11 = Vector3.Dot(v1, v1);
            float dot12 = Vector3.Dot(v1, v2);

            // Compute barycentric coordinates
            float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            if (u + v <= 1 && u > 0 && v > 0)
                return true;
            return false;
        }
        /// <summary>
        /// Triangle Ray intersection test
        /// Code taken from http://www.ziggyware.com/readarticle.php?article_id=78
        /// </summary>
        /// <param name="ray_origin"></param>
        /// <param name="ray_direction"></param>
        /// <param name="vert0"></param>
        /// <param name="vert1"></param>
        /// <param name="vert2"></param>
        /// <param name="t"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool intersectTriangleRay(Vector3 ray_origin, Vector3 ray_direction,
                    Vector3 vert0, Vector3 vert1, Vector3 vert2,
                    out float t, out float u, out float v)
        {
            t = 0; u = 0; v = 0;

            Vector3 edge1 = vert1 - vert0;
            Vector3 edge2 = vert2 - vert0;

            Vector3 tvec, pvec, qvec;
            float det, inv_det;

            pvec = Vector3.Cross(ray_direction, edge2);

            det = Vector3.Dot(edge1, pvec);

            if (det > -0.00001f)
                return false;

            inv_det = 1.0f / det;

            tvec = ray_origin - vert0;

            u = Vector3.Dot(tvec, pvec) * inv_det;
            if (u < -0.001f || u > 1.001f)
                return false;

            qvec = Vector3.Cross(tvec, edge1);

            v = Vector3.Dot(ray_direction, qvec) * inv_det;
            if (v < -0.001f || u + v > 1.001f)
                return false;

            t = Vector3.Dot(edge2, qvec) * inv_det;

            if (t <= 0)
                return false;

            return true;
        }
    }

}
