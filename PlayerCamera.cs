using Microsoft.Xna.Framework;
using MultiTether.NPCs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace MultiTether
{
    public class PlayerCamera : ModPlayer
    {
        public override bool CloneNewInstances => true;
        public override void PreUpdate()
        {
            int matchingCamera = -1;
            if (player.active && !player.dead)
            {

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].type == ModContent.NPCType<CameraCenter>() && Main.npc[i].ai[0] == player.whoAmI + 1 && Main.npc[i].active)
                    {
                        matchingCamera = i;
                        break;
                    }
                }
                if (matchingCamera == -1)
                {
                    matchingCamera = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<CameraCenter>(), 0, player.whoAmI + 1, player.team);
                }

                NPC npc = Main.npc[matchingCamera];
                if (npc.type == ModContent.NPCType<CameraCenter>() && npc.ai[1] == player.team && !WithinRange(npc, Main.screenWidth / 2, Main.screenHeight / 2) && WithinRange(npc, (Main.screenWidth / 2) + 100, (Main.screenHeight / 2) + 100))
                {
                    player.fallStart = (int)(player.position.Y / 16f);
                    PlayerDrag(matchingCamera);
                    player.position += player.velocity;

                    //player.velocity = 10 * Vector2.Normalize(npc.Center - player.Center);
                }
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
            Main.NewText("Matching camera: " + matchingCamera.ToString());
            Main.NewText("Player position: " + player.Center.X.ToString() + ", " + player.Center.Y.ToString());
            Main.NewText("Player Identity: " + player.whoAmI.ToString());
        }

        public override void ModifyScreenPosition()
        {
            if (player.active && !player.dead)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.type == ModContent.NPCType<CameraCenter>() && npc.ai[0] == player.whoAmI + 1 && Main.npc[i].active)
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
            if (Math.Abs(npc.Center.X - player.Center.X) > width || Math.Abs(npc.Center.Y - player.Center.Y) > height)
            {
                return false;
            }
            return true;
        }

        public void PlayerDrag(int i)
        {
            int cameraNPC = i;
            if (cameraNPC >= 0)
            {
                Vector2 center = player.Center;
                float num39 = Main.npc[cameraNPC].Center.X - center.X;
                float num40 = Main.npc[cameraNPC].Center.Y - center.Y;
                float num41 = (float)Math.Sqrt(num39 * num39 + num40 * num40);
                float num43 = 11f;
                float num44 = num41;
                if (num41 > num43)
                {
                    num44 = num43 / num41;
                }
                else
                {
                    num44 = 1f;
                }
                num39 *= num44;
                num40 *= num44;
                player.velocity.X = num39;
                player.velocity.Y = num40;
            }
        }
    }
}