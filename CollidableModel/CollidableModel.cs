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
using Microsoft.Xna.Framework.Graphics;
using XNAnimation;


namespace CollidableModel
{
    public class CollidableModel
    {
        private Model mModel;
        public Model model
        {
            get { return mModel; }
        }

        private SkinnedModel sModel;
        public SkinnedModel skinnedModel
        {
            get { return sModel; }
        }

        private BspTree mCollisionData;
        public BspTree collisionData
        {
            get { return mCollisionData; }
        }

        public CollidableModel(Model xnaModel, SkinnedModel skinnedModel, BspTree tree )
        {
            mModel = xnaModel;
            mCollisionData = tree;
        }

    }
}
