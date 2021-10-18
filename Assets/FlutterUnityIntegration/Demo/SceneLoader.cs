using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // mMessenger = new UnityMessageManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int idx)
    {
        Debug.Log("scene = " + idx);
        SceneManager.LoadScene(idx, LoadSceneMode.Single);
    }

    public void MessengerFlutter()
    {

        UnityMessageManager.Instance.SendMessageToFlutter("Hey man");
    }

    public void SwitchNative()
    {
        UnityMessageManager.Instance.ShowHostMainWindow();
    }

    public void UnloadNative()
    {
        UnityMessageManager.Instance.UnloadMainWindow();
    }

    public void QuitNative()
    {
        UnityMessageManager.Instance.QuitUnityWindow();
    }
}
