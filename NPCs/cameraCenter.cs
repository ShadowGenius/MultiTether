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
            npc.width = 1;
            npc.height = 1;
            npc.lifeMax = 100;
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.dontCountMe = true;
            npc.dontTakeDamage = true;
            npc.immortal = true;
            npc.chaseable = false;
            npc.timeLeft = 2;
            npc.netAlways = true;
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
                if (player.whoAmI == npc.ai[0] + 1 && (player.dead || !player.active || !player.GetModPlayer<PlayerCamera>().WithinRange(npc, Main.screenWidth + 10, Main.screenHeight + 10)))
                {
                    npc.life = 0;
                    npc.active = false;
                }

                if (player.active && !player.dead && player.GetModPlayer<PlayerCamera>().WithinRange(npc, Main.screenWidth, Main.screenHeight) && player.team == npc.ai[1])
                {
                    npc.timeLeft = 2;
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
            
            Main.NewText("Camera position: " + cameraCenter.X.ToString() + ", " + cameraCenter.Y.ToString());
            Main.NewText("Camera Player: " + npc.ai[0].ToString());
            Main.NewText("Camera Team: " + npc.ai[1].ToString());
            
            if (Vector2.Distance(npc.Center, cameraCenter) <= 10)
            {
                npc.Center = cameraCenter;
                npc.velocity = Vector2.Zero;
            }
            else
            {
                //npc.velocity = speed * Vector2.Normalize(cameraCenter - npc.Center);
                npc.velocity = (cameraCenter - npc.Center) / 2;
            }
        }
    }
}