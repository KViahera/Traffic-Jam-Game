using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneGameLevel
{
    public static class AnnealingMethod
    {
        public static List<(float, float, string, string, int)> Launch(int desiredNumberOfMoves)
        {
            Preparation();

            _temperature = /* ? */1f;
			
			var startTime = System.DateTime.Now;
            for (int iteration = 2; ((System.DateTime.Now - startTime).TotalSeconds < (float)(desiredNumberOfMoves)) && (_maximumNumberOfMoves < desiredNumberOfMoves); iteration++)
            {
                _temperature = (float)(1) / (float)(Mathf.Log((float)(iteration)));

                int typeOfOperation = Random.Range(0, 3);

                if (typeOfOperation == 0)
                {
                    DeleteAnObject();
                }
                
                if (typeOfOperation == 1)
                {
                    MoveAnObject();
                }

                if (typeOfOperation == 2)
                {
                    AddAnObject();
                }
            }
            
            return ListOfObjects;
        }

        private static void Preparation()
        {
            ListOfObjects.Add((Random.Range(-2, 2), 0.5f, "Player", "", 2)); 
            
            for (int row = 0; row < 6; row++)
            {
                FieldOfObjects[row] = new int[6];
            }

            FillInTheMatrix();

            _temperature = /* ? */1f;
            _maximumNumberOfMoves = SearchInWidth.CalculatingTheMinimumNumberOfMoves(ListOfObjects);
        }

        private static void FillInTheMatrix()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    FieldOfObjects[row][column] = 0;
                }
            }

            foreach (var currentObject in ListOfObjects)
            {
                if (currentObject.Item3 == "Player")
                {
                    int row = (int)(2.5f - currentObject.Item2), column = (int)(2f + currentObject.Item1);
                    
                    FieldOfObjects[row][column] = 30;
                }

                else
                {
                    if (currentObject.Item4 == "Horizontal")
                    {
                        int row = (int)(2.5f - currentObject.Item2),
                            column = (int)(2.5f - (currentObject.Item5 > 2 ? 1f : 0.5f) + currentObject.Item1);

                        FieldOfObjects[row][column] = 98 * currentObject.Item5;
                    }

                    else
                    {
                        int row = (int)(2.5f - (currentObject.Item5 > 2 ? 1f : 0.5f) - currentObject.Item2),
                            column = (int)(2.5f + currentObject.Item1);

                        FieldOfObjects[row][column] = 294 * currentObject.Item5;
                    }
                }
            }
        }

        private static void DeleteAnObject()
        {
            if (ListOfObjects.Count == 0)
            {
                return;
            }
            
            int index = Random.Range(0, ListOfObjects.Count - 1);
            
            var currentObject = ListOfObjects[index];
            
            if (currentObject.Item3 != "Player")
            {
                ListOfObjects.RemoveAt(index);
            
                FillInTheMatrix();
            
                int currentNumberOfMoves = SearchInWidth.CalculatingTheMinimumNumberOfMoves(ListOfObjects);
            
                if (currentNumberOfMoves > _maximumNumberOfMoves ||
                    Random.value < Mathf.Exp((float)(currentNumberOfMoves - _maximumNumberOfMoves)) /
                    _temperature)
                {
                    _maximumNumberOfMoves = currentNumberOfMoves;
                }
            
                else
                {
                    ListOfObjects.Insert(index, currentObject);
            
                    FillInTheMatrix();
                }
            }
        }

        private static void MoveAnObject()
        {
            List<(int, int, string)> listOfMovableObjects = new List<(int, int, string)>();

            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    if (FieldOfObjects[row][column] == 30 || FieldOfObjects[row][column] == 196)
                    {
                        if (column > 0 && IsTheCellFree(row, column - 1))
                        {
                            listOfMovableObjects.Add((row, column, "Left"));
                        }

                        if (column < 4 && IsTheCellFree(row, column + 2))
                        {
                            listOfMovableObjects.Add((row, column, "Right"));
                        }
                    }

                    if (FieldOfObjects[row][column] == 294)
                    {
                        if (column > 0 && IsTheCellFree(row, column - 1))
                        {
                            listOfMovableObjects.Add((row, column, "Left"));
                        }

                        if (column < 3 && IsTheCellFree(row, column + 3))
                        {
                            listOfMovableObjects.Add((row, column, "Right"));
                        }
                    }

                    if (FieldOfObjects[row][column] == 588)
                    {
                        if (row > 0 && IsTheCellFree(row - 1, column))
                        {
                            listOfMovableObjects.Add((row, column, "Up"));
                        }

                        if (row < 4 && IsTheCellFree(row + 2, column))
                        {
                            listOfMovableObjects.Add((row, column, "Down"));  
                        }
                    }

                    if (FieldOfObjects[row][column] == 882)
                    {
                        if (row > 0 && IsTheCellFree(row - 1, column))
                        {
                            listOfMovableObjects.Add((row, column, "Up"));
                        }

                        if (row < 3 && IsTheCellFree(row + 3, column))
                        {
                            listOfMovableObjects.Add((row, column, "Down"));
                        }
                    }
                }
            }

            if (listOfMovableObjects.Count == 0)
            {
                return;
            }
            
            int index = Random.Range(0, listOfMovableObjects.Count - 1);

            int copyOfRow = listOfMovableObjects[index].Item1, copyOfColumn = listOfMovableObjects[index].Item2;
            string direction = listOfMovableObjects[index].Item3;
            
            if (direction == "Up")
            {
                Services.Swap(ref FieldOfObjects[copyOfRow - 1][copyOfColumn], ref FieldOfObjects[copyOfRow][copyOfColumn]);
            }
            
            if (direction == "Down")
            {
                Services.Swap(ref FieldOfObjects[copyOfRow][copyOfColumn], ref FieldOfObjects[copyOfRow + 1][copyOfColumn]);
            }
            
            if (direction == "Left")
            {
                Services.Swap(ref FieldOfObjects[copyOfRow][copyOfColumn - 1], ref FieldOfObjects[copyOfRow][copyOfColumn]);
            }
            
            if (direction == "Right")
            {
                Services.Swap(ref FieldOfObjects[copyOfRow][copyOfColumn], ref FieldOfObjects[copyOfRow][copyOfColumn + 1]);
            }
            
            FillInTheList();
                            
            int currentNumberOfMoves = SearchInWidth.CalculatingTheMinimumNumberOfMoves(ListOfObjects);
                            
            if (currentNumberOfMoves > _maximumNumberOfMoves ||
                Random.value <
                Mathf.Exp((float)(currentNumberOfMoves - _maximumNumberOfMoves)) / _temperature)
            {
                _maximumNumberOfMoves = currentNumberOfMoves;
            }
                            
            else
            {
                if (direction == "Up")
                {
                    Services.Swap(ref FieldOfObjects[copyOfRow - 1][copyOfColumn], ref FieldOfObjects[copyOfRow][copyOfColumn]);
                }
            
                if (direction == "Down")
                {
                    Services.Swap(ref FieldOfObjects[copyOfRow][copyOfColumn], ref FieldOfObjects[copyOfRow + 1][copyOfColumn]);
                }
            
                if (direction == "Left")
                {
                    Services.Swap(ref FieldOfObjects[copyOfRow][copyOfColumn - 1], ref FieldOfObjects[copyOfRow][copyOfColumn]);
                }
            
                if (direction == "Right")
                {
                    Services.Swap(ref FieldOfObjects[copyOfRow][copyOfColumn], ref FieldOfObjects[copyOfRow][copyOfColumn + 1]);
                }
                            
                FillInTheList();
            }
        }

        private static void FillInTheList()
        {
            ListOfObjects.Clear();

            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    if (FieldOfObjects[row][column] == 30)
                    {
                        ListOfObjects.Add((-2f + column, 2.5f - row, "Player", "", 2));
                    }

                    if (FieldOfObjects[row][column] == 196)
                    {
                        ListOfObjects.Add((-2f + column, 2.5f - row, "Obstacle", "Horizontal", 2));
                    }

                    if (FieldOfObjects[row][column] == 294)
                    {
                        ListOfObjects.Add((-1.5f + column, 2.5f - row, "Obstacle", "Horizontal", 3));
                    }

                    if (FieldOfObjects[row][column] == 588)
                    {
                        ListOfObjects.Add((-2.5f + column, 2f - row, "Obstacle", "Vertical", 2));
                    }

                    if (FieldOfObjects[row][column] == 882)
                    {
                        ListOfObjects.Add((-2.5f + column, 1.5f - row, "Obstacle", "Vertical", 3));
                    }
                }
            }
        }

        private static void AddAnObject()
        {
            List<(int, int, int)> listOfObjectsBeingAdded = new List<(int, int, int)>();
            
            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    if (IsTheCellFree(row, column))
                    {
                        if (column < 5 && IsTheCellFree(row, column + 1))
                        {
                            listOfObjectsBeingAdded.Add((row, column, 196));
                        }

                        if (column < 4 && IsTheCellFree(row, column + 1) && IsTheCellFree(row, column + 2))
                        {
                            listOfObjectsBeingAdded.Add((row, column, 294));
                        }

                        if (row < 5 && IsTheCellFree(row + 1, column))
                        {
                            listOfObjectsBeingAdded.Add((row, column, 588));
                        }

                        if (row < 4 && IsTheCellFree(row + 1, column) && IsTheCellFree(row + 2, column))
                        {
                            listOfObjectsBeingAdded.Add((row, column, 882));
                        }
                    }
                }
            }

            if (listOfObjectsBeingAdded.Count == 0)
            {
                return;
            }

            int index = Random.Range(0, listOfObjectsBeingAdded.Count - 1),
                copyOfRow = listOfObjectsBeingAdded[index].Item1,
                copyOfColumn = listOfObjectsBeingAdded[index].Item2,
                typeOfObject = listOfObjectsBeingAdded[index].Item3;
            
            FieldOfObjects[copyOfRow][copyOfColumn] = typeOfObject;

            FillInTheList();

            int currentNumberOfMoves = SearchInWidth.CalculatingTheMinimumNumberOfMoves(ListOfObjects);

            if (currentNumberOfMoves > _maximumNumberOfMoves ||
                Random.value <
                Mathf.Exp((float)(currentNumberOfMoves - _maximumNumberOfMoves)) / _temperature)
            {
                _maximumNumberOfMoves = currentNumberOfMoves;
            }

            else
            {
                FieldOfObjects[copyOfRow][copyOfColumn] = 0;

                FillInTheList();
            }
        }

        private static bool IsTheCellFree(int row, int column)
        {
            return !((row > 1 && FieldOfObjects[row - 2][column] == 882) ||
                     (row > 0 && FieldOfObjects[row - 1][column] == 882) ||
                     (row > 0 && FieldOfObjects[row - 1][column] == 588) ||
                     (column > 1 && FieldOfObjects[row][column - 2] == 294) ||
                     (column > 0 && FieldOfObjects[row][column - 1] == 294) ||
                     (column > 0 && FieldOfObjects[row][column - 1] == 196) ||
                     (column > 0 && FieldOfObjects[row][column - 1] == 30) || FieldOfObjects[row][column] > 0);
        }

        private static readonly List<(float, float, string, string, int)> ListOfObjects =
            new List<(float, float, string, string, int)>();

        private static float _temperature;
        private static int _maximumNumberOfMoves;
        private static readonly int[][] FieldOfObjects = new int[6][];
    }
    
    public static class SearchInWidth
    {
        public static int CalculatingTheMinimumNumberOfMoves(List<(float, float, string, string, int)> listOfObjects)
        {
            Preparation();

            List<(float, float, string, string, int)> copyOfListOfObjects =
                new List<(float, float, string, string, int)>(listOfObjects);

            _listOfObjects = copyOfListOfObjects;

            FillInTheMatrix();

            Conditions.Enqueue(copyOfListOfObjects);

            RequiredNumberOfMoves.Add(HashCalculation(), 0);

            int numberOfMoves = /* ? */1000000119;
            while (Conditions.Count > 0)
            {
                var currentCondition = Conditions.Dequeue();

                _listOfObjects = currentCondition;

                FillInTheMatrix();

                RequiredNumberOfMoves.TryGetValue(HashCalculation(), out _currentNumberOfMoves);

                if (FieldOfObjects[2][4] == 30)
                {
                    numberOfMoves = Services.Min(numberOfMoves, _currentNumberOfMoves);
                    continue;
                }

                _currentNumberOfMoves = _currentNumberOfMoves + 1;

                MoveAnObject();
            }

            return numberOfMoves < 1000000119 ? numberOfMoves : -1000000119;
        }

        private static void Preparation()
        {
            Conditions.Clear();

            RequiredNumberOfMoves.Clear();

            _listOfObjects.Clear();

            for (int row = 0; row < 6; row++)
            {
                FieldOfObjects[row] = new int [6];
            }
        }

        private static void FillInTheMatrix()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    FieldOfObjects[row][column] = 0;
                }
            }

            foreach (var currentObject in _listOfObjects)
            {
                if (currentObject.Item3 == "Player")
                {
                    int row = (int)(2.5f - currentObject.Item2), column = (int)(2f + currentObject.Item1);

                    FieldOfObjects[row][column] = 30;
                }

                else
                {
                    if (currentObject.Item4 == "Horizontal")
                    {
                        int row = (int)(2.5f - currentObject.Item2),
                            column = (int)(2.5f - (currentObject.Item5 > 2 ? 1f : 0.5f) + currentObject.Item1);

                        FieldOfObjects[row][column] = 98 * currentObject.Item5;
                    }

                    else
                    {
                        int row = (int)(2.5f - (currentObject.Item5 > 2 ? 1f : 0.5f) - currentObject.Item2),
                            column = (int)(2.5f + currentObject.Item1);

                        FieldOfObjects[row][column] = 294 * currentObject.Item5;
                    }
                }
            }
        }

        private static long HashCalculation()
        {
            long hash = 0, multiplier = 1, primeNumber = 887;
            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    hash = (hash + (((FieldOfObjects[row][column] + 1) * multiplier) % 1000000007)) % 1000000007;
                    multiplier = (multiplier * primeNumber) % 1000000007;
                }
            }

            return hash;
        }

        private static void MoveAnObject()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    if (FieldOfObjects[row][column] == 30 || FieldOfObjects[row][column] == 196)
                    {
                        if (column > 0 && IsTheCellFree(row, column - 1))
                        {
                            Services.Swap(ref FieldOfObjects[row][column - 1], ref FieldOfObjects[row][column]);

                            FillInTheList();

                            List<(float, float, string, string, int)> newCondition =
                                new List<(float, float, string, string, int)>(_listOfObjects);

                            long hash = HashCalculation();
                            if (RequiredNumberOfMoves.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                RequiredNumberOfMoves.Add(hash, _currentNumberOfMoves);
                            }

                            Services.Swap(ref FieldOfObjects[row][column - 1], ref FieldOfObjects[row][column]);

                            FillInTheList();
                        }

                        if (column < 4 && IsTheCellFree(row, column + 2))
                        {
                            Services.Swap(ref FieldOfObjects[row][column], ref FieldOfObjects[row][column + 1]);

                            FillInTheList();

                            List<(float, float, string, string, int)> newCondition =
                                new List<(float, float, string, string, int)>(_listOfObjects);

                            long hash = HashCalculation();
                            if (RequiredNumberOfMoves.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                RequiredNumberOfMoves.Add(hash, _currentNumberOfMoves);
                            }

                            Services.Swap(ref FieldOfObjects[row][column], ref FieldOfObjects[row][column + 1]);

                            FillInTheList();
                        }
                    }

                    if (FieldOfObjects[row][column] == 294)
                    {
                        if (column > 0 && IsTheCellFree(row, column - 1))
                        {
                            Services.Swap(ref FieldOfObjects[row][column - 1], ref FieldOfObjects[row][column]);

                            FillInTheList();

                            List<(float, float, string, string, int)> newCondition =
                                new List<(float, float, string, string, int)>(_listOfObjects);

                            long hash = HashCalculation();
                            if (RequiredNumberOfMoves.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                RequiredNumberOfMoves.Add(hash, _currentNumberOfMoves);
                            }

                            Services.Swap(ref FieldOfObjects[row][column - 1], ref FieldOfObjects[row][column]);

                            FillInTheList();
                        }

                        if (column < 3 && IsTheCellFree(row, column + 3))
                        {
                            Services.Swap(ref FieldOfObjects[row][column], ref FieldOfObjects[row][column + 1]);

                            FillInTheList();

                            List<(float, float, string, string, int)> newCondition =
                                new List<(float, float, string, string, int)>(_listOfObjects);

                            long hash = HashCalculation();
                            if (RequiredNumberOfMoves.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                RequiredNumberOfMoves.Add(hash, _currentNumberOfMoves);
                            }

                            Services.Swap(ref FieldOfObjects[row][column], ref FieldOfObjects[row][column + 1]);

                            FillInTheList();
                        }
                    }

                    if (FieldOfObjects[row][column] == 588)
                    {
                        if (row > 0 && IsTheCellFree(row - 1, column))
                        {
                            Services.Swap(ref FieldOfObjects[row - 1][column], ref FieldOfObjects[row][column]);

                            FillInTheList();

                            List<(float, float, string, string, int)> newCondition =
                                new List<(float, float, string, string, int)>(_listOfObjects);

                            long hash = HashCalculation();
                            if (RequiredNumberOfMoves.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                RequiredNumberOfMoves.Add(hash, _currentNumberOfMoves);
                            }

                            Services.Swap(ref FieldOfObjects[row - 1][column], ref FieldOfObjects[row][column]);

                            FillInTheList();
                        }

                        if (row < 4 && IsTheCellFree(row + 2, column))
                        {
                            Services.Swap(ref FieldOfObjects[row][column], ref FieldOfObjects[row + 1][column]);

                            FillInTheList();

                            List<(float, float, string, string, int)> newCondition =
                                new List<(float, float, string, string, int)>(_listOfObjects);

                            long hash = HashCalculation();
                            if (RequiredNumberOfMoves.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                RequiredNumberOfMoves.Add(hash, _currentNumberOfMoves);
                            }

                            Services.Swap(ref FieldOfObjects[row][column], ref FieldOfObjects[row + 1][column]);

                            FillInTheList();
                        }
                    }

                    if (FieldOfObjects[row][column] == 882)
                    {
                        if (row > 0 && IsTheCellFree(row - 1, column))
                        {
                            Services.Swap(ref FieldOfObjects[row - 1][column], ref FieldOfObjects[row][column]);

                            FillInTheList();

                            List<(float, float, string, string, int)> newCondition =
                                new List<(float, float, string, string, int)>(_listOfObjects);

                            long hash = HashCalculation();
                            if (RequiredNumberOfMoves.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                RequiredNumberOfMoves.Add(hash, _currentNumberOfMoves);
                            }

                            Services.Swap(ref FieldOfObjects[row - 1][column], ref FieldOfObjects[row][column]);

                            FillInTheList();
                        }

                        if (row < 3 && IsTheCellFree(row + 3, column))
                        {
                            Services.Swap(ref FieldOfObjects[row][column], ref FieldOfObjects[row + 1][column]);

                            FillInTheList();

                            List<(float, float, string, string, int)> newCondition =
                                new List<(float, float, string, string, int)>(_listOfObjects);

                            long hash = HashCalculation();
                            if (RequiredNumberOfMoves.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                RequiredNumberOfMoves.Add(hash, _currentNumberOfMoves);
                            }

                            Services.Swap(ref FieldOfObjects[row][column], ref FieldOfObjects[row + 1][column]);

                            FillInTheList();
                        }
                    }
                }
            }
        }

        private static bool IsTheCellFree(int row, int column)
        {
            return !((row > 1 && FieldOfObjects[row - 2][column] == 882) ||
                     (row > 0 && FieldOfObjects[row - 1][column] == 882) ||
                     (row > 0 && FieldOfObjects[row - 1][column] == 588) ||
                     (column > 1 && FieldOfObjects[row][column - 2] == 294) ||
                     (column > 0 && FieldOfObjects[row][column - 1] == 294) ||
                     (column > 0 && FieldOfObjects[row][column - 1] == 196) ||
                     (column > 0 && FieldOfObjects[row][column - 1] == 30) || FieldOfObjects[row][column] > 0);
        }

        private static void FillInTheList()
        {
            _listOfObjects.Clear();

            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    if (FieldOfObjects[row][column] == 30)
                    {
                        _listOfObjects.Add((-2f + column, 2.5f - row, "Player", "", 2));
                    }

                    if (FieldOfObjects[row][column] == 196)
                    {
                        _listOfObjects.Add((-2f + column, 2.5f - row, "Obstacle", "Horizontal", 2));
                    }

                    if (FieldOfObjects[row][column] == 294)
                    {
                        _listOfObjects.Add((-1.5f + column, 2.5f - row, "Obstacle", "Horizontal", 3));
                    }

                    if (FieldOfObjects[row][column] == 588)
                    {
                        _listOfObjects.Add((-2.5f + column, 2f - row, "Obstacle", "Vertical", 2));
                    }

                    if (FieldOfObjects[row][column] == 882)
                    {
                        _listOfObjects.Add((-2.5f + column, 1.5f - row, "Obstacle", "Vertical", 3));
                    }
                }
            }
        }

        private static readonly Queue<List<(float, float, string, string, int)>> Conditions =
            new Queue<List<(float, float, string, string, int)>>();

        private static readonly Dictionary<long, int> RequiredNumberOfMoves = new Dictionary<long, int>();

        private static List<(float, float, string, string, int)> _listOfObjects =
            new List<(float, float, string, string, int)>();

        private static readonly int[][] FieldOfObjects = new int[6][];
        private static int _currentNumberOfMoves;
    }
    
    public static class Services
    {
        public static void Swap(ref int x, ref int y)
        {
            x = x ^ y;
            y = y ^ x;
            x = x ^ y;
        }

        public static int Min(int x, int y)
        {
            if (x < y)
            {
                return x;
            }

            return y;
        }
    }
}