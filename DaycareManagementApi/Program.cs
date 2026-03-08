using Daycare.Core;
using Daycare.Service;
using Daycare.Data;
using Daycare.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 1. הזרקת שכבת הנתונים (Data)
// אנחנו משתמשים ב-Singleton כדי שהרשימות שלנו (הילדים, הקבוצות וכו') 
// יישמרו בזיכרון כל עוד האפליקציה רצה, ולא יתאפסו בכל בקשה.
builder.Services.AddSingleton<IDataContext, DataContext>();

// 2. הזרקת שכבת הלוגיקה העסקית (Service)
// אנחנו משתמשים ב-Scoped (נוצר מופע חדש לכל בקשת HTTP), 
// שזה הסטנדרט המקצועי לשכבות Service ו-API.
builder.Services.AddScoped<IChildService, ChildService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();