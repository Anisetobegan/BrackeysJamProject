using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AudioSystemEvents
{
    [System.Serializable] public class OnPlayFade : UnityEvent<AnimationCurve> { }
    [System.Serializable] public class OnPlay : UnityEvent { }
}

