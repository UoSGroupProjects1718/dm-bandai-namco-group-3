using System;
using System.Collections;
using System.Collections.Generic;
using Exploder2D;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private const string PlatformAnimState = "State";
    private const int PlatformAnimStateFadeOut = 1;
    private const int PlatformAnimStateFadeIn = 2;
    
    private const string BtnAnimState = "State";
    private const int BtnAnimStateZoomIn = 1;
    private const int BtnAnimStateZoomOut = 2;
    
    private const string TextAnimState = "State";
    private const int TextAnimStateZoomIn = 1;
    private const int TextAnimStateZoomOut = 2;
    
    private const string CameraAnimState = "State";
    private const int CameraAnimStateIdle = 0;
    private const int CameraAnimStateFocusRed = 1;
    private const int CameraAnimStateReturnRed = 2;
    private const int CameraAnimStateFocusBlue = 3;
    private const int CameraAnimStateReturnBlue = 4;
    
    private const int PlatformShapeVariations = 4;
    private const int PlatformMaterialVariations = 3;
    
    [Range(1, PlatformShapeVariations * PlatformMaterialVariations)] public int PlatformQuantityPerPlayer;

    [Range(1, PlatformShapeVariations)] public int StoneQuantityMax;
    [SerializeField] private Text _txtStoneQuantity;
    [SerializeField] private Text _txtStoneQuantityPanel;
    [SerializeField] private Button _btnStone;

    [Range(1, PlatformShapeVariations)] public int WoodQuantityMax;
    [SerializeField] private Text _txtWoodQuantity;
    [SerializeField] private Text _txtWoodQuantityPanel;
    [SerializeField] private Button _btnWood;

    [Range(1, PlatformShapeVariations)] public int GlassQuantityMax;
    [SerializeField] private Text _txtGlassQuantity;
    [SerializeField] private Text _txtGlassQuantityPanel;
    [SerializeField] private Button _btnGlass;

    [SerializeField] private Text _topText;
    [SerializeField] private Text _bottomText;

    [SerializeField] private Animator _animTopText;
    [SerializeField] private Animator _animBottomText;

    [SerializeField] private Animator _animRedAckButton;
    [SerializeField] private Animator _animBlueAckButton;
    [SerializeField] private Animator _animRedDoneButton;
    [SerializeField] private Animator _animBlueDoneButton;
    
    [SerializeField] private Scroll _scroll;

    [SerializeField] private GameObject _cannonObject;

    [SerializeField] private bool _acknowledgementReceived;

    [SerializeField] private GameObject _placedPlatforms;

    [SerializeField] private Castle _redCastle;
    [SerializeField] private Castle _blueCastle;
        
    [SerializeField] private Flag _redFlag1;
    [SerializeField] private Flag _redFlag2;

    [SerializeField] private Flag _blueFlag1;
    [SerializeField] private Flag _blueFlag2;

    private bool gameActive;

    public class PlayerData
    {
        public enum PlayerTeam
        {
            Red,
            Blue
        };
        
        public PlayerTeam Team;
        public int StoneQuantity;
        public int WoodQuantity;
        public int GlassQuantity;

        public PlayerData(PlayerTeam team, int stoneQuantity, int woodQuantity, int glassQuantity)
        {
            Team = team;
            StoneQuantity = stoneQuantity;
            WoodQuantity = woodQuantity;
            GlassQuantity = glassQuantity;
        }
    }

    public PlayerData ActivePlayer;

    private PlayerData _redTeam;
    private PlayerData _blueTeam;

    private Animator _cameraAnimator;
    
    private ScoreBoard _scoreBoard = new ScoreBoard();

    private void GenerateRandomPlatformQuantities()
    {
        var stoneQuantity = 0;
        var woodQuantity = 0;
        var glassQuantity = 0;

        var platformQuantityLeft = PlatformQuantityPerPlayer;
        while (platformQuantityLeft != 0)
        {
            var randomChoice = Random.Range(0, PlatformMaterialVariations);
            switch (randomChoice)
            {
                    case 0:
                        if (stoneQuantity != StoneQuantityMax)
                        {
                            stoneQuantity++;
                            platformQuantityLeft--;
                        }
                        break;
                    case 1:
                        if (woodQuantity != WoodQuantityMax)
                        {
                            woodQuantity++;
                            platformQuantityLeft--;
                        }
                        break;
                    case 2:
                        if (glassQuantity != GlassQuantityMax)
                        {
                            glassQuantity++;
                            platformQuantityLeft--;
                        }
                        break;
            }
        }
        
        _redTeam = new PlayerData(PlayerData.PlayerTeam.Red, stoneQuantity, woodQuantity, glassQuantity);
        _blueTeam = new PlayerData(PlayerData.PlayerTeam.Blue, stoneQuantity, woodQuantity, glassQuantity);

        ActivePlayer = _redTeam;
    }

    public void ReceiveAcknowledgement()
    {
        _acknowledgementReceived = true;
    }

    private void Awake()
    {
        _cameraAnimator = Camera.main.GetComponent<Animator>();
    }
    
    private void Start()
    {
        GenerateRandomPlatformQuantities();
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {

        // SHOW SCORE FLAGS IF APPLICABLE

        if (_scoreBoard.GetRedScore() >= 2 || _scoreBoard.GetBlueScore() >= 2)
        {
            _scoreBoard.ResetScores();
        }
        
        if(_scoreBoard.GetRedScore() == 1)
            _redFlag1.Show();
        if(_scoreBoard.GetBlueScore() == 1)
            _blueFlag1.Show(); 
        
        
//        // --- RED TEAM SETUP
//        
//        // PAN TO RED CASTLE
//        _cameraAnimator.SetInteger(CameraAnimState, CameraAnimStateFocusRed);
//        yield return new WaitForSeconds(_cameraAnimator.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // ZOOM IN RED TEAM TEXT
//        _topText.text = "RED TEAM";
//        _topText.color = new Color(255.0f / 255.0f, 77.0f / 255.0f, 77.0f / 255.0f);
//        
//        _animTopText.SetInteger(TextAnimState, TextAnimStateZoomIn);
//        yield return new WaitForSeconds(_animTopText.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // ZOOM IN ARE YOU READY TEXT
//        _bottomText.text = "ARE YOU READY?";
//        _bottomText.color = Color.black;
//        
//        _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomIn);
//        yield return new WaitForSeconds(_animBottomText.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // ZOOM IN THE ACKNOWLEDGEMENT BUTTON
//        _animRedAckButton.SetInteger(BtnAnimState, BtnAnimStateZoomIn);
//
//        // WAIT FOR THE ACKNOWLEDGEMENT BUTTON TO BE PRESSED
//        _acknowledgementReceived = false;
//        while (!_acknowledgementReceived)
//            yield return null;
//        
//        // ZOOM OUT THE ACKNOWLEDGEMENT BUTTON
//        _animRedAckButton.SetInteger(BtnAnimState, BtnAnimStateZoomOut);
//                
//        // ZOOM OUT THE TEXT
//        _animTopText.SetInteger(TextAnimState, TextAnimStateZoomOut);
//        _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomOut);
//        yield return new WaitForSeconds(_animTopText.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // PAN THE CAMERA BACK TO THE CENTER
//        _cameraAnimator.SetInteger(CameraAnimState, CameraAnimStateReturnRed);
//        yield return new WaitForSeconds(_cameraAnimator.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // ZOOM IN THE BUILD YOUR DEFENCES TEXT
//        _bottomText.text = "BUILD YOUR DEFENCES!";
//        _bottomText.color = Color.black;
//        
//        _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomIn);
//        yield return new WaitForSeconds(_animBottomText.GetCurrentAnimatorClipInfo(0).Length);
//
//        yield return new WaitForSeconds(1.5f);
//        
//        // ZOOM OUT THE BUILD YOUR DEFENCES TEXT
//        _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomOut);
//        yield return new WaitForSeconds(_animBottomText.GetCurrentAnimatorClipInfo(0).Length);
//
//        // DROP DOWN THE PLATFORM SCROLL
//        _scroll.DropdownPeak();
//
//        // WAIT FOR THE PLATFORMS TO BE PLACED
//        while (_placedPlatforms.transform.childCount != PlatformQuantityPerPlayer)
//            yield return null;
//
//        // HIDE THE PLATFORM SCROLL
//        _scroll.ShowMaterialPanel();
//        _scroll.Hide();
//
//        // ZOOM IN THE ACKNOWLEDGEMENT BUTTON
//        _animRedDoneButton.SetInteger(BtnAnimState, BtnAnimStateZoomIn);
//
//        // WAIT FOR THE ACKNOWLEDGEMENT BUTTON TO BE PRESSED
//        _acknowledgementReceived = false;
//        while (!_acknowledgementReceived)
//            yield return null;
//        
//        // ZOOM OUT THE ACKNOWLEDGEMENT BUTTON
//        _animRedDoneButton.SetInteger(BtnAnimState, BtnAnimStateZoomOut);
//        yield return new WaitForSeconds(_animRedDoneButton.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // FADE OUT THE PLATFORMS
//        foreach (Transform child in _placedPlatforms.transform)
//        {
//            child.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeOut);
//        }
//        
//        // -- BLUE TEAM SETUP
//
//        ActivePlayer = _blueTeam;
//        
//        // PAN TO BLUE CASTLE
//        _cameraAnimator.SetInteger(CameraAnimState, CameraAnimStateFocusBlue);
//        yield return new WaitForSeconds(_cameraAnimator.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // ZOOM IN BLUE TEAM TEXT
//        _topText.text = "BLUE TEAM";
//        _topText.color = new Color(0.0f / 255.0f, 157.0f / 255.0f, 220.0f / 255.0f);
//        
//        _animTopText.SetInteger(TextAnimState, TextAnimStateZoomIn);
//        yield return new WaitForSeconds(_animTopText.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // ZOOM IN ARE YOU READY TEXT
//        _bottomText.text = "ARE YOU READY?";
//        _bottomText.color = Color.black;
//        
//        _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomIn);
//        yield return new WaitForSeconds(_animBottomText.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // ZOOM IN THE ACKNOWLEDGEMENT BUTTON
//        _animBlueAckButton.SetInteger(BtnAnimState, BtnAnimStateZoomIn);
//
//        // WAIT FOR THE ACKNOWLEDGEMENT BUTTON TO BE PRESSED
//        _acknowledgementReceived = false;
//        while (!_acknowledgementReceived)
//            yield return null;
//        
//        // ZOOM OUT THE ACKNOWLEDGEMENT BUTTON
//        _animBlueAckButton.SetInteger(BtnAnimState, BtnAnimStateZoomOut);
//        yield return new WaitForSeconds(_animBlueAckButton.GetCurrentAnimatorClipInfo(0).Length);
//
//        // ZOOM OUT THE TEXT
//        _animTopText.SetInteger(TextAnimState, TextAnimStateZoomOut);
//        _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomOut);
//        yield return new WaitForSeconds(_animTopText.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // PAN THE CAMERA BACK TO THE CENTER
//        _cameraAnimator.SetInteger(CameraAnimState, CameraAnimStateReturnBlue);
//        yield return new WaitForSeconds(_cameraAnimator.GetCurrentAnimatorClipInfo(0).Length);
//        
//        // ZOOM IN THE BUILD YOUR DEFENCES TEXT
//        _bottomText.text = "BUILD YOUR DEFENCES!";
//        _bottomText.color = Color.black;
//        
//        _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomIn);
//        yield return new WaitForSeconds(_animBottomText.GetCurrentAnimatorClipInfo(0).Length);
//
//        yield return new WaitForSeconds(1.5f);
//        
//        // ZOOM OUT THE BUILD YOUR DEFENCES TEXT
//        _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomOut);
//        yield return new WaitForSeconds(_animBottomText.GetCurrentAnimatorClipInfo(0).Length);
//
//        // DROP DOWN THE PLATFORM SCROLL
//        _scroll.DropdownPeak();
//
//        // WAIT FOR THE PLATFORMS TO BE PLACED
//        while (_placedPlatforms.transform.childCount != PlatformQuantityPerPlayer * 2)
//            yield return null;
//
//        // HIDE THE PLATFORM SCROLL
//        _scroll.ShowMaterialPanel();
//        _scroll.Hide();
//
//        // ZOOM IN THE ACKNOWLEDGEMENT BUTTON
//        _animBlueDoneButton.SetInteger(BtnAnimState, BtnAnimStateZoomIn);
//
//        // WAIT FOR THE ACKNOWLEDGEMENT BUTTON TO BE PRESSED
//        _acknowledgementReceived = false;
//        while (!_acknowledgementReceived)
//            yield return null;
//        
//        // ZOOM OUT THE ACKNOWLEDGEMENT BUTTON
//        _animBlueDoneButton.SetInteger(BtnAnimState, BtnAnimStateZoomOut);
        
        // FADE IN THE PLATFORMS
        foreach (Transform child in _placedPlatforms.transform)
        {
            child.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeIn);
        }
        
//        _cannonObject.SetActive(true);
//        var gameObject = _cannonObject.GetComponentInChildren(typeof(Cannon));
//        ((Cannon) gameObject).Active = true;

        while (!_redCastle.Exploded && !_blueCastle.Exploded) 
            yield return null;
        
        yield return new WaitForSeconds(1.0f);

        if (_blueCastle.Exploded)
        {
            // RED TEAM WINS
            _scoreBoard.SetRedScore(_scoreBoard.GetRedScore() + 1);
            
            // PAN TO RED CASTLE
            _cameraAnimator.SetInteger(CameraAnimState, CameraAnimStateFocusRed);
            yield return new WaitForSeconds(_cameraAnimator.GetCurrentAnimatorClipInfo(0).Length);
            
            // ZOOM IN RED TEAM TEXT
            _topText.text = "RED TEAM";
            _topText.color = new Color(255.0f / 255.0f, 77.0f / 255.0f, 77.0f / 255.0f);
            
            _animTopText.SetInteger(TextAnimState, TextAnimStateZoomIn);
            
            // ZOOM IN WINS TEXT
            _bottomText.text = "WINS THE ROUND!";
            _bottomText.color = Color.black;
            
            _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomIn);
            
            yield return new WaitForSeconds(_animBottomText.GetCurrentAnimatorClipInfo(0).Length);
            
            if(_scoreBoard.GetRedScore() == 1)
                _redFlag1.Rise();
            else
                _redFlag2.Rise();
            
            yield return new WaitForSeconds(2.0f);
            
            // ZOOM OUT THE TEXT
            _animTopText.SetInteger(TextAnimState, TextAnimStateZoomOut);
            _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomOut);
            
            yield return new WaitForSeconds(_animTopText.GetCurrentAnimatorClipInfo(0).Length);
            
            // PAN THE CAMERA BACK TO THE CENTER
            _cameraAnimator.SetInteger(CameraAnimState, CameraAnimStateReturnRed);
            yield return new WaitForSeconds(_cameraAnimator.GetCurrentAnimatorClipInfo(0).Length);
            
        }
        else
        {
            // BLUE TEAM WINS
            _scoreBoard.SetBlueScore(_scoreBoard.GetBlueScore() + 1);
            
            // PAN TO BLUE CASTLE
            _cameraAnimator.SetInteger(CameraAnimState, CameraAnimStateFocusBlue);
            yield return new WaitForSeconds(_cameraAnimator.GetCurrentAnimatorClipInfo(0).Length);
            
            // ZOOM IN BLUE TEAM TEXT
            _topText.text = "BLUE TEAM";
            _topText.color = new Color(0.0f / 255.0f, 157.0f / 255.0f, 220.0f / 255.0f);
            
            _animTopText.SetInteger(TextAnimState, TextAnimStateZoomIn);
            
            // ZOOM IN WINS TEXT
            _bottomText.text = "WINS THE ROUND!";
            _bottomText.color = Color.black;
            
            _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomIn);
            
            yield return new WaitForSeconds(_animBottomText.GetCurrentAnimatorClipInfo(0).Length);
            
            if(_scoreBoard.GetBlueScore() == 1)
                _blueFlag1.Rise();
            else
                _blueFlag2.Rise();
            
            yield return new WaitForSeconds(2.0f);
            
            // ZOOM OUT THE TEXT
            _animTopText.SetInteger(TextAnimState, TextAnimStateZoomOut);
            _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomOut);
            
            yield return new WaitForSeconds(_animTopText.GetCurrentAnimatorClipInfo(0).Length);
            
            // PAN THE CAMERA BACK TO THE CENTER
            _cameraAnimator.SetInteger(CameraAnimState, CameraAnimStateReturnBlue);
            yield return new WaitForSeconds(_cameraAnimator.GetCurrentAnimatorClipInfo(0).Length);
        }

        var redScore = _scoreBoard.GetRedScore();
        var blueScore = _scoreBoard.GetBlueScore();

        if (redScore != 2 && blueScore != 2)
        {
            _bottomText.text = "THE BATTLE CONTINUES...";
            _bottomText.color = Color.black;
        }
        else
        {
            if (redScore == 2)
            {
                _topText.text = "RED TEAM";
                _topText.color = new Color(255.0f / 255.0f, 77.0f / 255.0f, 77.0f / 255.0f);
            }
            else
            {
                _topText.text = "BLUE TEAM";
                _topText.color = new Color(0.0f / 255.0f, 157.0f / 255.0f, 220.0f / 255.0f);
            }

            _bottomText.text = "WINS THE WAR!";
            _bottomText.color = Color.black;
        }
        
        if(redScore == 2 || blueScore == 2)
            _animTopText.SetInteger(TextAnimState, TextAnimStateZoomIn);
        
        _animBottomText.SetInteger(TextAnimState, TextAnimStateZoomIn);
        yield return new WaitForSeconds(_animBottomText.GetCurrentAnimatorClipInfo(0).Length);

        var transitionManager = Camera.main.GetComponent<TransitionManager>();
        transitionManager.PrepareSceneTransition(false);
        while (!transitionManager.ReadyForTransition)
            yield return null;
        
        SceneManager.LoadScene(redScore == 2 || blueScore == 2 ? "Menu" : "Loading");
    }

    private void Update()
    {
        _btnStone.interactable = ActivePlayer.StoneQuantity > 0;
        _btnWood.interactable = ActivePlayer.WoodQuantity > 0;
        _btnGlass.interactable = ActivePlayer.GlassQuantity > 0;
        
        _txtStoneQuantity.text = string.Format("x{0}", ActivePlayer.StoneQuantity);
        _txtStoneQuantityPanel.text = string.Format("{0} Left", ActivePlayer.StoneQuantity);

        _txtWoodQuantity.text = string.Format("x{0}", ActivePlayer.WoodQuantity);
        _txtWoodQuantityPanel.text = string.Format("{0} Left", ActivePlayer.WoodQuantity);
        
        _txtGlassQuantity.text = string.Format("x{0}", ActivePlayer.GlassQuantity);  
        _txtGlassQuantityPanel.text = string.Format("{0} Left", ActivePlayer.GlassQuantity);
        
        Debug.LogFormat("Red: {0} / Blue: {1}", _scoreBoard.GetRedScore(), _scoreBoard.GetBlueScore());
    }

 

}
