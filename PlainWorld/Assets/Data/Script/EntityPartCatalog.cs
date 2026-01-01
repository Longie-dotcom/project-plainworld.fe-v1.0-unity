using Assets.Data.Enum;
using Assets.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartEntry
{
    public string ID;
    public EntityPartFrame Frame;
}

public readonly struct PartDescriptor
{
    public readonly string ID;
    public readonly string Name;

    public PartDescriptor(string id, string name)
    {
        ID = id;
        Name = name;
    }
}

[CreateAssetMenu(
    fileName = "EntityPartCatalog",
    menuName = "Animation/EntityPartCatalog"
)]
public class EntityPartCatalog : ScriptableObject
{
    #region Attributes
    [Header("Catalog Info")]
    [SerializeField] private EntityPartType partType;

    [Header("Parts")]
    [SerializeField] private List<PartEntry> parts = new();

    private Dictionary<string, EntityPartFrame> lookup;
    #endregion

    #region Properties
    public EntityPartType PartType
    {
        get { return partType; }
    }
    #endregion

    #region Methods
    private void OnEnable()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<string, EntityPartFrame>();

        foreach (var entry in parts)
        {
            if (entry == null)
                continue;

            if (string.IsNullOrEmpty(entry.ID))
                continue;

            if (entry.Frame == null)
            {
                GameLogger.Warning(
                    Channel.System,
                    $"Null frame for ID '{entry.ID}' in {name}");
                continue;
            }

            if (entry.Frame.EntityPartType != partType)
            {
                GameLogger.Warning(
                    Channel.System,
                    $"Frame '{entry.Frame.name}' has type {entry.Frame.EntityPartType} " +
                    $"but is in {partType} catalog");
                continue;
            }

            if (!lookup.TryAdd(entry.ID, entry.Frame))
            {
                GameLogger.Warning(
                    Channel.System,
                    $"Duplicate ID '{entry.ID}' in {name}");
            }
        }
    }

    public EntityPartFrame GetPartFrame(string id)
    {
        if (lookup == null)
            BuildLookup();

        if (lookup.TryGetValue(id, out var frame))
            return frame;

        GameLogger.Warning(
            Channel.System,
            $"ID '{id}' not found in {partType} catalog");

        return null;
    }

    public IReadOnlyList<PartDescriptor> GetDescriptors()
    {
        if (lookup == null)
            BuildLookup();

        var list = new List<PartDescriptor>(parts.Count);

        foreach (var entry in parts)
        {
            if (entry == null || entry.Frame == null)
                continue;

            list.Add(new PartDescriptor(
                entry.ID,
                entry.Frame.EntityPartName
            ));
        }

        return list;
    }

    public EntityPartFrame GetDefault()
    {
        if (parts == null || parts.Count == 0)
            return null;

        var entry = parts[0];
        return entry != null ? entry.Frame : null;
    }
    #endregion
}
