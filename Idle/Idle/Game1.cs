using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Idle
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    public static int screenWidth = 1280;
    public static int screenHeight = 720;

    private SpriteFont font;
    private float accumulatedGold = 0;
    private int yourGold = 0;
    private float t;

    private string lastGametime;
    private DateTime currentGameTime;
    private TimeSpan ts;
    private string directoryName = @"GameSaves\";

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
      // TODO: Add your initialization logic here
      IsMouseVisible = true;
      graphics.PreferredBackBufferWidth = screenWidth;
      graphics.PreferredBackBufferHeight = screenHeight;

      var lines = new List<string>();
      using (StreamReader sr = new StreamReader(@"C:\Nexdox\TraningApps\Idle\Idle\bin\Windows\x86\Debug\GameSaves\saving.txt"))
      {

        while (!sr.EndOfStream)
        {
          lines.Add(sr.ReadLine());
        }
      }
      
      lastGametime = lines[0];
      currentGameTime = DateTime.Now;

      ts = currentGameTime.Subtract(DateTime.Parse(lastGametime));

      accumulatedGold += int.Parse(lines[1]);

      accumulatedGold += 1 * (int)ts.TotalSeconds;

      yourGold = int.Parse(lines[2]);

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

      // TODO: use this.Content to load your game content here
      font = Content.Load<SpriteFont>("Font");
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
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
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      {

        using (StreamWriter sw = new StreamWriter(@"C:\Nexdox\TraningApps\Idle\Idle\bin\Windows\x86\Debug\GameSaves\saving.txt", false))
        {
          sw.WriteLine(DateTime.Now.ToString());
          sw.WriteLine(t);
          sw.WriteLine(yourGold);
        }

        Exit();
      }

      var mouse = Mouse.GetState();



      accumulatedGold += 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

      t = (float)Math.Truncate(accumulatedGold);

      if (mouse.LeftButton == ButtonState.Pressed)
      {
        yourGold += (int)t;
        accumulatedGold = 0;
      }

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      spriteBatch.Begin();
      spriteBatch.DrawString(font, $"Accumulated Gold: {t}", new Vector2(50, 50), Color.Black);
      spriteBatch.DrawString(font, $"Your Gold: {yourGold}", new Vector2(600, 50), Color.Black);
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
