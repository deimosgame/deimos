using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    abstract class SceneTemplate
    {
        // Template class
        abstract public void Load(SceneManager sceneManager);

        abstract public void UnLoad(SceneManager sceneManager);

        abstract public void Initialize(SceneManager sceneManager, DeimosGame game);

        abstract public void Update(SceneManager sceneManager, DeimosGame game);
    }
}
