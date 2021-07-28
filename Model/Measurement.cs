using System;

namespace Model
{
    public class Measurement
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
        public Lesson Lesson { get; set; }
        public int? LessonId { get; set; }
        public Person Person { get; set; }
        public int? PersonId { get; set; }
    }
}