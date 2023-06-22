using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubi.SceneLoader
{
    public interface IReceiveComponent
    {
        public void InjectComponent(ISourceComponent sourceComponent);

        public void DissectComponent(ISourceComponent sourceComponent);

    }

}
