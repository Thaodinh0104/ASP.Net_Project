
namespace FreeGrab.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            this.Comments = new HashSet<Comment>();
            this.HistoryGrabs = new HashSet<HistoryGrab>();
            this.Photos = new HashSet<Photo>();
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(1)]
        public string FullName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public byte Gender { get; set; }
        public int HospitalId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Departure Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DateDeparture { get; set; }
        [Required]
        [MaxLength(14)]
        [MinLength(8)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(100)]

        public string Destination { get; set; }
        public int StatusId { get; set; }
        private DateTime _createdOn = DateTime.MinValue;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime DateCreate
        {
            get
            {
                return (_createdOn == DateTime.MinValue) ? DateTime.Now : _createdOn;
            }
            set { _createdOn = value; }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public System.DateTime DateUpdate { get; set; }
        public string Image { get; set; }
        public List<HttpPostedFileBase> files { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryGrab> HistoryGrabs { get; set; }
        public virtual Hospital Hospital { get; set; }
        public virtual PatientStatus PatientStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Photo> Photos { get; set; }
        public static object Identity { get; internal set; }
    }
}
