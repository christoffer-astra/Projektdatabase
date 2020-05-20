using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.Configuration;
using Projektdatabase.Models;

namespace Projektdatabase.Persistence
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnectionFactory _connection;
        private readonly IRepository<UddOmrModel> _uddOmr;
        private readonly IRepository<OmrModel> _Omr;
        private readonly IRepository<KlassifikationModel> _klassifikation;
        private readonly IRepository<ProjektHolderModel> _projektHolder;
        private readonly IRepository<DeltagendeInstModel> _deltagendeInst;
        private readonly IRepository<ProjektKlassifikationModel> _projektKlassifikation;
        private readonly IRepository<ProjektUddOmrModel> _projektUddOmr;
        private readonly IProjektRepository _projekt;
        public UnitOfWork(IDbConnectionFactory connection, IRepository<UddOmrModel> uddOmr, IProjektRepository projekt, 
            IRepository<OmrModel> omr, IRepository<DeltagendeInstModel> deltagendeInst, 
            IRepository<KlassifikationModel> klassifikation, IRepository<ProjektHolderModel> projektHolder, 
            IRepository<ProjektKlassifikationModel> projektKlassifikation, IRepository<ProjektUddOmrModel> projektUddOmr)
        {
            _connection = connection;
            _uddOmr = uddOmr;
            _projekt = projekt;
            _Omr = omr;
            _deltagendeInst = deltagendeInst;
            _klassifikation = klassifikation;
            _projektHolder = projektHolder;
            _projektKlassifikation = projektKlassifikation;
            _projektUddOmr = projektUddOmr;
        }

        public List<UddOmrModel> Complete()
        {
            List<UddOmrModel> uddOmr = _uddOmr.GetAll(_connection);
            return uddOmr;
        }

        public IEnumerable<ProjektModel> RetrieveAllProjekts()
        {
            IEnumerable<ProjektModel> projekt = _projekt.GetAll(_connection);

            return projekt;
        }

        public ProjektModel GetProjektModel()
        {
            ProjektModel projekt = new ProjektModel();
            projekt.KlassifikationModels = _klassifikation.GetAll(_connection);
            projekt.UddOmrModels = _uddOmr.GetAll(_connection);
            return projekt;
        }

        public void SubmitProjekt(ProjektModel projektModel)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            string sql;
            sqlBuilder.Append("INSERT INTO Projekt ([ProjektName], [ProjektDescription], [ProjektStatus], [ProjektEvaluationStatus], [ProjektFundingDescription],");
            sqlBuilder.Append(" [ProjektStartDate], [ProjektEndDate], [ProjektLink], [ProjektEvaluation], [ProjektProgression])");
            sqlBuilder.Append(" OUTPUT INSERTED.[ProjektId]");
            sqlBuilder.Append(" VALUES (@projektName, @projektDescription, @projektStatus, @projektEvaluationStatus, @projektFundingDescription, @projektStartDate,");
            sqlBuilder.Append(" @projektEndDate, @projektLink, @projektEvaluation, @projektProgression)");
            sql = sqlBuilder.ToString();
            using (IDbConnection connection = _connection.CreateConnection())
            {
                ProjektModel projekt = connection.QuerySingle<ProjektModel>(sql, new
                {
                    projektModel.ProjektName,
                    projektModel.ProjektDescription,
                    projektModel.ProjektStatus,
                    projektModel.ProjektEvaluationStatus,
                    projektModel.ProjektFundingDescription,
                    projektModel.ProjektStartDate,
                    projektModel.ProjektEndDate,
                    projektModel.ProjektLink,
                    projektModel.ProjektEvaluation,
                    projektModel.ProjektProgression
                });
                foreach (var klassifikation in projektModel.KlassifikationModels)
                {
                    connection.Query(
                        $"INSERT INTO ProjektKlassifikation (ProjektId, KlassifikationId) VALUES ({projekt.ProjektId}, {klassifikation.KlassifikationId})");
                }
                foreach (var uddOmr in projektModel.UddOmrModels)
                {
                    connection.Query(
                        $"INSERT INTO ProjektUddOmr (ProjektId, UddOmrId) VALUES ({projekt.ProjektId}, {uddOmr.UddOmrId})");
                }
                foreach (var omr in projektModel.OmrModels)
                {
                    var exists = connection.ExecuteScalar<bool>($"select count(1) from Omr where OmrName = '{omr.OmrName}'");
                    if (exists)
                    {
                        int id = connection.QuerySingle<int>($"SELECT omrId from Omr where omrName = '{omr.OmrName}'");
                        connection.Query(
                            $"INSERT INTO ProjektOmr (projektId, omrId) VALUES ({projekt.ProjektId}, {id})");
                    }
                    else
                    {
                        OmrModel omrModel = connection.QuerySingle<OmrModel>($"INSERT INTO Omr (omrName) OUTPUT INSERTED.[OmrId] VALUES ('{omr.OmrName}')");
                        int id = omrModel.OmrId;
                        connection.Query(
                            $"INSERT INTO ProjektOmr (ProjektId, OmrId) VALUES ({projekt.ProjektId}, {id})");
                    }
                }
                foreach (var deltagendeInst in projektModel.DeltagendeInstModels)
                {
                    var exists = connection.ExecuteScalar<bool>($"select count(1) from DeltagendeInst where deltagendeInstName = '{deltagendeInst.DeltagendeInstName}'");
                    if (exists)
                    {
                        int id = connection.QuerySingle<int>($"SELECT deltagendeInstId from DeltagendeInst where deltagendeInstName = '{deltagendeInst.DeltagendeInstName}'"); connection.Query(
                            $"INSERT INTO ProjektDeltagendeInst (ProjektId, DeltagendeInstId) VALUES ({projekt.ProjektId}, {deltagendeInst.DeltagendeInstId})");
                    }
                    else
                    {
                        DeltagendeInstModel deltagendeInstModel = connection.QuerySingle<DeltagendeInstModel>($"INSERT INTO DeltagendeInst (deltagendeInstName) OUTPUT INSERTED.[deltagendeInstId] VALUES ('{deltagendeInst.DeltagendeInstName}')");
                        int id = deltagendeInstModel.DeltagendeInstId;
                        connection.Query(
                            $"INSERT INTO ProjektDeltagendeInst (ProjektId, DeltagendeInstId) VALUES ({projekt.ProjektId}, {id})");
                    }
                    
                }
                foreach (var projektHolder in projektModel.ProjektHolderModels)
                {
                    var exists = connection.ExecuteScalar<bool>($"select count(1) from ProjektHolder where projektHolderName = '{projektHolder.ProjektHolderName}'");
                    if (exists)
                    {
                        int id = connection.QuerySingle<int>($"SELECT projektHolderId from ProjektHolder where projektHolderName = '{projektHolder.ProjektHolderName}'"); connection.Query(
                            $"INSERT INTO ProjektProjektHolder (ProjektId, ProjektHolderId) VALUES ({projekt.ProjektId}, {projektHolder.ProjektHolderId})");
                    }
                    else
                    {
                        ProjektHolderModel projektHolderModel = connection.QuerySingle<ProjektHolderModel>($"INSERT INTO ProjektHolder (projektHolderName) OUTPUT INSERTED.[ProjektHolderId] VALUES ('{projektHolder.ProjektHolderName}')");
                        int id = projektHolderModel.ProjektHolderId;
                        connection.Query(
                            $"INSERT INTO ProjektProjektHolder (ProjektId, ProjektHolderId) VALUES ({projekt.ProjektId}, {id})");
                    }
                }
                //TODO: Make actual transaction.
                //TODO REFACTOR Dapper code: parameterize, use correct query methods, make repository generic methods
            }
        }
    }
}