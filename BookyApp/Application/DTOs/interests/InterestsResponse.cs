using Domain.Entities;


namespace Application.DTOs.interests
{
    public class InterestsResponse:BaseDto<InterestsResponse,Genres>
    {
        public string Name { get; set; }
    }
}
