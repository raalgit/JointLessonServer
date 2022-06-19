using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL_Migrator.Migrations
{
    [Migration(202206191001)]
    public class UpdateDataBase : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Create.Table("FileData").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("MongoName").AsString(50).NotNullable()
                .WithColumn("OriginalName").AsString(50).NotNullable()
                .WithColumn("MongoId").AsString(100).NotNullable();

            Delete.ForeignKey("FK_UserGroupId").OnTable("User").InSchema("JL");
            Delete.Column("GroupId").FromTable("User").InSchema("JL");

            Delete.Column("FileId").FromTable("Manual").InSchema("JL");
            Create.Column("FileDataId").OnTable("Manual").InSchema("JL")
                .AsInt32().ForeignKey("FK_ManualFileDataId", "JL", "FileData", "Id").Nullable();

            Delete.Column("AvatarId").FromTable("User").InSchema("JL");
            Create.Column("AvatarId").OnTable("User").InSchema("JL")
                .AsInt32().ForeignKey("FK_UserAvatarIdId", "JL", "FileData", "Id").Nullable();

            Delete.Column("AvatarId").FromTable("Course").InSchema("JL");
            Create.Column("AvatarId").OnTable("Course").InSchema("JL")
                .AsInt32().ForeignKey("FK_CourseAvatarIdId", "JL", "FileData", "Id").Nullable();

            Delete.Column("LastMaterialPage").FromTable("GroupAtCourse").InSchema("JL");

            Delete.Column("LastMaterialPage").FromTable("Lesson").InSchema("JL");
            Create.Column("LastMaterialPage").OnTable("Lesson").InSchema("JL").AsString(40).Nullable();
            Create.Column("CourseId").OnTable("Lesson").InSchema("JL").AsInt32()
               .ForeignKey("FK_LessonCourseId", "JL", "Course", "Id")
               .NotNullable()
               .SetExistingRowsTo("3");

            Create.Table("SignalUserConnection").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_SignalUserConnectionUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("ConnectionId").AsString(50).NotNullable();
               

            Create.Table("UserRemoteAccess").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("CourseId").AsInt32().ForeignKey("FK_UserRemoteAccessCourseId", "JL", "Course", "Id").NotNullable()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_UserRemoteAccessUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("ConnectionData").AsString(2000).NotNullable()
                .WithColumn("StartDate").AsDateTime().NotNullable();

            Alter.Table("CourseTeacher").InSchema("JL")
                .AddColumn("OnLesson").AsBoolean().NotNullable().SetExistingRowsTo(false);

            Alter.Table("LessonTabel").InSchema("JL")
                .AddColumn("HandUp").AsBoolean().NotNullable().SetExistingRowsTo(false);

            Create.Column("FileDataId").OnTable("WorkBook").InSchema("JL")
               .AsInt32().ForeignKey("FK_WorkBookFileDataId", "JL", "FileData", "Id").Nullable();

            Delete.Column("Text").FromTable("WorkBook").InSchema("JL");
            Delete.Column("Page").FromTable("WorkBook").InSchema("JL");

            Create.Table("UserGroup").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_UserGroupUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("GroupId").AsInt32().ForeignKey("FK_UserGroupGroupId", "JL", "Group", "Id").Nullable();
        }
    }
}
