using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [Header("Intersection check")]
    [SerializeField] private LayerMask intersectionLayer;
    [SerializeField] private Collider[] intersectionCheckColliders;
    [SerializeField] private Transform intersectionCheckParent;

    public bool IntersectionDetected()
    {
        Physics.SyncTransforms();

        foreach (var collider in intersectionCheckColliders)
        {
            Collider[] hitCollider = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity, intersectionLayer);

            foreach (var hit in hitCollider)
            {
                IntersectionCheck intersectionCheck = hit.GetComponentInParent<IntersectionCheck>();

                if (intersectionCheck != null && intersectionCheckParent != intersectionCheck.transform)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEntrancePoint();

        AlignTo(entrancePoint, targetSnapPoint);// ВНИМАНИЕ: выравниваение должно быть перед привязкой к положению
        SnapTo(entrancePoint, targetSnapPoint); 
    }

    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Вычислите смещение вращения между текущим вращением части уровня
        // и вращением собственной точки привязки.Это поможет в дальнейшем точно настроить выравнивание.
        var rotationOffset = ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;

        // Установите вращение части уровня в соответствии с вращением целевой точки привязки.
        // Это начальный шаг для выравнивания ориентации двух частей.
        transform.rotation = targetSnapPoint.transform.rotation;

        //Поверните часть уровня на 180 градусов вокруг оси Y. Это необходимо, поскольку точки привязки
        //обычно направлены в противоположные стороны, и этот поворот выравнивает их так, чтобы они правильно смотрели друг на друга.
        transform.Rotate(0, 180, 0);

        // Примените рассчитанное ранее смещение. Этот шаг позволяет точно настроить выравнивание путем регулировки
        // вращения части уровня, чтобы учесть начальную разницу в ориентации между уровнем
        // точкой привязки детали и основной частью детали.
        transform.Rotate(0, -rotationOffset, 0);
    }

    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Рассчитывыем смещение между текущим положением деталей уровня
        // и положением собственной точки привязки. Это смещение представляет собой
        // расстояние и направление от точки поворота детали уровня до точки привязки.
        var offset = transform.position - ownSnapPoint.transform.position;

        // Определяем новое положение детали уровня.Оно рассчитывается по формуле
        // добавления ранее вычисленного смещения к положению целевой точки привязки.
        // В результате деталь уровня перемещается так, что точка привязки выравнивается
        // с положением целевой точки привязки.
        var newPosition = targetSnapPoint.transform.position + offset;

        // Обновляем положение детали уровня до нового рассчитанного положения с помощью точек привязки.
        transform.position = newPosition;
    }

    public SnapPoint GetEntrancePoint() => GetSnapPointOfType(SnapPointType.Enter);
    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);

    private SnapPoint GetSnapPointOfType(SnapPointType pointType)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoints = new List<SnapPoint>();

        // собрать все точки привязки специфицированного типа
        foreach (SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.pointType == pointType)
            {
                filteredSnapPoints.Add(snapPoint);
            }
        }

        // если есть совпадающие точки привязки, выберите одну наугад
        if (filteredSnapPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredSnapPoints.Count);
            return filteredSnapPoints[randomIndex];
        }

        // 
        return null;
    }
}
