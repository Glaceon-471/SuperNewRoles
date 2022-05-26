using SuperNewRoles.CustomOption;
using SuperNewRoles.CustomRPC;
using SuperNewRoles.Patch;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperNewRoles.Roles
{
    class SeerFriends
    {
        public static List<byte> CheckedJackal;
        public static bool CheckJackal(PlayerControl p)
        {
            if (!RoleClass.MadSeer.IsImpostorCheck) return false;
            if (!p.isRole(RoleId.MadSeer)) return false;
            if (CheckedJackal.Contains(p.PlayerId)) return true;
            /*
            SuperNewRolesPlugin.Logger.LogInfo("�C���|�X�^�[�`�F�b�N�^�X�N��:"+RoleClass.MadSeer.ImpostorCheckTask);
            SuperNewRolesPlugin.Logger.LogInfo("�I���^�X�N��:"+TaskCount.TaskDate(p.Data).Item1);*/
            SuperNewRolesPlugin.Logger.LogInfo("�L����:" + (RoleClass.MadSeer.ImpostorCheckTask <= TaskCount.TaskDate(p.Data).Item1));
            if (RoleClass.MadSeer.ImpostorCheckTask <= TaskCount.TaskDate(p.Data).Item1)
            {
                SuperNewRolesPlugin.Logger.LogInfo("�L����Ԃ��܂���");
                return true;
            }
            // SuperNewRolesPlugin.Logger.LogInfo("��ԉ��܂Œʉ�");
            return false;
        }
    }
}
