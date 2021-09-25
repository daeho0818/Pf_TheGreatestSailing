using DG.Tweening;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField]
    private float _curMoveSpeed = 0;
    [SerializeField]
    private float _initMoveSpeed = 0;
    [SerializeField]
    private float _rotationSpeed = 0;
    [SerializeField]
    private float _curDurability = 0;
    [SerializeField]
    private float _maxDurability = 0;
    [SerializeField]
    private bool _isWreck = false;

    [SerializeField]
    private GameObject _panel = null;

    private float _debuffMoveSpeed = 0;

    public float MoveSpeed { get => _curMoveSpeed; set => _curMoveSpeed = value; }
    public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
    public float CurDurability { get => _curDurability; set => _curDurability = value; }
    public bool IsWreck { get => _isWreck; set => _isWreck = value; }

    private void FixedUpdate()
    {
        _isWreck = (_curDurability <= 0) ? true : false;
        _curMoveSpeed = (_isWreck == true) ? 0f : _curMoveSpeed;
    }

    private void Update()
    {
        if (_isWreck)
        {
            _panel.SetActive(true);
            Invoke("Dead", 3f);
        }
    }

    // 암초. (Enter)
    public void caughtReefEnter(float damage, float debuffSpeed)
    {
        if (!_isWreck)
        {
            Camera.main.transform.DOShakePosition(2);
            _curDurability = Mathf.Clamp(_curDurability - damage, 0f, _maxDurability);
            _debuffMoveSpeed = _curMoveSpeed * (1f - debuffSpeed);
        }
    }

    // 암초. (Stay)
    public void caughtReefStay()
    {
        if (!_isWreck)
            _curMoveSpeed = Mathf.Clamp(Mathf.Lerp(_curMoveSpeed, _debuffMoveSpeed, Time.deltaTime), 0f, Mathf.Infinity);
    }

    // 암초. (Exit)
    public void caughtReefExit()
    {
        _curMoveSpeed = _initMoveSpeed;
    }

    public void Dead()
    {
        LoadingSceneController.LoadScene("Voyage");
    }
}
