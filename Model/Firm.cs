﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace ADO_LINQ.Model
{
	//Data Context
	class Firm
	{
		private IConfiguration _configuration;
		public List<Department> Departments { get; set; }

		public List<Manager> Managers { get; set; }

		public Firm(IConfiguration configuration)
		{
			_configuration = configuration;
			SqlConnection connection = new SqlConnection(
				_configuration.GetConnectionString("manDb"));
			connection.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = connection;
			cmd.CommandText =
				"SELECT COUNT(*) FROM sys.tables WHERE name='Departments'";
			int n = Convert.ToInt32(cmd.ExecuteScalar());
			if (n == 0)
			{
				// CREATE TABLE
				cmd.CommandText = @"CREATE TABLE Departments (
	                                    Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                                    Name		NVARCHAR(50) NOT NULL
                                    )";
				cmd.ExecuteNonQuery();

				// FILL TABLE
				cmd.CommandText = @"INSERT INTO Departments 
	                                ( Id, Name )
                                VALUES 
	                                ( 'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',  N'IT одтел'		 	 ), 
	                                ( '131EF84B-F06E-494B-848F-BB4BC0604266',  N'Бухгалтерия'		 ), 
	                                ( '8DCC3969-1D93-47A9-8B79-A30C738DB9B4',  N'Служба безопасности'), 
	                                ( 'D2469412-0E4B-46F7-80EC-8C522364D099',  N'Отдел кадров'		 ),
	                                ( '1EF7268C-43A8-488C-B761-90982B31DF4E',  N'Канцелярия'		 ), 
	                                ( '415B36D9-2D82-4A92-A313-48312F8E18C6',  N'Одтел продаж'		 ), 
	                                ( '624B3BB5-0F2C-42B6-A416-099AAB799546',  N'Юридическая служба' )";
				cmd.ExecuteNonQuery();
			}

			cmd.CommandText =
					"SELECT COUNT(*) FROM sys.tables WHERE name = 'Managers'";
			n = Convert.ToInt32(cmd.ExecuteScalar());
			if (n == 0)
			{
				cmd.CommandText = @"CREATE TABLE Managers (
	                                Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                                Surname		NVARCHAR(50) NOT NULL,
	                                Name		NVARCHAR(50) NOT NULL,
	                                Secname		NVARCHAR(50) NOT NULL,
	                                Id_main_dep UNIQUEIDENTIFIER NOT NULL REFERENCES Departments( Id ),
	                                Id_sec_dep	UNIQUEIDENTIFIER REFERENCES Departments( Id ),
	                                Id_chief	UNIQUEIDENTIFIER
                                ) ";
				cmd.ExecuteNonQuery();
				cmd.CommandText = @"
						INSERT INTO Managers 
							( Id, Surname, Name, Secname, Id_main_dep, Id_sec_dep, Id_chief )
						VALUES 
							( '743C93F2-4717-4E81-A093-69903476E176',  N'Носков',	N'Орест',		N'Ярославович',		'131EF84B-F06E-494B-848F-BB4BC0604266',		null,										null	),
							( '63531753-4D76-4A93-AD15-C727FFECA6AB',  N'Никитин',	N'Станислав',	N'Брониславович',	'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'3618D1D1-32DE-40B5-B823-9F82924A3CAF'		),
							( 'CDE086E1-D25C-4251-A234-10727818EE28',  N'Воронов',	N'Александр',	N'Леонидович',		'D2469412-0E4B-46F7-80EC-8C522364D099',		'131EF84B-F06E-494B-848F-BB4BC0604266',		null	),
							( '0B2BE83A-7FB4-403B-8CE8-37BE257B038C',  N'Евдокимов',N'Клим',		N'Викторович',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										null	),
							( '7585D790-6E5A-4F73-A85C-4F9BD883D811',  N'Жуков',	N'Влад',		N'Виталиевич',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										null	),
							( '45489FE7-86C8-4FA1-9D79-A82197566BF3',  N'Кулагин',	N'Максим',		N'Вадимович',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		'131EF84B-F06E-494B-848F-BB4BC0604266',		null	),
							( '0017AAAE-3E22-462D-9031-4276A9788D51',  N'Журавлёв',	N'Зигмунд',		N'Владимирович',	'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'FEA65EE4-A8A0-425B-8F11-3896C1E2197E'		),
							( '521C07BE-6FBD-411F-BCCB-93E2672BD50E',  N'Соболев',	N'Нестор',		N'Юхимович',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null,										null	),
							( '381C2888-1CB0-41FA-9650-48B953F31EF6',  N'Беляков',	N'Олег',		N'Грегориевич',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null,										'663C3142-1C9D-4957-800D-F6C6824B9C88'		),
							( 'E1AC29AD-122E-474D-926A-F93AC636F605', N'Моисеев',	N'Конрад',		N'Леонидович',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		'3E229EB8-E99A-455F-8AF3-5871337A092C'		),
							( '39D57DFB-8DA7-49C9-AE8D-464509618F02', N'Гуляев',	N'Семён',		N'Юхимович',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		null,										null	),
							( '542CB2C1-A8E3-42DB-97FA-B3C79B12A1A9', N'Назаров',	N'Сергей',		N'Платонович',		'131EF84B-F06E-494B-848F-BB4BC0604266',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null	),
							( 'FE7E578E-5FC8-4D80-AD6B-500DDF2506C4', N'Рожков',	N'Радислав',	N'Дмитриевич',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'7A88B1B9-0216-4259-8BA6-C123ABB3C6A8'		),
							( '7B8219FC-9FD2-431E-985C-7CAA6E9BD013', N'Герасимов',	N'Лука',		N'Грегориевич',		'D2469412-0E4B-46F7-80EC-8C522364D099',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		'3E229EB8-E99A-455F-8AF3-5871337A092C'		),
							( '23D52416-D994-4564-A106-1FDF5FECEF25', N'Куликов',	N'Заур',		N'Иванович',		'D2469412-0E4B-46F7-80EC-8C522364D099',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'23DBE38C-0ED4-4E90-8BC7-F168134E8674'		),
							( 'EE860EE3-6CCA-4EA3-A2F1-FB79F4FC823A', N'Корнилов',	N'Ярослав',		N'Романович',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										'676D8ED4-8307-4196-9776-107C40C1DF84'		),
							( 'DD860E7E-C2F0-47A6-BA29-165BE015E5A2', N'Князев',	N'Клим',		N'Эдуардович',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null	),
							( '267F7528-2D4B-4063-A2C8-98E8F19FB6EE', N'Кириллов',	N'Герасим',		N'Анатолиевич',		'131EF84B-F06E-494B-848F-BB4BC0604266',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'207CDCF2-89AD-49A5-A669-A082FA9CCCBA'		),
							( 'FEA65EE4-A8A0-425B-8F11-3896C1E2197E', N'Галкин',	N'Пётр',		N'Максимович',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null	),
							( 'D13F3CCA-B9F8-4BC1-96F4-C80583928E55', N'Бородай',	N'Люций',		N'Львович',			'1EF7268C-43A8-488C-B761-90982B31DF4E',		null,										'DC268B00-1727-4381-9878-6DA1BFEF2701'		),
							( '5FE63A0F-C1AE-44BE-9397-0F7DB0B95C1F', N'Спивак',	N'Оливер',		N'Иванович',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'29219DB8-16A0-4046-A7E1-6E455B0559CD'		),
							( 'DC268B00-1727-4381-9878-6DA1BFEF2701', N'Ершов',		N'Владлен',		N'Богданович',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'868F6394-3CA3-4700-90BB-6B73EC6719A7'		),
							( '2FA70965-5BCE-44F0-B6DD-2AF6072EB8B0', N'Комаров',	N'Адриан',		N'Петрович',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null,										null	),
							( '1166ECDD-63C8-42FC-A68A-C292176A7B04', N'Веселов',	N'Роберт',		N'Евгеньевич',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'C5F771FB-A645-4BA1-8155-F3F5002B2B89'		),
							( '0989E3A2-3D6D-4BC3-A538-C4055F9A09DD', N'Данилов',	N'Добрыня',		N'Львович',			'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null,										'23DBE38C-0ED4-4E90-8BC7-F168134E8674'		),
							( '6CBEA09E-E3E4-4DD3-A6C5-ED9CCD986BC0', N'Журавлёв',	N'Аким',		N'Петрович',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										null	),
							( '676D8ED4-8307-4196-9776-107C40C1DF84', N'Ерёменко',	N'Кристиан',	N'Евгеньевич',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'7B8219FC-9FD2-431E-985C-7CAA6E9BD013'		),
							( 'FF559AE5-64B6-459E-9771-CB36130B3B75', N'Туров',		N'Станислав',	N'Михайлович',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null,										'435EEE28-E5EA-4EC9-9F01-DE884DFD6292'		),
							( '1A930DE7-647B-4A32-AD3B-0CAF4528B356', N'Шумейко',	N'Абрам',		N'Романович',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null	),
							( '3618D1D1-32DE-40B5-B823-9F82924A3CAF', N'Бобылёв',	N'Всеволод',	N'Ярославович',		'131EF84B-F06E-494B-848F-BB4BC0604266',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null	),
							( '66034616-24E5-4E90-815F-476EB0CBB6B1', N'Гурьева',	N'Антонина',	N'Евгеньевна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null,										'FEA65EE4-A8A0-425B-8F11-3896C1E2197E'		),
							( 'C5F771FB-A645-4BA1-8155-F3F5002B2B89', N'Павлик',	N'Ника',		N'Эдуардовна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		'8939ED0C-BBDB-435E-923E-68158D2153C6'		),
							( '15F36ECC-EF25-495F-ADFF-169DB3339B88', N'Копылова',	N'Екатерина',	N'Дмитриевна',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		null,										'05E31241-7274-43B5-8B59-9A62D725E54F'		),
							( '101BE2B1-C0AF-493E-BBF2-C8D8E4EB826C', N'Корнейчук',	N'Нина',		N'Платоновна',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'2B3170C4-3063-43E6-985D-A38D9E45AF09'		),
							( '868F6394-3CA3-4700-90BB-6B73EC6719A7', N'Гордеева',	N'Капитолина',	N'Станиславовна',	'1EF7268C-43A8-488C-B761-90982B31DF4E',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null	),
							( '05E31241-7274-43B5-8B59-9A62D725E54F', N'Майборода',	N'Алёна',		N'Александровна',	'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'E1AC29AD-122E-474D-926A-F93AC636F605'		),
							( '1ADC048C-E346-47C3-8C35-7AD4FDAA6EB7', N'Шубина',	N'Екатерина',	N'Викторовна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null	),
							( '435EEE28-E5EA-4EC9-9F01-DE884DFD6292', N'Лазарева',	N'Вера',		N'Евгеньевна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null	),
							( '0889C51E-7728-4ABD-9987-3588D48B54A9', N'Кобзар',	N'Полина',		N'Львовна',			'131EF84B-F06E-494B-848F-BB4BC0604266',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		'542CB2C1-A8E3-42DB-97FA-B3C79B12A1A9'		),
							( '46D73A48-3906-44F4-A4B4-E29F1CC40B4F', N'Милославска',N'Инна',		N'Эдуардовна',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null,										'435EEE28-E5EA-4EC9-9F01-DE884DFD6292'		),
							( 'EFEF5433-7E26-43A3-A737-3BB032D7D88A', N'Степанова',	N'Нина',		N'Михайловна',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		null,										'63531753-4D76-4A93-AD15-C727FFECA6AB'		),
							( '55FF549E-1489-4B4A-9482-B843CD70C546', N'Ялова',		N'Любовь',		N'Ивановна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null	),
							( '79679ED4-0CCD-480A-8D5B-4A68287DE6C4', N'Макарова',	N'Полина',		N'Васильевна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null,										'0B2BE83A-7FB4-403B-8CE8-37BE257B038C'		),
							( '29219DB8-16A0-4046-A7E1-6E455B0559CD', N'Дементьева',N'Альбина',		N'Ивановна',		'131EF84B-F06E-494B-848F-BB4BC0604266',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null	),
							( '13DED219-A580-4FF8-8211-90A408B0AFA6', N'Егорова',	N'Ярослава',	N'Романовна',		'131EF84B-F06E-494B-848F-BB4BC0604266',		null,										'1166ECDD-63C8-42FC-A68A-C292176A7B04'		),
							( '2B3170C4-3063-43E6-985D-A38D9E45AF09', N'Коваленко',	N'Ольга',		N'Владимировна',	'131EF84B-F06E-494B-848F-BB4BC0604266',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null	),
							( '3E229EB8-E99A-455F-8AF3-5871337A092C', N'Белоусова',	N'Валерия',		N'Петровна',		'131EF84B-F06E-494B-848F-BB4BC0604266',		null,										null	),
							( '5319FD22-9BDE-48E5-819D-FE884B70AFD8', N'Бердник',	N'Ирина',		N'Ивановна',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'39D57DFB-8DA7-49C9-AE8D-464509618F02'		),
							( '8939ED0C-BBDB-435E-923E-68158D2153C6', N'Красинец',	N'Нелли',		N'Ярославовна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										'743C93F2-4717-4E81-A093-69903476E176'		),
							( '663C3142-1C9D-4957-800D-F6C6824B9C88', N'Баранова',	N'Флорентина',	N'Брониславовна',	'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null,										'0017AAAE-3E22-462D-9031-4276A9788D51'		),
							( '239450EB-A92F-4093-A74F-EAA38F8ADBE2', N'Толочко',	N'Анжелика',	N'Борисовна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										'23D52416-D994-4564-A106-1FDF5FECEF25'		),
							( '23DBE38C-0ED4-4E90-8BC7-F168134E8674', N'Родионова',	N'Эльвира',		N'Фёдоровна',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'3E229EB8-E99A-455F-8AF3-5871337A092C'		),
							( '7A88B1B9-0216-4259-8BA6-C123ABB3C6A8', N'Трясило',	N'Инга',		N'Артёмовна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null	),
							( '789A53AB-A54D-4AF7-94A5-DD288428A37C', N'Гуляева',	N'Клара',		N'Даниловна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null,										'DC268B00-1727-4381-9878-6DA1BFEF2701'		),
							( 'A93A1B20-155A-43BD-ACEE-87A6088C969E', N'Исаева',	N'Марта',		N'Борисовна',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null,										null	),
							( 'E56F5DE6-A1D3-4C3E-A09A-A9B9FA96C9B3', N'Одинцова',	N'Зинаида',		N'Евгеньевна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'D2469412-0E4B-46F7-80EC-8C522364D099',		'DD860E7E-C2F0-47A6-BA29-165BE015E5A2'		),
							( '207CDCF2-89AD-49A5-A669-A082FA9CCCBA', N'Соловьёва',	N'Флорентина',	N'Виталиевна',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null,										null	),
							( 'C5EE780A-4D53-40FB-A592-C35CFC9455F2', N'Мирна',		N'Рада',		N'Сергеевна',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		null,										null	),
							( 'D3FCC76B-09A2-4578-A72C-34468DA36C45', N'Одинцова',	N'Мальвина',	N'Дмитриевна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'1A930DE7-647B-4A32-AD3B-0CAF4528B356'		),
							( '6FB5BCA3-2CAE-4450-AAB5-E0184FD45BE9', N'Ткаченко',	N'Альбина',		N'Викторовна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										null	)";
				cmd.ExecuteNonQuery();
			}
			cmd.CommandText = "SELECT * FROM Departments";
			using (var reader = cmd.ExecuteReader())
			{
				Departments = new List<Department>();
				while (reader.Read())
				{
					Departments.Add(new Model.Department
					{
						Id = reader.GetGuid(0),
						Name = reader.GetString(1)
					});


				}
			}
			cmd.CommandText = "SELECT Id, Surname, Name, Secname, Id_main_dep, Id_sec_dep, Id_chief FROM Managers";
			using (var reader = cmd.ExecuteReader())
			{
				Managers = new List<Manager>();
				while (reader.Read())
				{
					Managers.Add(new Model.Manager
					{
						Id = reader.GetGuid(0),
						Surname = reader.GetString(1),
						Name = reader.GetString(2),
						Secname = reader.GetString(3),
						Id_main_dep = reader.GetGuid(4),

						Id_sec_dep = reader.GetValue(5) == DBNull.Value
									 ? null
									 : reader.GetGuid(5),

						Id_chief = reader.GetValue(6) == DBNull.Value
									 ? null
									 : reader.GetGuid(6)
					});
				}
			}
		}
	}
}
