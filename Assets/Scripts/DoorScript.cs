using System.Collections;
using UnityEngine;
public class DoorScript : MonoBehaviour
{
    private bool _opened = false;
    private bool _canChange = true;
    [SerializeField] GameObject _doorPivot;
    public void ChangeState()
    {
        if (_canChange == false) return;
        _opened = !_opened;

        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        _canChange = false;
        if (_opened == true)
        {
            float rotation = _doorPivot.transform.rotation.eulerAngles.y + 90;
            for (int i = 0; i < 90; i++)
            {
                yield return new WaitForFixedUpdate();
                _doorPivot.transform.rotation = Quaternion.Lerp(_doorPivot.transform.rotation, Quaternion.Euler(0, rotation, 0), Time.deltaTime * 10);
            }
        }
        else
        {
            float rotation = _doorPivot.transform.rotation.eulerAngles.y - 90;
            for (int i = 0; i < 90; i++)
            {
                yield return new WaitForFixedUpdate();
                _doorPivot.transform.rotation = Quaternion.Lerp(_doorPivot.transform.rotation, Quaternion.Euler(0, rotation, 0), Time.deltaTime * 10);
            }
        }
        _canChange = true;
    }
}
