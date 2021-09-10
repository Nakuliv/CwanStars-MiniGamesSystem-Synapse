// -----------------------------------------------------------------------
// <copyright file="HatPlayerComponent.cs" company="SCPStats.com">
// Copyright (c) SCPStats.com. All rights reserved.
// Licensed under the Apache v2 license.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using InventorySystem.Items.Pickups;
using MEC;
using Mirror;
using Synapse;
using Synapse.Api;
using UnityEngine;
using Scp096 = PlayableScps.Scp096;

namespace MiniGamesSystem.Hats
{
    public class HatPlayerComponent : MonoBehaviour
    {
        internal HatItemComponent item;

        private bool _threw = false;

        private void Start()
        {
            Timing.RunCoroutine(MoveHat().CancelWith(this).CancelWith(gameObject));
        }

        private IEnumerator<float> MoveHat()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(.1f);

                try
                {
                    if (item == null || item.gameObject == null) continue;

                    var player = gameObject.GetComponent<Player>();
                    var pickup = item.item;
                    var pickupInfo = pickup.NetworkInfo;
                    var pickupType = pickup.GetType();

                    if (player.RoleType == RoleType.None || player.RoleType == RoleType.Spectator || Helper.IsPlayerGhost(player))
                    {
                        pickupInfo.Position = Vector3.one * 6000f;
                        pickup.transform.position = Vector3.one * 6000f;

                        pickup.NetworkInfo = pickupInfo;

                        continue;
                    }

                    var camera = player.CameraReference;

                    var rotAngles = camera.rotation.eulerAngles;
                    if (player.Team == Team.SCP) rotAngles.x = 0;

                    var rotation = Quaternion.Euler(rotAngles);

                    var rot = rotation * item.rot;
                    var transform1 = pickup.transform;
                    var pos = (player.RoleType != RoleType.Scp079 ? rotation * (item.pos+item.itemOffset) : (item.pos+item.itemOffset)) + camera.position;

                    transform1.rotation = rot;
                    pickupInfo.Rotation = new LowPrecisionQuaternion(rot);

                    transform1.position = pos;
                    pickupInfo.Position = pos;

                    var fakePickupInfo = pickup.NetworkInfo;
                    fakePickupInfo.Position = Vector3.zero;
                    fakePickupInfo.Rotation = new LowPrecisionQuaternion(Quaternion.identity);

                    foreach (var player1 in Server.Get.Players)
                    {
                        if (player1?.UserId == null || player1.ClassManager.IsHost || !player1.ClassManager.IsVerified || Helper.IsPlayerNPC(player1)) continue;
                      
                    }
                }
                catch (Exception e)
                {
                    if (!_threw)
                    {
                        Synapse.Api.Logger.Get.Error(e);
                        _threw = true;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (item != null && item.gameObject != null)
            {
                UnityEngine.Object.Destroy(item.gameObject);
            }
        }
    }
}