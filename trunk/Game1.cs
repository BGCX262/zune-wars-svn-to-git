using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Timers;

namespace ZUNE_WAR_WINDOWS
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 


     
    public class Object
    {

    }

    public class Tank : Microsoft.Xna.Framework.Game
    {
        public double x;
        public double y;
        public double xVel;
        public double yVel;

        public Tank(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.xVel = 0.0;
            this.yVel = 0.0;
        }
    }
    public class Bullet : Microsoft.Xna.Framework.Game
    {
        public double x;
        public double y;
        public double xVel;
        public double yVel;

        public Bullet(double x, double y, double xv, double yv)
        {
            this.x = x;
            this.y = y;
            this.xVel = xv;
            this.yVel = yv;
        }
    }


    

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Texture2D tankImageRed;
        Texture2D bulletImage;
        Tank player1 = new Tank(25, 25);
        double rot = 0.0;
        List<Bullet> bullets = new List<Bullet>();

        int shotCount = 0;  // 3 shots per second
        float timer = 0.0f;
        float interval = 1000.0f;
        float singleShotInterval = 200.0f;
        float singleShotTimer = 0.0f;



       

        

        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // change screen resolution
            this.graphics.PreferredBackBufferWidth = 240;
            this.graphics.PreferredBackBufferHeight = 320;
            this.graphics.ApplyChanges();
            Random random = new Random();
            // TODO: Add your initialization logic here
           
            
           

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tankImageRed = Content.Load<Texture2D>("tank1");
            bulletImage = Content.Load<Texture2D>("bullet");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (bullets != null)
            {
                Bullet bul = null;
                foreach (Bullet b in bullets)
                {
                    b.x = b.x + b.xVel;
                    b.y = b.y + b.yVel;
                    if (b.x > 240)
                        bul = b;
                    if (b.x < 0)
                        bul = b;
                    if (b.y > 320)
                        bul = b;
                    if (b.y < 0)
                        bul = b;

                }
                bullets.Remove(bul);
                
            }

            //update timer information
            timer = timer + (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            singleShotTimer = singleShotTimer + (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                shotCount = 0;
                timer = 0;
            }

            


            KeyboardState keyboard = Keyboard.GetState();
          
            if (keyboard.IsKeyDown(Keys.Up))
            {
                player1.x = player1.x + player1.xVel;
                player1.y = player1.y + player1.yVel;
                if (player1.y > 320-16)
                    player1.y = 320-16;
                if (player1.y < 0+16)
                    player1.y = 0+16;
                if (player1.x > 240 - 16)
                    player1.x = 240 - 16;
                if (player1.x < 16)
                    player1.x = 16;

            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                rot = rot + 0.1f;
                player1.xVel = Math.Cos(rot);
                player1.yVel = Math.Sin(rot);
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                rot = rot - 0.1f;
                player1.xVel = Math.Cos(rot);
                player1.yVel = Math.Sin(rot);
            }
            if(keyboard.IsKeyDown(Keys.Space))
            {
                if (singleShotTimer > singleShotInterval && shotCount <= 2)
                {
                    Bullet b = new Bullet(player1.x, player1.y, 3*player1.xVel, 3*player1.yVel);
                    bullets.Add(b);
                    singleShotTimer = 0;
                    shotCount++;
                }
            }


       




            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Rectangle pos = new Rectangle((int)player1.x, (int)player1.y, 32, 32);

            spriteBatch.Begin();
            spriteBatch.Draw(tankImageRed, pos, new Rectangle(0, 0, tankImageRed.Height, tankImageRed.Width), Color.White, (float)rot, new Vector2(tankImageRed.Height / 2, tankImageRed.Width / 2), SpriteEffects.None, 0);
            spriteBatch.End();

            spriteBatch.Begin();
            foreach (Bullet b in bullets)
            {
                spriteBatch.Draw(bulletImage, new Rectangle((int)b.x, (int)b.y, 5, 5), Color.White);
            }
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
