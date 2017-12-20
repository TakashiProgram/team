using UnityEngine;

public class ResultStop : MonoBehaviour {
   

    public void Display()
    {
        this.GetComponent<Animator>().enabled = true;
    }

    private void Stop()
    {
        this.GetComponent<Animator>().speed = 0;
    }
}
