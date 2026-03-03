//I, Camilo Becerra, 000035320 certify that this material is my original work.  No other person's work has been used without due acknowledgement.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5b
{
    /// <summary>
    /// Represents a Doctor from the Doctor Who database
    /// </summary>
    internal class Doctor
    {
        /// <summary>
        /// The unique identifier for this Doctor
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// The name of the actor who played this Doctor
        /// </summary>
        public string Actor { get; set; }

        /// <summary>
        /// The number of series this Doctor appeared in
        /// </summary>
        public int Series { get; set; }

        /// <summary>
        /// The actor's age when they started playing the Doctor
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// The StoryId of the Doctor's first episode
        /// </summary>
        public string Debut { get; set; }

        /// <summary>
        /// Binary image data for the Doctor's photo
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// Creates a new Doctor object
        ///