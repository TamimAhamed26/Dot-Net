//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace labexam.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Post
    {
        public int PostID { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int UserID { get; set; }
    
        public virtual User User { get; set; }
    }
}
