using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using c_engine.Content;
using System;
using System.Diagnostics;
using System.Threading;

namespace c_engine
{
    public class Game1 : Game
    {
        Texture2D wolfTexture;
        private Events Events;
        private SceneTree SceneTree;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private DeltaTime DeltaTime;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Events = new Events();
            SceneTree = new SceneTree();
            DeltaTime = new DeltaTime();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            wolfTexture = Content.Load<Texture2D>("wolfy");
            c_engine.Content.Object new_object = new c_engine.Content.Object();

            SceneChild new_child = new SceneChild();
            SceneChild new_child2 = new SceneChild();
            new_child2.set_child_name("Arm");

            new_child.add_child(new_child2);

            SceneChild by_name = new_child.get_child_by_name("Arm");

            Debug.WriteLine(by_name.child_name);

            Rect new_rect = new Rect();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            DeltaTime.restart();

            Events.update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            long deltaTime = DeltaTime.get();
            Debug.WriteLine(deltaTime);
        }
    }
}