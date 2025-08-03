using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Animator _transitionAnimator;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (_transitionAnimator != null)
        {
            if (!_transitionAnimator.gameObject.activeSelf)
            {
                _transitionAnimator.gameObject.SetActive(true);
            }
        }
    }

    public void LoadScene(int TargetScene)
    {
        if (TargetScene > SceneManager.sceneCountInBuildSettings || TargetScene < 0) 
        {
            Debug.LogError("TargetScene index is invalid:" + TargetScene + ">" + SceneManager.sceneCount);
            return;
        }
        StartCoroutine(ExecuteLoadScene(TargetScene));
    }

    private IEnumerator ExecuteLoadScene(int _targetScene)
    {
        if (_transitionAnimator != null)
        {
            //trigger Transition
               
            _transitionAnimator.SetTrigger("End");
            if (_audioSource != null)
            {
                while (_audioSource.volume > 0.1f) 
                {
                    _audioSource.volume -= 0.1f;
                    yield return null;
                }
            }
            
        }  
        yield return new WaitForSeconds(.8f);
        SceneManager.LoadScene(_targetScene);
    }
}
