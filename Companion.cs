namespace lab5b
{
    /// <summary>
    /// Represents a companion who traveled with the Doctor
    /// </summary>
    internal class Companion
    {
        /// <summary>
        /// The companion's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The actor who played the companion
        /// </summary>
        public string Actor { get; set; }

        /// <summary>
        /// The StoryId of the companion's first episode
        /// </summary>
        public string Episode { get; set; }

        /// <summary>
        /// The ID of the Doctor this companion traveled with
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// Creates a new Companion object
        /// </summary>
        /// <param name="name">The companion's name</param>
        /// <param name="actor">The actor who played the companion</param>
        /// <param name="episode">The StoryId of their first episode</param>
        /// <param name="doctorId">The ID of the Doctor they traveled with</param>
        public Companion(string name, string actor, string episode, int doctorId)
        {
            Name = name;
            Actor = actor;
            Episode = episode;
            DoctorId = doctorId;
        }
    }
}