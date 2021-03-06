﻿using PU.MissionGen.Core.Data;
using PU.MissionGen.Core.GeometryGen.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PU.MissionGen.Core.GeometryFill
{
    public class GeometryFillTileset : AbstractTileset
    {
        public static readonly int BLOCK_SIZE = 3;

        public override string Name => "Geometry Fill Ship";
        
        public override Mission Generate(ShipSpec shipSpec)
        {
            var random = new Random(shipSpec.Seed);
            var minX = shipSpec.ShipGeometry.Min(hs => hs.Center.X - hs.Width / 2) - 1;
            var maxX = shipSpec.ShipGeometry.Max(hs => hs.Center.X + hs.Width / 2) + 1;
            var minY = shipSpec.ShipGeometry.Min(hs => hs.Center.Y - hs.Length / 2) - 1;
            var maxY = shipSpec.ShipGeometry.Max(hs => hs.Center.Y + hs.Length / 2) + 1;
            var minZ = shipSpec.ShipGeometry.Min(hs => hs.Center.Z - hs.Height / 2) - 1;
            var maxZ = shipSpec.ShipGeometry.Max(hs => hs.Center.Z + hs.Height / 2) + 1;

            var blocksHigh = (int)((maxX - minX) / BLOCK_SIZE);
            var blocksWide = (int)((maxY - minY) / BLOCK_SIZE);
            
            var blockGrid = new RoomType[blocksHigh, blocksWide];

            var finalGrid = new RoomType[(int)(maxX - minX), (int)(maxY - minY), (int)(maxZ - minZ)];

            RenderToGrid(shipSpec.ShipGeometry, finalGrid);
            
            var placedRooms = new List<PlacedRoom>();

            var allCoordinates = Enumerable.Range(0, finalGrid.GetLength(0))
                .SelectMany(x => Enumerable.Range(0, finalGrid.GetLength(1))
                    .SelectMany(y => Enumerable.Range(0, finalGrid.GetLength(2))
                        .Select(z => Tuple.Create(x, y, z))))
                .ToArray();


            PlaceEngines(shipSpec, finalGrid, placedRooms);

            //PlaceHangars(role, hullVolumeBlocks, sizeInBlocks, midline, blockGrid, random, placedRooms);

            //PlacePrimaries(role, hullVolumeBlocks, sizeInBlocks, midline, blockGrid, random, placedRooms);

            //PlaceReactors(role, hullVolumeBlocks, sizeInBlocks, midline, blockGrid, random, placedRooms);

            //PlaceSecondaries(role, hullVolumeBlocks, sizeInBlocks, midline, blockGrid, random, placedRooms);

            //PlaceAirlocks(role, hullVolumeBlocks, sizeInBlocks, midline, blockGrid, random, placedRooms);

            PlaceFillerRooms(shipSpec, finalGrid, placedRooms, random, allCoordinates);
            
            //var finalWidth = blockGrid.GetLength(0) * BLOCK_SIZE + 1;
            //var finalLength = blockGrid.GetLength(1) * BLOCK_SIZE + 1;

            //var finalGrid = new RoomType[finalWidth, finalLength];
            //var hallGrid = new bool[finalWidth, finalLength];
            //var toRoomLookup = new PlacedRoom[finalWidth, finalLength];

            //for (var bx = 0; bx < blockGrid.GetLength(0); bx += 1)
            //{
            //    for (var by = 0; by < blockGrid.GetLength(1); by += 1)
            //    {
            //        for (var i = 0; i <= BLOCK_SIZE; i++)
            //        {
            //            for (var j = 0; j <= BLOCK_SIZE; j++)
            //            {
            //                var x = bx * BLOCK_SIZE + i;
            //                var y = by * BLOCK_SIZE + j;

            //                if (blockGrid[bx, by] != RoomType.Void && x < finalWidth && y < finalLength)
            //                {
            //                    finalGrid[x, y, z] = RoomType.Hull;
            //                }
            //            }
            //        }
            //    }
            //}

            //foreach(var room in placedRooms)
            //{
            //    var startX = room.StartBlockX * BLOCK_SIZE + 1;
            //    var startY = room.StartBlockY * BLOCK_SIZE + 1;
            //    var endX = (room.StartBlockX + room.BlocksWide) * BLOCK_SIZE;
            //    var endY = (room.StartBlockY + room.BlocksHigh) * BLOCK_SIZE;

            //    if(room.OpenToAft)
            //    {
            //        endY += 1;
            //    }
            //    if (room.OpenToBow)
            //    {
            //        startY -= 1;
            //    }
            //    if (room.OpenToStbd)
            //    {
            //        endX += 1;
            //    }
            //    if (room.OpenToPort)
            //    {
            //        startX -= 1;
            //    }

            //    for (var x = startX; x < endX && x < finalWidth; x++)
            //    {
            //        for(var y = startY; y < endY && y < finalLength; y++)
            //        {
            //            finalGrid[x, y, z] = room.RoomType;

            //            toRoomLookup[x, y] = room;
            //        }
            //    }
            //}

            //PlaceHalls(size, finalGrid, toRoomLookup, random, hallGrid);

            //PlaceDoorways(size, finalGrid, toRoomLookup, random, hallGrid);

            var mission = new Mission
            {
                Map = new Tile[finalGrid.GetLength(0), finalGrid.GetLength(1), finalGrid.GetLength(2)],
                //Volume = hullVolumeBlocks * BLOCK_SIZE * BLOCK_SIZE * 9,
                Volume = 4,
                Role = shipSpec.RoleName,
                Length = finalGrid.GetLength(1) * 3,
            };
            
            for (var y = 0; y < mission.Map.GetLength(1); y++)
            {
                for (var x = 0; x < mission.Map.GetLength(0); x++)
                {
                    for (var z = 0; z < mission.Map.GetLength(2); z++)
                    {
                        var tile = new Tile();

                        mission.Map[x, y, z] = tile;

                        if (finalGrid[x, y, z] == RoomType.Engines)
                        {
                            tile.Type = TileType.EngineRoom;
                        }
                        else if (finalGrid[x, y, z] == RoomType.Hangar)
                        {
                            tile.Type = TileType.HangarRoom;
                        }
                        else if (finalGrid[x, y, z] == RoomType.Hull)
                        {
                            tile.Type = TileType.Hull;
                        }
                        else if (finalGrid[x, y, z] == RoomType.Reactor)
                        {
                            tile.Type = TileType.ReactorRoom;
                        }
                        else if (finalGrid[x, y, z] == RoomType.SecondaryWep)
                        {
                            tile.Type = TileType.SecondaryWepRoom;
                        }
                        else if (finalGrid[x, y, z] == RoomType.PrimaryWep)
                        {
                            tile.Type = TileType.PrimaryWepRoom;
                        }
                        else if (finalGrid[x, y, z] == RoomType.Hallway)
                        {
                            tile.Type = TileType.HallFourway;
                            //var mask = 0;

                            //if (GetOrFalse(hallGrid, x - 1, y))
                            //{
                            //    mask += 1000;
                            //}
                            //if (GetOrFalse(hallGrid, x + 1, y))
                            //{
                            //    mask += 100;
                            //}
                            //if (GetOrFalse(hallGrid, x, y - 1))
                            //{
                            //    mask += 10;
                            //}

                            //if (GetOrFalse(hallGrid, x, y + 1))
                            //{
                            //    mask += 1;
                            //}

                            //switch(mask)
                            //{
                            //    case 0001: tile.Type = TileType.HallVertical; break;
                            //    case 0010: tile.Type = TileType.HallVertical; break;
                            //    case 0011: tile.Type = TileType.HallVertical; break;
                            //    case 0100: tile.Type = TileType.HallHorizontal; break;
                            //    case 0101: tile.Type = TileType.HallCornerEastToSouth; break;
                            //    case 0110: tile.Type = TileType.HallCornerEastToNorth; break;
                            //    case 0111: tile.Type = TileType.HallTVerticalEast; break;
                            //    case 1000: tile.Type = TileType.HallHorizontal; break;
                            //    case 1001: tile.Type = TileType.HallCornerWestToSouth; break;
                            //    case 1010: tile.Type = TileType.HallCornerWestToNorth; break;
                            //    case 1011: tile.Type = TileType.HallTVerticalWest; break;
                            //    case 1100: tile.Type = TileType.HallHorizontal; break;
                            //    case 1101: tile.Type = TileType.HallTHorizontalSouth; break;
                            //    case 1110: tile.Type = TileType.HallTHorizontalNorth; break;
                            //    case 1111: tile.Type = TileType.HallFourway; break;
                            //    default: tile.Type = TileType.HallFourway; break;
                            //}
                        }
                        else if (finalGrid[x, y, z] == RoomType.MiscRoom)
                        {
                            tile.Type = TileType.MiscRoom;
                        }
                        else if (finalGrid[x, y, z] == RoomType.Airlock)
                        {
                            tile.Type = TileType.AirlockRoom;
                        }
                    }
                }
            }
            
            return mission;
        }

        

        private void RenderToGrid(
            List<HullShape> hullShapes,
            RoomType[,,] finalGrid)
        {
            var width = finalGrid.GetLength(0);
            var length = finalGrid.GetLength(1);
            var height = finalGrid.GetLength(2);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < length; y++)
                {
                    for (var z = 0; z < height; z++)
                    {
                        var testX = x - width / 2f;
                        var testY = y - length / 2f;
                        var testZ = z - height / 2f;

                        if (hullShapes.Any(hs => hs.TestHull(testX, testY, testZ)))
                        {
                            finalGrid[x, y, z] = RoomType.Hull;
                        }
                        else
                        {
                            finalGrid[x, y, z] = RoomType.Void;
                        }
                    }
                }
            }
        }

        private void PlaceEngines(
            ShipSpec shipSpec, 
            RoomType[,,] finalGrid, 
            List<PlacedRoom> placedRooms)
        {
            var width = finalGrid.GetLength(0);
            var length = finalGrid.GetLength(1);
            var height = finalGrid.GetLength(2);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < length; y++)
                {
                    for (var z = 0; z < height; z++)
                    {
                        var testX = x - width / 2f;
                        var testY = y - length / 2f;
                        var testZ = z - height / 2f;

                        var enclosingShapes = shipSpec.ShipGeometry
                            .Where(hs => hs.TestHull(testX, testY, testZ))
                            .ToArray();

                        if (enclosingShapes.Any(hs => hs.Fittings.Contains(ShipFittings.Engines)))
                        {
                            finalGrid[x, y, z] = RoomType.Engines;
                        }
                    }
                }
            }
        }

        //private void PlaceEngines(
        //    ShipRole role,
        //    int hullVolume, 
        //    int size, 
        //    int width, 
        //    int midline,
        //    RoomType[,] blockGrid,
        //    Random random,
        //    List<PlacedRoom> placedRooms)
        //{
        //    var enginesNeeded = Math.Max(hullVolume / role.VolPerEngine, role.MinEngine);

        //    var possibilities = new List<RoomPossibility[]>();

        //    // centerline engines
        //    for (var roomW = 1; roomW <= width; roomW += 2)
        //    {
        //        for (var roomH = Math.Max(1, roomW /2); roomH <= roomW * 2; roomH += 1)
        //        {
        //            if (roomW * roomH < enginesNeeded * 2)
        //            {
        //                possibilities.Add(new[] {
        //                    new RoomPossibility
        //                    {
        //                        UlX = midline - roomW / 2,
        //                        UlY = size - roomH,
        //                        Width = roomW,
        //                        Height = roomH,
        //                        OpenToAft = true
        //                    }});
        //            }
        //        }
        //    }

        //    // Side engines
        //    for (var x = 0; x < midline; x++)
        //    {
        //        for (var y = 0; y < size; y++)
        //        {
        //            for (var roomW = 1; roomW <= midline - x; roomW += 1)
        //            {
        //                for (var roomH = Math.Max(1, roomW / 2); roomH <= roomW * 2; roomH += 1)
        //                {
        //                    if (roomW * roomH < enginesNeeded * 4)
        //                    {
        //                        if (ClearToAft(x, y, roomW, roomH, blockGrid))
        //                        {
        //                            possibilities.Add(new[] {
        //                                new RoomPossibility
        //                                {
        //                                    UlX = x,
        //                                    UlY = y,
        //                                    Width = roomW,
        //                                    Height = roomH,
        //                                    OpenToAft = true
        //                                },
        //                                new RoomPossibility
        //                                {
        //                                    UlX = size - x - roomW,
        //                                    UlY = y,
        //                                    Width = roomW,
        //                                    Height = roomH,
        //                                    OpenToAft = true
        //                                }
        //                            });
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    possibilities = possibilities
        //        .Where(p => CanFit(p, blockGrid))
        //        .ToList();

        //    var permutations = FindRoomPermutations(possibilities, enginesNeeded, enginesNeeded * 2, size);

        //    if (!permutations.Any())
        //    {
        //        permutations = FindRoomPermutations(possibilities, 1, int.MaxValue, size);
        //    }

        //    if (permutations.Any())
        //    {
        //        var permutation = random.Choose(permutations);

        //        PlaceRoom(permutation, blockGrid, RoomType.Engines, placedRooms);
        //    }
        //}

        //private void PlaceHangars(
        //    ShipRole role,
        //    int hullVolume,
        //    int size,
        //    int midline,
        //    RoomType[,] blockGrid,
        //    Random random,
        //    List<PlacedRoom> placedRooms)
        //{
        //    var hangarsNeeded = Math.Max(hullVolume / role.VolPerHangar, role.MinHangar);

        //    var possibilities = new List<RoomPossibility[]>();

        //    // Front hangars
        //    for (var roomW = 3; roomW <= size / 2; roomW += 2)
        //    {
        //        for (var roomH = Math.Max(3, roomW / 2); roomH <= roomW * 2; roomH += 1)
        //        {
        //            if (roomW * roomH < hangarsNeeded * 2)
        //            {
        //                possibilities.Add(new[] {
        //                    new RoomPossibility
        //                    {
        //                        UlX = midline - roomW / 2,
        //                        UlY = 0,
        //                        Width = roomW,
        //                        Height = roomH,
        //                        OpenToBow = true
        //                    }});
        //            }
        //        }
        //    }

        //    // Rear hangars
        //    for (var roomW = 3; roomW <= size / 2; roomW += 2)
        //    {
        //        for (var roomH = Math.Max(3, roomW / 2); roomH <= roomW * 2; roomH += 1)
        //        {
        //            if (roomW * roomH < hangarsNeeded * 2)
        //            {
        //                possibilities.Add(new[] {
        //                    new RoomPossibility
        //                    {
        //                        UlX = midline - roomW / 2,
        //                        UlY = size - roomH,
        //                        Width = roomW,
        //                        Height = roomH,
        //                        OpenToAft = true
        //                    }});
        //            }
        //        }
        //    }

        //    // Side hangars
        //    for (var x = 0; x < midline; x++)
        //    {
        //        for (var y = 0; y < size; y++)
        //        {
        //            for (var roomW = 2; roomW <= midline - x; roomW += 1)
        //            {
        //                for (var roomH = 2; roomH < roomW * 2; roomH += 1)
        //                {
        //                    if (roomW * roomH < hangarsNeeded * 4)
        //                    {
        //                        if (ClearToPort(x, y, roomW, roomH, blockGrid))
        //                        {
        //                            possibilities.Add(new[] {
        //                            new RoomPossibility
        //                            {
        //                                UlX = x,
        //                                UlY = y,
        //                                Width = roomW,
        //                                Height = roomH,
        //                                OpenToPort = true
        //                            },
        //                            new RoomPossibility
        //                            {
        //                                UlX = size - x - roomW,
        //                                UlY = y,
        //                                Width = roomW,
        //                                Height = roomH,
        //                                OpenToStbd = true
        //                            }
        //                        });
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    possibilities = possibilities
        //        .Where(p => CanFit(p, blockGrid))
        //        .ToList();

        //    var permutations = FindRoomPermutations(possibilities, hangarsNeeded, hangarsNeeded * 2, size);

        //    if (permutations.Any())
        //    {
        //        var permutation = random.Choose(permutations);

        //        PlaceRoom(permutation, blockGrid, RoomType.Hangar, placedRooms);
        //    }
        //}

        //private void PlaceReactors(
        //    ShipRole role,
        //    int hullVolume,
        //    int size,
        //    int midline,
        //    RoomType[,] blockGrid,
        //    Random random,
        //    List<PlacedRoom> placedRooms)
        //{
        //    var reactorsNeeded = Math.Max(hullVolume / role.VolPerReactor, role.MinReactor);

        //    var possibilities = new List<RoomPossibility[]>();

        //    // Centerline reactors
        //    for (var y = 0; y < size; y += 1)
        //    {
        //        for (var roomW = 1; roomW <= 5; roomW += 2)
        //        {
        //            for (var roomH = Math.Max(roomW - 1, 1); roomH < roomW + 1; roomH += 2)
        //            {
        //                if (roomW * roomH < reactorsNeeded * 2)
        //                {
        //                    possibilities.Add(new[] {
        //                        new RoomPossibility
        //                        {
        //                            UlX = midline - roomW / 2,
        //                            UlY = y,
        //                            Width = roomW,
        //                            Height = roomH
        //                        }});
        //                }
        //            }
        //        }
        //    }

        //    // Side reactors
        //    for (var x = 0; x < midline; x++)
        //    {
        //        for (var y = 0; y < size; y++)
        //        {
        //            for (var roomW = 1; roomW <= 4; roomW += 1)
        //            {
        //                for (var roomH = Math.Max(roomW - 1, 1); roomH < roomW + 1; roomH += 1)
        //                {
        //                    if (roomW * roomH < reactorsNeeded * 4)
        //                    {
        //                        possibilities.Add(new[] {
        //                        new RoomPossibility
        //                            {
        //                                UlX = x,
        //                                UlY = y,
        //                                Width = roomW,
        //                                Height = roomH
        //                            },
        //                            new RoomPossibility
        //                            {
        //                                UlX = size - x - roomW,
        //                                UlY = y,
        //                                Width = roomW,
        //                                Height = roomH
        //                            }
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    possibilities = possibilities
        //        .Where(p => CanFit(p, blockGrid))
        //        .ToList();

        //    var permutations = FindRoomPermutations(possibilities, reactorsNeeded, reactorsNeeded * 2, size);

        //    if (permutations.Any())
        //    {
        //        var minSize = permutations.Min(p => p.Count());
        //        var smallOptions = permutations.Where(p => p.Count() == minSize).ToArray();

        //        var permutation = random.Choose(smallOptions);

        //        PlaceRoom(permutation, blockGrid, RoomType.Reactor, placedRooms);
        //    }
        //}

        //private void PlaceSecondaries(
        //    ShipRole role,
        //    int hullVolume,
        //    int size,
        //    int midline,
        //    RoomType[,] blockGrid,
        //    Random random,
        //    List<PlacedRoom> placedRooms)
        //{
        //    var secondariesNeeded = Math.Max(hullVolume / role.VolPerSecondary, role.MinSecondaries);

        //    var possibilities = new List<RoomPossibility[]>();

        //    // Side weapons
        //    for (var x = 0; x < midline; x++)
        //    {
        //        for (var y = 0; y < size; y++)
        //        {
        //            possibilities.Add(new[] {
        //                new RoomPossibility
        //                    {
        //                        UlX = x,
        //                        UlY = y,
        //                        Width = 1,
        //                        Height = 1
        //                    },
        //                new RoomPossibility
        //                    {
        //                        UlX = size - x - 1,
        //                        UlY = y,
        //                        Width = 1,
        //                        Height = 1
        //                    }
        //                });
        //        }
        //    }

        //    possibilities = possibilities
        //        .Where(p => CanFit(p, blockGrid))
        //        .ToList();

        //    var permutations = FindRoomPermutations(possibilities, secondariesNeeded, secondariesNeeded * 2, size);

        //    if (permutations.Any())
        //    {
        //        var minSize = permutations.Min(p => p.Count());
        //        var smallOptions = permutations.Where(p => p.Count() == minSize).ToArray();

        //        var permutation = random.Choose(smallOptions);

        //        PlaceRoom(permutation, blockGrid, RoomType.SecondaryWep, placedRooms);
        //    }
        //}

        //private void PlacePrimaries(
        //    ShipRole role,
        //    int hullVolume,
        //    int size,
        //    int midline,
        //    RoomType[,] blockGrid,
        //    Random random,
        //    List<PlacedRoom> placedRooms)
        //{
        //    var primariesNeeded = Math.Max(hullVolume / role.VolPerPrimary, role.MinPrimaries);

        //    var possibilities = new List<RoomPossibility[]>();

        //    // Centerline
        //    for (var y = 0; y < size; y += 1)
        //    {
        //        var roomW = 3;
        //        var roomH = 3;
        //        if (roomW * roomH < primariesNeeded * 2)
        //        {
        //            possibilities.Add(new[] {
        //                new RoomPossibility
        //                {
        //                    UlX = midline - roomW / 2,
        //                    UlY = y,
        //                    Width = roomW,
        //                    Height = roomH
        //                }});
        //        }
        //    }

        //    //// Side reactors
        //    //for (var x = 0; x < midline; x++)
        //    //{
        //    //    for (var y = 0; y < size; y++)
        //    //    {
        //    //        for (var roomW = 1; roomW <= 4; roomW += 1)
        //    //        {
        //    //            for (var roomH = Math.Max(roomW - 1, 1); roomH < roomW + 1; roomH += 1)
        //    //            {
        //    //                if (roomW * roomH < primariesNeeded * 4)
        //    //                {
        //    //                    possibilities.Add(new[] {
        //    //                    new RoomPossibility
        //    //                        {
        //    //                            UlX = x,
        //    //                            UlY = y,
        //    //                            Width = roomW,
        //    //                            Height = roomH
        //    //                        },
        //    //                        new RoomPossibility
        //    //                        {
        //    //                            UlX = size - x - roomW,
        //    //                            UlY = y,
        //    //                            Width = roomW,
        //    //                            Height = roomH
        //    //                        }
        //    //                    });
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    possibilities = possibilities
        //        .Where(p => CanFit(p, blockGrid))
        //        .ToList();

        //    var permutations = FindRoomPermutations(possibilities, primariesNeeded, primariesNeeded * 2, size);

        //    if (permutations.Any())
        //    {
        //        var minSize = permutations.Min(p => p.Count());
        //        var smallOptions = permutations.Where(p => p.Count() == minSize).ToArray();

        //        var permutation = random.Choose(smallOptions);

        //        PlaceRoom(permutation, blockGrid, RoomType.PrimaryWep, placedRooms);
        //    }
        //}

        //private void PlaceAirlocks(
        //    ShipRole role,
        //    int hullVolume,
        //    int size,
        //    int midline,
        //    RoomType[,] blockGrid,
        //    Random random,
        //    List<PlacedRoom> placedRooms)
        //{
        //    var possibilities = new List<RoomPossibility[]>();

        //    // Front Airlocks
        //    for(var x = 0; x < size; x++)
        //    {
        //        for(var y = 0; y < size; y++)
        //        {
        //            if (ClearToFore(x, y, 1, 1, blockGrid))
        //            {
        //                possibilities.Add(new[] {
        //                    new RoomPossibility
        //                    {
        //                        UlX = x,
        //                        UlY = y,
        //                        Width = 1,
        //                        Height = 1,
        //                        OpenToBow = true
        //                    }});
        //            }

        //            if (ClearToAft(x, y, 1, 1, blockGrid))
        //            {
        //                possibilities.Add(new[] {
        //                    new RoomPossibility
        //                    {
        //                        UlX = x,
        //                        UlY = y,
        //                        Width = 1,
        //                        Height = 1,
        //                        OpenToAft = true
        //                    }});
        //            }

        //            if (ClearToPort(x, y, 1, 1, blockGrid))
        //            {
        //                possibilities.Add(new[] {
        //                    new RoomPossibility
        //                    {
        //                        UlX = x,
        //                        UlY = y,
        //                        Width = 1,
        //                        Height = 1,
        //                        OpenToPort = true
        //                    }});
        //            }

        //            if (ClearToStbd(x, y, 1, 1, blockGrid))
        //            {
        //                possibilities.Add(new[] {
        //                    new RoomPossibility
        //                    {
        //                        UlX = x,
        //                        UlY = y,
        //                        Width = 1,
        //                        Height = 1,
        //                        OpenToStbd = true
        //                    }});
        //            }
        //        }
        //    }

        //    possibilities = possibilities
        //        .Where(p => CanFit(p, blockGrid))
        //        .ToList();

        //    random.Shuffle(possibilities);

        //    var permutations = FindRoomPermutations(possibilities, role.MinAirlock, role.MaxAirlock, size);

        //    if (permutations.Any())
        //    {
        //        var permutation = random.Choose(permutations);

        //        PlaceRoom(permutation, blockGrid, RoomType.Airlock, placedRooms);
        //    }
        //}

        private void PlaceFillerRooms(
            ShipSpec shipSpec,
            RoomType[,,] finalGrid, 
            List<PlacedRoom> placedRooms, 
            Random random, 
            IEnumerable<Tuple<int, int, int>> allCoordinates)
        {
            var traversal = random.Shuffle(allCoordinates
                .Where(t => finalGrid[t.Item1, t.Item2, t.Item3] != RoomType.Void))
                .ToArray();

            var volume = traversal.Length;

            var roomsToPlace = new List<RoomSpecification>();
            foreach(var room in shipSpec.RoomsToPlace)
            {
                var count = Math.Max(room.MinCount, volume / room.VolumePerCount);

                for(var i = 0; i <  count; i++)
                {
                    roomsToPlace.Add(room);
                }
            }

            roomsToPlace = random.Shuffle(roomsToPlace)
                .OrderBy(r => r.Priority)
                .ToList();

            foreach (var room in roomsToPlace)
            {
                var coordinate = random.Choose(traversal);
                //foreach (var coordinate in traversal)
                //{
                    if (CanFit(room, coordinate, finalGrid))
                    {
                        Place(room, coordinate, finalGrid);
                    }
                //}
            }
        }

        private bool CanFit(RoomSpecification room, Tuple<int, int, int> coordinate, RoomType[,,] finalGrid)
        {
            foreach(var box in room.RoomShapes)
            {
                var sx = coordinate.Item1 + box.Center.X;
                var sy = coordinate.Item2 + box.Center.Y;
                var sz = coordinate.Item3 + box.Center.Z;

                for(var i = -1; i < box.Width + 1; i++)
                {
                    for (var j = -1; j < box.Length + 1; j++)
                    {
                        for (var k = 0; k < box.Height; k++)
                        {
                            var x = (int)(sx + i);
                            var y = (int)(sy + j);
                            var z = (int)(sz + k);

                            if(x >= 0 && x < finalGrid.GetLength(0)
                                && y >= 0 && y < finalGrid.GetLength(1)
                                && z >= 0 && z < finalGrid.GetLength(2))
                            {
                                if(finalGrid[x,y,z] != RoomType.Hull)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        private void Place(RoomSpecification room, Tuple<int, int, int> coordinate, RoomType[,,] finalGrid)
        {
            foreach (var box in room.RoomShapes)
            {
                var sx = coordinate.Item1 + box.Center.X;
                var sy = coordinate.Item2 + box.Center.Y;
                var sz = coordinate.Item3 + box.Center.Z;

                for (var i = 0; i < box.Width; i++)
                {
                    for (var j = 0; j < box.Length; j++)
                    {
                        for (var k = 0; k < box.Height; k++)
                        {
                            var x = (int)(sx + i);
                            var y = (int)(sy + j);
                            var z = (int)(sz + k);

                            finalGrid[x, y, z] = room.RoomType;
                        }
                    }
                }
            }
        }
        
        private void PlaceFillerRooms(
            int size,
            RoomType[,] blockGrid,
            Random random,
            List<PlacedRoom> placedRooms)
        {
            var possibilities = new List<RoomPossibility[]>();

            // 1x1
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    possibilities.Add(new[]
                    {
                        new RoomPossibility
                        {
                            UlX = x,
                            UlY = y,
                            Width = 1,
                            Height = 1
                        }
                    });
                }
            }
            // 1x2
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    possibilities.Add(new[]
                    {
                        new RoomPossibility
                        {
                            UlX = x,
                            UlY = y,
                            Width = 1,
                            Height = 2
                        }
                    });
                }
            }
            // 2x1
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    possibilities.Add(new[]
                    {
                        new RoomPossibility
                        {
                            UlX = x,
                            UlY = y,
                            Width = 2,
                            Height = 1
                        }
                    });
                }
            }
            // 2x2
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    possibilities.Add(new[]
                    {
                        new RoomPossibility
                        {
                            UlX = x,
                            UlY = y,
                            Width = 2,
                            Height = 2
                        }
                    });
                }
            }
            // 1x3
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    possibilities.Add(new[]
                    {
                        new RoomPossibility
                        {
                            UlX = x,
                            UlY = y,
                            Width = 1,
                            Height = 3
                        }
                    });
                }
            }
            // 3x1
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    possibilities.Add(new[]
                    {
                        new RoomPossibility
                        {
                            UlX = x,
                            UlY = y,
                            Width = 3,
                            Height = 1
                        }
                    });
                }
            }
            // 2x3
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    possibilities.Add(new[]
                    {
                        new RoomPossibility
                        {
                            UlX = x,
                            UlY = y,
                            Width = 2,
                            Height = 3
                        }
                    });
                }
            }
            // 3x2
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    possibilities.Add(new[]
                    {
                        new RoomPossibility
                        {
                            UlX = x,
                            UlY = y,
                            Width = 3,
                            Height = 2
                        }
                    });
                }
            }

            possibilities = possibilities
                .Where(p => CanFit(p, blockGrid))
                .ToList();
            
            while(possibilities.Any())
            {
                PlaceRoom(random.Choose(possibilities), blockGrid, RoomType.MiscRoom, placedRooms);

                possibilities = possibilities
                    .Where(p => CanFit(p, blockGrid))
                    .ToList();
            }
        }

        //private void PlaceHalls(
        //    int size,
        //    RoomType[,] finalGrid,
        //    PlacedRoom[,] toRoomLookup,
        //    Random random,
        //    bool[,] hallGrid)
        //{
        //    var directions = new[]
        //    {
        //        Tuple.Create(1, 0),
        //        Tuple.Create(-1, 0),
        //        Tuple.Create(0, 1),
        //        Tuple.Create(0, -1),
        //    };

        //    Func<int,int,bool> inRange = (x, y) => x >= 0 && y >= 0 && x < size && y < size;

        //    for (var bx = 0; bx < size / BLOCK_SIZE; bx += 1)
        //    {
        //        for (var by = 0; by < size / BLOCK_SIZE; by += 1)
        //        {
        //            //finalGrid[bx * hallStep, by * hallStep] = RoomType.Hallway;

        //            foreach (var hallDirection in directions)
        //            {
        //                var isValid = true;
        //                var x = 0;
        //                var y = 0;

        //                for (var i = 0; i < BLOCK_SIZE; i++)
        //                {
        //                    foreach (var neighborDirection in directions)
        //                    {
        //                        x = bx * BLOCK_SIZE + i * hallDirection.Item1 + neighborDirection.Item1;
        //                        y = by * BLOCK_SIZE + i * hallDirection.Item2 + neighborDirection.Item2;

        //                        if (inRange(x, y))
        //                        {
        //                            isValid = isValid && finalGrid[x, y, z] != RoomType.Void;
        //                        }
        //                        else if(y < 0 || y > size)
        //                        {
        //                            isValid = false;
        //                        }
        //                    }

        //                    x = bx * BLOCK_SIZE + i * hallDirection.Item1;
        //                    y = by * BLOCK_SIZE + i * hallDirection.Item2;

        //                    isValid = isValid && inRange(x, y) && (finalGrid[x, y, z] == RoomType.Hull || finalGrid[x, y, z] == RoomType.Hallway);

        //                    if (isValid)
        //                    {
        //                        finalGrid[x, y, z] = RoomType.Hallway;
        //                        hallGrid[x, y] = true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private void PlaceDoorways(
            int size, 
            RoomType[,] finalGrid, 
            PlacedRoom[,] toRoomLookup, 
            Random random, 
            bool[,] hallGrid)
        {
            var directions = new[]
            {
                Tuple.Create(1, 0),
                Tuple.Create(-1, 0),
                Tuple.Create(0, 1),
                Tuple.Create(0, -1),
            };

            var hallPositions = new List<Tuple<int, int>>();
            for(var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    if(hallGrid[x, y])
                    {
                        hallPositions.Add(Tuple.Create(x, y));
                    }
                }
            }

            random.Shuffle(hallPositions);

            var connectedRooms = new List<PlacedRoom>();

            foreach(var hallPos in hallPositions)
            {
                foreach(var nDir in directions)
                {
                    var x = hallPos.Item1 + nDir.Item1;
                    var y = hallPos.Item2 + nDir.Item2;

                    if(x >= 0 && x < size && y >= 0 && y < size && toRoomLookup[x, y] != null)
                    {
                        var connectionCount = connectedRooms
                            .Where(r => r == toRoomLookup[x, y])
                            .Count();

                        var connectionChance = 1 - connectionCount / 3.0;

                        if (random.NextDouble() < connectionChance)
                        {
                            connectedRooms.Add(toRoomLookup[x, y]);

                            hallGrid[x, y] = true;
                        }
                    }
                }
            }
        }

        private List<RoomPossibility[]> FindRoomPermutations(
            List<RoomPossibility[]> possibilities, 
            int minEngines, 
            int maxEngines,
            int shipSize)
        {
            var permutations = new List<RoomPossibility[]>();

            for(var i = 0; i < possibilities.Count; i++)
            {
                var permutation = new List<RoomPossibility>();
                permutation.AddRange(possibilities[i]);
                var totalSize = permutation.Sum(r => r.Width * r.Height);

                if(totalSize > minEngines && totalSize < maxEngines)
                {
                    permutations.Add(permutation.ToArray());
                }

                for(var j = 0; j < possibilities.Count && totalSize < maxEngines; j++)
                {
                    if(i != j)
                    {
                        if(NoOverlap(permutation, possibilities[j], shipSize))
                        {
                            permutation.AddRange(possibilities[j]);
                            totalSize = permutation.Sum(r => r.Width * r.Height);
                        }
                    }

                    if (totalSize > minEngines && totalSize < maxEngines)
                    {
                        permutations.Add(permutation.ToArray());
                    }
                }
            }

            return permutations;
        }
        
        private bool NoOverlap(List<RoomPossibility> currentRooms, RoomPossibility[] newRooms, int size)
        {
            var occupied = new bool[size, size];
            foreach(var room in currentRooms)
            {
                for(var x = room.UlX; x < room.UlX + room.Width; x++)
                {
                    for(var y = room.UlY; y < room.UlY + room.Height; y++)
                    {
                        occupied[x, y] = true;
                    }
                }
            }

            foreach (var room in newRooms)
            {
                for (var x = room.UlX; x < room.UlX + room.Width; x++)
                {
                    for (var y = room.UlY; y < room.UlY + room.Height; y++)
                    {
                        if(occupied[x, y])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool ClearToAft(int startX, int startY, int width, int height, RoomType[,] rooms)
        {
            for(var x = startX; x < startX + width; x++)
            {
                for(var y = startY + height; y < rooms.GetLength(1); y++)
                {
                    if(rooms[x,y] != RoomType.Void)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ClearToFore(int startX, int startY, int width, int height, RoomType[,] rooms)
        {
            for (var x = startX; x < startX + width; x++)
            {
                for (var y = startY; y >= 0; y--)
                {
                    if (rooms[x, y] != RoomType.Void)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ClearToPort(int startX, int startY, int width, int height, RoomType[,] blockGrid)
        {
            for (var x = startX - 1; x >= 0; x--)
            {
                for (var y = startY; y < startY + height; y++)
                {
                    if (y < 0
                        || y >= blockGrid.GetLength(1)
                        || blockGrid[x, y] != RoomType.Void)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ClearToStbd(int startX, int startY, int width, int height, RoomType[,] blockGrid)
        {
            for (var x = startX + width; x < blockGrid.GetLength(0); x++)
            {
                for (var y = startY; y < startY + height; y++)
                {
                    if (y < 0
                        || y >= blockGrid.GetLength(1)
                        || blockGrid[x, y] != RoomType.Void)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CanFit(RoomPossibility[] roomGroup, RoomType[,] rooms)
        {
            foreach(var room in roomGroup)
            {
                for(var x = room.UlX; x < room.UlX + room.Width; x++)
                {
                    for (var y = room.UlY; y < room.UlY + room.Height; y++)
                    {
                        if(x < 0 || x >= rooms.GetLength(0)
                            || y < 0 || y >= rooms.GetLength(1)
                            || rooms[x,y] != RoomType.Hull)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void PlaceRoom(RoomPossibility[] roomGroup, RoomType[,] blockGrid, RoomType roomType, List<PlacedRoom> placedRooms)
        {
            foreach (var room in roomGroup)
            {
                for (var x = room.UlX; x < room.UlX + room.Width; x++)
                {
                    for (var y = room.UlY; y < room.UlY + room.Height; y++)
                    {
                        blockGrid[x, y] = roomType;
                    }
                }

                placedRooms.Add(
                    new PlacedRoom
                    {
                        StartBlockX = room.UlX,
                        StartBlockY = room.UlY,
                        BlocksWide = room.Width,
                        BlocksHigh = room.Height,
                        RoomType = roomType,
                        OpenToStbd = room.OpenToStbd,
                        OpenToPort  = room.OpenToPort,
                        OpenToAft = room.OpenToAft,
                        OpenToBow = room.OpenToBow
                    });
            }
        }

        private bool GetOrFalse(bool[,] hallGrid, int x, int y)
        {
            if(x < 0 || x >= hallGrid.GetLength(0)
                || y < 0 || y >= hallGrid.GetLength(1))
            {
                return false;
            }

            return hallGrid[x, y];
        }
    }
}
