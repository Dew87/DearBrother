using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingShroom : MonoBehaviour
{
    public float timerDuration = 0.4f;
    public float poisonCloudDuration = 0.5f;
    public float respawnDelay = 0.5f;
    public GameObject poisonCloud;

    [Header("Debug")]
    public float timer;

    private SpriteRenderer spriteRenderer;
    private Bouncer bouncer;
    private Collider2D bounceCollider;

    private StateMachine sm = new StateMachine();
    private State regularState;
    private State countdownState;
    private State poisonState;
    private State respawningState;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		bouncer = GetComponent<Bouncer>();
		bounceCollider = GetComponent<Collider2D>();

        regularState = new State(null, RegularStateEnter, RegularStateExit);
        countdownState = new State(CountdownStateUpdate, CountdownStateEnter);
        poisonState = new State(PoisonStateUpdate, PoisonStateEnter);
        respawningState = new State(RespawningStateUpdate, RespawningStateEnter);

		poisonCloud.SetActive(false);
		sm.Transition(regularState);
	}

	private void OnEnable()
	{
		bouncer.onBounce += OnBounce;
	}

	private void OnDisable()
	{
		bouncer.onBounce -= OnBounce;
	}

	private void OnBounce()
	{
		sm.Transition(countdownState);
	}

	private void Update()
	{
		sm.Update();
	}

    private void RegularStateEnter()
    {
        bounceCollider.enabled = true;
        spriteRenderer.color = Color.white;
    }

    private void RegularStateExit()
    {
        bounceCollider.enabled = false;
    }

    private void CountdownStateEnter()
    {
        timer = timerDuration;
    }

    private void CountdownStateUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.white, Color.black, 1 - timer / timerDuration);
            if (timer <= 0)
            {
                sm.Transition(poisonState);
            }
        }
    }

    private void PoisonStateEnter()
    {
        spriteRenderer.enabled = false;
        poisonCloud.SetActive(true);
        timer = poisonCloudDuration;
    }

    private void PoisonStateUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                poisonCloud.SetActive(false);
                sm.Transition(respawningState);
            }
        }
    }

    private void RespawningStateEnter()
    {
        timer = respawnDelay;
    }

    private void RespawningStateUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                spriteRenderer.enabled = true;
                sm.Transition(regularState);
            }
        }
    }
}
