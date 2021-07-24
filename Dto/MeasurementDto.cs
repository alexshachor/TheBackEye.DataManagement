using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
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
        public PersonDto Person { get; set; }
        public int PersonId { get; set; }
    }

    public static class MeasurementDtoExtension
    {
        public static Measurement ToModel(this MeasurementDto dto)
        {
            return new Measurement
            {
                Id = dto.Id,
                PersonId = dto.PersonId,
                Person = dto.Person?.ToModel(),
                DateTime = dto.DateTime,
                LessonId = dto.LessonId,
                Lesson = dto.Lesson?.ToModel(),
                FaceDetector = dto.FaceDetector,
                FaceRecognition = dto.FaceRecognition,
                HeadPose = dto.HeadPose,
                ObjectDetection = dto.ObjectDetection,
                OnTop = dto.OnTop,
                SleepDetector = dto.SleepDetector,
                SoundCheck = dto.SoundCheck
            };
        }
    }

    public static class MeasurementExtension
    {
        public static MeasurementDto ToDto(this Measurement model)
        {
            return new MeasurementDto
            {
                Id = model.Id,
                PersonId = model.PersonId,
                Person = model.Person?.ToDto(),
                DateTime = model.DateTime,
                LessonId = model.LessonId,
                Lesson = model.Lesson?.ToDto(),
                FaceDetector = model.FaceDetector,
                FaceRecognition = model.FaceRecognition,
                HeadPose = model.HeadPose,
                ObjectDetection = model.ObjectDetection,
                OnTop = model.OnTop,
                SleepDetector = model.SleepDetector,
                SoundCheck = model.SoundCheck
            };
        }

        public static List<MeasurementDto> ToDto(this List<Measurement> model)
        {
            List<MeasurementDto> measurements = new List<MeasurementDto>();
            model.ForEach(x => measurements.Add(x.ToDto()));
            return measurements;
        }
    }
}
