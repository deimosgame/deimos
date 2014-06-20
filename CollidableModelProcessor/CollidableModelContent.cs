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

using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using XNAnimationPipeline;

namespace CollidableModelProcessor
{
    public class CollidableModelContent
    {
        private ModelContent mModel;
        public ModelContent model
        {
            get { return mModel; }
        }

        private SkinnedModelContent sModel;
        public SkinnedModelContent skinnedModel
        {
            get { return sModel; }
        }

        private BspTreeContent mCollisionData;
        public BspTreeContent collisionData
        {
            get { return mCollisionData; }
        }



        public CollidableModelContent(ModelContent model, SkinnedModelContent skinnedModel, BspTreeContent bspTree)
        {
            mModel = model;
            sModel = skinnedModel;
            mCollisionData = bspTree;
        }
    }
}
