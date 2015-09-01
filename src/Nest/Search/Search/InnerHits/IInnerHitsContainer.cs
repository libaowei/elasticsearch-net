using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Nest
{
	public interface IInnerHitsContainer
	{
		[JsonProperty(PropertyName = "type")]
		[JsonConverter(typeof (VerbatimDictionaryKeysJsonConverter))]
		IDictionary<TypeName, IGlobalInnerHit> Type { get; set; }

		[JsonProperty(PropertyName = "path")]
		[JsonConverter(typeof (VerbatimDictionaryKeysJsonConverter))]
		IDictionary<FieldName, IGlobalInnerHit> Path { get; set; }
	}

	public class InnerHitsContainer : IInnerHitsContainer
	{
		public IDictionary<TypeName, IGlobalInnerHit> Type { get; set; }
		public IDictionary<FieldName, IGlobalInnerHit> Path { get; set; }
	}

	public class InnerHitsContainerDescriptor<T> : IInnerHitsContainer where T : class
	{
		private IInnerHitsContainer Self { get { return this; }}

		IDictionary<TypeName, IGlobalInnerHit> IInnerHitsContainer.Type { get; set; }
		IDictionary<FieldName, IGlobalInnerHit> IInnerHitsContainer.Path { get; set; }

		public InnerHitsContainerDescriptor<T> Type(Func<GlobalInnerHitDescriptor<T>, IGlobalInnerHit> globalInnerHitsSelector = null) 
		{
			var globalInnerHit = globalInnerHitsSelector == null ? new GlobalInnerHit() : globalInnerHitsSelector(new GlobalInnerHitDescriptor<T>());
			Self.Type = new Dictionary<TypeName, IGlobalInnerHit> {{typeof(T), globalInnerHit}};
			return this;
		}
		
		public InnerHitsContainerDescriptor<T> Type<TOther>(Func<GlobalInnerHitDescriptor<TOther>, IGlobalInnerHit> globalInnerHitsSelector = null) where TOther : class
		{
			var globalInnerHit = globalInnerHitsSelector == null ? new GlobalInnerHit() : globalInnerHitsSelector(new GlobalInnerHitDescriptor<TOther>());
			Self.Type = new Dictionary<TypeName, IGlobalInnerHit> {{typeof(TOther), globalInnerHit}};
			return this;
		}

		public InnerHitsContainerDescriptor<T> Path(string path, Func<GlobalInnerHitDescriptor<T>, IGlobalInnerHit> globalInnerHitsSelector = null) 
		{
			var globalInnerHit = globalInnerHitsSelector == null ? new GlobalInnerHit() : globalInnerHitsSelector(new GlobalInnerHitDescriptor<T>());
			Self.Path = new Dictionary<FieldName, IGlobalInnerHit> {{ path, globalInnerHit}};
			return this;
		}

		public InnerHitsContainerDescriptor<T> Path(Expression<Func<T, object>> path, Func<GlobalInnerHitDescriptor<T>, IGlobalInnerHit> globalInnerHitsSelector = null) 
		{
			var globalInnerHit = globalInnerHitsSelector == null ? new GlobalInnerHit() : globalInnerHitsSelector(new GlobalInnerHitDescriptor<T>());
			Self.Path = new Dictionary<FieldName, IGlobalInnerHit> {{ path, globalInnerHit}};
			return this;
		}
	}
}