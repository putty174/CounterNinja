using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CounterNinja;

namespace CounterNinja
{
    public class MenuOptionScreen : GameScreen
    {
        private CounterNinjaGame game;
        private const float NumEntries = 9;
        private List<MenuOptionEntry> menuEntries = new List<MenuOptionEntry>();
        private string menuTitle;
        private Vector2 titlePosition;
        private Vector2 titleOrigin;
        private int selectedEntry;
        private float menuBorderTop;
        private float menuBorderBottom;
        private float menuBorderMargin;
        private float menuOffset;
        private float maxOffset;

        private Texture2D texScrollButton;
        private Texture2D texSlider;

        private MenuButton scrollUp;
        private MenuButton scrollDown;
        private MenuButton scrollSlider;
        private bool scrollLock;

        private CounterNinjaScreen screen;

        public MenuOptionScreen(string menuTitle, CounterNinjaGame game)
        {
            this.game = game;
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.7);
            TransitionOffTime = TimeSpan.FromSeconds(0.7);
            HasCursor = true;
        }

        public MenuOptionScreen(string menuTitle, CounterNinjaScreen screen)
        {
            this.screen = screen;
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.7);
            TransitionOffTime = TimeSpan.FromSeconds(0.7);
            HasCursor = true;
        }

        public void AddMenuItem(string name, EntryOptionType type, GameScreen screen)
        {
            MenuOptionEntry entry = new MenuOptionEntry(this, name, type, screen);
            menuEntries.Add(entry);
        }

        public void AddMenuItem(string name, int sWidth, int sHeight)
        {
            MenuOptionEntry entry = new MenuOptionEntry(this, name, EntryOptionType.Resolution, null, sWidth, sHeight);
            menuEntries.Add(entry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont font = ScreenManager.Fonts.MenuSpriteFont;

            texScrollButton = ScreenManager.Content.Load<Texture2D>("Common/arrow");
            texSlider = ScreenManager.Content.Load<Texture2D>("Common/slider");

            float scrollBarPos = viewport.Width / 2f;
            for (int i = 0; i < menuEntries.Count; ++i)
            {
                menuEntries[i].Initialize();
                scrollBarPos = Math.Min(scrollBarPos,
                                         (viewport.Width - menuEntries[i].GetWidth()) / 2f);
            }
            scrollBarPos -= texScrollButton.Width + 2f;

            titleOrigin = font.MeasureString(menuTitle) / 2f;
            titlePosition = new Vector2(viewport.Width / 2f, font.MeasureString("M").Y / 2f + 10f);

            menuBorderMargin = font.MeasureString("M").Y * 0.8f;
            menuBorderTop = (viewport.Height - menuBorderMargin * (NumEntries - 1)) / 2f;
            menuBorderBottom = (viewport.Height + menuBorderMargin * (NumEntries - 1)) / 2f;

            menuOffset = 0f;
            maxOffset = Math.Max(0f, (menuEntries.Count - NumEntries) * menuBorderMargin);

            scrollUp = new MenuButton(texScrollButton, false,
                                       new Vector2(scrollBarPos, menuBorderTop - texScrollButton.Height), this);
            scrollDown = new MenuButton(texScrollButton, true,
                                         new Vector2(scrollBarPos, menuBorderBottom + texScrollButton.Height), this);
            scrollSlider = new MenuButton(texSlider, false, new Vector2(scrollBarPos, menuBorderTop), this);

            scrollLock = false;
        }

        
        // Returns the index of the menu entry at the position of the given mouse state.
        // Index of menu entry if valid, -1 otherwise
        private int GetMenuEntryAt(Vector2 position)
        {
            int index = 0;
            foreach (MenuOptionEntry entry in menuEntries)
            {
                float width = entry.GetWidth();
                float height = entry.GetHeight();
                Rectangle rect = new Rectangle((int)(entry.Position.X - width / 2f),
                                               (int)(entry.Position.Y - height / 2f),
                                               (int)width, (int)height);
                if (rect.Contains((int)position.X, (int)position.Y) && entry.Alpha > 0.1f)
                {
                    return index;
                }
                ++index;
            }
            return -1;
        }

        
        // Responds to user input, changing the selected entry and accepting
        // or cancelling the menu.
        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            // Mouse or touch on a menu item
            int hoverIndex = GetMenuEntryAt(input.Cursor);
            if (hoverIndex > -1 && menuEntries[hoverIndex].IsSelectable() && !scrollLock)
            {
                selectedEntry = hoverIndex;
            }
            else
            {
                selectedEntry = -1;
            }

            scrollSlider.Hover = false;
            if (input.IsCursorValid)
            {
                scrollUp.Collide(input.Cursor);
                scrollDown.Collide(input.Cursor);
                scrollSlider.Collide(input.Cursor);
            }
            else
            {
                scrollUp.Hover = false;
                scrollDown.Hover = false;
                scrollLock = false;
            }

            // Accept or cancel the menu? 
            if (input.IsMenuSelect() && selectedEntry != -1)
            {
                if (menuEntries[selectedEntry].IsExitScreen())
                {
                    ExitScreen();
                }
                else if (menuEntries[selectedEntry].IsExitToMainMenu())
                {
                    ExitScreen();
                    screen.ExitScreen();
                }
                else if (menuEntries[selectedEntry].IsOptionAntiAlias())
                {
                    game.ToggleAntiAlias();
                    if (game.IsAntiAlias())
                        menuEntries[selectedEntry].Text = "Anti-Alias: Enabled";
                    else
                        menuEntries[selectedEntry].Text = "Anti-Alias: Disabled";
                }
                else if (menuEntries[selectedEntry].IsOptionFullScreen())
                {
                    game.ToggleFullScreen();
                    if (game.IsFullScreen())
                        menuEntries[selectedEntry].Text = "Full Screen: Enabled";
                    else
                        menuEntries[selectedEntry].Text = "Full Screen: Disabled";
                }
                else if (menuEntries[selectedEntry].IsOptionVSync())
                {
                    game.ToggleVSync();
                    if (game.IsVSync())
                        menuEntries[selectedEntry].Text = "VSync: Enabled";
                    else
                        menuEntries[selectedEntry].Text = "VSync: Disabled";
                }
                else if (menuEntries[selectedEntry].IsOptionResolution())
                {
                    game.ChangeResolution(menuEntries[selectedEntry].GetResolutionWidth(),
                        menuEntries[selectedEntry].GetResolutionHeight());
                }
                else if (menuEntries[selectedEntry].Screen != null)
                {
                    ScreenManager.AddScreen(menuEntries[selectedEntry].Screen);
                    if (menuEntries[selectedEntry].Screen is IDemoScreen)
                    {
                        ScreenManager.AddScreen(
                            new MessageBoxScreen((menuEntries[selectedEntry].Screen as IDemoScreen).GetDetails()));
                    }
                }
            }
            else if (input.IsMenuCancel())
            {
                ScreenManager.Game.Exit();
            }

            if (input.IsMenuPressed())
            {
                if (scrollUp.Hover)
                {
                    menuOffset = Math.Max(menuOffset - 200f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0f);
                    scrollLock = false;
                }
                if (scrollDown.Hover)
                {
                    menuOffset = Math.Min(menuOffset + 200f * (float)gameTime.ElapsedGameTime.TotalSeconds, maxOffset);
                    scrollLock = false;
                }
                if (scrollSlider.Hover)
                {
                    scrollLock = true;
                }
            }
            if (input.IsMenuReleased())
            {
                scrollLock = false;
            }
            if (scrollLock)
            {
                scrollSlider.Hover = true;
                menuOffset = Math.Max(Math.Min(((input.Cursor.Y - menuBorderTop) / (menuBorderBottom - menuBorderTop)) * maxOffset, maxOffset), 0f);
            }
        }

        
        // Allows the screen the chance to position the menu entries. By default
        // all menu entries are lined up in a vertical list, centered on the screen.
        
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = Vector2.Zero;
            position.Y = menuBorderTop - menuOffset;

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; ++i)
            {
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2f;
                if (ScreenState == ScreenState.TransitionOn)
                {
                    position.X -= transitionOffset * 256;
                }
                else
                {
                    position.X += transitionOffset * 256;
                }

                // set the entry's position
                menuEntries[i].Position = position;

                if (position.Y < menuBorderTop)
                {
                    menuEntries[i].Alpha = 1f -
                                            Math.Min(menuBorderTop - position.Y, menuBorderMargin) / menuBorderMargin;
                }
                else if (position.Y > menuBorderBottom)
                {
                    menuEntries[i].Alpha = 1f -
                                            Math.Min(position.Y - menuBorderBottom, menuBorderMargin) /
                                            menuBorderMargin;
                }
                else
                    menuEntries[i].Alpha = 1f;

                // move down for the next entry the size of this entry
                position.Y += menuEntries[i].GetHeight();
            }
            Vector2 scrollPos = scrollSlider.Position;
            scrollPos.Y = MathHelper.Lerp(menuBorderTop, menuBorderBottom, menuOffset / maxOffset);
            scrollSlider.Position = scrollPos;
        }

        // Updates the menu.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; ++i)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntries[i].Update(isSelected, gameTime);
            }

            scrollUp.Update(gameTime);
            scrollDown.Update(gameTime);
            scrollSlider.Update(gameTime);
        }

        
        // Draws the menu.
        
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Fonts.MenuSpriteFont;

            spriteBatch.Begin();
            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; ++i)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntries[i].Draw();
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            Vector2 transitionOffset = new Vector2(0f, (float)Math.Pow(TransitionPosition, 2) * 100f);

            spriteBatch.DrawString(font, menuTitle, titlePosition - transitionOffset + Vector2.One * 2f, Color.Black, 0,
                                   titleOrigin, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, menuTitle, titlePosition - transitionOffset, new Color(255, 210, 0), 0,
                                   titleOrigin, 1f, SpriteEffects.None, 0);
            scrollUp.Draw();
            scrollSlider.Draw();
            scrollDown.Draw();
            spriteBatch.End();
        }
    }
}