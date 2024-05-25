using bochonok_server_side;

var builder = WebApplication.CreateBuilder(args);
var bc = new BuilderConfiguration(builder);
var app = bc.Configure();

app.Run();
