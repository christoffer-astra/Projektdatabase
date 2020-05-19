using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Projektdatabase.Models;

namespace Projektdatabase.Persistence
{
    class ProjektRepository : IProjektRepository
    {
        public ProjektRepository()
        {
        }

        public ProjektModel GetModel(int id)
        {
            return null;
        }

        public List<ProjektModel> GetAll(IDbConnectionFactory conn)
        {
            List<ProjektModel> projektList = new List<ProjektModel>();
            using (var connect = conn.CreateConnection())
            {
                projektList = connect.Query<ProjektModel>("Select * FROM Projekt").ToList();
                foreach (var projektModel in projektList)
                {
                    int id = projektModel.ProjektId;
                    projektModel.KlassifikationModels = connect.Query<KlassifikationModel>(
                        $"SELECT  k.klassifikationId, k.klassifikationName FROM ProjektKlassifikation as pk LEFT JOIN Klassifikation as k on pk.KlassifikationId = k.klassifikationId WHERE pk.projektId = {id} ") as IList<KlassifikationModel>;
                    projektModel.UddOmrModels = connect.Query<UddOmrModel>(
                        $"SELECT  k.uddOmrId, k.uddOmrName FROM ProjektUddOmr as pk LEFT JOIN UddOmr as k on pk.UddOmrId = k.uddOmrId WHERE pk.projektId = {id} ") as IList<UddOmrModel>;
                    projektModel.OmrModels = connect.Query<OmrModel>(
                        $"SELECT  k.omrId, k.omrName FROM ProjektOmr as pk LEFT JOIN Omr as k on pk.OmrId = k.omrId WHERE pk.projektId = {id} ") as IList<OmrModel>;
                    projektModel.DeltagendeInstModels = connect.Query<DeltagendeInstModel>(
                        $"SELECT  k.deltagendeInstId, k.deltagendeInstName FROM ProjektDeltagendeInst as pk LEFT JOIN DeltagendeInst as k on pk.DeltagendeInstId = k.deltagendeInstId WHERE pk.projektId = {id} ") as IList<DeltagendeInstModel>;
                    projektModel.ProjektHolderModels = connect.Query<ProjektHolderModel>(
                        $"SELECT  k.projektHolderId, k.projektHolderName FROM ProjektProjektHolder as pk LEFT JOIN ProjektHolder as k on pk.ProjektHolderId = k.projektHolderId WHERE pk.projektId = {id} ") as IList<ProjektHolderModel>;
                }
            }
            return projektList;
        }
    }       
}