using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class HexagonTile : MonoBehaviour
{
    [SerializeField] private AssetReference assetReference;

    private AsyncOperationHandle<GameObject> _operationHandle;
    private GameObject _tileAsset;
    
    public async Task InstantiateTileAsset()
    {
        if (_tileAsset != null) return;
        
        _operationHandle = assetReference.InstantiateAsync(transform.position, transform.rotation, transform);

        await _operationHandle.Task;

        _tileAsset = _operationHandle.Result;
    }

    public void ReleaseTileAsset()
    {
        if (_tileAsset == null) return;
        
        Addressables.ReleaseInstance(_operationHandle);
        Addressables.ReleaseInstance(_tileAsset);
    }
    
    [ContextMenu("FindAssetReferenceByName")]
    private void FindAssetReferenceByName()
    {
        var path = $"Assets/Prefabs/HexagonTiles/{transform.name}.prefab";
        var asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        assetReference.SetEditorAsset(asset);
    }
}
