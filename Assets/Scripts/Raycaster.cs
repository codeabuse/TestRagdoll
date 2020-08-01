using UnityEngine;

public class Raycaster : MonoBehaviour
{
    private new Camera camera;
    private int layerMask;
    
    private void Awake()
    {
        camera = GetComponent<Camera>();
        layerMask = 1 << LayerMask.NameToLayer("Enemy");
    }

    public bool DoPointerRaycast(out RaycastHit hit)
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        var objectHit = Physics.Raycast(ray, out hit, 100f, layerMask);
        return objectHit;
    }
}
