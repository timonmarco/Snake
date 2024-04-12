using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    public class Board
    {
        private readonly Dictionary<Coordinate, HashSet<Item>> itemsByCoordinate = [];
        private readonly Terrain[,] board;
        private Coordinate? end;
        private Coordinate? start;
        private int streetCount;
        private readonly HashSet<Coordinate> teleporters = [];
        private readonly Dictionary<Coordinate, ushort> doors = [];

        public int Width { get; }

        public int Height { get; }

        public decimal LowestTerrainMultiplier => streetCount > 0 ? Terrain.Street.GetDistanceMultiplier() : Terrain.Empty.GetDistanceMultiplier();

        public Coordinate? Start
        {
            get => start;
            set
            {
                if (value is null || IsValid(value.Value))
                    start = value;
                if (Start is not null && !CombinationIsValid(Start.Value, GetTerrain(Start.Value)))
                    SetTerrain(Start.Value, Terrain.Empty);
            }
        }

        public Coordinate? End
        {
            get => end;
            set
            {
                if (value is null || IsValid(value.Value))
                    end = value;
                if (End is not null && !CombinationIsValid(End.Value, GetTerrain(End.Value)))
                    SetTerrain(End.Value, Terrain.Empty);
            }
        }

        public Board(int dimension)
            : this(dimension, dimension)
        {
        }

        public Board(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentException("Argument must be greater than 0", nameof(width));
            if (height <= 0)
                throw new ArgumentException("Argument must be greater than 0", nameof(height));
            Width = width;
            Height = height;
            board = new Terrain[width, height];
        }

        public void SetTeleporter(Coordinate coordinate)
        {
            if (coordinate == Start || coordinate == End)
                return;
            SetTerrain(coordinate, Terrain.Empty);
            teleporters.Add(coordinate);
        }

        public void SetDoor(Coordinate coordinate, ushort doorValue) => doors[coordinate] = doorValue;

        public void RemoveDoor(Coordinate coordinate) => doors.Remove(coordinate);

        public int GetDoor(Coordinate coordinate) => doors.TryGetValue(coordinate, out var door) ? door : -1;

        public void RemoveTeleporter(Coordinate coordinate) => teleporters.Remove(coordinate);

        public Coordinate[] GetTeleporters() => teleporters.ToArray();

        public Terrain GetTerrain(Coordinate coordinate) => IsValid(coordinate) ?
            board[coordinate.X, coordinate.Y] :
            Terrain.Obstacle;

        public Item[] GetItems(Coordinate coordinate) => IsValid(coordinate) && itemsByCoordinate.TryGetValue(coordinate, out var items) ?
            items.ToArray() :
            [];

        public bool IsValid(Coordinate coordinate) => coordinate.X < Width &&
            coordinate.Y < Height &&
            coordinate.X >= 0 &&
            coordinate.Y >= 0;

        public void Randomize(double obstaclePercentage) => Randomize(new(), obstaclePercentage);

        public void Randomize(Random rng, double obstaclePercentage)
        {
            foreach (var coordinate in GetCoordinates())
                SetTerrain(coordinate, rng.NextDouble() <= obstaclePercentage ? Terrain.Obstacle : Terrain.Empty);
        }

        public void SetTerrain(Coordinate coordinate, Terrain terrain)
        {
            if (!CombinationIsValid(coordinate, terrain))
                return;
            UpdateTerrainTypeCounts(coordinate, terrain);
            board[coordinate.X, coordinate.Y] = terrain;
        }

        public void AddItem(Coordinate coordinate, Item item)
        {
            if (!IsValid(coordinate))
                return;
            if (!itemsByCoordinate.TryGetValue(coordinate, out var items))
            {
                items = [];
                itemsByCoordinate.Add(coordinate, items);
            }
            items.Add(item);
        }

        public void RemoveItem(Coordinate coordinate, Item item)
        {
            if (!IsValid(coordinate))
                return;
            if (!itemsByCoordinate.TryGetValue(coordinate, out var items))
            {
                items = [];
                itemsByCoordinate.Add(coordinate, items);
            }
            items.Remove(item);
        }

        private bool CombinationIsValid(Coordinate coordinate, Terrain terrain) =>
            IsValid(coordinate) && (coordinate != End && coordinate != Start && !teleporters.Contains(coordinate) || terrain != Terrain.Obstacle);

        private void UpdateTerrainTypeCounts(Coordinate coordinate, Terrain terrain)
        {
            if (GetTerrain(coordinate) is Terrain.Street)
                streetCount--;
            if (terrain is Terrain.Street)
                streetCount++;
        }

        public IEnumerable<Coordinate> GetCoordinates() => Enumerable.Range(0, Width).SelectMany(x => Enumerable.Range(0, Height).Select(y => new Coordinate(x, y)));

        public bool CoordinateIsAccessible(Coordinate coordinate) => GetTerrain(coordinate) != Terrain.Obstacle;
    }
}