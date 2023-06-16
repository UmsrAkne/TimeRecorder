using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeRecorder.Models
{
    public class TimeStamp
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        /// <summary>
        /// 特定の TimeStamp を修正した祭、その TimeStamp の情報を変更するのではなく、別の新しい TimeStamp を生成する。
        /// 新しく生成したものには編集元の TimeStamp の Id をこの BaseId に記録する。
        /// </summary>
        /// <value>
        /// 生成元の TimeStamp の Id を記録。
        /// 生成元が存在しない新規 TimeStamp の場合は 0 とする。
        /// </value>
        [Required]
        public int BaseId { get; set; }

        [Required]
        public bool IsLatest { get; set; } = true;

        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Required]
        public string Comment { get; set; } = string.Empty;

        [NotMapped]
        public TimeSpan ElapsedTime { get; set; }
    }
}