using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Runtime.Serialization.Formatters.Binary;

namespace MajorProject
{
    [Serializable]
    public class World
    {
        [XmlIgnore]
        [NonSerialized]
        Random rand;
        [XmlIgnore]
        [NonSerialized]
        Texture2D WorldImageTexture;

        public List<GameScreen.RoomType> roomTypes;

        public int LevelIndex;

        /// <summary>
        /// values for setting the size of standard tiles
        /// display debug - 50 * 50 px, 1200px display = 24 tiles lengthways, 14 tiles downwards
        /// </summary>

        public const int tilePixelWidth = 50;
        public const int tilePixelHeight = 50;

        /*
        [XmlIgnore]
        ContentManager content;
        */

        [NonSerialized]
        ResourcePack Resources;

        public Vector2 entry;
        public Vector2 entryIndex;

        string[] TextureFileNames = new string[6] {
            "",
            "Ground_Texture",
            "Wall_Texture",
            "Door_Texture",
            "Entrance_Texture",
            "Exit_Texture"
        };


        enum directions
        {
            up,
            right,
            down,
            left
        }

        Dictionary<directions, Vector2> DirectionVector2Map;

        directions[] DirectionArray = new directions[4] { directions.up, directions.right, directions.down, directions.left };

        int roomPlacementAttempts = 20; // try to place a room randomly 20 times before using a systematic search (otherwise could be O(n!) )

        public enum cellType
        {
            empty,    // 0
            floor,    // 1
            wall,     // 2
            door,     // 3
            entrance, // 4
            exit      // 5
        }

        int[,] generation_VisitedLayer; // marks which cells have been visited
        // 0 - unvisited
        // 1 - visited by room
        // 2 - visited by corridor
        public int[,] Map;

        public List<Vector2> generation_RoomPositions;
        public List<Vector2> generation_RoomDimensions;

        public List<Vector2> generation_RoomIndexPositions;
        public List<Vector2> generation_RoomIndexDimensions;

        public List<Vector2> generation_DoorPositions;

        //List<Vector2> MazeGenerationPath;
        public List<Vector2> CorridorEndings;

        #region Parameters
        // values to be assigned before generation
        // default values

        public int level_cell_width;
        public int level_cell_height;

        // internally used
        public int width;
        public int height;

        // how many rooms there are
        public int room_count;

        // how many cells each room takes up
        public Vector2 room_min_cell;
        public Vector2 room_max_cell;

        #endregion Parameters


        public World()
        {


            DirectionVector2Map = new Dictionary<directions, Vector2>();
            DirectionVector2Map.Add(directions.up, new Vector2(0, -1));
            DirectionVector2Map.Add(directions.right, new Vector2(1, 0));
            DirectionVector2Map.Add(directions.down, new Vector2(0, 1));
            DirectionVector2Map.Add(directions.left, new Vector2(-1, 0));

            roomTypes = new List<GameScreen.RoomType>();
        }

        public void GenerateNewRandom()
        {
            rand = new Random();
        }

        // converts a cell position to the position on the grid
        public int ToCellIndex(int cell)
        {
            return cell * 2 + 1;
        }

        void GenerateCorridors()
        {
            // randomly select entry point
            entry = new Vector2(rand.Next(0, level_cell_width), rand.Next(0, level_cell_height));
            while (generation_VisitedLayer[(int)entry.Y, (int)entry.X] != 0) // find unvisited
            {
                entry = new Vector2(rand.Next(0, level_cell_width), rand.Next(0, level_cell_height));
            }

            entryIndex = (entry * 2) + new Vector2(1, 1);

            Map[(int)entryIndex.Y, (int)entryIndex.X] = (int)cellType.entrance;

            CorridorEndings = new List<Vector2>();

            // start the recursive function
            AdvanceMaze(entry);

            /*
            if (level_cell_width * level_cell_height > 500)
            {
                MazeGenerationPath = new List<Vector2>();
                // use a path-recording maze generation to avoid stack overflow
            } else
            {
                // use recursion - faster, but higher likelihood(?) of crashing
                AdvanceMaze(entry);
            }
            */
        }

        void AdvanceMaze(Vector2 cellPosition)
        {

            generation_VisitedLayer[(int)cellPosition.Y, (int)cellPosition.X] = 2; // visited by corridor

            bool isEnd = true;
            while (true)
            {
                // loops through directions
                int possibleDirectionCount = 0;
                List<Vector2> possibleDirections = new List<Vector2>();
                if (cellPosition.X > 0) if (generation_VisitedLayer[(int)cellPosition.Y, (int)cellPosition.X - 1] == 0)
                    {
                        possibleDirections.Add(DirectionVector2Map[directions.left]);
                        possibleDirectionCount++;
                    }
                if (cellPosition.X < level_cell_width - 1) if (generation_VisitedLayer[(int)cellPosition.Y, (int)cellPosition.X + 1] == 0)
                    {
                        possibleDirections.Add(DirectionVector2Map[directions.right]);
                        possibleDirectionCount++;
                    }
                if (cellPosition.Y > 0) if (generation_VisitedLayer[(int)cellPosition.Y - 1, (int)cellPosition.X] == 0)
                    {
                        possibleDirections.Add(DirectionVector2Map[directions.up]);
                        possibleDirectionCount++;
                    }
                if (cellPosition.Y < level_cell_height - 1) if (generation_VisitedLayer[(int)cellPosition.Y + 1, (int)cellPosition.X] == 0)
                    {
                        possibleDirections.Add(DirectionVector2Map[directions.down]);
                        possibleDirectionCount++;
                    }
                if (possibleDirectionCount == 0 && isEnd)
                {
                    CorridorEndings.Add(cellPosition);
                }
                if (possibleDirectionCount == 0) break;

                isEnd = false;

                int index = (int)(rand.NextDouble() * possibleDirectionCount);
                Map[ToCellIndex((int)cellPosition.Y) + (int)possibleDirections[index].Y, ToCellIndex((int)cellPosition.X) + (int)possibleDirections[index].X] = 1;

                // continue recursion
                AdvanceMaze(new Vector2(cellPosition.X + possibleDirections[index].X, cellPosition.Y + possibleDirections[index].Y));
                possibleDirections.RemoveAt(index);
            }


        }

        void GenerateRooms()
        {
            // places rooms of random sizes at random positions

            generation_RoomPositions = new List<Vector2>();
            generation_RoomDimensions = new List<Vector2>();
            generation_RoomIndexPositions = new List<Vector2>();
            generation_RoomIndexDimensions = new List<Vector2>();

            for (int i = 0; i < room_count; i++)
            {
                bool placed = false;
                int attempts = 0;

                Vector2 dimensions = new Vector2();
                Vector2 position = new Vector2();
                while (!placed)
                {
                    //DisplayVisited();
                    dimensions = new Vector2(rand.Next((int)room_min_cell.X, (int)room_max_cell.Y), rand.Next((int)room_min_cell.X, (int)room_max_cell.Y));
                    position = new Vector2(rand.Next(0, level_cell_width - (int)dimensions.X), rand.Next(0, level_cell_height - (int)dimensions.Y));
                    bool intersectsRoom = false;

                    // checks for intersection for each space (yes I know this is inefficient, just humour me)
                    for (int j = 0; j < dimensions.Y; j++)
                    {
                        for (int k = 0; k < dimensions.X; k++)
                        {
                            if (generation_VisitedLayer[j + (int)position.Y, k + (int)position.X] == 1) intersectsRoom = true;
                            if (intersectsRoom) break; // break from loop, so you don't iterate more than you have to
                        }
                        if (intersectsRoom) break;
                    }

                    if (intersectsRoom)
                    {
                        attempts++;
                        if (roomPlacementAttempts == attempts) break;
                    }
                    else
                    {
                        generation_RoomPositions.Add(position);
                        generation_RoomDimensions.Add(dimensions);
                        placed = true;
                        for (int j = 0; j < dimensions.Y; j++)
                        {
                            for (int k = 0; k < dimensions.X; k++)
                            {
                                generation_VisitedLayer[(int)position.Y + j, (int)position.X + k] = 1;
                            }
                        }
                    }
                }
                if (!placed)
                {
                    break; // no more rooms can fit in the level, so break loop
                }
                else
                {
                    // break walls in the map
                    Vector2 indexPosition = new Vector2(ToCellIndex((int)position.X), ToCellIndex((int)position.Y));
                    Vector2 indexDimensions = new Vector2(dimensions.X * 2 - 1, dimensions.Y * 2 - 1);
                    generation_RoomIndexPositions.Add(indexPosition);
                    generation_RoomIndexDimensions.Add(indexDimensions);
                    for (int j = 0; j < indexDimensions.Y; j++)
                    {
                        for (int k = 0; k < indexDimensions.X; k++)
                        {
                            Map[j + (int)indexPosition.Y, k + (int)indexPosition.X] = 1;
                        }
                    }



                }
            }
        }

        public void GenerateWorld()
        {
            if (rand == null) rand = new Random();

            width = ToCellIndex(level_cell_width);
            height = ToCellIndex(level_cell_height);

            generation_VisitedLayer = new int[level_cell_height, level_cell_width];

            // create new int[,] instance
            Map = new int[height, width];

            // generate base cell structure to edit
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Map[i, j] = (i % 2 == 0) ? 2 : (j % 2 == 0) ? 2 : 1; // generates a cell pattern
                }
            }

            // generate room positions

            GenerateRooms();

            // generate the maze-like corridors

            GenerateCorridors();

            // poke holes in room walls (now that paths to corridor walls can be avoided)
            GenerateDoors();
        }

        public void GenerateDoors()
        {

            generation_DoorPositions = new List<Vector2>();

            for (int roomNumber = 0; roomNumber < generation_RoomIndexPositions.Count; roomNumber++)
            {
                int wallNumber = 0;
                while (wallNumber < 4)
                {

                    //DisplayWorld();

                    // get corner position
                    // 0 - top-left
                    // 1 - top-right
                    // 2 - bottom-right
                    // 3 - bottom-left

                    Vector2 cornerIndexPosition = new Vector2();

                    bool CanMakeDoor = true;

                    switch (wallNumber)
                    {
                        case 0:
                            cornerIndexPosition = generation_RoomIndexPositions[roomNumber];
                            CanMakeDoor = cornerIndexPosition.Y != 1;
                            break;
                        case 1:
                            cornerIndexPosition = generation_RoomIndexPositions[roomNumber] + new Vector2(generation_RoomIndexDimensions[roomNumber].X, 0);
                            CanMakeDoor = cornerIndexPosition.X != width - 2;
                            break;
                        case 2:
                            cornerIndexPosition = generation_RoomIndexPositions[roomNumber] + new Vector2(generation_RoomIndexDimensions[roomNumber].X, generation_RoomIndexDimensions[roomNumber].Y);
                            CanMakeDoor = cornerIndexPosition.Y != width - 2;
                            break;
                        case 3:
                            cornerIndexPosition = generation_RoomIndexPositions[roomNumber] + new Vector2(0, generation_RoomIndexDimensions[roomNumber].Y);
                            CanMakeDoor = cornerIndexPosition.X != 1;
                            break;
                    }

                    //check that door can be created (doesn't lead out of map)

                    if (CanMakeDoor)
                    {

                        bool spaceAvailable = false;
                        Vector2 DoorPosition = new Vector2();
                        while (!spaceAvailable)
                        {
                            // select random distance along wall
                            int DistanceAlongWall = rand.Next(0, ((wallNumber % 2 == 0) ? (int)generation_RoomIndexDimensions[roomNumber].X : (int)generation_RoomIndexDimensions[roomNumber].Y) - 1); // -1 since you don't want to place a door in the far corner

                            // check that space perpendicular to wall isn't a wall, else reselect

                            // find position on wall

                            Vector2 DirectionVector = DirectionVector2Map[DirectionArray[wallNumber]];

                            DoorPosition = cornerIndexPosition + DirectionVector2Map[DirectionArray[(wallNumber + 1) % 4]] * DistanceAlongWall;
                            if (wallNumber == 0 || wallNumber == 3) DoorPosition += DirectionVector;

                            spaceAvailable = Map[(int)DoorPosition.Y + (int)DirectionVector.Y, (int)DoorPosition.X + (int)DirectionVector.X] != 2;
                        }

                        // break space in wall
                        Map[(int)DoorPosition.Y, (int)DoorPosition.X] = 3;

                        // mark position in door array
                        generation_DoorPositions.Add(new Vector2((DoorPosition.X - 1) / 2, (DoorPosition.Y - 1) / 2));


                    }

                    wallNumber++;

                }
            }
        }

        public void GenerateWorld(int seed)
        {
            // sets the random seed
            rand = new Random(seed);
            DirectionVector2Map = new Dictionary<directions, Vector2>();
            DirectionVector2Map.Add(directions.up, new Vector2(0, -1));
            DirectionVector2Map.Add(directions.right, new Vector2(1, 0));
            DirectionVector2Map.Add(directions.down, new Vector2(0, 1));
            DirectionVector2Map.Add(directions.left, new Vector2(-1, 0));
            GenerateWorld();
        }

        /// <summary>
        /// debug functiona
        /// </summary>
        
        /*
        public void DisplayWorld()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write((Map[i, j] == 1) ? "  " : (Map[i, j] == 2) ? "# " : "| ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void DisplayVisited()
        {
            for (int i = 0; i < level_cell_height; i++)
            {
                for (int j = 0; j < level_cell_width; j++)
                {
                    Console.Write((generation_VisitedLayer[i, j] == 0) ? " " : "#");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        */

        public void LoadContent(ref ResourcePack resources)
        {
            Resources = resources;
        }

        public void RenderTexture()
        {
            Vector2 dimensions = Vector2.Zero;



            // put textures in a dictionary to be easily called upon to be displayed

            width = level_cell_width * 2 + 1;
            height = level_cell_width * 2 + 1;

            // finds the dimensions of the image from the dimensions of the map
            dimensions.X = tilePixelWidth * width;

            dimensions.Y = tilePixelHeight * height;


            // creates new render target to draw to
            RenderTarget2D renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice,
                (int)dimensions.X, (int)dimensions.Y);
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();

            // for every tile in the map, draw the respective image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int originPointX = tilePixelWidth * x;
                    int originPointY = tilePixelHeight * y;
                    Vector2 OriginPoint = new Vector2(tilePixelWidth * x, tilePixelHeight * y);

                    if (x == entryIndex.X && y == entryIndex.Y)
                    {
                        ScreenManager.Instance.SpriteBatch.Draw(Resources.TexturePack[TextureFileNames[Map[y, x]]], OriginPoint);
                    }
                    else if (Map[y, x] > 0) // can't draw nothing
                        ScreenManager.Instance.SpriteBatch.Draw(Resources.TexturePack[TextureFileNames[Map[y, x]]], OriginPoint);

                }
            }

            ScreenManager.Instance.SpriteBatch.End();


            WorldImageTexture = renderTarget;


            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null); // reset rendertarget to default - main display rendertarget

            //renderTarget.Dispose();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draws the world to the rendertarget
            spriteBatch.Draw(WorldImageTexture, destinationRectangle: new Rectangle(0, 0, tilePixelWidth * width, tilePixelHeight * height));
        }

        public void UnloadContent()
        {
            // unloads the world texture
            WorldImageTexture.Dispose();

            Resources = null;
        }








    }
}
