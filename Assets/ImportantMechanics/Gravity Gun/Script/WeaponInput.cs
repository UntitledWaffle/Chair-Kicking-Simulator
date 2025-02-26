using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthbarGames
{
    public class WeaponInput : MonoBehaviour
    {
        public enum Action { Primary, Secondary }

        public virtual bool GetActionActivate(Action action)
        {
            return false;
        }

        public virtual bool GetActionState(Action action)
        {
            return false;
        }

        public virtual bool GetActionDeactivate(Action action)
        {
            return false;
        }
    }
}
