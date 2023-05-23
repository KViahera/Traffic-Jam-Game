using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Workshop
{
    public static class GameLevelValidator
    {
        public static bool isValid(List<GameObject> basiclistOfGameObjects)
        {
            foreach (var currentGameObject in basiclistOfGameObjects)
            {
                if (Mathf.Max(currentGameObject.transform.localScale.x, currentGameObject.transform.localScale.z) == 1f)
                {
                    return false;
                }
            }

            for (float x = -3.25f; x < 3.5f; x = x + 0.25f)
            {
                for (float z = -3.25f; z < 3.5f; z = z + 0.25f)
                {
                    int numberOfObjects = 0;
                    foreach (var currentGameObject in GameObject.FindGameObjectsWithTag("Game object"))
                    {
                        float centralX = currentGameObject.transform.position.x, centralZ = currentGameObject.transform.position.z;

                        float leftX = centralX - currentGameObject.transform.localScale.x / 2f,
                            rightX = centralX + currentGameObject.transform.localScale.x / 2f,
                            lowerZ = centralZ - currentGameObject.transform.localScale.z / 2f,
                            upperZ = centralZ + currentGameObject.transform.localScale.z / 2f;

                        if (leftX < -3f || rightX > 3f || lowerZ < -3f || upperZ > 3f)
                        {
                            return false;
                        } 
                        
                        if (leftX < x && x < rightX && z > lowerZ && z < upperZ)
                        {
                            numberOfObjects = numberOfObjects + 1;
                        }
                    }
                    
                    foreach (var currentGameObject in GameObject.FindGameObjectsWithTag("Playing field"))
                    {
                        float centralX = currentGameObject.transform.position.x, centralZ = currentGameObject.transform.position.z;

                        float leftX = centralX - currentGameObject.transform.localScale.x / 2f,
                            rightX = centralX + currentGameObject.transform.localScale.x / 2f,
                            lowerZ = centralZ - currentGameObject.transform.localScale.z / 2f,
                            upperZ = centralZ + currentGameObject.transform.localScale.z / 2f;


                        if (leftX < x && x < rightX && z > lowerZ && z < upperZ)
                        {
                            numberOfObjects = numberOfObjects + 1;
                        }
                    }

                    if (numberOfObjects > 1)
                    {
                        return false;
                    }
                }
            }
            
            List<(float, float, string, string, int)> finalListOfGameObjects = new List<(float, float, string, string, int)>();
            foreach (var currentGameObject in basiclistOfGameObjects)
            {
                finalListOfGameObjects.Add((currentGameObject.transform.position.x, currentGameObject.transform.position.z,
                    "Obstacle", currentGameObject.transform.localScale.x > 1f ? "Horizontal" : "Vertical",
                    (int)Mathf.Max(currentGameObject.transform.localScale.x, currentGameObject.transform.localScale.z)));
            }

            bool isAPlayer = false;
            for (float x = 2f; !(x < -2f); x = x - 0.5f)
            {
                float z = 0.5f;
                for (int index = 0; index < finalListOfGameObjects.Count; index = index + 1)
                {
                    if (finalListOfGameObjects[index].Item1 == x && finalListOfGameObjects[index].Item2 == z)
                    {
                        var tuple = finalListOfGameObjects[index];
                        tuple.Item3 = "Player";
                        finalListOfGameObjects[index] = tuple;

                        isAPlayer = true;
                        break;
                    }
                }

                if (isAPlayer == true)
                {
                    break;
                }
            }

            if (isAPlayer == false)
            {
                return false;
            }
            
            if (SearchInWidth.CalculatingTheMinimumNumberOfMoves(finalListOfGameObjects) < 0)
            {
                return false;
            }
            
            return true;
        }
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
