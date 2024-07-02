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
        PlayMakerFSM _teleportFSM;

        void Awake()
        {
            eyeRing = GameObjectSpawns.SpawnEyeRing(BiblicallyAccurateLasers.Instance.settings.eyeCount);
            eyeRing.transform.parent = transform;
            eyeRing.transform.localPosition = new Vector3(-0.1f, 1.5f, -0.001f);
            eyeRing.transform.localScale = Vector3.one * 3;

            _phaseControlFSM = gameObject.LocateMyFSM("Phase Control");
            _teleportFSM = gameObject.LocateMyFSM("Teleport");
        }

        void Start()
        {
            // MethodAction addition process in each foreach taken from SFCore, under MIT License

            foreach (FsmState state in _phaseControlFSM.FsmStates.Where(s => s.Name == "Stun 1"))
            {
                FsmStateAction[] currentActions = state.Actions;
                FsmStateAction[] newActions = new FsmStateAction[currentActions.Length + 1];

                FsmStateAction a = new MethodAction
                {
                    method = () => {
                        foreach (LaserEye laserEye in eyeRing.transform.GetComponentsInChildren<LaserEye>())
                        {
                            laserEye.SetActive(false);
                        }
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
                        foreach (LaserEye laserEye in eyeRing.transform.GetComponentsInChildren<LaserEye>())
                        {
                            laserEye.SetActive(true);
                        }
                    }
                };

                newActions[0] = a;

                state.Actions = newActions;
                a.Init(state);
            }
            /*foreach (FsmState state in _phaseControlFSM.FsmStates.Where(s => s.Name == "Set Ascend"))
            {
                FsmStateAction[] currentActions = state.Actions;
                FsmStateAction[] newActions = new FsmStateAction[currentActions.Length + 1];

                FsmStateAction a = new MethodAction
                {
                    method = () => {
                        foreach (LaserEye laserEye in eyeRing.transform.GetComponentsInChildren<LaserEye>())
                        {
                            laserEye.SetActive(false);
                        }
                    }
                };

                currentActions.CopyTo(newActions, 0);
                newActions[currentActions.Length] = a;

                state.Actions = newActions;
                a.Init(state);
            }*/

            foreach (FsmState state in _teleportFSM.FsmStates.Where(s => s.Name == "Antic"))
            {
                FsmStateAction[] currentActions = state.Actions;
                FsmStateAction[] newActions = new FsmStateAction[currentActions.Length + 1];

                FsmStateAction a = new MethodAction
                {
                    method = () => {
                        foreach (LaserEye laserEye in eyeRing.transform.GetComponentsInChildren<LaserEye>())
                        {
                            laserEye.SetActive(false);
                        }
                    }
                };

                currentActions.CopyTo(newActions, 0);
                newActions[currentActions.Length] = a;

                state.Actions = newActions;
                a.Init(state);
            }
            foreach (FsmState state in _teleportFSM.FsmStates.Where(s => s.Name == "Notify"))
            {
                FsmStateAction[] currentActions = state.Actions;
                FsmStateAction[] newActions = new FsmStateAction[currentActions.Length + 1];

                FsmStateAction a = new MethodAction
                {
                    method = () => {
                        BiblicallyAccurateLasers.Instance.Log(_phaseControlFSM.ActiveStateName);
                        if (_phaseControlFSM.ActiveStateName != "Set Ascend")
                            foreach (LaserEye laserEye in eyeRing.transform.GetComponentsInChildren<LaserEye>())
                            {
                                laserEye.SetActive(true);
                            }
                    }
                };

                currentActions.CopyTo(newActions, 0);
                newActions[currentActions.Length] = a;

                state.Actions = newActions;
                a.Init(state);
            }
            
        }

    }

    internal class MethodAction : FsmStateAction
    {
        // Copied from SFCore, under MIT License

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
