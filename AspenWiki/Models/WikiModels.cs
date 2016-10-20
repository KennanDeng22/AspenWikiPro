using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspenWiki.Models
{
    public enum ArticleType { Wiki, AspenWiki, Link }

    public class Category
    {
        public Category()
        {
            this.Articles = new HashSet<Article>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int OrderIdx { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }

    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Keywords { get; set; }
        public string UserId { get; set; }
        public ArticleType ArticleType { get; set; }
        public string Url { get; set; }//for Link
        public bool UseIframe { get; set; }//for Link
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public int CategoryId { get; set; }
        public bool IsHidden { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual ArticleContent ArticleContent { get; set; }

        public virtual ICollection<ArticleComment> ArticleComments { get; set; }
    }

    public class ArticleContent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[ForeignKey("ArticleId")]
        public int ArticleId { get; set; }
        public int Version { get; set; }
        public System.DateTime CreateTime { get; set; }
        public byte[] Content { get; set; }

        //public virtual Article Article { get; set; }
    }

    public class ArticleComment
    {
        [Key]
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string UserId { get; set; }
        public System.DateTime CreateTime { get; set; }

        public byte[] Content { get; set; }
    }

    public class ArticleContentHistory
    {
        [Key]
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int Version { get; set; }
        public System.DateTime CreateTime { get; set; }
        public byte[] Content { get; set; }
    }
}