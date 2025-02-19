﻿using API_calculadora.Models;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Web.Http;


namespace API_calculadora.Controllers
{
    public class ItemsController : ApiController
    {
        private string connectionName = "User ID =lmzbqjgdarlvte; Password =1bb829a00da2d53999826b2e32b86af9aff29a33a7d9fff483f6f1f7f4f87b61; Host =ec2-52-87-107-83.compute-1.amazonaws.com; Port =5432; Database =db9lde3i86qlva; Pooling = true; Use SSL Stream = True; SSL Mode = Require; TrustServerCertificate = True; ";

        private static List<AG_ITEM_PRINCIPAL> itens = new List<AG_ITEM_PRINCIPAL>();

        //[HttpGet]
        public List<AG_ITEM_PRINCIPAL> Get()
        {
            fillItens();

            return itens;
        }

        public List<AG_ITEM_PRINCIPAL> Get(int Id)
        {
            fillItens(Id);

            return itens;
        }

        // POST api/values

        // public int Post(string nome,  string nomeIcone)
        //{


        //    return 0;
        //}





        // DELETE api/values/5
        //public void Delete(int id)
        //{
        //}

        /*
         * 
         * select * from AG_CUSTO_VARIADO
SELECT * FROM AG_CUSTO_FIXO
SELECT * FROM AG_ITEM_PRINCIPAL
select * from AG_CUSTO_VARIADO
SELECT * FROM AG_CUSTO_FIXO
SELECT * FROM AG_ITEM_PRINCIPAL
CREATE TABLE AG_ITEM_PRINCIPAL (
ID_ITEM SERIAL  PRIMARY KEY NOT NULL,
NOME_ITEM VARCHAR NOT NULL,
DATA_INSERIDO TIMESTAMP NOT NULL,
NOME_ICONE VARCHAR
)

CREATE TABLE AG_CUSTO_VARIADO (
ID_ITEM SERIAL  NOT NULL,
ID_CUSTO_VARIADO SERIAL PRIMARY KEY,
DESCRICAO VARCHAR NOT NULL,
VALOR  NUMERIC(10,2) NOT NULL,
UNIDADE_MEDIDA VARCHAR NOT NULL,
QUANTIDADE NUMERIC(10,2)  NOT NULL,
DATA_INSERIDO TIMESTAMP NOT NULL,
    CONSTRAINT FK_ID_ITEM
      FOREIGN KEY(ID_ITEM) 
      REFERENCES AG_ITEM_PRINCIPAL(ID_ITEM)
)

CREATE TABLE AG_CUSTO_FIXO (
ID_ITEM SERIAL  NOT NULL,
ID_CUSTO_FIXO SERIAL PRIMARY KEY,
DESCRICAO VARCHAR NOT NULL,
VALOR  NUMERIC(10,2) NOT NULL,
UNIDADE_MEDIDA VARCHAR NOT NULL,
QUANTIDADE NUMERIC(10,2)  NOT NULL,
DATA_INSERIDO TIMESTAMP NOT NULL,
    CONSTRAINT FK_ID_ITEM
      FOREIGN KEY(ID_ITEM) 
      REFERENCES AG_ITEM_PRINCIPAL(ID_ITEM)
)

        INSERT INTO AG_ITEM_PRINCIPAL (NOME_ITEM,DATA_INSERIDO,NOME_ICONE) VALUES ('COXINHA',NOW(),'')
         * */ //DB DATA


        public void fillItens(int Id = -1)
        {
            itens.Clear();

            //NpgsqlConnection conn = new NpgsqlConnection("postgres://qplkemryuxkzar:b5b4b8a68c0eb36a8d7d28d4a1a3c2d6fe3ed8d75badd09f611ade3a38100f69@Server=ec2-23-21-229-200.compute-1.amazonaws.com:5432/d1n6ia2nei0rp0");

            //var connString = "Server=ec2-23-21-229-200.compute-1.amazonaws.com;Port=5432;User Id=qplkemryuxkzar;Password=b5b4b8a68c0eb36a8d7d28d4a1a3c2d6fe3ed8d75badd09f611ade3a38100f69;Database=d1n6ia2nei0rp0;";


            using (var conn = new NpgsqlConnection(connectionName))
            {
                conn.Open();

                {
                    // NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM AG_ITEM_PRINCIPAL", conn);
                    NpgsqlCommand command;


                    if (Id <= -1)
                        command = new NpgsqlCommand("SELECT * FROM AG_ITEM_PRINCIPAL", conn);
                    else
                        command = new NpgsqlCommand("SELECT * FROM AG_ITEM_PRINCIPAL WHERE ID_ITEM = "+Id, conn);


                    NpgsqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {

                        //(int id, string nome, DateTime data, string icone)


                        int id = dr.GetInt32(0);
                        string nome = dr.GetString(1);
                        DateTime _data = dr.GetDateTime(2);
                        string data = _data.ToString("dd/MM/yyyy");
                        string icone = dr.GetString(3);
                        itens.Add(new AG_ITEM_PRINCIPAL(id, nome, data, icone));
                    }
                }
                conn.Close();

                conn.Open();
                {
                    // NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM AG_ITEM_PRINCIPAL", conn);
                    NpgsqlCommand command2 = new NpgsqlCommand("SELECT * FROM AG_CUSTO_VARIADO ", conn);

                    NpgsqlDataReader dr2 = command2.ExecuteReader();
                    while (dr2.Read())
                    {
                        int id = dr2.GetInt32(1);
                        string descricao = dr2.GetString(2);
                        float valor = dr2.GetFloat(3);
                        string unidade_medida = dr2.GetString(4);
                        int quantidade = dr2.GetInt32(5);
                        DateTime _data_inserido = (dr2.GetDateTime(6));
                        string data_inserido = _data_inserido.ToString("dd/MM/yyyy");

                        foreach (AG_ITEM_PRINCIPAL item in itens)
                        {
                            if (item.ID_ITEM == dr2.GetInt32(0))
                            {
                                item.custosVariados.Add(new AG_CUSTO_VARIADO(dr2.GetInt32(0), id, descricao, valor, unidade_medida, quantidade, data_inserido));
                                break;
                            }
                        }

                    }
                }
                conn.Close();
                conn.Open();

                {
                    // NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM AG_ITEM_PRINCIPAL", conn);
                    NpgsqlCommand command3 = new NpgsqlCommand("SELECT * FROM AG_CUSTO_FIXO  ", conn);

                    NpgsqlDataReader dr3 = command3.ExecuteReader();
                    while (dr3.Read())
                    {
                        int id = dr3.GetInt32(1);
                        string descricao = dr3.GetString(2);
                        float valor = dr3.GetFloat(3);
                        string unidade_medida = dr3.GetString(4);
                        int quantidade = dr3.GetInt32(5);
                        DateTime _data_inserido = (dr3.GetDateTime(6));
                        string data_inserido = _data_inserido.ToString("dd/MM/yyyy");

                        foreach (AG_ITEM_PRINCIPAL item in itens)
                        {
                            if (item.ID_ITEM == dr3.GetInt32(0))
                            {
                                item.custosFixos.Add(new AG_CUSTO_FIXO(dr3.GetInt32(0), id, descricao, valor, unidade_medida, quantidade, data_inserido));
                                break;
                            }


                        }
                    }
                }

                //Console.WriteLine("I hope that works");
                //NpgsqlDataReader dr = command.ExecuteReader();

                /*
                while (dr.Read())
                {
                    int id = int.Parse(dr.GetString(0));
                    string nome = dr.GetString(1);
                    DateTime data = DateTime.Parse(dr.GetString(2));
                    string icone = dr.GetString(3);
                    itens.Add(new AG_ITEM_PRINCIPAL(id, nome, data, icone));
                }
                */

                conn.Close();

            }


            //NpgsqlTransaction tran = conn.BeginTransaction();

            //var command = new NpgsqlCommand("SELECT * FROM AG_ITEM_PRINCIPAL", conn);

            //var reader = command.ExecuteReader();




            /*
            command.CommandType = System.Data.CommandType.StoredProcedure;

            NpgsqlDataReader dr = command.ExecuteReader();

            if (conn.State == ConnectionState.Open)
            {
                Console.WriteLine("Connected!!");
            }

            while (dr.Read())
            {
                Console.Write("{0} \t {1} \t {2} \t {3}", dr[0], dr[1], dr[2], dr[3]);
            }
            */
            //tran.Commit();

        }

        
        public string addItem(string Nome, string Icone)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionName))
            {
                conn.Open();

                {
                    if (Nome.Length == 0 || Icone.Length == 0)
                    {
                        conn.Close();
                        return "{Error: 'Você precisa digitar todos os campos (Nome=nome_example&Icone=icon_example')}";
                    }

                    NpgsqlCommand command = new NpgsqlCommand("INSERT INTO AG_ITEM_PRINCIPAL (NOME_ITEM,DATA_INSERIDO,NOME_ICONE) VALUES ('"+Nome+"',NOW(),'"+Icone+"')", conn);

                    NpgsqlDataReader comRes = command.ExecuteReader();
                    

                }
                conn.Close();
                return "{return:200, data:'Inserido com sucesso!'}";


            }

        }
        public string alterItem(string ItemId, string Nome, string Icone)
        {

            using (var conn = new NpgsqlConnection(connectionName))
            {
                conn.Open();

                {
                    NpgsqlCommand command = new NpgsqlCommand("UPDATE AG_ITEM_PRINCIPAL SET NOME_ITEM ='" + Nome + "', NOME_ICONE ='" + Icone + "'  WHERE ID_ITEM='" + ItemId + "'", conn);

                   
                    NpgsqlDataReader comRes = command.ExecuteReader();

                    conn.Close();
                    return "{return:200, data:'Alterado com sucesso!'}";

                }
                conn.Close();

            }
            
            return "{error:'What i need to do ?'}";
        }

        public string Post(string t, string Nome, string Icone)
        {
            if (t == "addItem")
            {
                return addItem(Nome, Icone);
            }
            else
            {
                return alterItem(t, Nome, Icone);
            }
        }

        public string Delete(string Id)
        {
            using (var conn = new NpgsqlConnection(connectionName))
            {
                conn.Open();

                {
                    NpgsqlCommand command = new NpgsqlCommand("DELETE FROM AG_CUSTO_VARIADO WHERE ID_ITEM = " + Id, conn);

                    NpgsqlDataReader comRes = command.ExecuteReader();

                }

                conn.Close();

                conn.Open();

                {
                    NpgsqlCommand command = new NpgsqlCommand("DELETE FROM AG_CUSTO_FIXO  WHERE ID_ITEM = " + Id, conn);

                    NpgsqlDataReader comRes = command.ExecuteReader();

                }

                conn.Close();

                conn.Open();

                {
                    NpgsqlCommand command = new NpgsqlCommand("DELETE FROM AG_ITEM_PRINCIPAL WHERE ID_ITEM = " + Id, conn);

                    NpgsqlDataReader comRes = command.ExecuteReader();

                }

                conn.Close();

                return "{return:200, data:'Item e seus custos deletados com sucesso!'}";
            }

        }
    }
}

