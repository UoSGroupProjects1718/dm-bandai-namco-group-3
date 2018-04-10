using System.Collections;
using UnityEngine;
using System;
using System.Collections;

public class CameraPanTask : MonoBehaviour, ICoroutineTask
{
    private const string CameraAnimState = "State";
    private const int CameraAnimStateIdle = 0;
    private const int CameraAnimStateFocusRed = 1;
    private const int CameraAnimStateReturnRed = 2;
    private const int CameraAnimStateFocusBlue = 3;
    private const int CameraAnimStateReturnBlue = 4;
    
    private Animator _cameraAnimator;
    
    public bool IsComplete { get; set; }
    
    public void SetAnimator(Animator cameraAnimator)
    {
        _cameraAnimator = cameraAnimator;
    }

    private IEnumerator PerformAnimation(int desiredState)
    {
        IsComplete = false;
        _cameraAnimator.SetInteger(CameraAnimState, desiredState);
        yield return new WaitForSeconds(_cameraAnimator.GetCurrentAnimatorClipInfo(0).Length);
        _cameraAnimator.SetInteger(CameraAnimState, CameraAnimStateIdle);
        IsComplete = true;
    }

    public void PanToRedCastle()
    {
        StartCoroutine(PerformAnimation(CameraAnimStateFocusRed));
    }

    public void PanFromRedCastle()
    {
        StartCoroutine(PerformAnimation(CameraAnimStateReturnRed));
    }

    public void PanToBlueCastle()
    {
        StartCoroutine(PerformAnimation(CameraAnimStateFocusBlue));
    }

    public void PanFromBlueCastle()
    {
        StartCoroutine(PerformAnimation(CameraAnimStateReturnBlue));
    }
}