using UnityEngine;

public class SceneActions : MonoBehaviour
{
    public void GoMain()  { GameManager.instance.GoMain();  }
    public void GoOffice(){ GameManager.instance.GoOffice(); }
    public void GoRoom()  { GameManager.instance.GoRoom();  }
}