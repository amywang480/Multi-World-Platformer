//Author:           Amy Wang
//File Name:        Player.cs
//Project Name:     ISU
//Creation Date:    December 19, 2018
//Modified Date:    January 20, 2019
//Description:      Create and manage player movements and actions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;
using Camera;

namespace ISU
{
    class Player
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Generate random numbers
        Random rng = new Random();

        //Determine if player is on platform
        bool isOnPlatform = true;
        bool isDucking = false;

        //Track player position
        string dir = "RIGHT";

        //Store player location
        Vector2 playerLoc;

        //Store speeds
        int speed = 3;
        double jumpSpeed = -8.2;
        
        //Store gravity and player velocity
        Vector2 gravity;
        Vector2 velocity = new Vector2(0, 0);

        //Store player animations
        Rectangle playerRec;
        Animation playerRun;
        Animation playerJump;
        Animation playerDuck;
        Rectangle playerBounds;

        //Store player images:
        //Running
        Texture2D playerRunImg;
        //Jumping
        Texture2D playerJumpImg;
        Texture2D playerDuckImg;
        //Idle
        Texture2D playerIdleLeft;
        Texture2D playerIdleRight;

        //Store rectangles of different parts of player
        Rectangle head;
        Rectangle feet;
        Rectangle leftSide;
        Rectangle rightSide;

        //Store collectibles the player collects
        int[] collected = new int[6];

        //Store health of player
        int health;

        //Store time boost from time potion
        public int timeBoost = 0;

        //Store shield time
        public int shieldTimer = 0;

        //Track when collision is disabled
        public bool isColDisabled = false;

        //Store potion queues
        ItemQueue[] itemQueues = new ItemQueue[3];

        //Track when arrow keys can be used
        bool canUseKeys = true;

        //Track when player is in shop
        bool isInShop = false;
        
        /// <summary>
        /// Retrieve player bounds
        /// </summary>
        /// <returns></returns>
        public Rectangle GetPlayerBounds()
        {
            return playerBounds;
        }

        /// <summary>
        /// Retrieve player rectangle
        /// </summary>
        /// <returns>Player rectangle of player's current location</returns>
        public Rectangle GetPlayerRec()
        {
            return playerRec;
        }
    
        /// <summary>
        /// Retrieve to determine if player is in shop
        /// </summary>
        /// <returns>Boolean to determine if player is in shop</returns>
        public bool GetIsInShop()
        {
            return isInShop;
        }

        /// <summary>
        /// Retrieve number of coins collected
        /// </summary>
        /// <returns>Number of coins</returns>
        public int GetNumCoins()
        {
            return collected[0];
        }

        /// <summary>
        /// Retrieve number of stars collected
        /// </summary>
        /// <returns>Number of stars</returns>
        public int GetNumStars()
        {
            return collected[1];
        }

        /// <summary>
        /// Retrieve number of mirrors collected
        /// </summary>
        /// <returns>Number of mirrors</returns>
        public int GetNumMirrors()
        {
            return collected[2];
        }

        /// <summary>
        /// Retrieve number of chalices collected
        /// </summary>
        /// <returns>Number of chalices</returns>
        public int GetNumChalices()
        {
            return collected[3];
        }

        /// <summary>
        /// Retrieve number of diamonds collected
        /// </summary>
        /// <returns>Number of diamonds</returns>
        public int GetNumDiamonds()
        {
            return collected[4];
        }

        /// <summary>
        /// Retrieve number of crowns collected
        /// </summary>
        /// <returns>Number of crowns</returns>
        public int GetNumCrowns()
        {
            return collected[5];
        }

        /// <summary>
        /// Retrieve player health
        /// </summary>
        /// <returns>Player health</returns>
        public int GetHealth()
        {
            return health;
        }

        /// <summary>
        /// Retrieve number of time potions bought
        /// </summary>
        /// <returns>Number of time potions</returns>
        public int GetNumTime()
        {
            return itemQueues[0].GetSize();
        }

        /// <summary>
        /// Retrieve number of health potions bought
        /// </summary>
        /// <returns>Number of health potions</returns>
        public int GetNumHealth()
        {
            return itemQueues[1].GetSize();
        }

        /// <summary>
        /// Retrieve number of shield potions bought
        /// </summary>
        /// <returns></returns>
        public int GetNumShield()
        {
            return itemQueues[2].GetSize();
        }
    
        /// <summary>
        /// Set player location
        /// </summary>
        /// <param name="playerLoc">New player location</param>
        public void SetPlayerLoc(Vector2 playerLoc)
        {
            this.playerLoc = playerLoc;
        }
  
        /// <summary>
        /// Reset values in game restart
        /// </summary>
        public void Reset()
        {
            int initialHealth = rng.Next(200, 270);
            int initialCoins = rng.Next(90, 130);

            for (int i = 1; i < collected.Length; ++i)
            {
                collected[i] = 0;
            }
        
            collected[0] = initialCoins;
            health = initialHealth;

            for (int i = 0; i < itemQueues.Length; ++i)
            {
                for (int j = 0; j < itemQueues[i].GetSize(); ++i)
                {
                    itemQueues[i].Dequeue();
                }
            }
        }

        /// <summary>
        /// Focus camera on player
        /// </summary>
        /// <param name="cam">Camera</param>
        public void Camera(Cam2D cam)
        {
            cam.LookAt(playerRec);
        }
    
        /// <summary>
        /// Load player content
        /// </summary>
        /// <param name="Content">Load content</param>
        /// <param name="curWorld">Current world player is in</param>
        public void Load(ContentManager Content, World curWorld)
        {
            //Load running image and animation
            playerRunImg = Content.Load<Texture2D>("Player/Run");
            playerRun = new Animation(playerRunImg, 10, 1, 10, 0, 0, Animation.ANIMATE_FOREVER, 3, playerLoc, 0.45f, false);

            //Load jumping image and animation
            playerJumpImg = Content.Load<Texture2D>("Player/Jump");
            playerJump = new Animation(playerJumpImg, 7, 1, 7, 0, 0, Animation.ANIMATE_FOREVER, 4, playerLoc, 0.5f, false);

            //Load ducking image and animation
            playerDuckImg = Content.Load<Texture2D>("Player/Duck");
            playerDuck = new Animation(playerDuckImg, 3, 1, 3, 0, 0, Animation.ANIMATE_FOREVER, 6, new Vector2(playerLoc.X, 
                         playerLoc.Y + 10), 1.2f, false);
           
            //Load idle images
            playerIdleLeft = Content.Load<Texture2D>("Player/IdleLeft");
            playerIdleRight = Content.Load<Texture2D>("Player/IdleRight");

            //Load player rectangle
            playerRec = new Rectangle((int)playerLoc.X, (int)playerLoc.Y, (int)(playerIdleLeft.Width * 0.45),
                            (int)(playerIdleLeft.Height * 0.45));
            
            //Load player bounds
            playerBounds = new Rectangle(curWorld.GetWorldBounds().X + (int)playerLoc.X, curWorld.GetWorldBounds().Y + 
            (int)playerLoc.Y, playerRec.Width, playerRec.Height);

            //Set player location to player rectangle
            playerLoc.X = playerRec.X;
            playerLoc.Y = playerRec.Y;

            //Set gravity
            gravity = new Vector2(0, 9.8f / 60f);
        }
    
        /// <summary>
        /// Load items and collectibles
        /// </summary>
        public void LoadItems()
        {
            //Set collectibles collected to 0
            for (int i = 0; i < collected.Length; ++i)
            {
                collected[i] = 0;
            }

            //Initialize player coins 
            collected[0] = rng.Next(90, 130);

            //Create item queues for potions
            for (int i = 0; i < itemQueues.Length; ++i)
            {
                itemQueues[i] = new ItemQueue();
            }

            //Initialize health
            health = rng.Next(200, 270);
        }

        /// <summary>
        /// Control player movement
        /// </summary>
        /// <param name="kb">Keyboard state</param>
        /// <param name="worldBounds">World bounds rectangle</param>
        public void PlayerMovement(KeyboardState kb, Rectangle worldBounds)
        {
            //If the player can use arrow keys:
            if (canUseKeys)
            {
                if (kb.IsKeyDown(Keys.Right))
                {
                    //Turn right when pressing right key
                    RunRight(worldBounds);
                }
                else if (kb.IsKeyDown(Keys.Left))
                {
                    //Turn left when pressing left key
                    RunLeft(worldBounds);
                }
                else
                {
                    //Stop running when no keys are pressed
                    StopRun();
                }
                
                //Jump when pressing up key
                if (kb.IsKeyDown(Keys.Up) && playerJump.isAnimating == false)
                {
                    Jump(worldBounds);
                }

                //Duck when pressing down key
                if (kb.IsKeyDown(Keys.Down))
                {
                    isDucking = true;
                }
                else
                {
                    isDucking = false;
                }

                //Allow for player to duck
                Duck(isDucking);
            }
        }

        /// <summary>
        /// Allow player to run left by changing velocity and displaying running animation
        /// </summary>
        /// <param name="speed">Integer revealing speed at which player runs</param>
        public void RunLeft(Rectangle worldBounds)
        {
            //Set direction
            dir = "LEFT";

            //Start running animation
            playerRun.isAnimating = true;

            //Move left
            velocity.X = -speed;

            //Control horizontal movement
            MoveHorizontal(-speed, true, worldBounds);
        }

        /// <summary>
        /// Allow player to run right by changing velocity and displaying running animation
        /// </summary>
        /// <param name="speed">Integer revealing speed at which player runs</param>
        public void RunRight(Rectangle worldBounds)
        {
            //Set direction
            dir = "RIGHT";

            //Start running animation
            playerRun.isAnimating = true;

            //Move right
            velocity.X = speed;

            //Control horizontal movement
            MoveHorizontal(speed, true, worldBounds);
        }

        /// <summary>
        /// Prevent player from running
        /// </summary>
        public void StopRun()
        {
            //Set horizontal velocity to 0
            velocity.X = 0;

            //Stop player running animation
            playerRun.isAnimating = false;
        }

        /// <summary>
        /// Move the player horizontally while restricting it to the world boundaries
        /// </summary>
        /// <param name="amount">The amound to move the player horizontally</param>
        /// <param name="restrictMovement">true if player should remain in the world, false otherwise</param>
        public void MoveHorizontal(float amount, bool restrictMovement, Rectangle worldBounds)
        {
            if (restrictMovement == true)
            {
                if (amount < 0)
                {
                    //Keep the player to the right of the left world edge
                    playerRec.X = (int)Math.Max(worldBounds.X, playerRec.X + amount);
                }
                else
                {
                    //Keep the player to the left of the right world edge
                    playerRec.X = (int)Math.Min(worldBounds.Width - playerRec.Width, playerRec.X + amount);
                }
            }
            else
            {
                playerRec.X += (int)amount;
            }
        }

        /// <summary>
        /// Move the player vertically while restricting it to the world boundaries
        /// </summary>
        /// <param name="amount">The amound to move the player horizontally</param>
        /// <param name="restrictMovement">true if player should remain in the world, false otherwise</param>
        private void MoveVertical(float amount, bool restrictMovement, Rectangle worldBounds)
        {
            if (restrictMovement == true)
            {
                if (amount < 0)
                {
                    //Keep the player to the right of the left world edge
                    playerRec.Y = (int)Math.Max(worldBounds.Y, playerRec.Y + amount);
                }
                else
                {
                    //Keep the player to the left of the right world edge
                    playerRec.Y = (int)Math.Min(worldBounds.Height - playerRec.Height, playerRec.Y + amount);
                }
            }
            else
            {
                playerRec.Y += (int)amount;
            }
        }

        /// <summary>
        /// Allow player to jump upwards
        /// </summary>
        public void Jump(Rectangle worldBounds)
        {
            //Displaying jumping animation
            playerJump.isAnimating = true;

            //Change player's vertical velocity
            velocity.Y = (float)jumpSpeed;

            MoveVertical(speed, true, worldBounds);
            //UpdatePlayerLoc(worldBounds);
        }

        /// <summary>
        /// Allow player to duck downwards
        /// </summary>
        /// <param name="isDucking">Boolean determining if player is ducking</param>
        public void Duck(bool isDucking)
        {
            //Start ducking animation
            if (isDucking && isOnPlatform)
            {
                playerDuck.isAnimating = true;
            }
            else
            {
                playerDuck.isAnimating = false;
            }
        }

        /// <summary>
        /// Update player movement and detect collisions between player and platform
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        /// <param name="platforms">World platforms</param>
        /// <param name="boxes">World boxes</param>
        /// <param name="shopRec">Shop door rectangle</param>
        public void Update(GameTime gameTime, Platform[] platforms, Obstacle[] boxes, Rectangle shopRec)
        {
            //Adjust player location
            velocity.Y += gravity.Y;
            playerLoc.X += velocity.X;
            playerLoc.Y += velocity.Y;

            //Update player location
            UpdatePlayerLoc();

            //Detect platform collisions
            PlatformCollision(platforms);
            
            //Update animations
            playerRun.Update(gameTime);
            playerJump.Update(gameTime);
            playerDuck.Update(gameTime);

            //Player is in shop when colliding with shop rectangle
            if (RecCollision(playerRec, shopRec))
            {
                isInShop = true;
            }
        }

        /// <summary>
        /// Find collisions and execute player actions depending on where the collision occurs on the player
        /// </summary>
        /// <param name="platforms">Array of platforms in the current world</param>
        public void PlatformCollision(Platform[] platforms)
        {
            //Loop through all platforms
            for (int i = 0; i < platforms.Length; ++i)
            {
                if (platforms[i] != null)
                {
                    //Detect collision for each part and the platforms:
                    if (platforms[i].RecCollision(feet))
                    {
                        //Player remains on platform upon collision with feet
                        playerLoc.Y = platforms[i].GetY() - playerRec.Height;
                        isOnPlatform = true;
                        velocity.Y = 0;
                        playerJump.isAnimating = false;
                        UpdatePlayerLoc();

                        //Player can use arrow keys
                        canUseKeys = true;
                    }
               
                    if (platforms[i].RecCollision(head))
                    {
                        //Player bounces from platform upon collision with head
                        playerLoc.Y = platforms[i].GetBottom();
                        velocity.Y = -velocity.Y;
                        velocity.Y += gravity.Y;
                    }

                    if (platforms[i].RecCollision(leftSide))
                    {   
                        //Player falls downwards upon collision on the left side
                        playerLoc.X = platforms[i].GetX() + platforms[i].GetWidth();
                        velocity.Y += gravity.Y;
                    }

                    if (platforms[i].RecCollision(rightSide))
                    {
                        //Player falls downwards upon collision on the right side
                        playerLoc.X = platforms[i].GetX() - playerRec.Width;
                        velocity.Y += gravity.Y;
                    }
                }
            }
        }

        /// <summary>
        /// Detect collision with boxes
        /// </summary>
        /// <param name="boxes">Array of boxes</param>
        /// <param name="attack">Attack sound effect</param>
        public void BoxObsCollision(Obstacle[] boxes, SoundEffect attack)
        {
            //Store rectangle for player's feet
            Rectangle boxFeet = new Rectangle();

            //Depending on direction, adjust player rectangle for feet
            if (dir == "LEFT")
            {
                boxFeet = new Rectangle(playerRec.X + 8, playerRec.Y + 28, playerRec.Width - 23, 6);
            }
            else if (dir == "RIGHT")
            {
                boxFeet = new Rectangle(playerRec.X + 16, playerRec.Y + 28, playerRec.Width - 23, 6); 
            }

            //Loop through all boxes
            for (int i = 0; i < boxes.Length; ++i)
            {
                if (boxes[i] != null)
                {
                    //Detect collisions between player feet and boxes
                    if (RecCollision(boxFeet, boxes[i].GetRec()))
                    {
                        //When box is an obstacle:
                        if (boxes[i].GetObsType())
                        {
                            //Play sound effect
                            attack.CreateInstance().Play();

                            //Can use keys to avoid box
                            canUseKeys = true;
                        
                            //Jump upwards
                            velocity.Y = (int)jumpSpeed;
                           
                            //Update player location
                            UpdatePlayerLoc();

                            //Decrease health
                            health -= Math.Min(boxes[i].GetHealthStolen(), health);
                        }
                        else
                        {
                            //Keep player on top of box when not an obstacle
                            playerLoc.Y = boxes[i].GetRec().Y - playerRec.Height;
                            isOnPlatform = true;
                            velocity.Y = 0;
                            playerJump.isAnimating = false;
                            UpdatePlayerLoc();
                            canUseKeys = true;
                        }
                    }
                    else if (RecCollision(head, boxes[i].GetRec()))
                    {
                        //Player bounces from box upon collision with head
                        if (boxes[i].GetObsType())
                        {
                            //Play sound effect
                            attack.CreateInstance().Play();

                            //Decrease health when an obstacle
                            health -= Math.Min(boxes[i].GetHealthStolen(), health);
                        }
                        else
                        {
                            //Bounce from box
                            velocity.Y = -velocity.Y;
                        }
                    }
                    else if (RecCollision(leftSide, boxes[i].GetRec()))
                    {
                        //Player jumps upon collision with left side
                        if (boxes[i].GetObsType())
                        {
                            //Play sound effect
                            attack.CreateInstance().Play();

                            //Cannot use keys
                            canUseKeys = false;

                            //Jump upwards and to the side
                            velocity.Y = (int)jumpSpeed;
                            velocity.X = speed;

                            //Adjust player location
                            playerLoc.X += Math.Max(speed, Math.Abs(velocity.X));
                            UpdatePlayerLoc();

                            //Decrease health
                            health -= Math.Min(boxes[i].GetHealthStolen(), health);
                        }
                        else
                        {
                            //When not an obstacle, keep player to side of box
                            playerLoc.X = boxes[i].GetRec().X + boxes[i].GetRec().Width;
                            velocity.Y += gravity.Y;
                            canUseKeys = true;
                        }
                    }
                    else if (RecCollision(rightSide, boxes[i].GetRec()))
                    {
                        //Player jumps upon collision on the right side
                        if (boxes[i].GetObsType())
                        {
                            //Play sound effect
                            attack.CreateInstance().Play();

                            //Cannot use keys
                            canUseKeys = false;

                            //Jumps upwards and to the side
                            velocity.Y = (int)jumpSpeed;
                            velocity.X = -speed;

                            //Update player location
                            playerLoc.X -= Math.Max(speed, Math.Abs(velocity.X));
                            UpdatePlayerLoc();

                            //Decrease health
                            health -= Math.Min(boxes[i].GetHealthStolen(), health);
                        }
                        else
                        {
                            //When not an obstacle, keep player to side of the box
                            playerLoc.X = boxes[i].GetRec().X - playerRec.Width;
                            velocity.Y += gravity.Y;
                            canUseKeys = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Detect player and blade collision
        /// </summary>
        /// <param name="blades">Array of blades</param>
        /// <param name="attack">Attack sound effect</param>
        public void BladeCollision(Obstacle[] blades, SoundEffect attack)
        {
            //Store amount for player to move upon collision with head
            int move = 50;
        
            //Store left and right side rectangles of player
            rightSide = new Rectangle(playerRec.X + playerRec.Width - 5, playerRec.Y, 5, playerRec.Height - 24);
            leftSide = new Rectangle(playerRec.X, playerRec.Y, 7, playerRec.Height - 24);

            //Loop through all blades
            for (int i = 0; i < blades.Length; ++i)
            {
                if (blades[i] != null)
                {
                    //Detect collision based on parts of player hit
                    if (isOnPlatform && RecCollision(head, blades[i].GetColRec()))
                    {
                        //Play sound effect
                        attack.CreateInstance().Play();

                        //Upon collision with head, move player to side of blade
                        if (dir == "LEFT")
                        {
                            playerLoc.X = blades[i].GetRec().Right + move;
                        }
                        else if (dir == "RIGHT")
                        {
                            playerLoc.X = blades[i].GetRec().X - move;
                        }
                   
                        //Update player location
                        UpdatePlayerLoc();
                    
                        //Decrease health
                        health -= Math.Min(blades[i].GetHealthStolen(), health);
                    }
                    else if (!isOnPlatform && RecCollision(head, blades[i].GetColRec()))
                    {
                        //Play sound effect
                        attack.CreateInstance().Play();

                        //When player is not on platform and head collides with blade, move downwards
                        velocity.Y = -velocity.Y;
                    }
                    else if (RecCollision(leftSide, blades[i].GetColRec()))
                    {
                        //Play sound effect
                        attack.CreateInstance().Play();

                        //Upon collision with left side, cannot use keys
                        canUseKeys = false;

                        //Jump upwards and to the side
                        velocity.Y = (int)jumpSpeed;
                        velocity.X = speed;
                    
                        //Update player location
                        playerLoc.X += Math.Max(speed, Math.Abs(velocity.X));
                        UpdatePlayerLoc();

                        //Decrease health
                        health -= Math.Min(blades[i].GetHealthStolen(), health);
                    }
                    else if (RecCollision(rightSide, blades[i].GetColRec()))
                    {
                        //Play sound effect
                        attack.CreateInstance().Play();

                        //Upon collision with right side, cannot use keys
                        canUseKeys = false;

                        //Jump upwards and to the side
                        velocity.Y = (int)jumpSpeed;
                        velocity.X = -speed;

                        //Update player location
                        playerLoc.X -= Math.Max(speed, Math.Abs(velocity.X));
                        UpdatePlayerLoc();
                    
                        //Decrease health
                        health -= Math.Min(blades[i].GetHealthStolen(), health);
                    }
                    else if (RecCollision(feet, blades[i].GetColRec()))
                    {
                        //Play sound effect
                        attack.CreateInstance().Play();

                        //Jump upwards upon collision with feet
                        velocity.Y = (int)jumpSpeed;

                        //Decrease health
                        health -= Math.Min(blades[i].GetHealthStolen(), health);
                    }
                }
            }
        }

        /// <summary>
        /// Detect snake collision
        /// </summary>
        /// <param name="snakes">Array of snakes in current world</param>
        /// <param name="platforms">Array of platforms in current world</param>
        /// <param name="attack">Attack sound effect</param>
        public void SnakeCollision(Enemy[] snakes, Platform[] platforms, SoundEffect attack)
        {
            //Loop through all snakes
            for (int i = 0; i < snakes.Length; ++i)
            {
                if (snakes[i] != null)
                {
                    //Detect collision based on part of player hit
                    if (RecCollision(rightSide, snakes[i].GetColRec()))
                    {
                        //Play sound effect
                        attack.CreateInstance().Play();

                        //Upon collision with right side, cannot use keys
                        canUseKeys = false;
                        
                        //Jump upwards and to the side
                        velocity.Y = (int)jumpSpeed;
                        velocity.X = -speed;

                        //Update player location
                        playerLoc.X -= Math.Max(speed, Math.Abs(velocity.X));
                        UpdatePlayerLoc();

                        //Steal collectible 
                        snakes[i].Steal(collected);
                    }
                    else if (RecCollision(playerRec, snakes[i].GetColRec()))
                    {
                        //Play sound effect
                        attack.CreateInstance().Play();

                        //Upon collision with other parts, cannot use keys
                        canUseKeys = false; 
                        
                        //Jump upwards and to the side
                        velocity.Y = (int)jumpSpeed;
                        velocity.X = speed;

                        //Update player location
                        playerLoc.X += Math.Max(speed, Math.Abs(velocity.X));
                        UpdatePlayerLoc();

                        //Steal collectible
                        snakes[i].Steal(collected);
                    }
                }
            }
        }

        /// <summary>
        /// Detect fireball collision with player
        /// </summary>
        /// <param name="fireball">Fireball in the world</param>
        /// <param name="attack">Attack sound effect</param>
        public void FireballCollision(Enemy fireball, SoundEffect attack)
        {
            if (RecCollision(playerRec, fireball.GetColRec()))
            {
                //Play sound effect
                attack.CreateInstance().Play();

                //Steal collectible
                fireball.Steal(collected);
            }
        }
        
        /// <summary>
        /// Determine if two rectangles, r1 and r2, have collided with each other
        /// </summary>
        /// <param name="r1">First rectangle</param>
        /// <param name="r2">Second rectangle</param>
        /// <returns>Boolean as true or false</returns>
        private bool RecCollision(Rectangle r1, Rectangle r2)
        {
            //Test for all the impossible cases of collision
            if (r1.Left > r2.Right || r1.Top > r2.Bottom || r1.Right < r2.Left || r1.Bottom < r2.Top)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Update locations of player animations and rectangle 
        /// </summary>
        public void UpdatePlayerLoc()
        {
            //Store y value adjustment for ducking
            int adjust = 10;
        
            //Jumping animation
            playerJump.destRec.X = (int)playerLoc.X;
            playerJump.destRec.Y = (int)playerLoc.Y;

            //Running animation
            playerRun.destRec.X = (int)playerLoc.X;
            playerRun.destRec.Y = (int)playerLoc.Y;

            //Ducking animation
            playerDuck.destRec.X = (int)playerLoc.X;
            playerDuck.destRec.Y = (int)playerLoc.Y + adjust;

            //Set Y value of player rectangle
            if (isDucking)
            {
                //If ducking, set lower Y value
                playerRec.Y = (int)playerLoc.Y + adjust;
            }
            else
            {
                //If not, set to Y value of other animations
                playerRec.Y = (int)playerLoc.Y;
            }

            //Set X value of player rectangle
            playerRec.X = (int)playerLoc.X;

            //Store feet rectangle of player
            feet = new Rectangle(playerRec.X + 5, playerRec.Y + 28, playerRec.Width - 10, 6);
            
            //Depending on direction, adjust player part rectangles
            if (dir == "LEFT")
            {
                head = new Rectangle(playerRec.X + 7, playerRec.Y, playerRec.Width - 20, 15);
                rightSide = new Rectangle(playerRec.X + playerRec.Width - 5, playerRec.Y + 12, 5, playerRec.Height - 24);
                leftSide = new Rectangle(playerRec.X, playerRec.Y + 12, 7, playerRec.Height - 24);
            }
            else
            {
                head = new Rectangle(playerRec.X + 15, playerRec.Y, playerRec.Width - 20, 15);
                rightSide = new Rectangle(playerRec.X + playerRec.Width - 5, playerRec.Y + 12, 5, playerRec.Height - 24);
                leftSide = new Rectangle(playerRec.X, playerRec.Y + 12, 5, playerRec.Height - 24);
            }
        }

        /// <summary>
        /// Detect collision detection for when player collects collectibles
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        /// <param name="collectibles">Array of world collectibles</param>
        /// <param name="curWorld">Current world player is in</param>
        /// <param name="collect">Collect sound effect</param>
        public void Collect(GameTime gameTime, CollectibleManager[] collectibles, World curWorld, SoundEffect collect)
        {
            //Loop through all collectibles
            for (int i = 0; i < collectibles.Length; ++i)
            {
                //Coin collectible:
                if (!(curWorld is Final))
                {
                    if (RecCollision(playerRec, collectibles[i].GetCoin().GetRec()))
                    {
                        //When coin is drawn and there is collision:
                        if (collectibles[i].GetCoin().isDrawn)
                        {
                            //Play sound effect
                            collect.CreateInstance().Play();

                            //Increment coins
                            collected[0]++;
                        }

                        //Remove coin from screen
                        collectibles[i].GetCoin().isDrawn = false;
                    }
                    
                    //Star collectible:
                    if (RecCollision(playerRec, collectibles[i].GetStar().GetRec()))
                    {
                        //When star is drawn and there is collision:
                        if (collectibles[i].GetStar().isDrawn)
                        {
                            //Play sound effect
                            collect.CreateInstance().Play();

                            //Increment stars
                            collected[1]++;
                        }

                        //Remove star from screen
                        collectibles[i].GetStar().isDrawn = false;
                    }

                    //World collectibles:
                    if (RecCollision(playerRec, collectibles[i].GetCollect().GetRec()))
                    {
                        //When collectibles are drawn and there is collision, increment numbers
                        if (collectibles[i].GetCollect().isDrawn)
                        {
                            //Play sound effect
                            collect.CreateInstance().Play();

                            if (curWorld is Spring)
                            {
                                collected[2]++;
                            }
                            else if (curWorld is Summer)
                            {
                                collected[3]++;
                            }
                            else if (curWorld is Autumn)
                            {
                                collected[4]++;
                            }
                            else if (curWorld is Winter)
                            {
                                collected[5]++;
                            }
                        }
                    
                        //Remove world collectible from screen
                        collectibles[i].GetCollect().isDrawn = false;
                    }
                }
                else
                {
                    FinalWorldCollect(collectibles, i, collect);                    
                }
            }
        }
    
        /// <summary>
        /// Manage collecting of collectibles in Final world
        /// </summary>
        /// <param name="collectibles">Array of world collectibles</param>
        /// <param name="i">Collectible number</param>
        /// <param name="collect">Collect sound effect</param>
        private void FinalWorldCollect(CollectibleManager[] collectibles, int i, SoundEffect collect)
        {
            //Loop through all collectibles at each location
            for (int j = 0; j < 5; j++)
            {
                //Detect collision between player and collectible
                if (RecCollision(playerRec, collectibles[i].GetAllCollect()[j].GetRec()) &&
                    collectibles[i].GetAllCollect()[j].isDrawn)
                {
                    //Play sound effect
                    collect.CreateInstance().Play();

                    //Determine collectible collected
                    switch (collectibles[i].GetAllCollect()[j].GetName())
                    {
                        case "Star":
                            collected[1]++;
                            break;
                        case "Mirror":
                            collected[2]++;
                            break;
                        case "Chalice":
                            collected[3]++;
                            break;
                        case "Diamond":
                            collected[4]++;
                            break;
                        case "Crown":
                            collected[5]++;
                            break;
                    }

                    //Remove collectible from screen
                    collectibles[i].GetAllCollect()[j].isDrawn = false;
                }
            }
        }

        /// <summary>
        /// Determine if player has clicked a rectangle or not
        /// </summary>
        /// <param name="rec">Rectangle to be checked</param>
        /// <param name="mouse">MouseState revealing player's current mouse state</param>
        /// <param name="prevMouse">MouseState revealing player's previous mouse state</param>
        /// <returns>Boolean determining if a rectangle has been clicked</returns>
        public bool Click(Rectangle rec, MouseState mouse, MouseState prevMouse)
        {
            //Check if player presses left mouse button once
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                //If player clicks in location of rectangle, rectangle has been clicked
                if (rec.X <= mouse.X && mouse.X <= rec.Right && rec.Y <= mouse.Y && mouse.Y <= rec.Bottom)
                {
                    return true;
                }
            }

            //Return false if rectangle has not been clicked
            return false;
        }

        /// <summary>
        /// Execute actions for player to purchase potions from the shop
        /// </summary>
        /// <param name="mouse">MouseState revealing player's current mouse state</param>
        /// <param name="prevMouse">MouseState revealing player's previous mouse state</param>
        /// <param name="shop">Shop selling potions</param>
        /// <param name="curWorld">World player is currently in</param>
        /// <param name="cashRegister">Cash register sound effect for buying potions</param>
        public void Buy(MouseState mouse, MouseState prevMouse, Shop shop, World curWorld, SoundEffect cashRegister)
        {
            //Adjust player's X value
            int adjust = 10;

            //When shop exit button is clicked:
            if (Click(shop.GetExitRec(), mouse, prevMouse) && isInShop)
            {
                //Player is not in shop
                isInShop = false;

                //Adjust player's X value to be next to shop door
                playerLoc.X = curWorld.GetShopRec().Right + adjust;
            
                //Restock the shop
                shop.Restock();
            }

            //When time potion is clicked:
            if (Click(shop.GetTimeRec(), mouse, prevMouse) && shop.isTimeDrawn)
            {
                //Do not display other potion info
                shop.isHealthInfoDrawn = false;
                shop.isShieldInfoDrawn = false;

                //Display time potion info
                shop.isTimeInfoDrawn = true;
            }

            //When health potion is clicked:
            if (Click(shop.GetHealthRec(), mouse, prevMouse) && shop.isHealthDrawn)
            {
                //Do not display other potion info
                shop.isTimeInfoDrawn = false;
                shop.isShieldInfoDrawn = false;

                //Display health potion info
                shop.isHealthInfoDrawn = true;
            }

            //When shield potion is clicked:
            if (Click(shop.GetShieldRec(), mouse, prevMouse) && shop.isShieldDrawn)
            {
                //Do not display other potion info
                shop.isTimeInfoDrawn = false;
                shop.isHealthInfoDrawn = false;

                //Display shield potion info
                shop.isShieldInfoDrawn = true;
            }
        
            //Execute actions to purchase potions
            PurchasePotions(shop.GetBuyTimeRec(), "TIME", shop.isTimeInfoDrawn, shop.GetTime().GetCost(), 
                            shop, mouse, prevMouse, cashRegister);
            PurchasePotions(shop.GetBuyHealthRec(), "HEALTH", shop.isHealthInfoDrawn, shop.GetHealth().GetCost(), 
                            shop, mouse, prevMouse, cashRegister);
            PurchasePotions(shop.GetBuyShieldRec(), "SHIELD", shop.isShieldInfoDrawn, shop.GetShield().GetCost(), 
                            shop, mouse, prevMouse, cashRegister);
        }

        /// <summary>
        /// Allwo player to use potions
        /// </summary>
        /// <param name="rec">Potion rectangle clicked</param>
        /// <param name="type">Name of potion</param>
        /// <param name="mouse">Mouse state</param>
        /// <param name="prevMouse">Previous mouse state</param>
        /// <param name="potions">Potions sound effect</param>
        public void Use(Rectangle rec, string type, MouseState mouse, MouseState prevMouse, SoundEffect potions)
        {
            //If potion is clicked:
            if (Click(rec, mouse, prevMouse))
            {
                //Determine type of potion clicked
                switch (type)
                {
                    case "TIME":
                        //Use time potion
                        if (itemQueues[0].GetSize() > 0)
                        {
                            //Play sound effect
                            potions.CreateInstance().Play();

                            //Dequeue
                            timeBoost = itemQueues[0].Peek().GetPower();
                            itemQueues[0].Dequeue();
                        }
                        break;
                    case "HEALTH":
                        //Use health potion
                        if (itemQueues[1].GetSize() > 0)
                        {
                            //Play sound effect
                            potions.CreateInstance().Play();

                            //Dequeue
                            health += itemQueues[1].Peek().GetPower();
                            itemQueues[1].Dequeue();
                        }
                        break;
                    case "SHIELD":
                        //Use shield potion
                        if (itemQueues[2].GetSize() > 0)
                        {
                            //Play sound effect
                            potions.CreateInstance().Play();

                            //Dequeue
                            shieldTimer = itemQueues[2].Peek().GetPower();
                            itemQueues[2].Dequeue();

                            //Disable collision
                            isColDisabled = true;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Allow for shield potion to be used
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public void UseShield(GameTime gameTime)
        {
            //Decrease shield time
            shieldTimer -= gameTime.ElapsedGameTime.Milliseconds;

            //Collision is no longer disabled when time ends
            if (shieldTimer <= 0)
            {
                isColDisabled = false;
            }
        }

        /// <summary>
        /// Execute actions to purchase potions from shop, including adding potion to player's item queues
        /// </summary>
        /// <param name="rec">Rectangle the player clicked</param>
        /// <param name="type">Name of potion clicked</param>
        /// <param name="infoDrawn">Boolean to determine if potion info is drawn</param>
        /// <param name="cost">Integer revealing cost of potion</param>
        /// <param name="shop">Shop selling potions</param>
        /// <param name="mouse">MouseState revealing player's current mouse state</param>
        /// <param name="prevMouse">MouseState revealing player's previous mouse state</param>
        /// <param name="cashRegister">Cash register sound effect played when player buys potions</param>
        private void PurchasePotions(Rectangle rec, string type, bool infoDrawn, int cost, Shop shop, MouseState mouse, 
                                     MouseState prevMouse, SoundEffect cashRegister)
        {
            //Determine if player clicked on buy button and if potion info is drawn
            if (Click(rec, mouse, prevMouse) && infoDrawn)
            {
                //If player's coins is higher than potion's cost:
                if (collected[0] >= cost)
                {
                    //Determine type of potion purchased
                    switch (type)
                    {
                        case "TIME":
                            //Play sound efffect
                            cashRegister.CreateInstance().Play();

                            //Add time item to player's time queue
                            itemQueues[0].Enqueue(shop.GetTime());

                            //Take potion off of shop
                            shop.isTimeDrawn = false;
                            shop.isTimeInfoDrawn = false;
                            break;
                        case "HEALTH":
                            //Play sound efffect
                            cashRegister.CreateInstance().Play();

                            //Add health item to player's health queue
                            itemQueues[1].Enqueue(shop.GetHealth());

                            //Take potion off of shop
                            shop.isHealthDrawn = false;
                            shop.isHealthInfoDrawn = false;
                            break;
                        case "SHIELD":
                            //Play sound efffect
                            cashRegister.CreateInstance().Play();

                            //Add shield item to player's shield queue
                            itemQueues[2].Enqueue(shop.GetShield());

                            //Take potion off of shop
                            shop.isShieldDrawn = false;
                            shop.isShieldInfoDrawn = false;
                            break;
                    }

                    //Subtract potion cost from player's coins
                    collected[0] -= cost;
                }
            }
        }
    
        /// <summary>
        /// Determine if player collided with world door
        /// </summary>
        /// <param name="doorRec">Rectangle of door</param>
        /// <returns>Boolean determining if collision occurred</returns>
        public bool Transport(Rectangle doorRec)
        {
            if (RecCollision(doorRec, playerRec))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Allow player to open treasure chest
        /// </summary>
        /// <param name="curWorld">Word player is currently in</param>
        /// <returns>Boolean determining if collision with platform occurred</returns>
        public bool OpenChest(World curWorld)
        {
            //Store platform number to stand on
            int numPlatform = 26;

            //When stood on, collision occurred
            if (curWorld.GetPlatforms()[numPlatform].RecCollision(playerRec))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draw player on screen with correct animations and images
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="spriteBatch">Draw images</param>
        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;

            //Display player
            if (playerJump.isAnimating)
            {
                //Display jumping animation
                switch (dir)
                {
                    case "LEFT":
                        playerJump.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                        break;
                    case "RIGHT":
                        playerJump.Draw(spriteBatch, Color.White, Animation.FLIP_HORIZONTAL);
                        break;
                }
            }
            else if (playerDuck.isAnimating)
            {
                //Display ducking animation
                switch (dir)
                {
                    case "LEFT":
                        playerDuck.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                        break;
                    case "RIGHT":
                        playerDuck.Draw(spriteBatch, Color.White, Animation.FLIP_HORIZONTAL);
                        break;
                }
            }
            else if (playerRun.isAnimating)
            {
                //Display running animation
                switch (dir)
                {
                    case "LEFT":
                        playerRun.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                        break;
                    case "RIGHT":
                        playerRun.Draw(spriteBatch, Color.White, Animation.FLIP_HORIZONTAL);
                        break;
                }
            }
            else
            {
                //Display idle images
                switch (dir)
                {
                    case "LEFT":
                        spriteBatch.Draw(playerIdleLeft, playerRec, Color.White);
                        break;
                    case "RIGHT":
                        spriteBatch.Draw(playerIdleRight, playerRec, Color.White);
                        break;
                }
            }
        }
    }
}
