namespace NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;

using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions.Query;
using NecManager.Server.DataAccessLayer.Model;
using NecManager.Server.DataAccessLayer.Model.Query;

public interface ITrainingAccessLayer : IQueryBaseAccessLayer<Training, TrainingQuery>
{
}
