using Microsoft.Xna.Framework;
using MultiTether.NPCs;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MultiTether
{
    public class PlayerCamera : ModPlayer
    {
        protected override bool CloneNewInstances => true;
        public override void PreUpdate()
        {
            var entitySource = new EntitySource_Parent(Player);
            int matchingCamera = -1;
            if (Player.active && !Player.dead)
            {

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].type == ModContent.NPCType<CameraCenter>() && Main.npc[i].ai[0] == Player.whoAmI + 1 && Main.npc[i].active)
                    {
                        matchingCamera = i;
                        break;
                    }
                }
                if (matchingCamera == -1)
                {
                    matchingCamera = NPC.NewNPC(entitySource, (int)Player.Center.X, (int)Player.Center.Y, ModContent.NPCType<CameraCenter>(), 0, Player.whoAmI + 1, Player.team);
                }

                NPC npc = Main.npc[matchingCamera];
                //if (npc.type == ModContent.NPCType<CameraCenter>() && npc.ai[1] == Player.team && !WithinRange(npc, Main.screenWidth / 2, Main.screenHeight / 2) && WithinRange(npc, (Main.screenWidth / 2) + 100, (Main.screenHeight / 2) + 100))
                //{
                //    Player.velocity = Vector2.Zero;
                //    Player.fallStart = (int)(Player.position.Y / 16f);
                //    Player.position += PlayerDrag(matchingCamera);

                //    //player.velocity = 10 * Vector2.Normalize(npc.Center - player.Center);
                //    //player.position = player.oldPosition;
                //}
            }
            else
            {
                if (matchingCamera != -1)
                {
                    Main.npc[matchingCamera].life = 0;
                    Main.npc[matchingCamera].active = false;
                    matchingCamera = -1;
                }
            }
            // Debugging
            Main.NewText("Matching camera: " + matchingCamera.ToString());
            Main.NewText("Player position: " + Player.Center.X.ToString() + ", " + Player.Center.Y.ToString());
            Main.NewText("Player Identity: " + Player.whoAmI.ToString());
        }

        public override void ModifyScreenPosition()
        {
            if (Player.active && !Player.dead)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (Main.npc[i].active && npc.type == ModContent.NPCType<CameraCenter>() && npc.ai[0] == Player.whoAmI + 1 && Player.GetModPlayer<PlayerCamera>().WithinRange(npc, Main.screenWidth, Main.screenHeight))
                    {
                        Main.screenPosition = npc.Center;
                        Main.screenPosition.X -= Main.screenWidth / 2f;
                        Main.screenPosition.Y -= Main.screenHeight / 2f;
                    }
                }
            }
            base.ModifyScreenPosition();
        }

        public bool WithinRange(NPC npc, int width, int height)
        {
            if (Math.Abs(npc.Center.X - Player.Center.X) > width || Math.Abs(npc.Center.Y - Player.Center.Y) > height)
            {
                return false;
            }
            return true;
        }
        
        //public Vector2 PlayerDrag(int i)
        //{
        //    int cameraNPC = i;
        //    if (cameraNPC >= 0)
        //    {
        //        Vector2 center = Player.Center;
        //        float distanceX = Main.npc[cameraNPC].Center.X - center.X;
        //        float distanceY = Main.npc[cameraNPC].Center.Y - center.Y;
        //        float distance = (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
        //        float maxDistance = 11f;
        //        float finalDistance;
        //        if (distance > maxDistance)
        //        {
        //            finalDistance = maxDistance / distance;
        //        }
        //        else
        //        {
        //            finalDistance = 1f;
        //        }
        //        distanceX *= finalDistance;
        //        distanceY *= finalDistance;
        //        //player.velocity.X = num39;
        //        //player.velocity.Y = num40;
        //        return new Vector2(distanceX, distanceY);
        //    }
        //    return Vector2.Zero;
        //}
    }
}