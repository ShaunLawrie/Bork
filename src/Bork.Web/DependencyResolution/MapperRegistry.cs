using AutoMapper;
using Bork.Contracts;
using Bork.Web.Models.HomeViewModels;
using StructureMap;

namespace Bork.Web.DependencyResolution
{
    public class MapperRegistry : Registry
    {
        public MapperRegistry()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.CreateMap<BorkRecord, DisplayBork>()
                .ForMember(dest =>
                    dest.Type, opt =>
                        opt.UseValue("bork"))
                .ForAllOtherMembers(opt =>
                    opt.Condition((src, dest, srcVal, destVal) =>
                        srcVal != null));

                c.CreateMap<ReBorkRecord, DisplayBork>()
                .ForMember(dest =>
                    dest.Type, opt =>
                        opt.UseValue("rebork"))
                .ForAllOtherMembers(opt =>
                    opt.Condition((src, dest, srcVal, destVal) =>
                        srcVal != null));
            });

            var mapper = new Mapper(mapperConfig);

            For<IMapper>()
                .Use(mapper)
                .Singleton();
        }
    }
}
