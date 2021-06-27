using Microsoft.Xna.Framework;
using MultiTether.NPCs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace MultiTether
{
    public class PlayerCamera : ModPlayer
    {
        public override void PreUpdate()
        {
            bool matchingPlayerCamera = false;
            if (Main.ActivePlayersCount > 1 && player.active && !player.dead)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].type == ModContent.NPCType<CameraCenter>() && Main.npc[i].ai[0] == player.whoAmI)
                    {
                        matchingPlayerCamera = true;
                        break;
                    }
                }
                if (!matchingPlayerCamera)
                {
                    int matchingCamera = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<CameraCenter>());
                    Main.npc[matchingCamera].ai[0] = player.whoAmI;
                    Main.npc[matchingCamera].ai[1] = player.team;
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    MultiTether multiTether = ModContent.GetInstance<MultiTether>();
                    if (npc.type == ModContent.NPCType<CameraCenter>() && npc.ai[1] == player.team && !WithinRange(npc, Main.screenWidth / 2, Main.screenHeight / 2) && WithinRange(npc, Main.screenWidth / 2 + 100, Main.screenHeight / 2 + 100))
                    {
                        player.fallStart = (int)(player.position.Y / 16f);
                        PlayerDrag(i);
                        player.position += player.velocity;

                        //player.velocity = 10 * Vector2.Normalize(npc.Center - player.Center);
                    }
                }
            }
            //string text = "Player position: " + player.Center.X.ToString() + ", " + player.Center.Y.ToString();
            //Main.NewText(text);
        }

        public override void ModifyScreenPosition()
        {
            if (player.active && !player.dead && Main.ActivePlayersCount > 1)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.type == ModContent.NPCType<CameraCenter>() && npc.ai[0] == player.whoAmI)
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