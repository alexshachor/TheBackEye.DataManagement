using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class MeasurementDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public bool HeadPose { get; set; }
        public bool FaceRecognition { get; set; }
        public bool SleepDetector { get; set; }
        public bool OnTop { get; set; }
        public bool FaceDetector { get; set; }
        public bool ObjectDetection { get; set; }
        public bool SoundCheck { get; set; }
        public LessonDto Lesson { get; set; }
        public int LessonId { get; set; }
        public StudentDto Student { get; set; }
        public int StudentId { get; set; }
    }
}
