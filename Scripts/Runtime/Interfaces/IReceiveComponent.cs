using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dubi.SceneLoader
{
    public interface IReceiveComponent
    {
        public void InjectComponent(ISourceComponent sourceComponent);

        public void DissectComponent(ISourceComponent sourceComponent);

    }

}
