using Enterprise.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlDataReader = Microsoft.Data.SqlClient.SqlDataReader;
using SqlTransaction = Microsoft.Data.SqlClient.SqlTransaction;

namespace Enterprise.Data
{
    public static class AppDataConnection
    {
        // Conexão direta (sem ConfigurationManager para evitar erro)
        private static readonly string _connectionString =
            "Server=localhost\\ROBSONDEV;Database=Enterprise;Trusted_Connection=True;TrustServerCertificate=True;";

        // ─────────────────────────────────────────
        // LIGAÇÃO
        // ─────────────────────────────────────────
        private static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public static bool TestarLigacao()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch { return false; }
        }

        // ─────────────────────────────────────────
        // GERAR NÚMERO
        // ─────────────────────────────────────────
        public static string GerarNumero(string tabela, string prefixo)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string ano = DateTime.Now.Year.ToString();
                    string sql = $"SELECT COUNT(*) FROM {tabela} WHERE YEAR(criado_em) = {ano}";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                        return $"{prefixo}-{ano}-{count.ToString().PadLeft(3, '0')}";
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar número: " + e.Message);
            }
        }

        // ═══════════════════════════════════════════
        // EMPRESA
        // ═══════════════════════════════════════════
        public static Empresa? GetEmpresa()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT TOP 1 * FROM empresa";

                    using (var cmd = new SqlCommand(sql, conn))
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                            return new Empresa
                            {
                                Id = r.GetInt32(r.GetOrdinal("id")),
                                Nome = r.GetString(r.GetOrdinal("nome")),
                                Nuit = r.IsDBNull(r.GetOrdinal("nuit")) ? null : r.GetString(r.GetOrdinal("nuit")),
                                Endereco = r.IsDBNull(r.GetOrdinal("endereco")) ? null : r.GetString(r.GetOrdinal("endereco")),
                                Telefone = r.IsDBNull(r.GetOrdinal("telefone")) ? null : r.GetString(r.GetOrdinal("telefone")),
                                Email = r.IsDBNull(r.GetOrdinal("email")) ? null : r.GetString(r.GetOrdinal("email")),
                                Website = r.IsDBNull(r.GetOrdinal("website")) ? null : r.GetString(r.GetOrdinal("website")),
                                LogoPath = r.IsDBNull(r.GetOrdinal("logo_path")) ? null : r.GetString(r.GetOrdinal("logo_path")),
                                ContaBancaria = r.IsDBNull(r.GetOrdinal("conta_bancaria")) ? null : r.GetString(r.GetOrdinal("conta_bancaria")),
                                Banco = r.IsDBNull(r.GetOrdinal("banco")) ? null : r.GetString(r.GetOrdinal("banco")),
                                MoedaSimbolo = r.GetString(r.GetOrdinal("moeda_simbolo"))
                            };
                    }
                }
            }
            catch { }
            return null;
        }

        public static void SalvarEmpresa(Empresa emp)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                int count;
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM empresa", conn))
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                string sql = count == 0
                    ? @"INSERT INTO empresa (nome,nuit,endereco,telefone,email,
                         website,logo_path,conta_bancaria,banco,moeda_simbolo)
                         VALUES (@nome,@nuit,@end,@tel,@email,@web,@logo,@conta,@banco,@moeda)"
                    : @"UPDATE empresa SET nome=@nome,nuit=@nuit,endereco=@end,
                         telefone=@tel,email=@email,website=@web,logo_path=@logo,
                         conta_bancaria=@conta,banco=@banco,moeda_simbolo=@moeda";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", emp.Nome);
                    cmd.Parameters.AddWithValue("@nuit", (object?)emp.Nuit ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@end", (object?)emp.Endereco ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@tel", (object?)emp.Telefone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@email", (object?)emp.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@web", (object?)emp.Website ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@logo", (object?)emp.LogoPath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@conta", (object?)emp.ContaBancaria ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@banco", (object?)emp.Banco ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@moeda", emp.MoedaSimbolo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ═══════════════════════════════════════════
        // CLIENTES
        // ═══════════════════════════════════════════
        public static List<Cliente> GetClientes(string filtro = "")
        {
            var lista = new List<Cliente>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM clientes WHERE 1=1";

                if (!string.IsNullOrEmpty(filtro))
                    sql += " AND (nome LIKE @filtro OR tipo LIKE @filtro)";

                sql += " ORDER BY nome";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(filtro))
                        cmd.Parameters.AddWithValue("@filtro", $"%{filtro}%");

                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                            lista.Add(LerCliente(r));
                }
            }
            return lista;
        }

        public static Cliente? GetClientePorId(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM clientes WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var r = cmd.ExecuteReader())
                        if (r.Read()) return LerCliente(r);
                }
            }
            return null;
        }

        public static int SalvarCliente(Cliente c)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = c.Id == 0
                    ? @"INSERT INTO clientes (nome,tipo)
                         VALUES (@nome,@tipo);
                         SELECT SCOPE_IDENTITY();"
                    : @"UPDATE clientes SET nome=@nome,tipo=@tipo WHERE id=@id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", c.Nome);
                    cmd.Parameters.AddWithValue("@tipo", c.Tipo);
             
                    if (c.Id > 0) cmd.Parameters.AddWithValue("@id", c.Id);

                    var res = cmd.ExecuteScalar();
                    return c.Id == 0 ? Convert.ToInt32(res) : c.Id;
                }
            }
        }

        public static void ApagarCliente(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("DELETE FROM clientes WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static Cliente LerCliente(SqlDataReader r) => new Cliente
        {
            Id = r.GetInt32(r.GetOrdinal("id")),
            Nome = r.GetString(r.GetOrdinal("nome")),
            Tipo = r.GetString(r.GetOrdinal("tipo")),
            
        };

        // ═══════════════════════════════════════════
        // SERVIÇOS
        // ═══════════════════════════════════════════
        public static List<CategoriaServico> GetCategoriasServico()
        {
            var lista = new List<CategoriaServico>();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM categorias_servico ORDER BY nome", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        lista.Add(new CategoriaServico
                        {
                            Id = r.GetInt32(r.GetOrdinal("id")),
                            Nome = r.GetString(r.GetOrdinal("nome")),
                            Descricao = r.IsDBNull(r.GetOrdinal("descricao")) ? null : r.GetString(r.GetOrdinal("descricao"))
                        });
            }
            return lista;
        }

        public static List<Servico> GetServicos(string filtro = "", bool apenasActivos = true)
        {
            var lista = new List<Servico>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT s.*, c.nome AS cat_nome
                               FROM servicos s
                               LEFT JOIN categorias_servico c ON s.categoria_id = c.id
                               WHERE 1=1";

                if (apenasActivos) sql += " AND s.activo = 1";
                if (!string.IsNullOrEmpty(filtro))
                    sql += " AND (s.nome LIKE @filtro OR s.codigo LIKE @filtro)";
                sql += " ORDER BY s.nome";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(filtro))
                        cmd.Parameters.AddWithValue("@filtro", $"%{filtro}%");

                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                            lista.Add(LerServico(r));
                }
            }
            return lista;
        }

        public static int SalvarServico(Servico s)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = s.Id == 0
                    ? @"INSERT INTO servicos (categoria_id,codigo,nome,activo)
                         VALUES (@cat,@cod,@nome,@activo);
                         SELECT SCOPE_IDENTITY();"
                    : @"UPDATE servicos SET categoria_id=@cat,codigo=@cod,nome=@nome,
                         
                         WHERE id=@id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cat", (object?)s.CategoriaId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@cod", (object?)s.Codigo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nome", s.Nome);
                    cmd.Parameters.AddWithValue("@activo", s.Activo);
                    if (s.Id > 0) cmd.Parameters.AddWithValue("@id", s.Id);

                    var res = cmd.ExecuteScalar();
                    return s.Id == 0 ? Convert.ToInt32(res) : s.Id;
                }
            }
        }

        public static void ApagarServico(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("DELETE FROM servicos WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static Servico LerServico(SqlDataReader r) => new Servico
        {
            Id = r.GetInt32(r.GetOrdinal("id")),
            CategoriaId = r.IsDBNull(r.GetOrdinal("categoria_id")) ? null : r.GetInt32(r.GetOrdinal("categoria_id")),
            Codigo = r.IsDBNull(r.GetOrdinal("codigo")) ? null : r.GetString(r.GetOrdinal("codigo")),
            Nome = r.GetString(r.GetOrdinal("nome")),
            Activo = r.GetBoolean(r.GetOrdinal("activo")),
            CriadoEm = r.GetDateTime(r.GetOrdinal("criado_em")),
            CategoriaNome = r.IsDBNull(r.GetOrdinal("cat_nome")) ? null : r.GetString(r.GetOrdinal("cat_nome"))  // ← CORRIGIDO: string em vez de CategoriaServico
        };

        // ═══════════════════════════════════════════
        // ITENS — reutilizável
        // ═══════════════════════════════════════════
        public static List<ItemDocumento> GetItensPorDocumento(string tipo, int docId)
        {
            var lista = new List<ItemDocumento>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT i.*, s.nome AS svc_nome
                               FROM itens_documento i
                               LEFT JOIN servicos s ON i.servico_id = s.id
                               WHERE i.tipo_documento=@tipo AND i.documento_id=@id
                               ORDER BY i.ordem";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@id", docId);
                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                            lista.Add(new ItemDocumento
                            {
                                Id = r.GetInt32(r.GetOrdinal("id")),
                                ServicoId = r.IsDBNull(r.GetOrdinal("servico_id")) ? null : r.GetInt32(r.GetOrdinal("servico_id")),
                                Descricao = r.GetString(r.GetOrdinal("descricao")),
                                Unidade = r.GetString(r.GetOrdinal("unidade")),
                                Quantidade = r.GetDecimal(r.GetOrdinal("quantidade")),
                                PrecoUnitario = r.GetDecimal(r.GetOrdinal("preco_unitario")),
                                Desconto = r.GetDecimal(r.GetOrdinal("desconto")),
                                Ordem = r.GetInt32(r.GetOrdinal("ordem"))
                            });
                }
            }
            return lista;
        }

        private static void SalvarItens(SqlConnection conn, SqlTransaction trans,
            string tipo, int docId, List<ItemDocumento> itens)
        {
            using (var cmd = new SqlCommand(
                "DELETE FROM itens_documento WHERE tipo_documento=@tipo AND documento_id=@id",
                conn, trans))
            {
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@id", docId);
                cmd.ExecuteNonQuery();
            }

            int ordem = 1;
            foreach (var item in itens)
            {
                string sql = @"INSERT INTO itens_documento
                    (tipo_documento,documento_id,servico_id,descricao,unidade,
                     quantidade,preco_unitario,desconto,ordem)
                    VALUES (@tipo,@docId,@svcId,@desc,@un,@qtd,@preco,@desc2,@ordem)";

                using (var cmd = new SqlCommand(sql, conn, trans))
                {
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@docId", docId);
                    cmd.Parameters.AddWithValue("@svcId", (object?)item.ServicoId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@desc", item.Descricao);
                    cmd.Parameters.AddWithValue("@un", item.Unidade);
                    cmd.Parameters.AddWithValue("@qtd", item.Quantidade);
                    cmd.Parameters.AddWithValue("@preco", item.PrecoUnitario);
                    cmd.Parameters.AddWithValue("@desc2", item.Desconto);
                    cmd.Parameters.AddWithValue("@ordem", ordem++);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ═══════════════════════════════════════════
        // COTAÇÕES
        // ═══════════════════════════════════════════
        public static List<Cotacao> GetCotacoes()
        {
            var lista = new List<Cotacao>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT c.*, cl.nome AS cli_nome
                               FROM cotacoes c
                               LEFT JOIN clientes cl ON c.cliente_id = cl.id
                               ORDER BY c.criado_em DESC";

                using (var cmd = new SqlCommand(sql, conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        lista.Add(new Cotacao
                        {
                            Id = r.GetInt32(r.GetOrdinal("id")),
                            Numero = r.GetString(r.GetOrdinal("numero")),
                            ClienteId = r.GetInt32(r.GetOrdinal("cliente_id")),
                            Data = r.GetDateTime(r.GetOrdinal("data")),
                            Estado = r.GetString(r.GetOrdinal("estado")),
                            Iva = 16,
                            CriadoEm = r.GetDateTime(r.GetOrdinal("criado_em")),
                            Cliente = new Cliente { Nome = r.GetString(r.GetOrdinal("cli_nome")) }
                        });
            }
            foreach (var c in lista)
                c.Itens = GetItensPorDocumento("cotacao", c.Id);
            return lista;
        }

        public static List<Cotacao> GetCotacoesPorEstado(string estado)
        {
            var lista = new List<Cotacao>();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT * FROM cotacoes WHERE estado=@est ORDER BY criado_em DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@est", estado);
                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                            lista.Add(new Cotacao
                            {
                                Id = r.GetInt32(r.GetOrdinal("id")),
                                Numero = r.GetString(r.GetOrdinal("numero")),
                                ClienteId = r.GetInt32(r.GetOrdinal("cliente_id")),
                                Data = r.GetDateTime(r.GetOrdinal("data")),
                                Estado = r.GetString(r.GetOrdinal("estado"))
                            });
                }
            }
            return lista;
        }

        public static Cotacao? GetCotacaoPorId(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM cotacoes WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            var c = new Cotacao
                            {
                                Id = r.GetInt32(r.GetOrdinal("id")),
                                Numero = r.GetString(r.GetOrdinal("numero")),
                                ClienteId = r.GetInt32(r.GetOrdinal("cliente_id")),
                                Data = r.GetDateTime(r.GetOrdinal("data")),
                                Iva = 16,
                                Estado = r.GetString(r.GetOrdinal("estado"))
                            };
                            c.Itens = GetItensPorDocumento("cotacao", c.Id);
                            return c;
                        }
                    }
                }
            }
            return null;
        }

        public static int SalvarCotacao(Cotacao c)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = c.Id == 0
                            ? @"INSERT INTO cotacoes (numero,cliente_id,data,data_validade,
                                 local_obra,observacoes,iva,estado)
                                 VALUES (@num,@cli,@data,@val,@local,@obs,@iva,@est);
                                 SELECT SCOPE_IDENTITY();"
                            : @"UPDATE cotacoes SET cliente_id=@cli,data=@data,
                                 data_validade=@val,local_obra=@local,observacoes=@obs,
                                 iva=@iva,estado=@est WHERE id=@id";

                        int id;
                        using (var cmd = new SqlCommand(sql, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@num", c.Numero);
                            cmd.Parameters.AddWithValue("@cli", c.ClienteId);
                            cmd.Parameters.AddWithValue("@data", c.Data.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@val", c.DataValidade.HasValue ? c.DataValidade.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@local", (object?)c.LocalObra ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@obs", (object?)c.Observacoes ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@iva", c.Iva);
                            cmd.Parameters.AddWithValue("@est", c.Estado);
                            if (c.Id > 0) cmd.Parameters.AddWithValue("@id", c.Id);

                            var res = cmd.ExecuteScalar();
                            id = c.Id == 0 ? Convert.ToInt32(res) : c.Id;
                        }

                        SalvarItens(conn, trans, "cotacao", id, c.Itens);
                        trans.Commit();
                        return id;
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        // ═══════════════════════════════════════════
        // FACTURAS
        // ═══════════════════════════════════════════
        public static List<Factura> GetFacturas()
        {
            var lista = new List<Factura>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT f.*, cl.nome AS cli_nome,
                               ISNULL((SELECT SUM(valor_pago) FROM recibos WHERE factura_id=f.id),0) AS total_pago
                               FROM facturas f
                               LEFT JOIN clientes cl ON f.cliente_id = cl.id
                               ORDER BY f.criado_em DESC";

                using (var cmd = new SqlCommand(sql, conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        lista.Add(new Factura
                        {
                            Id = r.GetInt32(r.GetOrdinal("id")),
                            Numero = r.GetString(r.GetOrdinal("numero")),
                            ClienteId = r.GetInt32(r.GetOrdinal("cliente_id")),
                            CotacaoId = r.IsDBNull(r.GetOrdinal("cotacao_id")) ? null : r.GetInt32(r.GetOrdinal("cotacao_id")),
                            Data = r.GetDateTime(r.GetOrdinal("data")),
                            DataVencimento = r.IsDBNull(r.GetOrdinal("data_vencimento")) ? null : r.GetDateTime(r.GetOrdinal("data_vencimento")),
                            LocalObra = r.IsDBNull(r.GetOrdinal("local_obra")) ? null : r.GetString(r.GetOrdinal("local_obra")),
                            Observacoes = r.IsDBNull(r.GetOrdinal("observacoes")) ? null : r.GetString(r.GetOrdinal("observacoes")),
                            Iva = r.GetDecimal(r.GetOrdinal("iva")),
                            Estado = r.GetString(r.GetOrdinal("estado")),
                            TotalPago = r.GetDecimal(r.GetOrdinal("total_pago")),
                            CriadoEm = r.GetDateTime(r.GetOrdinal("criado_em")),
                            Cliente = new Cliente { Nome = r.GetString(r.GetOrdinal("cli_nome")) }
                        });
            }
            foreach (var f in lista)
                f.Itens = GetItensPorDocumento("factura", f.Id);
            return lista;
        }

        public static List<Factura> GetFacturasPendentes(int clienteId)
        {
            var lista = new List<Factura>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT f.*,
                               ISNULL((SELECT SUM(valor_pago) FROM recibos WHERE factura_id=f.id),0) AS total_pago
                               FROM facturas f
                               WHERE f.cliente_id=@cliId
                               AND f.estado IN ('Pendente','Parcial')
                               ORDER BY f.data DESC";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cliId", clienteId);
                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                        {
                            var f = new Factura
                            {
                                Id = r.GetInt32(r.GetOrdinal("id")),
                                Numero = r.GetString(r.GetOrdinal("numero")),
                                ClienteId = r.GetInt32(r.GetOrdinal("cliente_id")),
                                Data = r.GetDateTime(r.GetOrdinal("data")),
                                Iva = r.GetDecimal(r.GetOrdinal("iva")),
                                Estado = r.GetString(r.GetOrdinal("estado")),
                                TotalPago = r.GetDecimal(r.GetOrdinal("total_pago"))
                            };
                            f.Itens = GetItensPorDocumento("factura", f.Id);
                            lista.Add(f);
                        }
                }
            }
            return lista;
        }

        public static int SalvarFactura(Factura f)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = f.Id == 0
                            ? @"INSERT INTO facturas (numero,cliente_id,cotacao_id,data,
                                 data_vencimento,local_obra,observacoes,iva,estado)
                                 VALUES (@num,@cli,@cot,@data,@venc,@local,@obs,@iva,@est);
                                 SELECT SCOPE_IDENTITY();"
                            : @"UPDATE facturas SET cliente_id=@cli,cotacao_id=@cot,data=@data,
                                 data_vencimento=@venc,local_obra=@local,observacoes=@obs,
                                 iva=@iva,estado=@est WHERE id=@id";

                        int id;
                        using (var cmd = new SqlCommand(sql, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@num", f.Numero);
                            cmd.Parameters.AddWithValue("@cli", f.ClienteId);
                            cmd.Parameters.AddWithValue("@cot", (object?)f.CotacaoId ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@data", f.Data.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@venc", f.DataVencimento.HasValue ? f.DataVencimento.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@local", (object?)f.LocalObra ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@obs", (object?)f.Observacoes ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@iva", f.Iva);
                            cmd.Parameters.AddWithValue("@est", f.Estado);
                            if (f.Id > 0) cmd.Parameters.AddWithValue("@id", f.Id);

                            var res = cmd.ExecuteScalar();
                            id = f.Id == 0 ? Convert.ToInt32(res) : f.Id;
                        }

                        SalvarItens(conn, trans, "factura", id, f.Itens);
                        trans.Commit();
                        return id;
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        // ═══════════════════════════════════════════
        // RECIBOS
        // ═══════════════════════════════════════════
        public static List<Recibo> GetRecibos()
        {
            var lista = new List<Recibo>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT r.*, cl.nome AS cli_nome, f.numero AS fat_numero
                               FROM recibos r
                               LEFT JOIN clientes cl ON r.cliente_id = cl.id
                               LEFT JOIN facturas f  ON r.factura_id = f.id
                               ORDER BY r.criado_em DESC";

                using (var cmd = new SqlCommand(sql, conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        lista.Add(new Recibo
                        {
                            Id = r.GetInt32(r.GetOrdinal("id")),
                            Numero = r.GetString(r.GetOrdinal("numero")),
                            FacturaId = r.GetInt32(r.GetOrdinal("factura_id")),
                            ClienteId = r.GetInt32(r.GetOrdinal("cliente_id")),
                            Data = r.GetDateTime(r.GetOrdinal("data")),
                            ValorPago = r.GetDecimal(r.GetOrdinal("valor_pago")),
                            FormaPagamento = r.GetString(r.GetOrdinal("forma_pagamento")),
                            Referencia = r.IsDBNull(r.GetOrdinal("referencia")) ? null : r.GetString(r.GetOrdinal("referencia")),
                            Observacoes = r.IsDBNull(r.GetOrdinal("observacoes")) ? null : r.GetString(r.GetOrdinal("observacoes")),
                            CriadoEm = r.GetDateTime(r.GetOrdinal("criado_em")),
                            Cliente = new Cliente { Nome = r.GetString(r.GetOrdinal("cli_nome")) },
                            Factura = new Factura { Numero = r.GetString(r.GetOrdinal("fat_numero")) }
                        });
            }
            return lista;
        }

        public static int SalvarRecibo(Recibo rec)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = rec.Id == 0
                            ? @"INSERT INTO recibos (numero,factura_id,cliente_id,data,
                                 valor_pago,forma_pagamento,referencia,observacoes)
                                 VALUES (@num,@fat,@cli,@data,@val,@forma,@ref,@obs);
                                 SELECT SCOPE_IDENTITY();"
                            : @"UPDATE recibos SET factura_id=@fat,cliente_id=@cli,data=@data,
                                 valor_pago=@val,forma_pagamento=@forma,referencia=@ref,
                                 observacoes=@obs WHERE id=@id";

                        int id;
                        using (var cmd = new SqlCommand(sql, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@num", rec.Numero);
                            cmd.Parameters.AddWithValue("@fat", rec.FacturaId);
                            cmd.Parameters.AddWithValue("@cli", rec.ClienteId);
                            cmd.Parameters.AddWithValue("@data", rec.Data.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@val", rec.ValorPago);
                            cmd.Parameters.AddWithValue("@forma", rec.FormaPagamento);
                            cmd.Parameters.AddWithValue("@ref", (object?)rec.Referencia ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@obs", (object?)rec.Observacoes ?? DBNull.Value);
                            if (rec.Id > 0) cmd.Parameters.AddWithValue("@id", rec.Id);

                            var res = cmd.ExecuteScalar();
                            id = rec.Id == 0 ? Convert.ToInt32(res) : rec.Id;
                        }

                        ActualizarEstadoFactura(conn, trans, rec.FacturaId);
                        trans.Commit();
                        return id;
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        private static void ActualizarEstadoFactura(SqlConnection conn,
            SqlTransaction trans, int facturaId)
        {
            string sqlInfo = @"SELECT
                ISNULL((SELECT SUM(i.quantidade * i.preco_unitario * (1 - i.desconto/100))
                        FROM itens_documento i
                        WHERE i.tipo_documento='factura' AND i.documento_id=@id), 0)
                * (1 + (SELECT iva/100 FROM facturas WHERE id=@id)) AS total,
                ISNULL((SELECT SUM(valor_pago) FROM recibos WHERE factura_id=@id), 0) AS pago";

            decimal total = 0, pago = 0;

            using (var cmd = new SqlCommand(sqlInfo, conn, trans))
            {
                cmd.Parameters.AddWithValue("@id", facturaId);
                using (var r = cmd.ExecuteReader())
                    if (r.Read())
                    {
                        total = r.GetDecimal(0);
                        pago = r.GetDecimal(1);
                    }
            }

            string estado = pago <= 0 ? "Pendente"
                          : pago >= total ? "Paga"
                          : "Parcial";

            using (var cmd = new SqlCommand(
                "UPDATE facturas SET estado=@est WHERE id=@id", conn, trans))
            {
                cmd.Parameters.AddWithValue("@est", estado);
                cmd.Parameters.AddWithValue("@id", facturaId);
                cmd.ExecuteNonQuery();
            }
        }

        // ═══════════════════════════════════════════
        // ORDENS DE TRABALHO
        // ═══════════════════════════════════════════
        public static List<OrdemTrabalho> GetOrdens()
        {
            var lista = new List<OrdemTrabalho>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT o.*, cl.nome AS cli_nome
                               FROM ordens_trabalho o
                               LEFT JOIN clientes cl ON o.cliente_id = cl.id
                               ORDER BY o.criado_em DESC";

                using (var cmd = new SqlCommand(sql, conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        lista.Add(new OrdemTrabalho
                        {
                            Id = r.GetInt32(r.GetOrdinal("id")),
                            Numero = r.GetString(r.GetOrdinal("numero")),
                            ClienteId = r.GetInt32(r.GetOrdinal("cliente_id")),
                            Data = r.GetDateTime(r.GetOrdinal("data")),
                            DataInicio = r.IsDBNull(r.GetOrdinal("data_inicio")) ? null : r.GetDateTime(r.GetOrdinal("data_inicio")),
                            DataFim = r.IsDBNull(r.GetOrdinal("data_fim")) ? null : r.GetDateTime(r.GetOrdinal("data_fim")),
                            LocalObra = r.IsDBNull(r.GetOrdinal("local_obra")) ? null : r.GetString(r.GetOrdinal("local_obra")),
                            Descricao = r.IsDBNull(r.GetOrdinal("descricao")) ? null : r.GetString(r.GetOrdinal("descricao")),
                            Observacoes = r.IsDBNull(r.GetOrdinal("observacoes")) ? null : r.GetString(r.GetOrdinal("observacoes")),
                            Iva = r.GetDecimal(r.GetOrdinal("iva")),
                            Estado = r.GetString(r.GetOrdinal("estado")),
                            CriadoEm = r.GetDateTime(r.GetOrdinal("criado_em")),
                            Cliente = new Cliente { Nome = r.GetString(r.GetOrdinal("cli_nome")) }
                        });
            }
            foreach (var o in lista)
                o.Itens = GetItensPorDocumento("ordem_trabalho", o.Id);
            return lista;
        }

        public static int SalvarOrdem(OrdemTrabalho o)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = o.Id == 0
                            ? @"INSERT INTO ordens_trabalho (numero,cliente_id,data,data_inicio,
                                 data_fim,local_obra,descricao,observacoes,iva,estado)
                                 VALUES (@num,@cli,@data,@ini,@fim,@local,@desc,@obs,@iva,@est);
                                 SELECT SCOPE_IDENTITY();"
                            : @"UPDATE ordens_trabalho SET cliente_id=@cli,data=@data,
                                 data_inicio=@ini,data_fim=@fim,local_obra=@local,
                                 descricao=@desc,observacoes=@obs,iva=@iva,estado=@est
                                 WHERE id=@id";

                        int id;
                        using (var cmd = new SqlCommand(sql, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@num", o.Numero);
                            cmd.Parameters.AddWithValue("@cli", o.ClienteId);
                            cmd.Parameters.AddWithValue("@data", o.Data.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@ini", o.DataInicio.HasValue ? o.DataInicio.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@fim", o.DataFim.HasValue ? o.DataFim.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@local", (object?)o.LocalObra ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@desc", (object?)o.Descricao ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@obs", (object?)o.Observacoes ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@iva", o.Iva);
                            cmd.Parameters.AddWithValue("@est", o.Estado);
                            if (o.Id > 0) cmd.Parameters.AddWithValue("@id", o.Id);

                            var res = cmd.ExecuteScalar();
                            id = o.Id == 0 ? Convert.ToInt32(res) : o.Id;
                        }

                        SalvarItens(conn, trans, "ordem_trabalho", id, o.Itens);
                        trans.Commit();
                        return id;
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }
    }
}