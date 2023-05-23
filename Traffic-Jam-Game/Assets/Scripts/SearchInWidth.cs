using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLevel 
{
    public static class SearchInWidth
    {
        public static List<(float, float, Vector3)> SequenceSearch(List<(float, float, string, string, int)> listOfObjects)
        {
            Preparation();

            List<(float, float, string, string, int)> copyOfListOfObjects =
                new List<(float, float, string, string, int)>(listOfObjects);

            _listOfObjects = copyOfListOfObjects;

            FillInTheMatrix();

            Conditions.Enqueue(copyOfListOfObjects);

            isConditionsAdded.Add(HashCalculation(), true);
            ParentConditions.Add(HashCalculation(), (-1000000119, -1000000119, -1000000119, -1000000119, ""));

            List<(float, float, Vector3)> sequence = new List<(float, float, Vector3)>();
            while (Conditions.Count > 0)
            {
                var currentCondition = Conditions.Dequeue();

                _listOfObjects = currentCondition;

                FillInTheMatrix();
                
                if (FieldOfObjects[2][4] == 30)
                {
                    sequence.Add((4f, 0.5f, new Vector3(1f, 0f, 0f)));
                    sequence.Add((3f, 0.5f, new Vector3(1f, 0f, 0f)));
                    sequence.Add((2f, 0.5f, new Vector3(1f, 0f, 0f)));
                    
                    long hash = HashCalculation();
                    while (!(hash < 0))
                    {
                        (long, int, int, int, string) tuple;
                        ParentConditions.TryGetValue(hash, out tuple);

                        int row = tuple.Item2, column = tuple.Item3;
                        Vector3 direction = new Vector3(0f, 0f, 0f);
                        
                        if (tuple.Item5 == "Up")
                        {
                            direction.z = 1f;
                        }
                        
                        if (tuple.Item5 == "Down")
                        {
                            direction.z = -1f;
                        }
                        
                        if (tuple.Item5 == "Left")
                        {
                            direction.x = -1f;
                        }
                        
                        if (tuple.Item5 == "Right")
                        {
                            direction.x = 1f;
                        }

                        if (tuple.Item4 == 30)
                        {
                            sequence.Add((-2f + column, 2.5f - row, direction));
                        }

                        if (tuple.Item4 == 196)
                        {
                            sequence.Add((-2f + column, 2.5f - row, direction));
                        }

                        if (tuple.Item4 == 294)
                        {
                            sequence.Add((-1.5f + column, 2.5f - row, direction));
                        }

                        if (tuple.Item4 == 588)
                        {
                            sequence.Add((-2.5f + column, 2f - row, direction));
                        }

                        if (tuple.Item4 == 882)
                        {
                            sequence.Add((-2.5f + column, 1.5f - row, direction));
                        }
                        
                        hash = tuple.Item1;
                    }

                    sequence.Reverse();
                    return sequence;
                }
                
                MoveAnObject();
            }

            return sequence;
        }

        private static void Preparation()
        {
            Conditions.Clear();

            isConditionsAdded.Clear();
            ParentConditions.Clear();

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
                    long parentHash = HashCalculation();
                    int IDOfTheGameObject = FieldOfObjects[row][column];
                    
                    if (FieldOfObjects[row][column] == 30 || FieldOfObjects[row][column] == 196)
                    {
                        if (column > 0 && IsTheCellFree(row, column - 1))
                        {
                            Services.Swap(ref FieldOfObjects[row][column - 1], ref FieldOfObjects[row][column]);

                            FillInTheList();

                            List<(float, float, string, string, int)> newCondition =
                                new List<(float, float, string, string, int)>(_listOfObjects);

                            long hash = HashCalculation();
                            if (isConditionsAdded.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                isConditionsAdded.Add(hash, true);
                                ParentConditions.Add(hash, (parentHash, row, column, IDOfTheGameObject, "Left"));
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
                            if (isConditionsAdded.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                isConditionsAdded.Add(hash, true);
                                ParentConditions.Add(hash, (parentHash, row, column, IDOfTheGameObject, "Right"));
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
                            if (isConditionsAdded.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                isConditionsAdded.Add(hash, true);
                                ParentConditions.Add(hash, (parentHash, row, column, IDOfTheGameObject, "Left"));
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
                            if (isConditionsAdded.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                isConditionsAdded.Add(hash, true);
                                ParentConditions.Add(hash, (parentHash, row, column, IDOfTheGameObject, "Right"));
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
                            if (isConditionsAdded.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                isConditionsAdded.Add(hash, true);
                                ParentConditions.Add(hash, (parentHash, row, column, IDOfTheGameObject, "Up"));
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
                            if (isConditionsAdded.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                isConditionsAdded.Add(hash, true);
                                ParentConditions.Add(hash, (parentHash, row, column, IDOfTheGameObject, "Down"));
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
                            if (isConditionsAdded.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                isConditionsAdded.Add(hash, true);
                                ParentConditions.Add(hash, (parentHash, row, column, IDOfTheGameObject, "Up"));
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
                            if (isConditionsAdded.ContainsKey(hash) == false)
                            {
                                Conditions.Enqueue(newCondition);

                                isConditionsAdded.Add(hash, true);
                                ParentConditions.Add(hash, (parentHash, row, column, IDOfTheGameObject, "Down"));
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

        private static readonly Dictionary<long, bool> isConditionsAdded = new Dictionary<long, bool>();

        private static readonly Dictionary<long, (long, int, int, int, string)> ParentConditions =
            new Dictionary<long, (long, int, int, int, string)>();

        private static List<(float, float, string, string, int)> _listOfObjects =
            new List<(float, float, string, string, int)>();

        private static readonly int[][] FieldOfObjects = new int[6][];
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