using HutongGames.PlayMaker;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BiblicallyAccurateLasers
{
    internal class RadianceLaserControl : MonoBehaviour
    {

        GameObject eyeRing;

        PlayMakerFSM _phaseControlFSM;

        void Awake()
        {
            eyeRing = GameObjectSpawns.SpawnEyeRing(BiblicallyAccurateLasers.Instance.settings.eyeCount);
            eyeRing.transform.parent = transform;
            eyeRing.transform.localPosition = new Vector3(-0.1f, 1.5f, -0.001f);
            eyeRing.transform.localScale = Vector3.one * 3;

            _phaseControlFSM = gameObject.LocateMyFSM("Phase Control");

        }

        void Start()
        {

            foreach (FsmState state in _phaseControlFSM.FsmStates.Where(s => s.Name == "Stun 1"))
            {
                FsmStateAction[] currentActions = state.Actions;
                FsmStateAction[] newActions = new FsmStateAction[currentActions.Length + 1];

                FsmStateAction a = new MethodAction
                {
                    method = () => {
                        eyeRing.SetActive(false);
                    }
                };

                currentActions.CopyTo(newActions, 0);
                newActions[currentActions.Length] = a;

                state.Actions = newActions;
                a.Init(state);
            }
            foreach (FsmState state in _phaseControlFSM.FsmStates.Where(s => s.Name == "Idle 4"))
            {
                FsmStateAction[] newActions = new FsmStateAction[1];

                FsmStateAction a = new MethodAction
                {
                    method = () => {
                        eyeRing.SetActive(true);
                    }
                };

                newActions[0] = a;

                state.Actions = newActions;
                a.Init(state);
            }

        }

    }

    internal class MethodAction : FsmStateAction
    {
        public Action method;

        public override void Reset()
        {
            method = null;
            base.Reset();
        }
        public override void OnEnter()
        {
            if (method != null)
            {
                method();
            }

            Finish();
        }
    }
}
