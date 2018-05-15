using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    //Public Array of Layers (int)
    public Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Walkable
    };

    //May Need Adjustments Depending on Final Game Size, hence the SerializeField. Allows Script Specific adjustments, without making values public
    [SerializeField] float distanceToBackground = 100f;
    Camera viewCamera;

    //Here we set a RaycastHit var, which can be called by the "hit" method
    RaycastHit m_hit;
    public RaycastHit hit
    {
        get { return m_hit; }
    }

    Layer m_layerHit;
    public Layer layerHit
    {
        get { return m_layerHit; }
    }

    void Start() // TODO Awake?
    {
        viewCamera = Camera.main;
    }

    void Update()
    {
        // Priority Layer return following hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                m_hit = hit.Value;
                m_layerHit = layer;
                return;
            }
        }

        // Else background hit
        m_hit.distance = distanceToBackground;
        m_layerHit = Layer.RaycastEndStop;
    }

    RaycastHit? RaycastForLayer(Layer layer)//Doesn't have to return a value due to "?" after RaycastHit
    {
        int layerMask = 1 << (int)layer; // Unity standard set initial Bit Shift
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);//This activates Ray out of screen

        RaycastHit hit; 
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);//Takes ray value, and impresses it on the hit parameter, then give the hit a maximum travel distance and alayer assignment
        if (hasHit)
        {
            //print("Hit");
            return hit;
        }
        //else
        return null;
    }
}
