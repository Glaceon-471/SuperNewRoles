using System.Collections.Generic;
using UnityEngine;
using SuperNewRoles.CustomRPC;

namespace SuperNewRoles.Intro
{
    public class TeamDate
    {
        public string NameKey;
        public Color color;
        public Color BackGround;
        public List<RoleId> RoleIds;

        TeamDate(string NameKey, Color color, Color BackGround, List<RoleId> RoleId)
        {
            this.color = color;
            this.BackGround = BackGround;
            this.NameKey = NameKey;
            this.RoleIds = RoleId;
        }
        public static TeamDate VultureTeam = new("Test", Color.black, Color.yellow, new List<RoleId> { RoleId.Sheriff });
    }
}
