using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CellAnimation : MonoBehaviour
{
    #region --Переменные--
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI points;
    private float moveTime = .1f;
    private float appearTime = .1f;
    private Sequence sequence;
    #endregion

    #region --Методы--

    /// <summary>
    /// Анимация передвижения
    /// </summary>
    /// <param name="from">Из кого переходим</param>
    /// <param name="to">В кого переходим</param>
    /// <param name="isMerging">Флаг слияния ячеик</param>
    public void Move(Cell from, Cell to, bool isMerging) 
    {
        from.CancelAnimation();
        to.SetAnimation(this);

        image.color = CollorManegre.Instance.CellColors[from.Value];
        points.text = from.Points.ToString();
        points.color = from.Value <= 2 ? 
            CollorManegre.Instance.PointsDarkColor : 
            CollorManegre.Instance.PointsLightColor;

        transform.position = from.transform.position;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(to.transform.position, moveTime).SetEase(Ease.InOutQuad));

        if (isMerging) 
        {
            sequence.AppendCallback(() =>
            {
                image.color = CollorManegre.Instance.CellColors[to.Value];
                points.text = to.Points.ToString();
                points.color = to.Value <= 2 ?
                    CollorManegre.Instance.PointsDarkColor :
                    CollorManegre.Instance.PointsLightColor;
            });

            sequence.Append(transform.DOScale(1.2f, appearTime));
            sequence.Append(transform.DOScale(1f, appearTime));
        }

        sequence.AppendCallback(() =>
        {
            to.UpdateCell();
            Destroy();
        });
    }

    /// <summary>
    /// Метод для анимации появления 
    /// </summary>
    /// <param name="cell">Ячейка</param>
    public void Appear(Cell cell) 
    {
        cell.CancelAnimation();
        cell.SetAnimation(this);

        image.color = CollorManegre.Instance.CellColors[cell.Value];
        points.text = cell.Points.ToString();
        points.color = cell.Value <= 2 ?
            CollorManegre.Instance.PointsDarkColor :
            CollorManegre.Instance.PointsLightColor;

        transform.position = cell.transform.position;
        transform.localScale = Vector2.zero;

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.2f, appearTime * 2));
        sequence.Append(transform.DOScale(1f, appearTime * 2));
        sequence.AppendCallback(() =>
        {
            cell.UpdateCell();  
            Destroy();
        });
    }

    /// <summary>
    /// Метод для удаления ячейки
    /// </summary>
    public void Destroy()
    {
        sequence.Kill();
        Destroy(gameObject);
    }
    #endregion
}
