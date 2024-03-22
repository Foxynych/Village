using System.Collections;
using UnityEngine;

public class LevelTouches : MonoBehaviour
{
    public GameObject Cam;

    private Vector3 oldPosition;

    private void FixedUpdate()
    {
        if (Input.touchCount == 1) //передвижение камеры
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.deltaPosition.y > 0)
                    StartCoroutine(Translate(new Vector3(0f, 0f, -0.05f)));

                else if (touch.deltaPosition.y < 0)
                    StartCoroutine(Translate(new Vector3(0f, 0f, 0.05f)));
            }

        }
    }

    IEnumerator Translate(Vector3 speed)
    {
        if (Input.touchCount == 1 && -237f <= Cam.transform.position.z && Cam.transform.position.z <= -62f)
        {
            oldPosition = Cam.transform.position;
            Cam.transform.position += speed;

            yield return new WaitForFixedUpdate();
            StartCoroutine(Translate(speed));
        }

        else
            Cam.transform.position = oldPosition;
    }
}
