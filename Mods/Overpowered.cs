﻿using ExitGames.Client.Photon;
using GorillaGameModes;
using GorillaLocomotion.Gameplay;
using GorillaNetworking;
using GorillaTagScripts;
using HarmonyLib;
using iiMenu.Classes;
using iiMenu.Notifications;
using Pathfinding;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using static iiMenu.Classes.RigManager;
using static iiMenu.Menu.Main;




namespace iiMenu.Mods
{
    public class Overpowered
    {

        public static void MasterCheck()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> <color=white>You are master client.</color>");
            }
            else
            {
                NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>");
            }
        }

        public static void StartMoonEvent()
        {
            GreyZoneManager gzm = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/GreyZoneManager").GetComponent<GreyZoneManager>();
            if (PhotonNetwork.IsMasterClient)
            {
                gzm.ActivateGreyZoneAuthority();
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void EndMoonEvent()
        {
            GreyZoneManager gzm = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/GreyZoneManager").GetComponent<GreyZoneManager>();
            if (PhotonNetwork.IsMasterClient)
            {
                gzm.DeactivateGreyZoneAuthority();
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        private static float lastidiotstupidlaaa = 0f;
        public static void FlashScreen()
        {
            GreyZoneManager gzm = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/GreyZoneManager").GetComponent<GreyZoneManager>();
            if (PhotonNetwork.IsMasterClient)
            {
                if (Time.time > lastidiotstupidlaaa)
                {
                    lastidiotstupidlaaa = Time.time + 0.1f;
                    if (gzm.GreyZoneActive)
                    {
                        gzm.DeactivateGreyZoneAuthority();
                    } else
                    {
                        gzm.ActivateGreyZoneAuthority();
                    }
                }
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void LurkerGun()
        {
            LurkerGhost lg = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lurker Ghost/GhostLurker_Prefab").GetComponent<LurkerGhost>();

            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    NetPlayer buddy = GetPlayerFromVRRig(whoCopy);

                    Type type = typeof(LurkerGhost).Assembly.GetType("GorillaTagScripts.LurkerGhost+ghostState");
                    object val = Enum.ToObject(type, 1);

                    //NotifiLib.SendNotification(val.ToString());

                    Debug.Log(val);

                    Traverse.Create(lg).Method("PickPlayer", buddy).GetValue();
                    Traverse.Create(lg).Method("ChangeState", val).GetValue();
                }
                if (rightTrigger > 0.5f || Mouse.current.leftButton.isPressed)
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                }
            }
        }

        public static void SpawnLurker()
        {
            LurkerGhost lg = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lurker Ghost/GhostLurker_Prefab").GetComponent<LurkerGhost>();

            if (lg.IsMine)
            {
                Type type = typeof(LurkerGhost).Assembly.GetType("GorillaTagScripts.LurkerGhost+ghostState");
                object val = Enum.ToObject(type, 1);

                NotifiLib.SendNotification(val.ToString());

                Debug.Log(val);

                float nerd = 2000f;

                Traverse.Create(lg).Method("PickPlayer", nerd).GetValue();
                Traverse.Create(lg).Method("ChangeState", val).GetValue();
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void DespawnLurker()
        {
            LurkerGhost lg = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lurker Ghost/GhostLurker_Prefab").GetComponent<LurkerGhost>();
            try
            {
                if (lg.IsMine)
                {
                    Type type = typeof(LurkerGhost).Assembly.GetType("GorillaTagScripts.LurkerGhost+ghostState");
                    object val = Enum.ToObject(type, 0);

                    NotifiLib.SendNotification(val.ToString());

                    Debug.Log(val);

                    Traverse.Create(lg).Method("ChangeState", val).GetValue();
                }
                else
                {
                    NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>");
                }
            }
            catch (Exception e)
            {
                NotifiLib.SendNotification("FUCKKKKK");
                Debug.Log(e);
            }
        }



        public static void SeeLurker()
        {
            LurkerGhost lg = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lurker Ghost/GhostLurker_Prefab").GetComponent<LurkerGhost>();

            if (lg.meshRenderer.sharedMaterial != lg.visibleMaterial || lg.bonesMeshRenderer.sharedMaterial != lg.visibleMaterialBones)
            {
                lg.meshRenderer.sharedMaterial = lg.visibleMaterial;
                lg.bonesMeshRenderer.sharedMaterial = lg.visibleMaterialBones;
                //NotifiLib.SendNotification("mat updated");
            }
        }

        public static void SpawnBlueLucy()
        {
            HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton/").GetComponent<HalloweenGhostChaser>();
            if (hgc.IsMine)
            {
                hgc.timeGongStarted = Time.time;
                hgc.currentState = HalloweenGhostChaser.ChaseState.Gong;
                hgc.isSummoned = false;
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void SpawnRedLucy()
        {
            HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (hgc.IsMine)
            {
                hgc.timeGongStarted = Time.time;
                hgc.currentState = HalloweenGhostChaser.ChaseState.Gong;
                hgc.isSummoned = true;
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void DespawnLucy()
        {
            HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (hgc.IsMine)
            {
                hgc.currentState = HalloweenGhostChaser.ChaseState.Dormant;
                hgc.isSummoned = false;
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void LucyChaseSelf()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (rightTrigger > 0.5f || Mouse.current.leftButton.isPressed)
                {
                    HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
                    if (hgc.IsMine)
                    {
                        VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                        if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                        {
                            hgc.currentState = HalloweenGhostChaser.ChaseState.Chasing;
                            hgc.targetPlayer = NetworkSystem.Instance.LocalPlayer;
                            hgc.followTarget = GorillaTagger.Instance.offlineVRRig.transform;
                        }
                    }
                    else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
                }
            }
        }

        public static void LucyChaseGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (rightTrigger > 0.5f || Mouse.current.leftButton.isPressed)
                {
                    HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
                    if (hgc.IsMine)
                    {
                        VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                        if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                        {
                            hgc.currentState = HalloweenGhostChaser.ChaseState.Chasing;
                            hgc.targetPlayer = GetPlayerFromVRRig(possibly);
                            hgc.followTarget = possibly.transform;
                        }
                    }
                    else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
                }
            }
        }

        public static void LucyAttackSelf()
        {
            HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (hgc.IsMine)
            {
                hgc.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
                hgc.grabTime = Time.time;
                hgc.targetPlayer = NetworkSystem.Instance.LocalPlayer;
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void LucyAttackGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
                    if (hgc.IsMine)
                    {
                        if (Time.time > hgc.grabTime + hgc.grabDuration + 0.1f)
                        {
                            hgc.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
                            hgc.grabTime = Time.time;
                            hgc.targetPlayer = GetPlayerFromVRRig(whoCopy);
                        }
                    }
                    else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
                }
                if (rightTrigger > 0.5f || Mouse.current.leftButton.isPressed)
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                }
            }
        }

        private static float lasttimethingblahblabhabja = 0f;
        public static void SpazLucy()
        {
            HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (hgc.IsMine)
            {
                if (Time.time > lasttimethingblahblabhabja)
                {
                    hgc.timeGongStarted = hgc.timeGongStarted == 0f ? Time.time : 0f;
                    hgc.currentState = HalloweenGhostChaser.ChaseState.Gong;
                    hgc.isSummoned = true;
                    lasttimethingblahblabhabja = Time.time + 0.1f;
                }
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void AnnoyingLucy()
        {
            HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (hgc.IsMine)
            {
                if (Time.time > lasttimethingblahblabhabja)
                {
                    hgc.timeGongStarted = Time.time;
                    hgc.grabTime = Time.time;
                    hgc.currentState = hgc.currentState == HalloweenGhostChaser.ChaseState.Gong ? HalloweenGhostChaser.ChaseState.Grabbing : HalloweenGhostChaser.ChaseState.Gong;
                    hgc.targetPlayer = GetRandomPlayer(true);
                    lasttimethingblahblabhabja = Time.time + 0.1f;
                }
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void LucyGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (rightTrigger > 0.5f || Mouse.current.leftButton.isPressed)
                {
                    if (!PhotonNetwork.IsMasterClient) { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); return; }
                    //foreach (MonkeyeAI monkeyeAI in GetMonsters())
                    //{
                    //    if (!PhotonNetwork.IsMasterClient) { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); return; } // GetOwnership(monkeyeAI.GetComponent<PhotonView>());
                    //    monkeyeAI.gameObject.transform.position = NewPointer.transform.position + new Vector3(0f, 1f, 0f);
                    //}

                    HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();

                    hgc.gameObject.transform.position = NewPointer.transform.position + new Vector3(0f, 0f, 0f);
                }
            }
        }

        public static void BecomeLucy()
        {
            HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();

            Vector3 startpos = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0f, 0f);

            hgc.gameObject.transform.position = startpos;
        }

        public static void FastLucy()
        {
            HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (hgc.IsMine)
            {
                hgc.currentSpeed = 10f;
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void SlowLucy()
        {
            HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (hgc.IsMine)
            {
                hgc.currentSpeed = 1f;
            }
            else { NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>"); }
        }

        public static void SpawnSecondLook()
        {
            GameObject secondlook = GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton");
            secondlook.GetComponent<SecondLookSkeleton>().tapped = true;
            secondlook.GetComponent<SecondLookSkeletonSynchValues>().GetView.RPC("RemoteActivateGhost", RpcTarget.All, new object[] { });
        }

        public static void AngerSecondLook()
        {
            GameObject secondlook = GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton");
            secondlook.GetComponent<SecondLookSkeleton>().tapped = true;
            if (secondlook.GetComponent<SecondLookSkeleton>().currentState == SecondLookSkeleton.GhostState.Unactivated)
            {
                secondlook.GetComponent<SecondLookSkeletonSynchValues>().GetView.RPC("RemoteActivateGhost", RpcTarget.All, new object[] { });
            }
            secondlook.GetComponent<SecondLookSkeletonSynchValues>().GetView.RPC("RemotePlayerSeen", RpcTarget.All, new object[] { });
        }

        public static void ThrowSecondLook()
        {
            GameObject secondlook = GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton");
            secondlook.GetComponent<SecondLookSkeleton>().tapped = true;
            if (secondlook.GetComponent<SecondLookSkeleton>().currentState == SecondLookSkeleton.GhostState.Unactivated)
            {
                secondlook.GetComponent<SecondLookSkeletonSynchValues>().GetView.RPC("RemoteActivateGhost", RpcTarget.All, new object[] { });
            }
            secondlook.GetComponent<SecondLookSkeletonSynchValues>().GetView.RPC("RemotePlayerCaught", RpcTarget.All, new object[] { });
        }

        public static float lasttimeaa = 0f;
        public static void SpazSecondLook()
        {
            if (Time.time > lasttimeaa)
            {
                lasttimeaa = Time.time + 0.75f;
                GameObject secondlook = GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton");
                secondlook.GetComponent<SecondLookSkeleton>().tapped = true;
                if (secondlook.GetComponent<SecondLookSkeleton>().currentState == SecondLookSkeleton.GhostState.Unactivated || secondlook.GetComponent<SecondLookSkeleton>().currentState == SecondLookSkeleton.GhostState.PlayerThrown || secondlook.GetComponent<SecondLookSkeleton>().currentState == SecondLookSkeleton.GhostState.Reset)
                {
                    secondlook.GetComponent<SecondLookSkeletonSynchValues>().GetView.RPC("RemoteActivateGhost", RpcTarget.All, new object[] { });
                }
                if (secondlook.GetComponent<SecondLookSkeleton>().currentState == SecondLookSkeleton.GhostState.Activated || secondlook.GetComponent<SecondLookSkeleton>().currentState == SecondLookSkeleton.GhostState.Patrolling)
                {
                    secondlook.GetComponent<SecondLookSkeletonSynchValues>().GetView.RPC("RemotePlayerSeen", RpcTarget.All, new object[] { });
                }
                if (secondlook.GetComponent<SecondLookSkeleton>().currentState == SecondLookSkeleton.GhostState.Chasing)
                {
                    secondlook.GetComponent<SecondLookSkeletonSynchValues>().GetView.RPC("RemoteActivateGhost", RpcTarget.All, new object[] { });
                }
            }
        }

        // No, it's not skidded, read the debunk: https://pastebin.com/raw/FL5j8fcy
        private static bool lastfreezegarbage = false;
        private static float Garfield = 0f;
        public static void FreezeAll()
        {
            if (rightTrigger > 0.5f)
            {
                if (Time.time > Garfield)
                {
                    lastfreezegarbage = !lastfreezegarbage;
                    foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                    {
                        GorillaPlayerScoreboardLine.MutePlayer(line.linePlayer.UserId, line.linePlayer.NickName, lastfreezegarbage ? 1 : 0);
                    }
                    Garfield = Time.time + 0.1f;
                }
            }
        }

        public static void DestroyGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if ((rightTrigger > 0.5f || Mouse.current.leftButton.isPressed) && Time.time > kgDebounce)
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        NetPlayer player = GetPlayerFromVRRig(possibly);
                        PhotonNetwork.OpRemoveCompleteCacheOfPlayer(player.ActorNumber);
                        kgDebounce = Time.time + 0.5f;
                    }
                }
            }
        }

        public static void DestroyAll()
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
            {
                PhotonNetwork.OpRemoveCompleteCacheOfPlayer(player.ActorNumber);
            }
        }

        public static void AtticFlingGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    BuilderPiece[] them = GetPieces();
                    BuilderPiece that = GetPieces()[UnityEngine.Random.Range(0, GetPieces().Length - 1)];
                    Fun.BetaDropBlock(that, whoCopy.transform.position - new Vector3(0f, 0.1f, 0f), Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360))), new Vector3(0f, 10f, 0f), Vector3.zero);
                    RPCProtection();
                }
                if (rightTrigger > 0.5f || Mouse.current.leftButton.isPressed)
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                }
            }
        }

        public static void AtticFlingAll()
        {
            if (rightTrigger > 0.5f)
            {
                BuilderPiece[] them = GetPieces();
                BuilderPiece that = GetPieces()[UnityEngine.Random.Range(0, GetPieces().Length - 1)];
                Fun.BetaDropBlock(new object[] { that.pieceId, GetRandomVRRig(false).transform.position - new Vector3(0f, 0.1f, 0f), Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360))), new Vector3(0f, 10f, 0f), Vector3.zero, PhotonNetwork.LocalPlayer });
                RPCProtection();
            }
        }

        public static void InfectionToTag()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>");
            }
            else
            {
                GorillaTagManager gorillaTagManager = GameObject.Find("GT Systems/GameModeSystem/Gorilla Tag Manager").GetComponent<GorillaTagManager>();
                gorillaTagManager.SetisCurrentlyTag(true);
                gorillaTagManager.ClearInfectionState();
                gorillaTagManager.ChangeCurrentIt(GameMode.ParticipatingPlayers[UnityEngine.Random.Range(0, GameMode.ParticipatingPlayers.Count)]);
            }
        }

        public static void TagToInfection()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>");
            }
            else
            {
                GorillaTagManager gorillaTagManager = GameObject.Find("GT Systems/GameModeSystem/Gorilla Tag Manager").GetComponent<GorillaTagManager>();
                gorillaTagManager.SetisCurrentlyTag(false);
                gorillaTagManager.ClearInfectionState();
                NetPlayer victim = GameMode.ParticipatingPlayers[UnityEngine.Random.Range(0, GameMode.ParticipatingPlayers.Count)];
                gorillaTagManager.AddInfectedPlayer(victim);
                gorillaTagManager.lastInfectedPlayer = victim;
            }
        }

        public static void BetaSetStatus(int state, RaiseEventOptions balls)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You are not master client.</color>");
            }
            else
            {
                object[] statusSendData = new object[1];
                statusSendData[0] = state;
                object[] sendEventData = new object[3];
                sendEventData[0] = PhotonNetwork.ServerTimestamp;
                sendEventData[1] = (byte)2;
                sendEventData[2] = statusSendData;
                PhotonNetwork.RaiseEvent(3, sendEventData, balls, SendOptions.SendUnreliable);
            }
        }

        public static void SlowGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if ((rightTrigger > 0.5f || Mouse.current.leftButton.isPressed) && Time.time > kgDebounce)
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        NetPlayer player = GetPlayerFromVRRig(possibly);
                        BetaSetStatus(0, new RaiseEventOptions { TargetActors = new int[1] { player.ActorNumber } });
                        RPCProtection();
                        kgDebounce = Time.time + 1f;
                    }
                }
            }
        }

        public static void SlowAll()
        {
            if (Time.time > kgDebounce)
            {
                BetaSetStatus(0, new RaiseEventOptions { Receivers = ReceiverGroup.Others });
                RPCProtection();
                kgDebounce = Time.time + 1f;
            }
        }

        public static void VibrateGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if ((rightTrigger > 0.5f || Mouse.current.leftButton.isPressed) && Time.time > kgDebounce)
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        NetPlayer owner = GetPlayerFromVRRig(possibly);
                        BetaSetStatus(1, new RaiseEventOptions { TargetActors = new int[1] { owner.ActorNumber } });
                        RPCProtection();
                        kgDebounce = Time.time + 0.5f;
                    }
                }
            }
        }

        public static void VibrateAll()
        {
            if (Time.time > kgDebounce)
            {
                BetaSetStatus(1, new RaiseEventOptions { Receivers = ReceiverGroup.Others });
                RPCProtection();
                kgDebounce = Time.time + 0.5f;
            }
        }
        
        public static void GliderBlindGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (rightTrigger > 0.5f || Mouse.current.leftButton.isPressed)
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }

                if (isCopying)
                {
                    foreach (GliderHoldable glider in GetGliders())
                    {
                        if (glider.GetView.Owner == PhotonNetwork.LocalPlayer)
                        {
                            glider.gameObject.transform.position = whoCopy.headMesh.transform.position;
                            glider.gameObject.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                        }
                        else
                        {
                            glider.OnHover(null, null);
                        }
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                }
            }
        }

        public static void GliderBlindAll()
        {
            GliderHoldable[] those = GetGliders();
            int index = 0;
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    try
                    {
                        GliderHoldable glider = those[index];
                        if (glider.GetView.Owner == PhotonNetwork.LocalPlayer)
                        {
                            glider.gameObject.transform.position = vrrig.headMesh.transform.position;
                            glider.gameObject.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                        }
                        else
                        {
                            glider.OnHover(null, null);
                        }
                    } catch { }
                    index++;
                }
            }
        }

        public static void BreakAudioGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", GetPlayerFromVRRig(whoCopy), new object[]{
                        111,
                        false,
                        999999f
                        //0.01f
                    });
                    RPCProtection();
                }
                if (rightTrigger > 0.5f || Mouse.current.leftButton.isPressed)
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void BreakAudioAll()
        {
            if (rightTrigger > 0.5f)
            {
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]{
                    111,
                    false,
                    999999f
                });
            }
        }

        private static float RopeDelay = 0f;
        public static void JoystickRopeControl() // Thanks to ShibaGT for the fix
        {
            Vector2 joy = ControllerInputPoller.instance.rightControllerPrimary2DAxis;

            if ((Mathf.Abs(joy.x) > 0.05f || Mathf.Abs(joy.y) > 0.05f) && Time.time > RopeDelay)
            {
                RopeDelay = Time.time + 0.25f;
                foreach (GorillaRopeSwing rope in GetRopes())
                {
                    RopeSwingManager.instance.photonView.RPC("SetVelocity", RpcTarget.All, new object[] { rope.ropeId, 1, new Vector3(joy.x * 50f, joy.y * 50f, 0f), true, null });
                    RPCProtection();
                }
            }
        }

        public static void SpazRopeGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (rightTrigger > 0.5f || Mouse.current.leftButton.isPressed)
                {
                    GorillaRopeSwing possibly = Ray.collider.GetComponentInParent<GorillaRopeSwing>();
                    if (possibly && Time.time > RopeDelay)
                    {
                        RopeDelay = Time.time + 0.25f;
                        RopeSwingManager.instance.photonView.RPC("SetVelocity", RpcTarget.All, new object[] { possibly.ropeId, 1, new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f)), true, null });
                        RPCProtection();
                    }
                }
            }
        }

        public static void SpazAllRopes()
        {
            if (rightTrigger > 0.5f)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if ((rightTrigger > 0.5f || Mouse.current.leftButton.isPressed) && Time.time > RopeDelay)
                {
                    RopeDelay = Time.time + 0.25f;
                    foreach (GorillaRopeSwing rope in GetRopes())
                    {
                        RopeSwingManager.instance.photonView.RPC("SetVelocity", RpcTarget.All, new object[] { rope.ropeId, 1, new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f)), true, null });
                        RPCProtection();
                    }
                }
            }
        }

        public static void SpazGrabbedRopes()
        {
            if (Time.time > RopeDelay)
            {
                RopeDelay = Time.time + 0.1f;
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    GorillaRopeSwing rope = (GorillaRopeSwing)Traverse.Create(vrrig).Field("currentRopeSwing").GetValue();
                    if (rope != null)
                    {
                        RopeSwingManager.instance.photonView.RPC("SetVelocity", RpcTarget.All, new object[] { rope.ropeId, 1, new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f)), true, null });
                        RPCProtection();
                    }
                }
            }
        }

        public static void ConfusingRopes()
        {
            if (Time.time > RopeDelay)
            {
                RopeDelay = Time.time + 0.1f;
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    GorillaRopeSwing rope = (GorillaRopeSwing)Traverse.Create(vrrig).Field("currentRopeSwing").GetValue();
                    if (rope != null)
                    {
                        RopeSwingManager.instance.photonView.RPC("SetVelocity", NetPlayerToPlayer(GetPlayerFromVRRig(whoCopy)), new object[] { rope.ropeId, 1, new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f)), true, null });
                        RPCProtection();
                    }
                }
            }
        }

        public static void FlingRopeGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (rightTrigger > 0.5f || Mouse.current.leftButton.isPressed)
                {
                    GorillaRopeSwing possibly = Ray.collider.GetComponentInParent<GorillaRopeSwing>();
                    if (possibly)
                    {
                        RopeSwingManager.instance.photonView.RPC("SetVelocity", RpcTarget.All, new object[] { possibly.ropeId, 1, (possibly.transform.position - GorillaTagger.Instance.headCollider.transform.position).normalized * 50f, true, null });
                        RPCProtection();
                    }
                }
            }
        }

        public static void FlingAllRopesGun()
        {
            if (rightGrab || Mouse.current.rightButton.isPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if ((rightTrigger > 0.5f || Mouse.current.leftButton.isPressed) && Time.time > RopeDelay)
                {
                    RopeDelay = Time.time + 0.25f;
                    foreach (GorillaRopeSwing rope in GetRopes())
                    {
                        RopeSwingManager.instance.photonView.RPC("SetVelocity", RpcTarget.All, new object[] { rope.ropeId, 1, (NewPointer.transform.position - rope.transform.position).normalized * 50f, true, null });
                        RPCProtection();
                    }
                }
            }
        }
    }
}
