﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CONTEXT.Context
{
    public partial class HiringContext : DbContext,IHiringContext
    {
        public HiringContext()
        {
        }

        public HiringContext(DbContextOptions<HiringContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AcademicInformation> AcademicInformation { get; set; }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<AssigmentContract> AssigmentContract { get; set; }
        public virtual DbSet<AssignmentType> AssignmentType { get; set; }
        public virtual DbSet<Banks> Banks { get; set; }
        public virtual DbSet<ChangeContractContractor> ChangeContractContractor { get; set; }
        public virtual DbSet<Component> Component { get; set; }
        public virtual DbSet<ContractFolder> ContractFolder { get; set; }
        public virtual DbSet<Contractor> Contractor { get; set; }
        public virtual DbSet<ContractorPayments> ContractorPayments { get; set; }
        public virtual DbSet<CpcType> CpcType { get; set; }
        public virtual DbSet<DetailContract> DetailContract { get; set; }
        public virtual DbSet<DetailContractor> DetailContractor { get; set; }
        public virtual DbSet<DetailFile> DetailFile { get; set; }
        public virtual DbSet<DetailType> DetailType { get; set; }
        public virtual DbSet<DocumentType> DocumentType { get; set; }
        public virtual DbSet<EconomicdataContractor> EconomicdataContractor { get; set; }
        public virtual DbSet<ElementComponent> ElementComponent { get; set; }
        public virtual DbSet<ElementType> ElementType { get; set; }
        public virtual DbSet<EmptityHealth> EmptityHealth { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<Folder> Folder { get; set; }
        public virtual DbSet<FolderType> FolderType { get; set; }
        public virtual DbSet<HiringData> HiringData { get; set; }
        public virtual DbSet<MinuteType> MinuteType { get; set; }
        public virtual DbSet<NewnessContractor> NewnessContractor { get; set; }
        public virtual DbSet<NewnessType> NewnessType { get; set; }
        public virtual DbSet<Roll> Roll { get; set; }
        public virtual DbSet<RubroType> RubroType { get; set; }
        public virtual DbSet<SharedData> SharedData { get; set; }
        public virtual DbSet<StatusContract> StatusContract { get; set; }
        public virtual DbSet<StatusContractor> StatusContractor { get; set; }
        public virtual DbSet<StatusFile> StatusFile { get; set; }
        public virtual DbSet<TermContract> TermContract { get; set; }
        public virtual DbSet<TermType> TermType { get; set; }
        public virtual DbSet<UserFile> UserFile { get; set; }
        public virtual DbSet<UserFileType> UserFileType { get; set; }
        public virtual DbSet<UserT> UserT { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicInformation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AcademicInformationType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CollegeDegree).HasMaxLength(100);

                entity.Property(e => e.Institution)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ContractorNavigation)
                    .WithMany(p => p.AcademicInformation)
                    .HasForeignKey(d => d.Contractor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AcademicI__Contr__6B24EA82");
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.NombreActividad)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Activity__Compon__7C4F7684");
            });

            modelBuilder.Entity<AssigmentContract>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ContractId).HasColumnName("contractId");

                entity.HasOne(d => d.AssignmentTypeNavigation)
                    .WithMany(p => p.AssigmentContract)
                    .HasForeignKey(d => d.AssignmentType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Assigment__Assig__151B244E");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.AssigmentContract)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Assigment__contr__1332DBDC");

                entity.HasOne(d => d.Roll)
                    .WithMany(p => p.AssigmentContract)
                    .HasForeignKey(d => d.RollId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Assigment__RollI__14270015");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AssigmentContract)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Assigment__UserI__123EB7A3");
            });

            modelBuilder.Entity<AssignmentType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AssigmentTypeDescription).HasMaxLength(100);

                entity.Property(e => e.Code).HasMaxLength(200);
            });

            modelBuilder.Entity<Banks>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BankName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ChangeContractContractor>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Consecutivo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.FechaFinAdicion).HasColumnType("date");

                entity.Property(e => e.FechaInicioAdicion).HasColumnType("date");

                entity.Property(e => e.NoAdicion).HasMaxLength(20);

                entity.Property(e => e.NombreElemento).HasMaxLength(100);

                entity.Property(e => e.ObjetoElemento).IsUnicode(false);

                entity.Property(e => e.PerfilRequerido).IsUnicode(false);

                entity.Property(e => e.Recursos).HasColumnType("money");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("date")
                    .HasColumnName("registerDate");

                entity.Property(e => e.ValorPorDia).HasColumnType("money");

                entity.HasOne(d => d.DetailContractor)
                    .WithMany(p => p.ChangeContractContractor)
                    .HasForeignKey(d => d.DetailContractorId)
                    .HasConstraintName("FK__ChangeCon__Detai__17F790F9");

                entity.HasOne(d => d.EconomicdataContractor)
                    .WithMany(p => p.ChangeContractContractor)
                    .HasForeignKey(d => d.EconomicdataContractorId)
                    .HasConstraintName("FK__ChangeCon__Econo__18EBB532");

                entity.HasOne(d => d.MinuteTypeNavigation)
                    .WithMany(p => p.ChangeContractContractor)
                    .HasForeignKey(d => d.MinuteType)
                    .HasConstraintName("FK__ChangeCon__Minut__17036CC0");
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.NombreComponente)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Component)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Component__Contr__7B5B524B");
            });

            modelBuilder.Entity<ContractFolder>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.FuenteRubro).HasMaxLength(50);

                entity.Property(e => e.GastosOperativos).HasColumnType("money");

                entity.Property(e => e.NumberProject)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Project).HasMaxLength(30);

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.ValorContrato).HasColumnType("money");

                entity.Property(e => e.ValorSubTotal).HasColumnType("money");

                entity.HasOne(d => d.RubroNavigation)
                    .WithMany(p => p.ContractFolder)
                    .HasForeignKey(d => d.Rubro)
                    .HasConstraintName("FK__ContractF__Rubro__6EF57B66");

                entity.HasOne(d => d.StatusContract)
                    .WithMany(p => p.ContractFolder)
                    .HasForeignKey(d => d.StatusContractId)
                    .HasConstraintName("FK__ContractF__Statu__05D8E0BE");
            });

            modelBuilder.Entity<Contractor>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Barrio)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClaveUsuario).HasMaxLength(15);

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CuentaBancaria).HasMaxLength(100);

                entity.Property(e => e.Departamento)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActualizacion).HasColumnType("date");

                entity.Property(e => e.FechaCreacion).HasColumnType("date");

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.Genero)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Identificacion)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LugarExpedicion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nacionalidad)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCuenta).HasMaxLength(30);

                entity.HasOne(d => d.EntidadCuentaBancariaNavigation)
                    .WithMany(p => p.Contractor)
                    .HasForeignKey(d => d.EntidadCuentaBancaria)
                    .HasConstraintName("FK__Contracto__Entid__6D0D32F4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Contractor)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Contracto__UserI__6E01572D");
            });

            modelBuilder.Entity<ContractorPayments>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FromDate).HasColumnType("date");

                entity.Property(e => e.Paymentcant).HasColumnType("money");

                entity.Property(e => e.RegisterDate).HasColumnType("date");

                entity.Property(e => e.ToDate).HasColumnType("date");

                entity.HasOne(d => d.DetailContractorNavigation)
                    .WithMany(p => p.ContractorPayments)
                    .HasForeignKey(d => d.DetailContractor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Contracto__Detai__00200768");
            });

            modelBuilder.Entity<CpcType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CpcName).HasMaxLength(200);

                entity.Property(e => e.CpcNumber).HasMaxLength(100);
            });

            modelBuilder.Entity<DetailContract>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ContractId).HasColumnName("contractId");

                entity.Property(e => e.FechaContrato).HasColumnType("date");

                entity.Property(e => e.FechaFinalizacion).HasColumnType("date");

                entity.Property(e => e.ModifyDate).HasColumnType("date");

                entity.Property(e => e.RegisterDate).HasColumnType("date");

                entity.Property(e => e.TipoContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.DetailContract)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK__DetailCon__contr__01142BA1");

                entity.HasOne(d => d.DetailTypeNavigation)
                    .WithMany(p => p.DetailContract)
                    .HasForeignKey(d => d.DetailType)
                    .HasConstraintName("FK__DetailCon__Detai__160F4887");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DetailContract)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DetailCon__UserI__02084FDA");
            });

            modelBuilder.Entity<DetailContractor>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ContractId).HasColumnName("contractId");

                entity.Property(e => e.ContractorId).HasColumnName("contractorId");

                entity.Property(e => e.MinuteGenerate).HasColumnName("minuteGenerate");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.DetailContractor)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("FK__DetailCon__Activ__787EE5A0");

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.DetailContractor)
                    .HasForeignKey(d => d.ComponentId)
                    .HasConstraintName("FK__DetailCon__Compo__778AC167");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.DetailContractor)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK__DetailCon__contr__73BA3083");

                entity.HasOne(d => d.Contractor)
                    .WithMany(p => p.DetailContractor)
                    .HasForeignKey(d => d.ContractorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DetailCon__contr__74AE54BC");

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.DetailContractor)
                    .HasForeignKey(d => d.ElementId)
                    .HasConstraintName("FK__DetailCon__Eleme__76969D2E");

                entity.HasOne(d => d.HiringData)
                    .WithMany(p => p.DetailContractor)
                    .HasForeignKey(d => d.HiringDataId)
                    .HasConstraintName("FK__DetailCon__Hirin__75A278F5");

                entity.HasOne(d => d.StatusContractorNavigation)
                    .WithMany(p => p.DetailContractor)
                    .HasForeignKey(d => d.StatusContractor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DetailCon__Statu__797309D9");
            });

            modelBuilder.Entity<DetailFile>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Observation).IsUnicode(false);

                entity.Property(e => e.ReasonRejection)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterDate).HasColumnType("date");

                entity.Property(e => e.StatusFileId).HasColumnName("statusFileId");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.DetailFile)
                    .HasForeignKey(d => d.FileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DetailFil__FileI__0D7A0286");

                entity.HasOne(d => d.StatusFile)
                    .WithMany(p => p.DetailFile)
                    .HasForeignKey(d => d.StatusFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DetailFil__statu__0C85DE4D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DetailFile)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__DetailFil__UserI__0E6E26BF");
            });

            modelBuilder.Entity<DetailType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.DetailTypeDescripcion).HasMaxLength(100);
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.DocumentTypeDescription).HasMaxLength(100);

                entity.Property(e => e.TypeCode).HasMaxLength(5);
            });

            modelBuilder.Entity<EconomicdataContractor>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Debt).HasColumnType("money");

                entity.Property(e => e.Freed).HasColumnType("money");

                entity.Property(e => e.Missing).HasColumnType("money");

                entity.Property(e => e.ModifyDate).HasColumnType("date");

                entity.Property(e => e.RegisterDate).HasColumnType("date");

                entity.Property(e => e.TotalPaIdMonth).HasColumnType("money");

                entity.Property(e => e.TotalValue).HasColumnType("money");

                entity.Property(e => e.UnitValue).HasColumnType("money");

                entity.HasOne(d => d.DetailContractor)
                    .WithMany(p => p.EconomicdataContractor)
                    .HasForeignKey(d => d.DetailContractorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Economicd__Detai__7A672E12");
            });

            modelBuilder.Entity<ElementComponent>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Consecutivo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NombreElemento).HasMaxLength(100);

                entity.Property(e => e.ObjetoElemento).IsUnicode(false);

                entity.Property(e => e.PerfilRequerido).IsUnicode(false);

                entity.Property(e => e.Recursos).HasColumnType("money");

                entity.Property(e => e.TipoElemento).HasColumnName("tipoElemento");

                entity.Property(e => e.ValorPorDia).HasColumnType("money");

                entity.Property(e => e.ValorPorDiaContratista).HasColumnType("money");

                entity.Property(e => e.ValorTotal).HasColumnType("money");

                entity.Property(e => e.ValorTotalContratista).HasColumnType("money");

                entity.Property(e => e.ValorUnidad).HasColumnType("money");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ElementComponent)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("FK__ElementCo__Activ__7E37BEF6");

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.ElementComponent)
                    .HasForeignKey(d => d.ComponentId)
                    .HasConstraintName("FK__ElementCo__Compo__7D439ABD");

                entity.HasOne(d => d.Cpc)
                    .WithMany(p => p.ElementComponent)
                    .HasForeignKey(d => d.CpcId)
                    .HasConstraintName("FK__ElementCo__CpcId__07C12930");

                entity.HasOne(d => d.TipoElementoNavigation)
                    .WithMany(p => p.ElementComponent)
                    .HasForeignKey(d => d.TipoElemento)
                    .HasConstraintName("FK__ElementCo__tipoE__06CD04F7");
            });

            modelBuilder.Entity<ElementType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.ElementTypeDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<EmptityHealth>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Emptity)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EmptityType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TotalPayment)
                    .HasColumnType("money")
                    .HasColumnName("totalPayment");

                entity.HasOne(d => d.ContractorNavigation)
                    .WithMany(p => p.EmptityHealth)
                    .HasForeignKey(d => d.Contractor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmptityHe__Contr__6C190EBB");
            });

            modelBuilder.Entity<Files>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DescriptionFile).HasMaxLength(100);

                entity.Property(e => e.FileType)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.FilesName).HasMaxLength(100);

                entity.Property(e => e.MonthPayment).HasMaxLength(10);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Files__ContractI__09A971A2");

                entity.HasOne(d => d.Contractor)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.ContractorId)
                    .HasConstraintName("FK__Files__Contracto__0A9D95DB");

                entity.HasOne(d => d.DocumentTypeNavigation)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.DocumentType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Files__DocumentT__08B54D69");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.FolderId)
                    .HasConstraintName("FK__Files__FolderId__0B91BA14");
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FolderName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ModifyDate).HasColumnType("date");

                entity.Property(e => e.RegisterDate).HasColumnType("date");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Folder)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Folder__Contract__72C60C4A");

                entity.HasOne(d => d.Contractor)
                    .WithMany(p => p.Folder)
                    .HasForeignKey(d => d.ContractorId)
                    .HasConstraintName("FK__Folder__Contract__71D1E811");

                entity.HasOne(d => d.FolderTypeNavigation)
                    .WithMany(p => p.Folder)
                    .HasForeignKey(d => d.FolderType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Folder__FolderTy__1BC821DD");
            });

            modelBuilder.Entity<FolderType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.FolderDescription).HasMaxLength(200);

                entity.Property(e => e.FolderTypeDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<HiringData>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Caso).HasMaxLength(30);

                entity.Property(e => e.Cdp).HasMaxLength(50);

                entity.Property(e => e.Compromiso).HasMaxLength(100);

                entity.Property(e => e.Contrato).HasMaxLength(50);

                entity.Property(e => e.FechaDeComite).HasColumnType("date");

                entity.Property(e => e.FechaExaPreocupacional).HasColumnType("date");

                entity.Property(e => e.FechaExpedicionPoliza).HasColumnType("date");

                entity.Property(e => e.FechaFinalizacionConvenio).HasColumnType("date");

                entity.Property(e => e.FechaRealDeInicio).HasColumnType("date");

                entity.Property(e => e.NoPoliza).HasMaxLength(100);

                entity.Property(e => e.NumeroActa)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ValorAsegurado)
                    .HasColumnType("money")
                    .HasColumnName("ValorASegurado");

                entity.Property(e => e.VigenciaFinal).HasColumnType("date");

                entity.Property(e => e.VigenciaInicial).HasColumnType("date");

                entity.HasOne(d => d.Contractor)
                    .WithMany(p => p.HiringData)
                    .HasForeignKey(d => d.ContractorId)
                    .HasConstraintName("FK__HiringDat__Contr__70DDC3D8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HiringData)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__HiringDat__UserI__6FE99F9F");
            });

            modelBuilder.Entity<MinuteType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.MinuteTypeDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<NewnessContractor>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RegisterDate).HasColumnType("date");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.NewnessContractor)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK__NewnessCo__Contr__03F0984C");

                entity.HasOne(d => d.Contractor)
                    .WithMany(p => p.NewnessContractor)
                    .HasForeignKey(d => d.ContractorId)
                    .HasConstraintName("FK__NewnessCo__Contr__02FC7413");

                entity.HasOne(d => d.NewnessTypeNavigation)
                    .WithMany(p => p.NewnessContractor)
                    .HasForeignKey(d => d.NewnessType)
                    .HasConstraintName("FK__NewnessCo__Newne__04E4BC85");
            });

            modelBuilder.Entity<NewnessType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.NewnessDescription).HasMaxLength(200);

                entity.Property(e => e.NewnessType1)
                    .HasMaxLength(100)
                    .HasColumnName("NewnessType");
            });

            modelBuilder.Entity<Roll>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.RollName)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<RubroType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Rubro).HasMaxLength(200);

                entity.Property(e => e.RubroNumber).HasMaxLength(30);

                entity.Property(e => e.RubroOrigin).HasMaxLength(50);
            });

            modelBuilder.Entity<SharedData>(entity =>
            {
                entity.ToTable("sharedData");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DataRegisteredType)
                    .HasMaxLength(100)
                    .HasColumnName("dataRegisteredType");

                entity.Property(e => e.DataShareType).HasMaxLength(30);
            });

            modelBuilder.Entity<StatusContract>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.StatusContractDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<StatusContractor>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.StatusContractorDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<StatusFile>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.StatusFileDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<TermContract>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.TermDate).HasColumnType("date");

                entity.HasOne(d => d.DetailContractorNavigation)
                    .WithMany(p => p.TermContract)
                    .HasForeignKey(d => d.DetailContractor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TermContr__Detai__1AD3FDA4");

                entity.HasOne(d => d.TermTypeNavigation)
                    .WithMany(p => p.TermContract)
                    .HasForeignKey(d => d.TermType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TermContr__TermT__19DFD96B");
            });

            modelBuilder.Entity<TermType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.TermDescription)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserFile>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FileData).IsRequired();

                entity.Property(e => e.FileNameC).HasMaxLength(30);

                entity.Property(e => e.FileType).HasMaxLength(5);

                entity.Property(e => e.OwnerFirm).HasMaxLength(50);

                entity.Property(e => e.UserfileName).HasMaxLength(50);

                entity.HasOne(d => d.Roll)
                    .WithMany(p => p.UserFile)
                    .HasForeignKey(d => d.RollId)
                    .HasConstraintName("FK__UserFile__RollId__0F624AF8");

                entity.HasOne(d => d.UserFileTypeNavigation)
                    .WithMany(p => p.UserFile)
                    .HasForeignKey(d => d.UserFileType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserFile__UserFi__10566F31");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFile)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserFile__UserId__114A936A");
            });

            modelBuilder.Entity<UserFileType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.FileTypeDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<UserT>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Identification)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordMail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.Property(e => e.Professionalposition).IsRequired();

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.Roll)
                    .WithMany(p => p.UserT)
                    .HasForeignKey(d => d.RollId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserT__RollId__7F2BE32F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}