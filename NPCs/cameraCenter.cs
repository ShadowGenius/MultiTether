using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MultiTether.NPCs
{
    public class CameraCenter : ModNPC
    {
        //public float maxPosX = Main.leftWorld;
        //public float minPosX = Main.rightWorld;
        //public float maxPosY = Main.bottomWorld;
        //public float minPosY = Main.topWorld;

        public float speed = 20f;

        public override void SetDefaults()
        {
            //npc.width = Main.screenWidth;
            //npc.height = Main.screenHeight;
            NPC.width = 1;
            NPC.height = 1;
            NPC.lifeMax = 100;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontCountMe = true;
            NPC.dontTakeDamage = true;
            NPC.immortal = true;
            NPC.chaseable = false;
            NPC.timeLeft = 22500;
            NPC.netAlways = true;
            base.SetDefaults();
        }

        public override void AI()
        {
            float maxPosX = Main.leftWorld;
            float minPosX = Main.rightWorld;
            float maxPosY = Main.bottomWorld;
            float minPosY = Main.topWorld;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.whoAmI == NPC.ai[0] + 1 && player.active && !player.dead)
                {
                    NPC.ai[1] = player.team;

                    //if (!player.GetModPlayer<PlayerCamera>().WithinRange(NPC, Main.screenWidth + 10, Main.screenHeight + 10))
                    //{
                    //    NPC.Center = player.Center;
                    //}
                }

                if (player.active && !player.dead && player.GetModPlayer<PlayerCamera>().WithinRange(NPC, Main.screenWidth, Main.screenHeight) && player.team == NPC.ai[1])
                {
                    NPC.timeLeft = 22500;
                    if (player.BottomRight.X > maxPosX)
                    {
                        maxPosX = player.BottomRight.X;
                    }
                    if (player.TopLeft.X < minPosX)
                    {
                        minPosX = player.TopLeft.X;
                    }
                    if (player.TopLeft.Y < maxPosY)
                    {
                        maxPosY = player.TopLeft.Y;
                    }
                    if (player.BottomRight.Y > minPosY)
                    {
                        minPosY = player.BottomRight.Y;
                    }
                }
            }

            Vector2 cameraCenter;
            cameraCenter.X = (maxPosX + minPosX) / 2f;
            cameraCenter.Y = (maxPosY + minPosY) / 2f;
            
            // Debugging
            Main.NewText("Camera position: " + cameraCenter.X.ToString() + ", " + cameraCenter.Y.ToString());
            Main.NewText("Camera Player: " + NPC.ai[0].ToString());
            Main.NewText("Camera Team: " + NPC.ai[1].ToString());
            
            if (Vector2.Distance(Main.player[(int)NPC.ai[0] - 1].Center, cameraCenter) <= 8 || Vector2.Distance(NPC.Center, cameraCenter) <= 8)
            {
                NPC.Center = cameraCenter;
                NPC.velocity = Vector2.Zero;
            }
            else
            {
                //npc.velocity = speed * Vector2.Normalize(cameraCenter - npc.Center);
                //npc.Center = cameraCenter;
                NPC.velocity.X += (cameraCenter.X - NPC.Center.X) * 0.1f;
                NPC.velocity.Y += (cameraCenter.Y - NPC.Center.Y) * 0.1f;
            }
        }
    }
}