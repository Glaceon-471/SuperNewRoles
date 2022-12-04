using SuperNewRoles.CustomObject;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hazel;

namespace SuperNewRoles.Roles.Neutral;

public class Vulture
{
    public class FixedUpdate
    {
        public static void Postfix()
        {
            if (ArrowPointingToDeadBody == null) ArrowPointingToDeadBody.Add(new(RoleClass.Vulture.color));
            float min_target_distance = float.MaxValue;
            DeadBody target = null;
            DeadBody[] deadBodies = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            bool arrowUpdate = ArrowPointingToDeadBody.Count != deadBodies.Count();

            int index = 0;

            if (arrowUpdate)
            {
                foreach (Arrow arrow in ArrowPointingToDeadBody) UnityEngine.Object.Destroy(arrow.arrow);
                ArrowPointingToDeadBody = new List<Arrow>();
            }
            foreach (DeadBody db in deadBodies)
            {
                if (db == null)
                {
                    ArrowPointingToDeadBody[index].arrow.SetActive(false);
                }
                if (arrowUpdate)
                {
                    if (ArrowPointingToDeadBody.Count != 0 && ArrowPointingToDeadBody[index] != null && db != null && target != null)
                    {
                        ArrowPointingToDeadBody[index].Update(target.transform.position, color: RoleClass.Vulture.color);
                        ArrowPointingToDeadBody[index].arrow.SetActive(target != null);
                    }
                    float target_distance = Vector3.Distance(CachedPlayer.LocalPlayer.transform.position, db.transform.position);

                    if (target_distance < min_target_distance)
                    {
                        min_target_distance = target_distance;
                        target = db;
                    }
                }
                index++;
            }
            /*foreach (DeadBody db in deadBodies)
            {
                if (db == null)
                {
                    RoleClass.Vulture.Arrow.arrow.SetActive(false);
                }
                float target_distance = Vector3.Distance(CachedPlayer.LocalPlayer.transform.position, db.transform.position);

                if (target_distance < min_target_distance)
                {
                    min_target_distance = target_distance;
                    target = db;
                }
            }*/
            /*if (RoleClass.Vulture.Arrow != null && target != null)
            {
                RoleClass.Vulture.Arrow.Update(target.transform.position, color: RoleClass.Vulture.color);
            }
            RoleClass.Vulture.Arrow.arrow.SetActive(target != null);*/
        }
        /*public static void Postfix()
        {
            if (ArrowPointingToDeadBody == null) ArrowPointingToDeadBody.Add(new(RoleClass.Vulture.color));
            float min_target_distance = float.MaxValue;
            DeadBody target = null;
            DeadBody[] deadBodies = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            bool arrowUpdate = ArrowPointingToDeadBody.Count != deadBodies.Count();

            int index = 0;

            if (arrowUpdate)
            {
                foreach (Arrow arrow in ArrowPointingToDeadBody) UnityEngine.Object.Destroy(arrow.arrow);
                ArrowPointingToDeadBody = new List<Arrow>();
            }

            foreach (DeadBody db in deadBodies)
            {
                if (arrowUpdate)
                {
                    if (ArrowPointingToDeadBody.Count != 0 && ArrowPointingToDeadBody[index] != null && db != null)
                    {
                        ArrowPointingToDeadBody[index].Update(target.transform.position, color: RoleClass.Vulture.color);
                        ArrowPointingToDeadBody[index].arrow.SetActive(target != null);
                    }
                    float target_distance = Vector3.Distance(CachedPlayer.LocalPlayer.transform.position, db.transform.position);

                    if (target_distance < min_target_distance)
                    {
                        min_target_distance = target_distance;
                        target = db;
                    }
                }
                index++;
            }
        }*/

    }
    public static void RpcCleanDeadBody(int? count)
    {
        foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
        {
            if (collider2D.tag != "DeadBody") continue;

            DeadBody component = collider2D.GetComponent<DeadBody>();
            if (component && !component.Reported)
            {
                Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                Vector2 truePosition2 = component.TruePosition;
                if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance
                    && PlayerControl.LocalPlayer.CanMove
                    && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                {
                    GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CleanBody, SendOption.Reliable, -1);
                    writer.Write(playerInfo.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.CleanBody(playerInfo.PlayerId);
                    if (count != null)
                    {
                        count--;
                        Logger.Info($"DeadBodyCount:{count}", "Vulture");
                    }
                    break;
                }
            }
        }
    }

    public static List<Arrow> ArrowPointingToDeadBody = new();
    public static void ArrowClearAndReload()
    {
        ArrowPointingToDeadBody = null;
    }

    public static void ArrowDelete()
    {
        if (ArrowPointingToDeadBody == null) return;
        if (CachedPlayer.LocalPlayer.Data.IsDead)
        {
            foreach (Arrow arrow in ArrowPointingToDeadBody) UnityEngine.Object.Destroy(arrow.arrow);
            ArrowPointingToDeadBody = new List<Arrow>();
            return;
        }
    }
}
