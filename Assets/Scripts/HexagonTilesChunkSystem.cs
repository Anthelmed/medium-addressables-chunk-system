using System;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTilesChunkSystem : MonoBehaviour
{
    [SerializeField] private List<HexagonTile> tiles;
    [SerializeField] private float tileRadius = 0.6f;
    [SerializeField] private Camera mainCamera;

    private CullingGroup _cullingGroup;
    private BoundingSphere[] _boundingSpheres = new BoundingSphere[0];

    private void OnEnable()
    {
        InitCullingGroup();
    }

    private void OnDisable()
    {
        _cullingGroup.Dispose();
    }

    private void InitCullingGroup()
    {
        _cullingGroup = new CullingGroup();
        _boundingSpheres = new BoundingSphere[tiles.Count];

        for (var i = 0; i < _boundingSpheres.Length; i++)
        {
            _boundingSpheres[i].position = tiles[i].transform.position;
            _boundingSpheres[i].radius = tileRadius;
        }
        
        _cullingGroup.SetBoundingSpheres(_boundingSpheres);
        _cullingGroup.targetCamera = mainCamera;
        
        _cullingGroup.onStateChanged = OnStateChanged;
    }
    
    private void OnStateChanged(CullingGroupEvent evt)
    {
        if (evt.hasBecomeVisible)
            tiles[evt.index].InstantiateTileAsset();
        
        if(evt.hasBecomeInvisible)
            tiles[evt.index].ReleaseTileAsset();
    }

    private void OnDrawGizmos()
    {
        for (var i = 0; i < _boundingSpheres.Length; i++)
        {
            var boundingSphere = _boundingSpheres[i];
            var sphereIsVisible = _cullingGroup.IsVisible(i);

            Gizmos.color = sphereIsVisible ? Color.green : Color.red;
            Gizmos.DrawWireSphere(boundingSphere.position, boundingSphere.radius);
        }
    }
}
