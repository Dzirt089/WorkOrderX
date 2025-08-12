using AutoMapper;

using WorkOrderX.Http.Models;
using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Profiles
{
	public class ReferenseDataProfile : Profile
	{
		public ReferenseDataProfile()
		{
			CreateMap<ApplicationStatusDataModel, ApplicationStatus>().ReverseMap();
			CreateMap<ApplicationTypeDataModel, ApplicationType>().ReverseMap();
			CreateMap<InstrumentKindDataModel, InstrumentKind>().ReverseMap();
			CreateMap<InstrumentModelDataModel, InstrumentModel>().ReverseMap();
			CreateMap<InstrumentTypeDataModel, InstrumentType>().ReverseMap();
			CreateMap<TypeBreakdownDataModel, TypeBreakdown>().ReverseMap();
			CreateMap<ImportancesDataModel, Importance>().ReverseMap();
		}
	}
}
