using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CounterNinja
{
    public enum EntryOptionType
    {
        Screen,
        Separator,
        ExitScreen,
        FullScreen,
        AntiAlias,
        VSync,
        Resolution,
        ExitToMainMenu
    }

    // Helper class represents a single entry in a MenuScreen. By default this
    // just draws the entry text string, but it can be customized to display menu
    // entries in different ways. This also provides an event that will be raised
    // when the menu entry is selected.
    public sealed class MenuOptionEntry
    {
        private float alpha;
        private Vector2 baseOrigin;

        private float height;
        private MenuOptionScreen menu;


        // The position at which the entry is drawn. This is set by the MenuScreen
        // each frame in Update.
        private Vector2 position;

        private float scale;
        private GameScreen screen;

        // Tracks a fading selection effect on the entry.
        // The entries transition out of the selection effect when they are deselected.
        private float selectionFade;


        // The text rendered for this entry.
        private string text;

        private EntryOptionType type;
        private float width;

        //Variables for screen resolution option
        private int sWidth, sHeight;

        // Constructs a new menu entry with the specified text.
        public MenuOptionEntry(MenuOptionScreen menu, string text, EntryOptionType type, GameScreen screen)
        {
            this.text = text;
            this.screen = screen;
            this.type = type;
            this.menu = menu;
            scale = 0.9f;
            alpha = 1.0f;
        }

        public MenuOptionEntry(MenuOptionScreen menu, string text, EntryOptionType type, GameScreen screen, int sWidth, int sHeight)
            : this(menu, text, type, screen)
        {
            this.sWidth = sWidth;
            this.sHeight = sHeight;
        }

        #region Menu get/set methods

        public int GetResolutionWidth()
        {
            return sWidth;
        }

        public int GetResolutionHeight()
        {
            return sHeight;
        }

        // Gets or sets the text of this menu entry.
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        // Gets or sets the position at which to draw this menu entry.
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public GameScreen Screen
        {
            get { return screen; }
        }

        #endregion

        public void Initialize()
        {
            SpriteFont font = menu.ScreenManager.Fonts.MenuSpriteFont;

            baseOrigin = new Vector2(font.MeasureString(Text).X, font.MeasureString("M").Y) * 0.5f;

            width = font.MeasureString(Text).X * 0.8f;
            height = font.MeasureString("M").Y * 0.8f;
        }

        #region Menu type boolean

        public bool IsOptionAntiAlias()
        {
            return type == EntryOptionType.AntiAlias;
        }

        public bool IsOptionFullScreen()
        {
            return type == EntryOptionType.FullScreen;
        }

        public bool IsExitToMainMenu()
        {
            return type == EntryOptionType.ExitToMainMenu;
        }

        public bool IsOptionVSync()
        {
            return type == EntryOptionType.VSync;
        }

        public bool IsOptionResolution()
        {
            return type == EntryOptionType.Resolution;
        }

        public bool IsExitScreen()
        {
            return type == EntryOptionType.ExitScreen;
        }

        public bool IsSelectable()
        {
            return type != EntryOptionType.Separator;
        }

        #endregion

        // Updates the menu entry.
        public void Update(Boolean isSelected, GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            if (type != EntryOptionType.Separator)
            {
                float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
                if (isSelected)
                    selectionFade = Math.Min(selectionFade + fadeSpeed, 1f);
                else
                    selectionFade = Math.Max(selectionFade - fadeSpeed, 0f);
                scale = 0.7f + 0.1f * selectionFade;
            }
        }

        // Draws the menu entry. This can be overridden to customize the appearance
        public void Draw()
        {
            SpriteFont font = menu.ScreenManager.Fonts.MenuSpriteFont;
            SpriteBatch batch = menu.ScreenManager.SpriteBatch;

            Color color;
            if (type == EntryOptionType.Separator)
                color = Color.DarkOrange;
            else  // Draw the selected entry in yellow, otherwise white
                color = Color.Lerp(Color.White, new Color(255, 210, 0), selectionFade);
            color *= alpha;

            // Draw text, centered on the middle of each line.
            batch.DrawString(font, text, position - baseOrigin * scale + Vector2.One,
                              Color.DarkSlateGray * alpha * alpha, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            batch.DrawString(font, text, position - baseOrigin * scale, color, 0, Vector2.Zero, scale,
                              SpriteEffects.None, 0);
        }

        // Queries how much space this menu entry requires.
        public int GetHeight()
        {
            return (int)height;
        }

        // Queries how wide the entry is, used for centering on the screen.
        public int GetWidth()
        {
            return (int)width;
        }
    }
}