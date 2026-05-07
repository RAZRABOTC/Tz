using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;

    public void StartCutscene()
    {
        _director.Play();
        _director.stopped += OnCutsceneEnd; 
    }

    private void OnCutsceneEnd(PlayableDirector director)
    { 
        _director.stopped -= OnCutsceneEnd;
    }
    public void SkipCutscene()
    {
        _director.Stop();
    }
}