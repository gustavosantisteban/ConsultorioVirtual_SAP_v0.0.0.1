﻿using SeguridadWebv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Data.Entity;
using SeguridadWebv2.Models.Aplicacion;
using SeguridadWebv2.Services;
using SeguridadWebv2.Models.ReportClass;

namespace SeguridadWebv2.Models
{
    public class InicializarDatos
    {
        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string nombre = "William Gustavo";
            const string apellido = "Santisteban";
            const bool estado = true;
            const string name = "administrador@mcga.com";
            const string password = "Mcga@123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name,  Nombre = nombre, Apellido = apellido, Estado = estado, EmailConfirmed = true };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            var groupManager = new GrupoManager();
            var newGroup = new ApplicationGroup("Administradores", "Acceso General al Sistema");

            groupManager.CreateGroup(newGroup);
            groupManager.SetUserGroups(user.Id, new string[] { newGroup.Id });
            groupManager.SetGroupRoles(newGroup.Id, new string[] { role.Name });

            
            var especialidades = new List<Especialidad> {
                new Especialidad {
                    NombreEspecialidad = "Dietética / Nutricion",
                    Imagen = "~/Content/img/especialidad/nutricionista.png"
                },
                new Especialidad {
                    NombreEspecialidad = "Ginecología",
                    Imagen = "~/Content/img/especialidad/ginecologia.png"
                },
                new Especialidad {
                    NombreEspecialidad = "Psicología",
                    Imagen = "~/Content/img/especialidad/psicologia.png"
                },
                new Especialidad {
                    NombreEspecialidad = "Dermatología",
                    Imagen = "~/Content/img/especialidad/dermatologia.png"
                },
                new Especialidad {
                    NombreEspecialidad = "Pediatría",
                    Imagen = "~/Content/img/especialidad/pediatria.png"
                },
                new Especialidad {
                    NombreEspecialidad = "Neumología",
                    Imagen = "~/Content/img/especialidad/nutricionista.png"
                    //Imagen = "~/Content/img/especialidad/neumologia.jpg"
                },
                new Especialidad {
                    NombreEspecialidad = "Neurología",
                    Imagen = "~/Content/img/especialidad/nutricionista.png"
                    //Imagen = "~/Content/img/especialidad/neurologia.jpg"
                },
                new Especialidad {
                    NombreEspecialidad = "Psiquiatría",
                    Imagen = "~/Content/img/especialidad/psiquiatria.png"
                }
            };
            especialidades.ForEach(c => db.Especialidades.Add(c));


            var PermisosUsuario = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Usuario"
                },
                new ApplicationRole {
                    Name = "Editar_Usuario"
                },
                new ApplicationRole {
                    Name = "Detalle_Usuario"
                },
                new ApplicationRole {
                    Name = "Eliminar_Usuario"
                },
                new ApplicationRole {
                    Name = "AllUsuarios"
                }
            };
            PermisosUsuario.ForEach(c => db.Roles.Add(c));


            var PermisosGrupo = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Grupo"
                },
                new ApplicationRole {
                    Name = "Editar_Grupo"
                },
                new ApplicationRole {
                    Name = "Detalle_Grupo"
                },
                new ApplicationRole {
                    Name = "Eliminar_Grupo"
                },
                new ApplicationRole {
                    Name = "AllGrupos"
                }
            };
            PermisosGrupo.ForEach(c => db.Roles.Add(c));


            var PermisosAcciones = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Permiso"
                },
                new ApplicationRole {
                    Name = "Editar_Permiso"
                },
                new ApplicationRole {
                    Name = "Detalle_Permiso"
                },
                new ApplicationRole {
                    Name = "Eliminar_Permiso"
                },
                new ApplicationRole {
                    Name = "AllPermisos"
                }
            };
            PermisosUsuario.ForEach(c => db.Roles.Add(c));

            var PermisosTurnos = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Turno"
                },
                new ApplicationRole {
                    Name = "Editar_Turno"
                },
                new ApplicationRole {
                    Name = "Detalle_Turno"
                },
                new ApplicationRole {
                    Name = "Eliminar_Turno"
                },
                new ApplicationRole {
                    Name = "AllTurnos"
                }
            };
            PermisosTurnos.ForEach(c => db.Roles.Add(c));

            var PermisosHorarios = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Horario"
                },
                new ApplicationRole {
                    Name = "Editar_Horario"
                },
                new ApplicationRole {
                    Name = "Detalle_Horario"
                },
                new ApplicationRole {
                    Name = "Eliminar_Horario"
                },
                new ApplicationRole {
                    Name = "AllHorarios"
                }
            };
            PermisosHorarios.ForEach(c => db.Roles.Add(c));

            var PermisosProfesionales = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Profesional"
                },
                new ApplicationRole {
                    Name = "Editar_Profesional"
                },
                new ApplicationRole {
                    Name = "Detalle_Profesional"
                },
                new ApplicationRole {
                    Name = "Eliminar_Profesional"
                },
                new ApplicationRole {
                    Name = "AllProfesionales"
                }
            };
            PermisosProfesionales.ForEach(c => db.Roles.Add(c));

            var grupos = new List<ApplicationGroup> {
                new ApplicationGroup {
                    Name = "Gestionar Usuarios",
                    Description = "Gestionar Usuarios"
                },
                new ApplicationGroup {
                    Name = "Gestionar Grupos",
                    Description = "Gestionar Grupos"
                },
                new ApplicationGroup {
                    Name = "Gestionar Acciones",
                    Description = "Gestionar Acciones"
                },
                new ApplicationGroup {
                    Name = "Gestionar Autos",
                    Description = "Gestionar Autos"
                },
             };
            grupos.ForEach(c => db.ApplicationGroups.Add(c));


            const string nombreesp = "Agustin";
            const string apellidoesp = "Stelzer";
            const bool estadoesp = true;
            const string nameesp = "aguscrack@mcga.com";
            const string passwordesp = "Mcga@123456";
            const string roleNameesp = "Profesionales";
            const string matriculaesp = "EE2015E";
            const string telefonoesp = "3413544172";

            //Create Role Admin if it does not exist
            var roleesp = roleManager.FindByName(roleNameesp);
            if (roleesp == null)
            {
                roleesp = new ApplicationRole(roleNameesp);
                var roleresultado = roleManager.Create(roleesp);
            }

            var especialista = userManager.FindByName(nameesp);
            if (especialista == null)
            {
                especialista = new Especialista {
                    UserName = nameesp,
                    Email = nameesp,
                    Nombre = nombreesp,
                    Apellido = apellidoesp,
                    Estado = estadoesp,
                    PhoneNumber = telefonoesp,
                    EmailConfirmed = true,
                    NumeroMatricula = matriculaesp,
                    Prefijo = Prefijo.Dr,
                    ImagenMedico = "~/Content/img/medicos/agustin_stelzer.jpg",
                    Especialidad = especialidades[2]
                };
                var resultadoesp = userManager.Create(especialista, passwordesp);
                resultadoesp = userManager.SetLockoutEnabled(especialista.Id, false);
            }

            var groupManageresp = new GrupoManager();
            var newGroupesp = new ApplicationGroup("Especialistas", "Acceso Prefesionales al Sistema");
            groupManager.CreateGroup(newGroupesp);
            groupManager.SetUserGroups(especialista.Id, new string[] { newGroupesp.Id });
            groupManager.SetGroupRoles(newGroupesp.Id, new string[] { roleesp.Name });
            
            foreach(var horariosroles in PermisosHorarios.ToList())
            {
                userManager.AddToRoles(especialista.Id, horariosroles.Name);
            }
            foreach (var turnosroles in PermisosTurnos.ToList())
            {
                userManager.AddToRoles(especialista.Id, turnosroles.Name);
            }

            const string nombrepac = "Pamela";
            const string apellidopac = "Sosa";
            const bool estadopac = true;
            const string namepac = "lapame312@mcga.com";
            const string passwordpac = "Mcga@123456";
            const string roleNamepac = "Paciente";
            const string telefonopac = "3413544172";
            
            var rolepac = roleManager.FindByName(roleNamepac);
            if (rolepac == null)
            {
                rolepac = new ApplicationRole(roleNamepac);
                var roleresultadopaciente = roleManager.Create(rolepac);
            }

            var paciente = userManager.FindByName(namepac);
            if (paciente == null)
            {
                paciente = new Paciente
                {
                        UserName = namepac,
                        Email = namepac,
                        Nombre = nombrepac,
                        Apellido = apellidopac,
                        Estado = estadopac,
                        PhoneNumber = telefonopac,
                        EmailConfirmed = true,
                        ImagenPaciente = "~/Content/img/medicos/pamela_sosa.jpg",
                        FechadeCumpleanos = Convert.ToDateTime("1990-12-12 00:00"),
                        Genero = Sexo.Femenino,
                        HistoriaClinica = new HistoriaClinica {
                            Anamnesis = null,
                            FechaCreacion = DateTime.Now
                        }
                    };
                var resultadoesp = userManager.Create(paciente, passwordpac);
                resultadoesp = userManager.SetLockoutEnabled(paciente.Id, false);
            }
            var newGrouppac = new ApplicationGroup("Pacientes", "Acceso Pacientes al Sistema");

            groupManager.CreateGroup(newGrouppac);
            groupManager.SetUserGroups(paciente.Id, new string[] { newGrouppac.Id });
            groupManager.SetGroupRoles(newGrouppac.Id, new string[] { rolepac.Name });

        
            //var paciente2 = userManager.FindByName("pabloaudoglio@gmail.com");
            //if (paciente2 == null)
            //{
            //    paciente2 = new Paciente
            //    {
            //        UserName = "pabloaudoglio@gmail.com",
            //        Email = "pabloaudoglio@gmail.com",
            //        Nombre = "Pablo",
            //        Apellido = "Audoglio",
            //        Estado = true,
            //        PhoneNumber = "3413544172",
            //        EmailConfirmed = true,
            //        ImagenPaciente = "~/Content/img/medicos/pablo_audoglio.png",
            //        FechadeCumpleanos = Convert.ToDateTime("1990-12-12 00:00"),
            //        Genero = Sexo.Femenino,
            //        HistoriaClinica = new HistoriaClinica
            //        {
            //            Anamnesis = null,
            //            FechaCreacion = DateTime.Now
            //        }
            //    };
            //    const string clavepacient = "Mcga@123456";
            //    var resultadoesp2 = userManager.Create(paciente2, clavepacient);
            //    resultadoesp2 = userManager.SetLockoutEnabled(paciente2.Id, false);
            //}
            //groupManager.SetUserGroups(paciente2.Id, new string[] { newGrouppac.Id });
            //groupManager.SetGroupRoles(newGrouppac.Id, new string[] { rolepac.Name });

            var espe = userManager.FindByName("hernan_carballo@gmail.com");
            if (espe == null)
            {
                espe = new Especialista
                {
                    Prefijo = Prefijo.Dr,
                    UserName = "hernan_carballo@gmail.com",
                    Nombre = "Hernán",
                    Apellido = "Carballo",
                    Email = "hernan_carballo@gmail.com",
                    Estado = true,
                    PhoneNumber = "3413354582",
                    ImagenMedico = "~/Content/img/medicos/hernan_carballo.jpg",
                    Especialidad = especialidades[3],
                    NumeroMatricula = "MEDICO156",
                    EmailConfirmed = true,
                };

                const string claveespecialista = "Mcga@12345678";
                var resultadootro = userManager.Create(espe, claveespecialista);
                resultadootro = userManager.SetLockoutEnabled(espe.Id, false);
            }

            groupManager.SetUserGroups(espe.Id, new string[] { newGroupesp.Id });
            groupManager.SetGroupRoles(newGroupesp.Id, new string[] { roleesp.Name });

            foreach (var horariosroles in PermisosHorarios.ToList())
            {
                userManager.AddToRoles(especialista.Id, horariosroles.Name);
            }
            foreach (var turnosroles in PermisosTurnos.ToList())
            {
                userManager.AddToRoles(espe.Id, turnosroles.Name);
            }

            var reporteconsulta = new List<ReporteConsultaAnual>
            {
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 0,
                    CantidadConsultas30M = 0,
                    MesReporte = Convert.ToDateTime("2016-01-01 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 0,
                    CantidadConsultas30M = 0,
                    MesReporte = Convert.ToDateTime("2016-01-02 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 0,
                    CantidadConsultas30M = 0,
                    MesReporte = Convert.ToDateTime("2016-01-03 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 0,
                    CantidadConsultas30M = 0,
                    MesReporte = Convert.ToDateTime("2016-01-04 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 0,
                    CantidadConsultas30M = 0,
                    MesReporte = Convert.ToDateTime("2016-01-05 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 0,
                    CantidadConsultas30M = 0,
                    MesReporte = Convert.ToDateTime("2016-01-06 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 0,
                    CantidadConsultas30M = 0,
                    MesReporte = Convert.ToDateTime("2016-01-07 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 0,
                    CantidadConsultas30M = 0,
                    MesReporte = Convert.ToDateTime("2016-01-08 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 0,
                    CantidadConsultas30M = 0,
                    MesReporte = Convert.ToDateTime("2016-01-09 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 56,
                    CantidadConsultas30M = 14,
                    MesReporte = Convert.ToDateTime("2016-01-10 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 128,
                    CantidadConsultas30M = 32,
                    MesReporte = Convert.ToDateTime("2016-01-11 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 300,
                    CantidadConsultas30M = 75,
                    MesReporte = Convert.ToDateTime("2016-01-12 00:00")
                },
                //Segundo Año
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 576,
                    CantidadConsultas30M = 144,
                    MesReporte = Convert.ToDateTime("2017-01-01 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 1080,
                    CantidadConsultas30M = 270,
                    MesReporte = Convert.ToDateTime("2017-01-02 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 1600,
                    CantidadConsultas30M = 400,
                    MesReporte = Convert.ToDateTime("2017-01-03 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 1920,
                    CantidadConsultas30M = 480,
                    MesReporte = Convert.ToDateTime("2017-01-04 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 2464,
                    CantidadConsultas30M = 748,
                    MesReporte = Convert.ToDateTime("2017-01-05 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 2992,
                    CantidadConsultas30M = 748,
                    MesReporte = Convert.ToDateTime("2017-01-06 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 3520,
                    CantidadConsultas30M = 880,
                    MesReporte = Convert.ToDateTime("2017-01-07 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 4324,
                    CantidadConsultas30M = 1081,
                    MesReporte = Convert.ToDateTime("2017-01-08 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 4600,
                    CantidadConsultas30M = 1150,
                    MesReporte = Convert.ToDateTime("2017-01-09 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 4784,
                    CantidadConsultas30M = 1196,
                    MesReporte = Convert.ToDateTime("2017-01-10 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 5184,
                    CantidadConsultas30M = 1296,
                    MesReporte = Convert.ToDateTime("2017-01-11 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 5400,
                    CantidadConsultas30M = 1350,
                    MesReporte = Convert.ToDateTime("2017-01-12 00:00")
                },
                //Tercer Año
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 5600,
                    CantidadConsultas30M = 1400,
                    MesReporte = Convert.ToDateTime("2018-01-01 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 5700,
                    CantidadConsultas30M = 1425,
                    MesReporte = Convert.ToDateTime("2018-01-02 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 5700,
                    CantidadConsultas30M = 1425,
                    MesReporte = Convert.ToDateTime("2018-01-03 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 5800,
                    CantidadConsultas30M = 1450,
                    MesReporte = Convert.ToDateTime("2018-01-04 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 6000,
                    CantidadConsultas30M = 1500,
                    MesReporte = Convert.ToDateTime("2018-01-05 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 6000,
                    CantidadConsultas30M = 1500,
                    MesReporte = Convert.ToDateTime("2018-01-06 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 6000,
                    CantidadConsultas30M = 1500,
                    MesReporte = Convert.ToDateTime("2018-01-07 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 6000,
                    CantidadConsultas30M = 1500,
                    MesReporte = Convert.ToDateTime("2018-01-08 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 6100,
                    CantidadConsultas30M = 1525,
                    MesReporte = Convert.ToDateTime("2018-01-09 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 6448,
                    CantidadConsultas30M = 1612,
                    MesReporte = Convert.ToDateTime("2018-01-10 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 6696,
                    CantidadConsultas30M = 1674,
                    MesReporte = Convert.ToDateTime("2018-01-11 00:00")
                },
                new ReporteConsultaAnual()
                {
                    CantidadConsultas20M = 6944,
                    CantidadConsultas30M = 1736,
                    MesReporte = Convert.ToDateTime("2018-01-12 00:00")
                }
            };
            reporteconsulta.ForEach(c => db.ReporteConsulta.Add(c));
            db.SaveChanges();
        }
    }
}