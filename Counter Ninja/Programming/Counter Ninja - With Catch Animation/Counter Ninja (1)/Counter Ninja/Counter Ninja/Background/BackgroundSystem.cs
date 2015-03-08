using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CounterNinja;

namespace CounterNinja.Background
{
    public class BackgroundSystem
    {
        private List<BodyBackground> listBackground;
        private CounterNinjaScreen screen;
        private List<Body> listBody;
        private List<BodylessBackground> listBodylessBackground;
        private int posX, posY;

        public BackgroundSystem(CounterNinjaScreen screen)
        {
            listBackground = new List<BodyBackground>();
            listBody = new List<Body>();
            listBodylessBackground = new List<BodylessBackground>();
            this.screen = screen;
            posX = 0; posY = 0;
        }

        #region Accessor methods

        public World World
        {
            get { return screen.World; }
        }

        public List<Body> BodyList
        {
            get { return listBody; }
        }

        #endregion

        /// <summary>
        /// Unloads all content of this system.
        /// </summary>
        public void UnloadContent()
        {
            listBackground.Clear();
            listBodylessBackground.Clear();
            listBody.Clear();
        }

        /// <summary>
        /// Adds a BodyBackground to be managed by this system.
        /// </summary>
        /// <param name="background"></param>
        public void AddBackground(BodyBackground background)
        {
            background.Body.Position = ConvertUnits.ToSimUnits(new Vector2(posX, posY));
            listBackground.Add(background);
            
            posX += background.Texture.Width;
        }

        /// <summary>
        /// Adds a bodyless background to the system.
        /// Backgrounds are broken into several smaller files, 
        /// so this method allows loading them  up automatically.
        /// You can use Photoshop's Split to export backgrounds in smaller 
        /// files. It's faster to render than one big map.
        /// </summary>
        /// <param name="bgFile">filename for the texture</param>
        /// <param name="start">starting number to autoload multiple files</param>
        /// <param name="end">ending number of file</param>
        /// <param name="perColumn">How many there are per columns</param>
        public void AddBodylessBackgroundSet(string bgFile, int start, int end, int perColumn)
        {
            int posX = 0, posY = 0;
            for (int i = start; i <= end; i++)
            {
                string tempFile = bgFile + "_Background_";
                if (i < 10)
                    tempFile += "0";
                tempFile += i;
                Vector2 position = ConvertUnits.ToSimUnits(new Vector2(posX, posY));
                BodylessBackground background = new BodylessBackground(screen, tempFile, position);
                tempFile = bgFile + "_Overlay_";
                if (i < 10)
                    tempFile += "0";
                tempFile += i;
                try
                {
                    background.AddOverlay(tempFile, 0.9f);
                }
                catch
                {
                }
                posX += background.Background.Width;
                if (i % perColumn == 0)
                {
                    posY += background.Background.Height;
                    posX = 0;
                }
                listBodylessBackground.Add(background);
            }
        }

        /// <summary>
        /// Adds a basic background. It's add to (0, 0)
        /// </summary>
        /// <param name="file"></param>
        public void AddBodylessBackground(string file)
        {
            Vector2 position = ConvertUnits.ToSimUnits(new Vector2(0, 0));
            BodylessBackground background = new BodylessBackground(screen, file, position);
            listBodylessBackground.Add(background);
        }

        /// <summary>
        /// Adds the Body for the background.
        /// This is the body for the objects to fall on.
        /// We use scaled backgrounds to create the body because
        /// it saves space, it's faster, and it's easier.
        /// </summary>
        /// <param name="bodyFile">file of the body</param>
        /// <param name="scale">scale amount. This should multiply with the body's picture size to match the real size in pixels.</param>
        public void AddScaledBackgroundBody(string bodyFile, float scale)
        {
            Texture2D background = screen.ScreenManager.Content.Load<Texture2D>(bodyFile);
            uint[] dataA = new uint[background.Width * background.Height];
            background.GetData(dataA);
            Vertices textureVerticesA = PolygonTools.CreatePolygon(dataA, background.Width, false);
            //originA = Vector2.Zero;//-centroidA;

            textureVerticesA = SimplifyTools.DouglasPeuckerSimplify(textureVerticesA, 0);
            List<Vertices> listA = BayazitDecomposer.ConvexPartition(textureVerticesA);
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(scale));
            foreach (Vertices vertices in listA)
                vertices.Scale(ref vertScale);

            Body backgroundBody = BodyFactory.CreateCompoundPolygon(World, listA, 1f, BodyType.Static);
            backgroundBody.Friction = 15f;
            backgroundBody.Position = Vector2.Zero;
            listBody.Add(backgroundBody);
        }

        /// <summary>
        /// Useless for now.
        /// </summary>
        /// <param name="baseBodyFile"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="perColumn"></param>
        /// <param name="defaultWidth"></param>
        /// <param name="defaultHeight"></param>
        public void AddBackgroundBodySet(string baseBodyFile, int start, int end, int perColumn, int defaultWidth, int defaultHeight)
        {
            int posX = 0, posY = 0;
            for (int i = start; i <= end; i++)
            {
                string tempFile = baseBodyFile + "_Body_";
                if (i < 10)
                    tempFile += "0";
                tempFile += i;

                Texture2D background = screen.ScreenManager.Content.Load<Texture2D>(tempFile);
                uint[] dataA = new uint[background.Width * background.Height];
                background.GetData(dataA);
                if (!HasNoVertices(dataA))
                {
                    Vertices textureVerticesA = PolygonTools.CreatePolygon(dataA, background.Width, false);
                    //Vector2 centroidA = -textureVerticesA.GetCentroid();
                    //textureVerticesA.Translate(ref centroidA);
                    textureVerticesA = SimplifyTools.ReduceByDistance(textureVerticesA, 4f);
                    List<Vertices> listA = BayazitDecomposer.ConvexPartition(textureVerticesA);
                    Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));
                    foreach (Vertices vertices in listA)
                        vertices.Scale(ref vertScale);
                    Body backgroundBody = BodyFactory.CreateCompoundPolygon(World, listA, 1f, BodyType.Static);
                    backgroundBody.Friction = 5f;
                    backgroundBody.Position = ConvertUnits.ToSimUnits(new Vector2(posX,
                         posY));
                    listBody.Add(backgroundBody);
                }
                //posX += background.Width;
                //if (background.Width == 0)
                    posX += defaultWidth;
                //System.Console.WriteLine(posX);
                if (i % perColumn == 0)
                {
                    //posY += background.Height;
                    //if (background.Height == 0)
                        posY += defaultHeight;
                    posX = 0;
                }
            }            
        }

        /// <summary>
        /// Helper method, don't bother about it.
        /// </summary>
        public void IncrementYPosition()
        {
            posY += listBackground[0].Texture.Height;
            posX = 0;
        }

        /// <summary>
        /// Helper method to check if the data has no vertices.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool HasNoVertices(uint[] data)
        {
            foreach (uint num in data)
                if (num != 0)
                    return false;
            return true;
        }

        /// <summary>
        /// Draws all the backgrounds handled
        /// </summary>
        public void Draw()
        {
            foreach (BodyBackground background in listBackground)
                background.Draw();
            foreach (BodylessBackground background in listBodylessBackground)
                background.Draw();
        }
    }
}
