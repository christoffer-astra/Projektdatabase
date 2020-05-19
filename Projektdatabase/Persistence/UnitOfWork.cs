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
        private IProjektRepository _projekt;
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
            foreach (var projektModel in projekt)
            {
                int id = projektModel.ProjektId;
                using (var conn = _connection.CreateConnection())
                {
                    projektModel.KlassifikationModels = conn.Query<KlassifikationModel>(
                        $"SELECT  k.klassifikationId, k.klassifikationName FROM ProjektKlassifikation as pk LEFT JOIN Klassifikation as k on pk.KlassifikationId = k.klassifikationId WHERE pk.projektId = {id} ");
                    projektModel.UddOmrModels = conn.Query<UddOmrModel>(
                        $"SELECT  k.uddOmrId, k.uddOmrName FROM ProjektUddOmr as pk LEFT JOIN UddOmr as k on pk.UddOmrId = k.uddOmrId WHERE pk.projektId = {id} ");
                }


            }
            return projekt;
        }

        public void CheckAllOuterIds(IEnumerable<UddOmrModel> uddOmr, IEnumerable<OmrModel> omr, IEnumerable<KlassifikationModel> klassifikation,
            IEnumerable<DeltagendeInstModel> deltagendeInst)
        {
            List<int> uddOmrIds = new List<int>();
            foreach (var uddOmrModel in uddOmr)
            {
                _uddOmr.GetId(uddOmrModel.UddOmrName, _connection);
                uddOmrIds.Add(uddOmrModel.UddOmrId);
                _deltagendeInst.Get(2, _connection);
                //METHOD TO RETRIEVE ALL OUTER IDS
            }
        }

        //MAKE METHODS THAT LOCATE ALL OUTER TABLE IDs IF EXIST, OTHERWISE CREATE NEW IDS

        //STORE IDs in list for all conjunction models

        //Begin transaction that enters data into Projekt and all conjunction tables.
    }
}