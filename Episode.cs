//I, Camilo Becerra, 000035320 certify that this material is my original work.  No other person's work has been used without due acknowledgement.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5b
{
    /// <summary>
    /// Represents an episode from the Doctor Who database
    /// </summary>
    internal class Episode
    {
        /// <summary>
        /// The season number this episode belongs to
        /// </summary>
        public int Season { get; set; }

        /// <summary>
        /// The year this season aired
        /// </summary>
        public int SeasonYear { get; set; }

        /// <summary>
        /// The title of the episode
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The unique story identifier for this episode
        /// </summary>
        public string StoryId { get; set; }

        /// <summary>
        /// Creates a new Episode object
        /// </summary>
        /// <param name="storyId">The unique story identifier</param>
        /// <param name="season">The season number</param>
        /// <param name="seasonYear">The year the season aired</param>
        /// <param name="title">The title of the episode</param>
        public Episode(string storyId, int season, int seasonYear, string title)
        {
            StoryId = storyId;
            Season = season;
            SeasonYear = seasonYear;
            Title = title;
        }
    }
}