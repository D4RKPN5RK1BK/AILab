using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable

/// <summary>
/// <para>Компонент с базовым поиском по типам и интерфейсам</para>
/// <para><c>FindClass - различные поисковики через интерфейсы</c></para>
/// <para><c>HaveClass - проверка на наличие искомого компонента в GameObject</c></para>
/// <para><c>FindObjectByTag - поиск по тэгу в указаном GameObject и его потомках</c></para>
/// </summary>
public class NavigateComponent : MonoBehaviour 
{
    /// <summary>
    /// Поиск класса в переданном объекте
    /// </summary>
    /// <typeparam name="T">Искомый класс</typeparam>
    protected static T? FindClassInObject<T>(GameObject obj) where T : class
    {
        var components = obj.GetComponents<MonoBehaviour>();
        return components.Where(i => i is T).Select(i => (i as T)!).FirstOrDefault();
    }

    /// <summary>
    /// Поиск класса в текущем объекте
    /// </summary>
    /// <typeparam name="T">Искомый класс</typeparam>
    protected T? FindClass<T>() where T : class
    {
        return FindClassInObject<T>(gameObject);
    }

    /// <summary>
    /// Поиск всех указанных экземпляров класса в переданом объекте
    /// </summary>
    /// <typeparam name="T">Искомый класс</typeparam>
    protected static IEnumerable<T> FindClassesInObject<T>(GameObject obj) where T : class
    {
        var components = obj.GetComponents<MonoBehaviour>();
        return components.Where(i => i is T).Select(i => (i as T)!);
    }

    /// <summary>
    /// Поиск всех указанных экземпляров класса в текущем
    /// </summary>
    /// <typeparam name="T">Искомый класс</typeparam>
    protected IEnumerable<T> FindClasses<T>() where T : class
    {
        return FindClassesInObject<T>(gameObject);
    }

    /// <summary>
    /// Поиск классов по типу во всей цепочке родительских объектов, работает рекурсивно
    /// </summary>
    /// <typeparam name="T">Искомый тип объектов</typeparam>
    /// <returns>Список найденых элементов</returns>
    protected IEnumerable<T> FindClassesInParents<T>() where T : class
    {
        var elements = new List<T>();
        var parent = transform.parent;

        while (parent is not null) 
        {
            var components = parent.gameObject.GetComponents<MonoBehaviour>();
            
            elements.AddRange(components.Where(i => i is T).Select(i => (i as T)!));

            parent = parent.parent;
        }

        return elements;
    }

    /// <summary>
    /// Поиск класса в ближайшем родительском компоненте
    /// </summary>
    /// <typeparam name="T">Искомый тип объектов</typeparam>
    /// <returns>Список найденых элементов</returns>
    protected T? FindClassInParents<T>() where T : class
    {
        var parent = transform.parent;

        while (parent is not null) 
        {
            var components = parent.gameObject.GetComponents<MonoBehaviour>();

            if (components.Any(i => i is T))
                return components.Where(i => i is T).Select(i => (i as T)!).First();
                
            parent = parent.parent;
        }

        return null;
    }

    /// <summary>
    /// Поиск классов по всем дочерним объектам, работает рекурсивно
    /// </summary>
    /// <typeparam name="T">Искомый тип объектов</typeparam>
    /// <returns>Список найденых элементов</returns>
    protected IEnumerable<T> FindClassesInChildren<T>() where T : class 
    {
        return FindClassesInChildren<T>(transform);
    }

    /// <summary>
    /// Поиск классов во всей сцене
    /// </summary>
    /// <typeparam name="T">Искомый тип объектов</typeparam>
    /// <returns>Список найденых элементов</returns>
    protected IEnumerable<T> FindClassesInScene<T>() where T : class 
    {
        var elements = new List<T>();
        var allScripts = FindObjectsOfType<MonoBehaviour>();

        for (var i = 0; i < allScripts.Length; i++)
        {
            if (allScripts[i] is T && allScripts[i] as T is not null)
                elements.Add((allScripts[i] as T)!);
        }

        return elements;
    }

    /// <summary>
    /// Поиск вложенного объекта по тэгу, работает рекурсивно
    /// </summary>
    /// <param name="nodeObject">Объект в котором будет проиходить поиск</param>
    /// <param name="otherTag">Искомый тэг</param>
    /// <returns>Список найденных обектов</returns>
    protected List<GameObject> FindObjectByTag(GameObject nodeObject, string otherTag)
    {
        var objects = new List<GameObject>();

        if (nodeObject.CompareTag(otherTag))
            objects.Add(nodeObject);

        for (var i = 0; i < transform.childCount; i++)
            objects.AddRange(FindObjectByTag(transform.GetChild(i).gameObject, tag));

        return objects;
    }

    /// <summary>
    /// Возвращает все вложенные элементы, работает рекурсивно
    /// </summary>
    /// <typeparam name="T">Родительский компонент</typeparam>
    protected static List<T> FindClassesInChildren<T>(Transform parent) where T : class
    {
        var elements = new List<T>();

        var components = parent.gameObject.GetComponents<MonoBehaviour>();

        elements.AddRange(components.Where(i => i is T).Select(i => (i as T)!));

        for (var i = 0; i < parent.childCount; i++) 
            elements.AddRange(FindClassesInChildren<T>(parent.GetChild(i)));

        return elements;
    }

    /// <summary>
    /// Проверить есть ли в указанном объекте требуемый компонент
    /// </summary>
    /// <param name="target">Целевой объект</param>
    /// <typeparam name="T">Искомый тип компонента</typeparam>
    /// <returns></returns>
    protected static bool HaveClass<T>(GameObject target) where T : class
    {
        var components = target.GetComponents<MonoBehaviour>();
        return components.Any(i => i is T);
    }
}

#nullable disable