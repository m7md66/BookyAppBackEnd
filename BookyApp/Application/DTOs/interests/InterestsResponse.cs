using System;
using Domain.Entities;


namespace Application.DTOs.interests
{
    public class InterestsResponse:BaseDto<InterestsResponse,Genres>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
