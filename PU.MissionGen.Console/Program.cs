using System;
using System.Collections.Generic;
using System.Linq;
using PU.MissionGen.Core;
using PU.MissionGen.Core.Data;
using PU.MissionGen.Core.GeometryFill;
using PU.MissionGen.Core.GeometryGen;
using PU.MissionGen.Core.HumanShip;

namespace PU.MissionGen.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var tilesets = new AbstractTileset[]
            {
                new HumanShipTileset(),
                new GeometryFillTileset()
            };
            
            ShowOptions(tilesets);
        }

        private static void ShowOptions(AbstractTileset[] tilesets)
        {
            System.Console.WriteLine($"=========================");
            System.Console.WriteLine($"Choose a Tileset");
            for (var i = 0; i < tilesets.Length; i++)
            {
                System.Console.WriteLine($"{i}: {tilesets[i].Name}");
            }
            var input = System.Console.ReadKey().KeyChar.ToString();

            if(int.TryParse(input, out var choice)
                && choice >= 0
                && choice < tilesets.Length)
            {
                System.Console.WriteLine();
                System.Console.WriteLine($"Seed? (blank for random)");
                input = System.Console.ReadLine();

                if(string.IsNullOrWhiteSpace(input))
                {
                    input = new Random().Next().ToString();
                }

                VisualizerMenu(tilesets, tilesets[choice], input.GetHashCode());
            }

            ShowOptions(tilesets);
        }

        private static void VisualizerMenu(AbstractTileset[] tilesets, AbstractTileset chosenTileset, int seed)
        {
            var spec = new ShipGeometryGenerator().BuildShipSpec(seed);

            var mission = chosenTileset.Generate(spec);

            PrintOrthoTop(mission);
            PrintCommands();
            
            var inputKey = ConsoleKey.A;
            while (inputKey != ConsoleKey.Escape)
            {
                inputKey = System.Console.ReadKey().Key;

                if(inputKey == ConsoleKey.R)
                {
                    spec = new ShipGeometryGenerator().BuildShipSpec(new Random().Next());
                    mission = chosenTileset.Generate(spec);

                    PrintOrthoTop(mission);
                    PrintCommands();
                }
                if (inputKey == ConsoleKey.F)
                {
                    PrintOrthoFront(mission);
                    PrintCommands();
                }
                if (inputKey == ConsoleKey.T)
                {
                    PrintOrthoTop(mission);
                    PrintCommands();
                }
                if (inputKey == ConsoleKey.S)
                {
                    PrintOrthoSide(mission);
                    PrintCommands();
                }
                if (inputKey == ConsoleKey.D)
                {
                    PrintDeckPlans(mission);
                    PrintCommands();
                }
            }
            ShowOptions(tilesets);
        }

        private static void PrintCommands()
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"[F]ront View    [S]ide View    [T]op View    [D]eck Plans");
            System.Console.WriteLine($"[R]egeneate");
            System.Console.WriteLine($"ESC For Menu");
        }

        private static void PrintOrthoTop(Mission mission)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"=========================");
            System.Console.WriteLine();
            System.Console.WriteLine($"Type: {mission.Role} Length: {mission.Length * 3}m Volume: {mission.Volume}");
            System.Console.WriteLine($"View: Ortho Top");
            System.Console.WriteLine();

            var heightMap = new int[mission.Map.GetLength(0), mission.Map.GetLength(1)];

            var maxHeight = 0;
            var minHeight = int.MaxValue;

            for (var y = 0; y < mission.Map.GetLength(1); y++)
            {
                for (var x = 0; x < mission.Map.GetLength(0); x++)
                {
                    var found = false;
                    for (var z = mission.Map.GetLength(2) - 1; z >= 0 && !found; z--)
                    {
                        heightMap[x, y] = mission.Map.GetLength(2) - z;

                        if (mission.Map[x, y, z].Type != TileType.Void)
                        {
                            found = true;
                            
                            if (z != 0 && z > maxHeight)
                            {
                                maxHeight = z;
                            }
                            if (z != 0 && z < minHeight)
                            {
                                minHeight = z;
                            }
                        }
                    }
                }
            }

            PrintHeightMap(heightMap, maxHeight, minHeight);
        }

        private static void PrintOrthoSide(Mission mission)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"=========================");
            System.Console.WriteLine();
            System.Console.WriteLine($"Type: {mission.Role} Length: {mission.Length * 3}m Volume: {mission.Volume}");
            System.Console.WriteLine($"View: Ortho Side");
            System.Console.WriteLine();

            var heightMap = new int[mission.Map.GetLength(1), mission.Map.GetLength(2)];

            var maxHeight = 0;
            var minHeight = int.MaxValue;

            for (var y = 0; y < mission.Map.GetLength(1); y++)
            {
                for (var z = 0; z < mission.Map.GetLength(2); z++)
                {
                    var found = false;
                    for (var x = mission.Map.GetLength(0) - 1; x >= 0 && !found; x--)
                    {
                        heightMap[mission.Map.GetLength(1) - y - 1, z] = mission.Map.GetLength(0) - x;

                        if (mission.Map[x, y, z].Type != TileType.Void)
                        {
                            found = true;

                            if (x != 0 && x > maxHeight)
                            {
                                maxHeight = x;
                            }
                            if (x != 0 && x < minHeight)
                            {
                                minHeight = x;
                            }
                        }
                    }
                }
            }

            PrintHeightMap(heightMap, maxHeight, minHeight);
        }

        private static void PrintOrthoFront(Mission mission)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"=========================");
            System.Console.WriteLine();
            System.Console.WriteLine($"Type: {mission.Role} Length: {mission.Length * 3}m Volume: {mission.Volume}");
            System.Console.WriteLine($"View: Ortho Front");
            System.Console.WriteLine();

            var heightMap = new int[mission.Map.GetLength(0), mission.Map.GetLength(2)];

            var maxHeight = 0;
            var minHeight = int.MaxValue;

            for (var z = 0; z < mission.Map.GetLength(2); z++)
            {
                for (var x = 0; x < mission.Map.GetLength(0); x++)
                {
                    var found = false;
                    for (var y = 0; y < mission.Map.GetLength(1) && !found; y++)
                    {
                        heightMap[x, z] = y;

                        if (mission.Map[x, y, z].Type != TileType.Void)
                        {
                            found = true;

                            if (y != 0 && y > maxHeight)
                            {
                                maxHeight = y;
                            }
                            if (y != 0 && y < minHeight)
                            {
                                minHeight = y;
                            }
                        }
                    }
                }
            }

            PrintHeightMap(heightMap, maxHeight, minHeight);
        }

        private static void PrintHeightMap(int[,] heightMap, int maxHeight, int minHeight)
        {
            var bracket0 = (maxHeight - minHeight) * 0;
            var bracket1 = (maxHeight - minHeight) * 0.25;
            var bracket2 = (maxHeight - minHeight) * 0.50;
            var bracket3 = (maxHeight - minHeight) * 0.75;

            for (var y = 0; y < heightMap.GetLength(1); y++)
            {
                for (var x = 0; x < heightMap.GetLength(0); x++)
                {
                    if (heightMap[x, y] > maxHeight)
                    {
                        System.Console.Write(' ');
                    }
                    else if (heightMap[x, y] >= bracket3)
                    {
                        System.Console.Write('░');
                    }
                    else if (heightMap[x, y] >= bracket2)
                    {
                        System.Console.Write('▒');
                    }
                    else if (heightMap[x, y] >= bracket1)
                    {
                        System.Console.Write('▓');
                    }
                    else if (heightMap[x, y] >= bracket0)
                    {
                        System.Console.Write('█');
                    }
                }
                System.Console.WriteLine();
            }
        }

        private static void PrintDeckPlans(Mission mission)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"=========================");
            System.Console.WriteLine();
            System.Console.WriteLine($"Type: {mission.Role} Length: {mission.Length * 3}m Volume: {mission.Volume}");
            System.Console.WriteLine($"View: Deck Plan");
            System.Console.WriteLine();


            var decks = Enumerable.Range(0, mission.Map.GetLength(2)).ToArray();

            var decksPerRow = 230 / (mission.Map.GetLength(0) + 2);
            var deckBatches = new List<int[]>();

            for (var i = 0; i < decks.Length; i += decksPerRow)
            {
                deckBatches.Add(decks.Skip(i).Take(decksPerRow).ToArray());
            }

            foreach (var deckBatch in deckBatches)
            {
                for (var y = 0; y < mission.Map.GetLength(1); y++)
                {
                    foreach(var deck in deckBatch)
                    {
                        for (var x = 0; x < mission.Map.GetLength(0); x++)
                        {
                            System.Console.Write(DisplayFor(mission.Map[x, y, deck]));
                        }

                        System.Console.Write("  ");
                    }
                    System.Console.WriteLine();
                }

                System.Console.WriteLine();
                System.Console.WriteLine();
            }
        }

        private static string DisplayFor(Tile tile)
        {
            switch(tile.Type)
            {
                case TileType.Void: return " ";
                case TileType.HallVertical: return "│";
                case TileType.HallHorizontal: return "─";
                case TileType.HallFourway: return "┼";
                case TileType.HallTVerticalEast: return "├";
                case TileType.HallTVerticalWest: return "┤";
                case TileType.HallTHorizontalNorth: return "┴";
                case TileType.HallTHorizontalSouth: return "┬";
                case TileType.HallCornerEastToNorth: return "└";
                case TileType.HallCornerEastToSouth: return "┌";
                case TileType.HallCornerWestToNorth: return "┘";
                case TileType.HallCornerWestToSouth: return "┐";
                case TileType.Hull: return "▒";
                case TileType.EngineRoom: return "E";
                case TileType.HangarRoom: return "H";
                case TileType.ReactorRoom: return "R";
                case TileType.SecondaryWepRoom: return "w";
                case TileType.PrimaryWepRoom: return "W";
                case TileType.MiscRoom: return "░";
                case TileType.AirlockRoom: return "A";
                default: throw new Exception("Unknown tile type: " + tile.Type);
            }

            //─┌│┐└┘├┤┬┴┼▒ └┌┐┘

            return " ";
        }
    }
}
