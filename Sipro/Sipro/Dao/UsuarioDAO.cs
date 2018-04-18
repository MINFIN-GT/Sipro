using System;
using SiproModel.Models;
using Sipro.Utilities;
using Sipro.Utilities.Identity;
using Dapper;
using Dapper.FastCrud;
using Dapper.Contrib;

namespace Sipro.Dao
{
    public class UsuarioDAO
    {
        public UsuarioDAO()
        {
            new CMariaDB();
        }

        public static Usuario getUsuario(String usuario)
        {
            Usuario ret = new Usuario();
            try
            {
                if (CMariaDB.connect())
                {
                    String query = String.Join(" ", "Select * from usuario u where u.usuario=@usuario");
                    using (var connection = CMariaDB.getConnection())
                    {
                        ret = connection.QueryFirstOrDefault<Usuario>(query, new { usuario = usuario });
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "UsuarioDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }

            return ret;
        }

        public static void userLoginHistory(String usuario)
        {
            try
            {
                if (CMariaDB.connect())
                {
                    using (var connection = CMariaDB.getConnection())
                    {
                        Usuariolog usuario_log = new Usuariolog();
                        usuario_log.usuario = usuario;
                        usuario_log.fecha = Utils.getFechaHora(DateTime.Now);
                        connection.Update<Usuariolog>(usuario_log);
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "UsuarioDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }
        }

        public static Boolean tienePermiso(String usuario, String permisonombre)
        {
            Boolean ret = false;
            try
            {
                if (CMariaDB.connect())
                {
                    using (var connection = CMariaDB.getConnection())
                    {
                        String query = String.Join(" ", "Select * from permiso p, usuario_permiso up, usuario u",
                        "where p.id = up.permisoid and up.usuariousuario = u.usuario",
                        "and u.usuario = @usuario and p.nombre = @permisonombre");
                        Permiso permiso = connection.QueryFirstOrDefault<Permiso>(query, new { usuario = usuario, permisonombre = permisonombre });
                        if (permiso != null)
                        {
                            if (permiso.estado == 1)
                                ret = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "UsuarioDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }

            return ret;
        }

        public static Boolean registroUsuario(String cadenausuario, String email, String passwordTextoPlano, String usuarioCreo, Int32 sistemaUsuario)
        {
            Boolean ret = false;
            try
            {
                if (CMariaDB.connect())
                {
                    using (var connection = CMariaDB.getConnection())
                    {
                        Usuario usuario = new Usuario();
                        usuario._usuario = cadenausuario;
                        usuario.email = email;
                        String[] encripted = SHA256Hasher.ComputeHash(passwordTextoPlano);
                        usuario.password = encripted[0];
                        usuario.salt = encripted[1];
                        usuario.fecha_creacion = Utils.getFechaHora(DateTime.Now);
                        usuario.estado = 1;
                        usuario.usuario_creo = usuarioCreo;
                        usuario.sistema_usuario = sistemaUsuario;
                        //connection.Insert(new Usuario
                        //{
                        //    _usuario = cadenausuario,
                        //    email = email,
                        //    password = encripted[0],
                        //    salt = encripted[1],
                        //    fecha_creacion = Utils.getFechaHora(DateTime.Now),
                        //    fecha_actualizacion = null,
                        //    estado = 1,
                        //    usuario_creo = usuarioCreo,
                        //    usuario_actualizo = null,
                        //    sistema_usuario = sistemaUsuario
                        //});
                        connection.Insert<Usuario>(usuario);
                        ret = true;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "UsuarioDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }

            return ret;
        }

        public static Boolean cambiarPassword(String usuario, String password, String usuarioActualizo)
        {
            Boolean ret = false;
            try
            {
                if (CMariaDB.connect())
                {
                    using (var connection = CMariaDB.getConnection())
                    {
                        string query = String.Join(" ", "Select * from usuario where usuario=@usuario");
                        Usuario getusaurio = connection.QueryFirstOrDefault<Usuario>(query, new { usuario = usuario });
                        String[] encripted = SHA256Hasher.ComputeHash(password);
                        getusaurio.password = encripted[0];
                        getusaurio.salt = encripted[1];
                        getusaurio.usuario_actualizo = usuarioActualizo;
                        getusaurio.fecha_actualizacion = Utils.getFechaHora(DateTime.Now);
                        connection.Update<Usuario>(getusaurio);
                        ret = true;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "UsuarioDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }

            return ret;
        }


    }

}
