using SuperNewRoles.CustomObject;
using static SuperNewRoles.Roles.Neutral.Vulture;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hazel;
namespace SuperNewRoles.Roles.Impostor;

public class EvilSeer
{
    public class FixedUpdate
    {
        public static void Postfix()
        {
            if (ArrowPointingToDeadBody == null) ArrowPointingToDeadBody.Add(new(RoleClass.Seer.color));
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
                    if (ArrowPointingToDeadBody.Count != 0 && ArrowPointingToDeadBody[index] != null && db != null)
                    {
                        ArrowPointingToDeadBody[index].Update(target.transform.position, color: RoleClass.Seer.color);
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
        }
    }
}
