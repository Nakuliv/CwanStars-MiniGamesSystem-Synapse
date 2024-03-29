﻿using Synapse.Api;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Synapse.Api.Items;

namespace MiniGamesSystem.Pets
{
    [API]
    public class Pet : Dummy
    {

        [API]
        public static bool SpawnPet(Player owner, string Nick, PetType type, out Pet pet)
        {
            pet = null;
            PetOwnerScript pos;
            if (!owner.TryGetComponent(out pos))
            {
                owner.gameObject.AddComponent<PetOwnerScript>();
            }
            if(type == PetType.Custom)
            {
                if (owner.GetPetOwnerScript().SpawnedPets.Contains(type))
                    pet.Despawn();
            }
            if (owner.GetPetOwnerScript().SpawnedPets.Contains(type)) return false;

            pet = new Pet(owner, Nick, type);

            return true;
        }

        public Player Owner { get; }

        //public Pet(Player player, string Nick) : this(player, Nick) { }

        public Pet(Player player, string Nick, PetType Type) :
            base(player.Position, player.transform.localRotation)
        {
            player.GetPetOwnerScript().SpawnedPets.Add(Type);
            switch (Type)
            {
                case PetType.Amogus:
                    Player.ClassManager.CurClass = RoleType.ClassD;
                    Player.Scale = new Vector3(1f, 0.5f, 1f);
                    Player.RankName = "Pet";
                    Player.RankColor = "yellow";
                    break;
                case PetType.Doggo:
                    Player.ClassManager.CurClass = RoleType.Scp93953;
                    Player.Scale = new Vector3(0.5f, 0.5f, 0.5f);
                    Player.RankName = "Pet";
                    Player.RankColor = "yellow";
                    break;
                case PetType.Custom:
                    Player.ClassManager.CurClass = Handler.pInfoDict[player.UserId].custompetClass;
                    Player.Scale = Handler.pInfoDict[player.UserId].custompetSize;
                    Player.RankName = "Custom Pet";
                    Player.RankColor = "yellow";
                    break;
            }
            Owner = player;
            Player.GodMode = false;
            Player.Health = 150;
            Player.DisplayInfo = $"<color=white>[</color><color=blue>Nazwa</color><color=white>]</color> {Nick}\n<color=white>[</color><color=#ff7518>Właściciel</color><color=white>]</color> <color=green>{Owner.NickName}</color>\n<color=white>[</color><color=#EFC01A>Typ</color><color=white>]</color> <color=brown>{Type}</color>";

            Player.RemoveDisplayInfo(PlayerInfoArea.Nickname);
            Player.RemoveDisplayInfo(PlayerInfoArea.Role);
            Player.RemoveDisplayInfo(PlayerInfoArea.PowerStatus);
            Player.RemoveDisplayInfo(PlayerInfoArea.UnitName);

            Movement = PlayerMovementState.Sprinting;
            Timing.RunCoroutine(Walk());
        }

        private IEnumerator<float> Walk()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(0.1f);

                if (Owner == null) Destroy();
                if (GameObject == null) yield break;
                RotateToPosition(Owner.Position);

                var distance = Vector3.Distance(Owner.Position, Position);

                if ((PlayerMovementState)Owner.AnimationController.Network_curMoveState == PlayerMovementState.Sneaking) Movement = PlayerMovementState.Sneaking;
                else Movement = PlayerMovementState.Sprinting;

                if (Movement == PlayerMovementState.Sneaking)
                {
                    if (distance > 5f) Position = Owner.Position;

                    else if (distance > 1f) Direction = Synapse.Api.Enum.MovementDirection.Forward;

                    else if (distance <= 1f) Direction = Synapse.Api.Enum.MovementDirection.Stop;

                    continue;
                }

                if (distance > 10f)
                    Position = Owner.Position;

                else if (distance > 2f)
                    Direction = Synapse.Api.Enum.MovementDirection.Forward;

                else if (distance <= 1.25f)
                    Direction = Synapse.Api.Enum.MovementDirection.Stop;

            }
        }
    }
}