using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;

namespace Charon_DNS_Changer.Core
{
    class CharonPlayerSounds
    {
        public class CharonPlayer
        {
            private SoundPlayer player;

            public CharonPlayer()
            {
                player = new SoundPlayer();
            }

            public void PlaySound(Stream soundStream)
            {
                if (soundStream == null)
                {
                    throw new ArgumentNullException(nameof(soundStream), "Sound stream cannot be null.");
                }

                try
                {
                    player.Stream = soundStream;
                    player.Load();
                    player.Play(); // Use Play() for async play or PlayLooping() to loop the sound
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    Console.WriteLine($"Error playing sound: {ex.Message}");
                }
            }

            public void sPlayer(string req_sound)
            {
                string filePath = req_sound + ".wav";

                if (File.Exists(filePath))
                {
                    try
                    {
                        using (var soundPlayer = new SoundPlayer(filePath))
                        {
                            soundPlayer.Play();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception for debugging purposes
                        Console.WriteLine($"Error playing sound from file: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"File {filePath} does not exist.");
                }
            }
        }


        public class CharonPlayer2
        {
            private SoundPlayer player;

            public CharonPlayer2()
            {
                player = new SoundPlayer();
            }

            public void LoadSound(Stream soundStream)
            {
                if (soundStream == null)
                {
                    throw new ArgumentNullException(nameof(soundStream), "Sound stream cannot be null.");
                }

                try
                {
                    player.Stream = soundStream;
                    player.Load();
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    Console.WriteLine($"Error loading sound: {ex.Message}");
                }
            }

            public void PlaySound()
            {
                if (player.Stream == null)
                {
                    throw new InvalidOperationException("Sound stream is not loaded.");
                }

                try
                {
                    player.Play();
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    Console.WriteLine($"Error playing sound: {ex.Message}");
                }
            }

            public void StopSound()
            {
                try
                {
                    player.Stop();
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    Console.WriteLine($"Error stopping sound: {ex.Message}");
                }
            }
        }
    }
}
