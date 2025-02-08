using UnityEngine;
using System.Collections;

public class AmbientSoundSpawner : MonoBehaviour
{
    [SerializeField] private AudioClip[] ambientSounds; // Массив звуков
     private float minInterval = 33f; // Минимальное время между звуками
     private float maxInterval = 83f; // Максимальное время между звуками
     private float minDistance = 6f; // Минимальное расстояние от игрока
     private float maxDistance = 19f; // Максимальное расстояние от игрока
     private float volume = 0.9f; // Громкость звука

    private void Start()
    {
        StartCoroutine(PlayAmbientSounds());
    }

    private IEnumerator PlayAmbientSounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            PlayRandomSound();
        }
    }

    private void PlayRandomSound()
    {
        if (ambientSounds.Length == 0) return;

        Vector3 randomPosition = GetRandomPosition();
        AudioClip clip = ambientSounds[Random.Range(0, ambientSounds.Length)];
        
        // Воспроизводит звук в указанной позиции без явного создания объекта
        float vol = Random.Range(0.3f, volume);
        AudioSource.PlayClipAtPoint(clip, randomPosition, vol);
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);
        return transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);
    }
}