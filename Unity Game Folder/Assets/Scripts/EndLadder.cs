using UnityEngine;
using UnityEngine.Video;

public class EndLadder : Interactable
{
    [SerializeField]
    VideoPlayer video;

    public bool interactable = false;

    public override void Action()
    {
        if (!interactable)
            return;
        video.enabled = true;
        GameManager.Instance.taskManager.UpdateTaskCompletion("Escape");
        GameManager.Instance.EnableCamera = false;
        GameManager.Instance.EnableControls = false;
        video.loopPointReached += EndGame;
    }
    void EndGame(VideoPlayer vp)
    {
        GameManager.Instance.MoveToNextLevel();
    }
}
