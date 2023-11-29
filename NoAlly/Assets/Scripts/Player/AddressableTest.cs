using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

public class AddressableTest : MonoBehaviour
{
    [SerializeField]
    Transform pos = null;

    AssetReference reference = new AssetReference("Assets/Weapon/PlayerWeapons/Prefab/ShieldWall.prefab");
    // アンロードに必要
    AsyncOperationHandle handle;

    void Start()
    {
        LoadAsset();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            UnloadAsset();
        }
    }

    void LoadAsset()
    {
        handle = reference.InstantiateAsync(pos);
    }

    void UnloadAsset()
    {
        if (!handle.IsValid())
        {
            return;
        }

        Addressables.ReleaseInstance(handle);
    }

}