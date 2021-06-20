using System;
using AutoMapper;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;

namespace Raintels.Service
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<StudentDataModel, StudentViewModel>().ReverseMap();
            CreateMap<EventDataModel, EventViewModel>().ReverseMap();
            CreateMap<EventAnalysisDataModel, EventAnalyticsViewModel>().ReverseMap();
        }
    }
}
