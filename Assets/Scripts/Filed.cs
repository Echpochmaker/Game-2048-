using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filed : MonoBehaviour
{
    #region --Переменные--
    public static Filed Instance;

    [Header("Filed Properties")]
    public float CellSize;
    public float Spacing;
    public int FieldSize;
    public int InitCellsCount;

    [Space(10)]
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private RectTransform rect;

    private Cell[,] field;
    private bool anyCellMoved; //Перемещена ли ячейка 
    private static AudioSource audioSource;
    #endregion

    #region --Методы--
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    private void Start()
    {
        SwipeDirection.SwipeEvent += OnInput;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        //#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))         
            OnInput(Vector2.left);        
        if (Input.GetKeyDown(KeyCode.D))        
            OnInput(Vector2.right);        
        if (Input.GetKeyDown(KeyCode.W))       
            OnInput(Vector2.up);        
        if (Input.GetKeyDown(KeyCode.S))     
            OnInput(Vector2.down);      
        //#endif
    }

    /// <summary>
    /// Для перемещения
    /// </summary>
    /// <param name="direction">Направление в котором будет перемещение</param>
    private void OnInput(Vector2 direction) 
    {
        if (!GameController.GameStarted)
            return;

        anyCellMoved = false;
        ResetCellFlags();

        Move(direction);

        if (anyCellMoved) 
        {
            audioSource.Play();
            GenerateRandomCell();
            CheecGameResult();
        }
    }

    /// <summary>
    /// Перемещение 
    /// </summary>
    /// <param name="direction"> Направление для перемещения</param>
    private void Move(Vector2 direction) 
    {
        int startXY = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0;
        int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        for (int i = 0; i < FieldSize; i++) 
        {
            for (int k = startXY; k >= 0 && k < FieldSize; k -= dir) 
            {
                var cell = direction.x != 0 ? field[k, i] : field[i, k];

                if (cell.IsEmpty)
                    continue;

                var cellToMerge = FindeCellToMerge(cell, direction);
                if(cellToMerge != null) 
                {
                    cell.MergeWithCell(cellToMerge);
                    anyCellMoved = true;
                    continue;
                }

                var emptyCell = FindEmptyCell(cell, direction);
                if (emptyCell != null) 
                {
                    cell.MoveToCell(emptyCell);
                    anyCellMoved = true;
                }
            }
        }
    }

    /// <summary>
    /// Поиск свободных ячеик для слияний
    /// </summary>
    /// <param name="cell">Ячейка</param>
    /// <param name="direction">Направление для перемещения</param>
    /// <returns></returns>
    private Cell FindeCellToMerge(Cell cell, Vector2 direction) 
    {
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY;
           x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
           x += (int)direction.x, y -= (int)direction.y) 
        {
            if (field[x, y].IsEmpty)
                continue;

            if (field[x, y].Value == cell.Value && !field[x, y].HasMerged)
                return field[x, y];

            break;
        }
        return null;
    }

    /// <summary>
    /// Поиск пустых ячеик
    /// </summary>
    /// <param name="cell">Ячейка</param>
    /// <param name="direction">Направление для перемещения</param>
    /// <returns></returns>
    private Cell FindEmptyCell(Cell cell, Vector2 direction) 
    {
        Cell emptyCell = null;
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY;
            x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
            x += (int)direction.x, y -= (int)direction.y) 
        {
            if (field[x, y].IsEmpty)
                emptyCell = field[x, y];
            else
                break;
        }

        return emptyCell;
    }

    /// <summary>
    /// Проверка игры 
    /// </summary>
    private void CheecGameResult()
    {
        bool lose = true;
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                if(field[x, y].Value == Cell.MaxValue)
                {
                    Cell.MaxValue = 15;
                    GameController.Instance.Win();                
                    return;
                }

                if (lose && field[x, y].IsEmpty ||
                    FindeCellToMerge(field[x, y], Vector2.left) || FindeCellToMerge(field[x, y], Vector2.right) ||
                    FindeCellToMerge(field[x, y], Vector2.up) || FindeCellToMerge(field[x, y], Vector2.down))
                {
                    lose = false;
                }
            }         
        }

        if (lose)
            GameController.Instance.Lose();     
    }

    /// <summary>
    /// Создание поля для игры
    /// </summary>
    private void CreateField() 
    {
        field = new Cell[FieldSize, FieldSize];
        float fieltWidth = FieldSize * (CellSize + Spacing) + Spacing;
        rect.sizeDelta = new Vector2(fieltWidth, fieltWidth);

        float startX = -(fieltWidth / 2) + (CellSize / 2) + Spacing;
        float startY = (fieltWidth / 2) - (CellSize / 2) - Spacing;

        for (int x = 0; x < FieldSize; x++) 
        {
            for (int y = 0; y < FieldSize; y++)
            {
                var cell = Instantiate(cellPrefab, transform, false);
                var position = new Vector2(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                cell.transform.localPosition = position;
                field[x, y] = cell;
                cell.SetValue(x, y, 0);
            }
        }
    }

    /// <summary>
    /// Генирация ячеик для игры 
    /// </summary>
    public void GenerateFiled()
    {
        if (field == null)
            CreateField();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].SetValue(x, y, 0);

        for (int i = 0; i < InitCellsCount; i++)
            GenerateRandomCell();

        //field[0, 0].SetValue(0, 0, 10);
        //field[0, 1].SetValue(0, 1, 10);
    }  

    /// <summary>
    /// Генирация ячеик (2 и 4) в любом свободном месте на поле
    /// </summary>
    /// <exception cref="System.Exception">Исключение, которое происходит когда все ячейки заняты</exception>
    public void GenerateRandomCell() 
    {
        var emptyCell = new List<Cell>();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                if (field[x, y].IsEmpty)
                    emptyCell.Add(field[x, y]);

        if (emptyCell.Count == 0)
            throw new System.Exception("There is no any empty cell!");

        int value = Random.Range(0, 10) == 0 ? 2 : 1;
        var cell = emptyCell[Random.Range(0, emptyCell.Count)];
        cell.SetValue(cell.X, cell.Y, value, false);

        CellAnimationController.Instance.SmoothAppear(cell);
            
    }

    /// <summary>
    /// Сброс флага ячеик
    /// </summary>
    private void ResetCellFlags() 
    {
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].ResetFlags();
    }
    #endregion
}
