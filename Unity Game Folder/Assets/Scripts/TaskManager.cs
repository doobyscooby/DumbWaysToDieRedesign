using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{

    bool randomTasks = false;

    TextMeshPro notepadText;
    
    // Todays Current Tasks
    private Task[] currentTasks;
    public Task[] CurrentTasks { get => currentTasks; }
    public Task[] beforeTransitionTasks;
    public Task[] afterTransitionTasks;


    private void Awake()
    {
        SetUp();
        ResetAllTraps();
        if (randomTasks)
        {
            throw new NotImplementedException("RandomTasks not implemented yet");
        }
        if (currentTasks.Length == 0)
        {
            throw new Exception("Not Enough Traps in GameManager");
        }
    }
    public void SetUp()
    {
        currentTasks = beforeTransitionTasks;
    }

    public void SwapTasksOver(bool time = true)
    {
        currentTasks = afterTransitionTasks;
        ResetAllTraps();
        if (currentTasks.Length == 0)
        {
            throw new Exception("Not Enough Traps in GameManager");
        }
        if (time)
            DynamicSky.Instance.AdvanceTime();
    }

    public void ResetAllTraps()
    {
        foreach(var task in currentTasks)
        {
            task.Reset();
        }
    }
    public void FindNotepadText()
    {
        notepadText = GameObject.Find("Notepad Text").GetComponent<TextMeshPro>();
        Debug.Log("Found Notepad Text");
        GameObject.Find("Notepad").SetActive(false);
        UpdateNotepad();
        Debug.Log("Set Notepad Text");
    }

    /* AllTasksComplete
    * Are tasks complete? Returns true/false if all tasks excluding final tasks are complete.
    */
    public bool AllTasksComplete()
    {
        foreach(var task in currentTasks)
        {
            if (task.taskComplete == false && !task.isDependent)
            {
                return false;
            }
        }
        return true;
    }
    /* UpdateNotepad
    * Updates notepad with any new text added
    */
    private void UpdateNotepad()
    {
        string newText = "";
        // First Task
        if (currentTasks[0].stepsComplete >= currentTasks[0].steps)
            newText = newText + "<s>";
         newText = newText + "1. " + currentTasks[0].taskName;
 
        if (currentTasks[0].steps > 1)
            newText = newText + " (" + currentTasks[0].stepsComplete + " / " + currentTasks[0].steps + ")";
        if (currentTasks[0].stepsComplete >= currentTasks[0].steps)
            newText = newText + "</s>";
        // New Tasks
        for (int i = 1; i < currentTasks.Length; i++)
        {
            newText = newText + "\n";
            if (currentTasks[i].stepsComplete >= currentTasks[i].steps)
                newText = newText + "<s>";
            newText = newText + i + ". " + currentTasks[i].taskName;
            if (currentTasks[i].steps > 1)
                newText = newText + " (" + currentTasks[i].stepsComplete + " / " + currentTasks[i].steps + ")";
            if (currentTasks[i].stepsComplete >= currentTasks[i].steps)
                newText = newText + "</s>";
        }
        Debug.Log(currentTasks[0].taskName);
        Debug.Log(newText);
        notepadText.text = newText;
    }

    /* UpdateTaskCompletion
     * Takes in the task name and updates the task, throws exception if task does not exist on 
     * either the notepad or the current task list.
     */
    public void UpdateTaskCompletion(string taskName)
    {
        foreach (var task in currentTasks)
        {
            if (taskName == task.taskName)
            {
                task.stepsComplete++;
                if (task.stepsComplete >= task.steps)
                {
                    // Play time pass SFX
                    //DynamicSky.Instance.transform.GetComponent<AudioSource>().Play();
#pragma warning disable CS0642 // Possible mistaken empty statement
                    if (notepadText.text.Replace(" ", "").Contains(task.taskName.Replace(" ", ""))) ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                    {
                        GameUI.Instance.NotifyAnim.SetTrigger("Notify");
                        task.taskComplete = true;
                        UpdateNotepad();
                        return;
                    }
                        throw new Exception("Trying to complete task that is not on notepad");
                }
                UpdateNotepad();
                return;
            }
        }
        throw new Exception("Trying to complete task that does not exist");
    }

    public Task GetTask(string taskName)
    {
        foreach (var task in currentTasks)
        {
            if (taskName == task.taskName)
            {
                return task;
            }
        }
        return null;
    }

    public override string ToString()
    {
        return "";
    }
}
