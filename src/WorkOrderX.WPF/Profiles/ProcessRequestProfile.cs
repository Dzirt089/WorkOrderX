using AutoMapper;

using WorkOrderX.Http.Models;
using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Profiles
{
	public class ProcessRequestProfile : Profile
	{
		public ProcessRequestProfile()
		{
			CreateMap<ProcessRequestDataModel, ProcessRequest>().ReverseMap();
		}
	}
}
