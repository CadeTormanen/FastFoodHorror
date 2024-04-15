using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    private AudioSource[] songs;
    private AudioSource songPlaying;
    private int songNumber;


    private void PickSong()
    {
        int newSongNumber = songNumber;
        while (newSongNumber == songNumber)
        {
            songNumber = Random.Range(0, songs.Length);
        }
        songPlaying = songs[songNumber];
        songPlaying.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        songs = GetComponents<AudioSource>();
        songNumber = Random.Range(0, songs.Length);
        songPlaying = songs[songNumber];
        songPlaying.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (songPlaying.isPlaying == false) { PickSong(); }
    }
}
