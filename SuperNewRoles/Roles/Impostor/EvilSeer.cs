using SuperNewRoles.CustomObject;
using SuperNewRoles.Roles.Neutral;
using UnityEngine;
using Hazel;

namespace SuperNewRoles.Roles.Impostor;

public class EvilSeer
{
    public class FixedUpdate
    {
        public static void Postfix()
        {
            if (Vulture.ArrowPointingToDeadBody == null)
            {
                Arrow arrow = new(RoleClass.Vulture.color);
                Vulture.ArrowPointingToDeadBody = arrow;
            }
            DeadBody[] targets = null;
            targets = Vulture.ArrowForFindDeadBody();
            foreach (DeadBody target in targets)
            {
                if (Vulture.ArrowPointingToDeadBody != null && target != null)
                {
                    Vulture.ArrowPointingToDeadBody.Update(target.transform.position, color: RoleClass.Vulture.color);
                }
                Vulture.ArrowPointingToDeadBody.arrow.SetActive(target != null);
            }
        }
    }
}
