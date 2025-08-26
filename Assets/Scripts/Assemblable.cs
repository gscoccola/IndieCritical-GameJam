using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Assemblable : MonoBehaviour, IStateLoader
{

    [SerializeField] StatusEffectAffinity ElectricAffinity;
    [SerializeField] StatusEffectAffinity BloodAffinity;
    [SerializeField] public HeightLayer _heightLayer = HeightLayer.AboveUpperCover;
    [SerializeField] bool _isUncoveredOnCheckpoint;

    [SerializeField] Collider2D _interactionTrigger;
    [SerializeField] Collider2D _physicsCollider;
    [SerializeField] GameObject _coverCheckerPrefab;
    [SerializeField] SpriteRenderer _electrifiedSprite;

    private GameObject _coverChecker;

    public List<GameObject> CurrentCollidingObjects = new List<GameObject>();
    public UnityEvent  OnCollisionUpdate;
    public Dictionary<StatusEffect, bool> CurrentStatusEffects = new();
    public int AssemblyIndex;

    public Dictionary<StatusEffect, StatusEffectAffinity> StatusEffectAffinities = new ();

    public bool _isElectrified;
    public bool _isBloodPowered;
    public bool IsUncovered;

    private void Update()
    {
        _isElectrified = CurrentStatusEffects[StatusEffect.Electrified];
        _isBloodPowered = CurrentStatusEffects[StatusEffect.BloodPowered];
        IsUncovered = _heightLayer == HeightLayer.AboveUpperCover;
    }

    private void Awake()
    {
        AffinityAndStatusSetup();
        

    }

    private void InitialHeightLayerSetup()
    {
        float ordenInLayer = transform.position.z - (int)_heightLayer;
        //transform.position = new Vector3(transform.position.x, transform.position.y, (int)_heightLayer + ordenInLayer);
        if (GetComponent<Cover>() != null) return;
        if (_heightLayer != HeightLayer.AboveUpperCover)
        {
            SetActive(false);
        }
    }

    public void SetActive(bool value)
    {
        if (_interactionTrigger.enabled == value) return;
        _interactionTrigger.enabled = value;
        if (_physicsCollider != null) _physicsCollider.enabled = value;
        if (value)
        {
            float ordenInLayer = (transform.position.z - (int)_heightLayer) %1f ;
            _heightLayer = HeightLayer.AboveUpperCover;
            
            //transform.position = new Vector3(transform.position.x, transform.position.y, (int)_heightLayer + ordenInLayer);
           foreach (IActivable activable in GetComponents<IActivable>())
            {
                activable.SetActive(true);
            }
        }
        else
        {
            if (_coverCheckerPrefab != null) _coverChecker = Instantiate(_coverCheckerPrefab, transform.position, Quaternion.identity, transform);
        }
    }

    private void AffinityAndStatusSetup()
    {
        StatusEffectAffinities[StatusEffect.Electrified] = ElectricAffinity;
        StatusEffectAffinities[StatusEffect.BloodPowered] = BloodAffinity;
        foreach (StatusEffect effect in System.Enum.GetValues(typeof(StatusEffect)))
            CurrentStatusEffects[effect] = false;
    }

    private void Start()
    {
        InitialHeightLayerSetup();
        InitialCollisionsCheck();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Assemblable>() == null) return;
        CurrentCollidingObjects.Add(col.gameObject);
        Assembly.Instance.UpdateConnections(true, this, col.GetComponent<Assemblable>());
        OnCollisionUpdate?.Invoke();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<Assemblable>() == null || Assembly.Instance == null) return;
        CurrentCollidingObjects.Remove(col.gameObject);
        Assembly.Instance.UpdateConnections(false, this, col.GetComponent<Assemblable>());
        OnCollisionUpdate?.Invoke();
    }

    private void InitialCollisionsCheck()
    {
        Collider2D[] colliders = new Collider2D[30];
        GetComponent<Collider2D>().Overlap(new ContactFilter2D(), colliders);

        foreach (var col in colliders)
        {
            if (col != null && col.GetComponent<Assemblable>() != null)
            {
                CurrentCollidingObjects.Add(col.gameObject);
                Assembly.Instance.UpdateConnections(true, this, col.GetComponent<Assemblable>());
            }
        }
    }

    public void SetStatusEffect(StatusEffect statusEffect, bool value)
    {
        if (CurrentStatusEffects[statusEffect] == value) return;
        CurrentStatusEffects[statusEffect] = value;
    }

    public void LoadState()
    {
        if (!_isUncoveredOnCheckpoint) return;
        _heightLayer = HeightLayer.AboveUpperCover;
        if (_coverChecker != null) Destroy(_coverChecker);
        SetActive(true);
    }

    public void SetElectrifiedVisual(bool value)
    {
        //if (_electrifiedSprite != null) _electrifiedSprite.enabled = value;
        if (_electrifiedSprite != null) _electrifiedSprite.color = !value ? Color.white : Color.yellow;
    }
}
