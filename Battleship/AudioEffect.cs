//-----------------------------------------------------
// <copyright file="AudioEffect.cs" company="none">
//    Copyright (c) Andreas Andersson 2014
// </copyright>
// <author>Andreas Andersson</author>
//-----------------------------------------------------

namespace Battleship
{
    using System;
    using System.Media;

    /// <summary>
    /// A simple sound effect player class.
    /// Sound file credits:
    /// hit.wav:         Mike Koenig (soundbible.com)
    /// miss.wav:        Steveygos93 (soundbible.com)
    /// sunk.wav:        Mike Koenig (soundbible.com) (Edited by Andreas Andersson 2014)
    /// forbidden.wav:   Mike Koenig (soundbible.com)
    /// </summary>
    public class AudioEffect
    {
        /// <summary>
        /// Filenames for sound effect wav files.
        /// </summary>
        private string[] soundFile = new string[Enum.GetNames(typeof(Square)).Length];

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioEffect"/> class where the Sound
        /// property defaults to no sound.
        /// </summary>
        public AudioEffect()
            : this(Square.Water)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioEffect"/> class.
        /// </summary>
        /// <param name="effect">Square.Hit, Square.Miss, Square.Sunk and Square.Forbidden each has its
        /// own sound effect, while passing other Square's results in no sound.</param>
        public AudioEffect(Square effect)
        {
            const string PATH = @"audio\";

            // Initialize filenames and the Effect property
            this.soundFile[(int)Square.Hit] = PATH + "hit.wav";
            this.soundFile[(int)Square.Miss] = PATH + "miss.wav";
            this.soundFile[(int)Square.Sunk] = PATH + "sunk.wav";
            this.soundFile[(int)Square.Forbidden] = PATH + "forbidden.wav";
            this.Sound = effect;
        }

        /// <summary>
        /// Gets or sets the sound effect to play. Square.Hit, Square.Miss, Square.Sunk and
        /// Square.Forbidden each has its own sound effect, while passing other Square's
        /// results in no sound.
        /// </summary>
        public Square Sound { get; set; }

        /// <summary>
        /// Plays a sound effect as indicated by the Sound property.
        /// </summary>
        public void Play()
        {
            SoundPlayer player;

            // Don't do anything if we don't have any sound file for this Square
            if (this.soundFile[(int)this.Sound] == null)
            {
                return;
            }

            // Play sound
            player = new SoundPlayer(this.soundFile[(int)this.Sound]);
            player.Play();
        }
    }
}
