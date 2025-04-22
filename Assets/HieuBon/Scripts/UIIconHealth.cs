using UnityEngine;

namespace Hunter
{
    public class IconHealth : MonoBehaviour
    {
        void Update()
        {
            if (GameController.instance != null)
            {
                transform.LookAt(new Vector3(transform.position.x, UIInGame.instance.virtualCam.cam.transform.position.y, UIInGame.instance.virtualCam.cam.transform.position.z));
            }
        }
    }
}
