using System.Collections;
using UnityEngine;
using static EventAgregator;

public class Touches : MonoBehaviour
{
    private Vector3 oldPosition;
    private float oldView;

    private void FixedUpdate()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.deltaPosition.x > 0)
                    StartCoroutine(Translate(new Vector3(0.002f, 0f)));
                else
                    StartCoroutine(Translate(new Vector3(-0.002f, 0f)));

                if (touch.deltaPosition.y > 0)
                    StartCoroutine(Translate(new Vector3(0f, 0f, 0.002f)));
                else
                    StartCoroutine(Translate(new Vector3(0f, 0f, -0.002f)));
            }

        }

        else if (Input.touchCount == 2)
        {
            if (hubManager.cam.GetComponentInChildren<Camera>().fieldOfView >= 21f && hubManager.cam.GetComponentInChildren<Camera>().fieldOfView <= 50f)
            {
                oldView = hubManager.cam.GetComponentInChildren<Camera>().fieldOfView;

                Touch
                    touch_1 = Input.GetTouch(0),
                    touch_2 = Input.GetTouch(1);
                Vector3
                    dir_1 = touch_1.position - touch_1.deltaPosition,
                    dir_2 = touch_2.position - touch_2.deltaPosition;
                float
                    dist = Vector3.Distance(touch_1.position, touch_2.position),
                    deltaDist = Vector3.Distance(dir_1, dir_2);

                if (touch_1.phase == TouchPhase.Moved || touch_2.phase == TouchPhase.Moved)
                {
                    if (dist < deltaDist)
                        hubManager.cam.GetComponentInChildren<Camera>().fieldOfView += 0.5f;
                    else
                        hubManager.cam.GetComponentInChildren<Camera>().fieldOfView -= 0.5f;
                }
            }

            else
                hubManager.cam.GetComponentInChildren<Camera>().fieldOfView = oldView;
        }
    }

    IEnumerator Translate(Vector3 speed)
    {
        if (Input.touchCount == 1 
            && hubManager.cam.transform.position.x >= -40.5f && hubManager.cam.transform.position.x <= 45.5f 
            && hubManager.cam.transform.position.z >= -55.5f && hubManager.cam.transform.position.z <= 34.5f)
        {
            oldPosition = hubManager.cam.transform.position;
            hubManager.cam.transform.position += speed;

            yield return new WaitForFixedUpdate();
            StartCoroutine(Translate(speed));
        }

        else
            hubManager.cam.transform.position = oldPosition;
    }
}