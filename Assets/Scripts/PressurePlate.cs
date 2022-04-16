using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public List<Trap> traps;
    public float activateInterval;
    public float activationMass;
    public bool random;
    public bool loop;
    public float loopInterval;

    private float _debounce = .3f, _debouceCount, _loopCount;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        InitReferences();
    }

    // Update is called once per frame
    void Update()
    {
        _debouceCount += Time.deltaTime;

        if(loop)
        {
            _loopCount += Time.deltaTime;
            if(_loopCount >= loopInterval)
            {
                _loopCount = 0f;
                StartCoroutine(TiggerTraps());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_debouceCount >= _debounce)
        {
            Rigidbody2D rb;
            if ((rb = collision.gameObject.GetComponent<Rigidbody2D>()) == null || rb.mass < activationMass)
                return;


            _debouceCount = 0f;
            _animator.SetBool("Touched", true);

            StartCoroutine(TiggerTraps());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _animator.SetBool("Touched", false);
    }
    private void InitReferences()
    {
        _animator = GetComponent<Animator>();
    }

    IEnumerator TiggerTraps()
    {
        List<Trap> traps2Remove = new List<Trap>();
        if (random)
        {
            List<Trap> temp = new List<Trap>(traps);
            while(temp.Count > 0)
            {
                int idx = Random.Range(0, temp.Count);
                Trap trap = temp[idx];
                temp.RemoveAt(idx);
                trap.Trigger();
                if (trap is DeleteTrap)
                    traps2Remove.Add(trap);

                yield return new WaitForSeconds(Random.RandomRange(0, activateInterval));
            }
        }
        else
        {
            
            foreach (Trap trap in traps)
            {
                trap.Trigger();
                if (trap is DeleteTrap)
                    traps2Remove.Add(trap);

                yield return new WaitForSeconds(activateInterval);
            }
        }
        traps.RemoveAll(trap => traps2Remove.Contains(trap));
    }
}
