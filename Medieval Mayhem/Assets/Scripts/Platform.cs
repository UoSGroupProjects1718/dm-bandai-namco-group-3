using Exploder2D;
using UnityEngine;

[RequireComponent(typeof(Exploder2DObject))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Draggable))]
public class Platform : MonoBehaviour
{
    [SerializeField] private PlatformMaterial _platformMaterial;
    
    private Exploder2DObject _exploder2DObject;

    public int Strength { private set; get; }

    [SerializeField] private Collider2D _physicalCollider;

    private const int StrengthNone = 0;
    private const int StrengthHitDecrement = 1;

    private bool BeenSorted;
    
    private void Start()
    {
        _exploder2DObject = GetComponent<Exploder2DObject>();

        _exploder2DObject.SFXOptions.ExplosionSoundClip = _platformMaterial.BreakSound;
        
        _physicalCollider.sharedMaterial = new PhysicsMaterial2D()
        {
            bounciness = _platformMaterial.Bounciness,
            friction = _platformMaterial.Friction
        };
        Strength = _platformMaterial.Strength;     
    }

    private void Update()
    {
        if (!BeenSorted && GetComponent<Draggable>().BeenDragged)
        {
            var gameManager = (GameManager)FindObjectOfType(typeof(GameManager));
            switch (_platformMaterial.Name)
            {
                case "Stone":
                    gameManager.ActivePlayer.StoneQuantity--;
                    break;
                case "Wood":
                    gameManager.ActivePlayer.WoodQuantity--;
                    break;
                case "Glass":
                    gameManager.ActivePlayer.GlassQuantity--;
                    break;
            }
      
            GameObject.Find("Scroll").GetComponent<Scroll>().HidePeak();
            transform.parent = GameObject.Find("[ Placed Platforms ]").transform;
            BeenSorted = true;
        }
    }
    
    private void DamagePlatform()
    {
        Strength -= StrengthHitDecrement;

        if (Strength == StrengthNone)
        {
            AudioSource.PlayClipAtPoint(_platformMaterial.BreakSound, Vector3.zero);
            _exploder2DObject.Explode();
        }
        else
        {
            AudioSource.PlayClipAtPoint(_platformMaterial.HitSound, Vector3.zero);
        }
    } 
   
   private void OnCollisionEnter2D(Collision2D collision2D)
   {
        if (collision2D.gameObject.CompareTag("Bomb"))
            DamagePlatform();
   } 
}
