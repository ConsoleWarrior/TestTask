using UnityEngine;

// висит на объекте с Animator - туда приходят события из анимаций (шаги, приземление)
[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip landing;
    [SerializeField] private float volume = 0.5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnFootstep(AnimationEvent e)
    {
        // в blend tree событие приходит сразу от нескольких клипов, берем только основной
        if (e.animatorClipInfo.weight < 0.5f || footsteps.Length == 0)
            return;

        audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)], volume);
    }

    void OnLand(AnimationEvent e)
    {
        if (e.animatorClipInfo.weight < 0.5f || landing == null)
            return;

        audioSource.PlayOneShot(landing, volume);
    }
}
