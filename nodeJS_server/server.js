var express = require('express');
var app = express();

var server = require('http').createServer(app);
var io = require('socket.io').listen(server);
var moment = require('moment');
require('moment-timezone');
moment.tz.setDefault("Asia/Seoul");

var date = moment().format('YYYY-MM-DD HH:mm:ss');

console.log(date);

app.set('port', process.env.PORT || 8080);

var mysql      = require('mysql');
var connection = mysql.createConnection({
  host     : 'localhost',
  user     : 'kpu',
  password : 'kpu',
  port     :  3306 ,
  database : 'Billiard_DB'
});

io.on('connection', function(socket){
    console.log("user connected");
    socket.on('beep', function(){
            socket.emit('boop');
    });

     socket.on('CreateTable',function(data){
             console.log("createtable..."+data.user_id);
        var sql = 'SELECT replayTable.id FROM replayTable order by save_time desc limit 1;';
        connection.query(sql,function(err,rows, fields){
                if(err){

                }else{
                        if(rows[0]==undefined){
                        //      console.log("row empty by CreateTable");
                        var sql = 'insert into replayTable values(?,?,?,?)';
                        var params = [1,"replay1",data.user_id,date];
                        connection.query(sql,params,function(err,rows,fields){
                                if(err){
                                }else{
                                 console.log("insert into replayTable: first rows");
                                }
                        });
                        }else{
                                console.log(++rows[0].id);
                        }

                }
        });
    });



    socket.on('SendDatabyUnity', function(data){
        var sql = 'insert into replay1(ball1pos_x, ball1pos_y, ball1pos_z,ball1rot_x, ball1rot_y, ball1rot_z,ball2pos_x, ball2pos_y, ball2pos_z,ball2rot_x, ball2rot_y, ball2rot_z,ball3pos_x, ball3pos_y, ball3pos_z,ball3rot_x, ball3rot_y, ball3rot_z,quepos_x, quepos_y, quepos_z,querot_x, querot_y, querot_z) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)';
        var params = [data.ball1pos_x,data.ball1pos_y,data.ball1pos_z,data.ball1rot_x,data.ball1rot_y,data.ball1rot_z,data.ball2pos_x,data.ball2pos_y,data.ball2pos_z,data.ball2rot_x,data.ball2rot_y,data.ball2rot_z,data.ball3pos_x,data.ball3pos_y,data.ball3pos_z,data.ball3rot_x,data.ball3rot_y,data.ball3rot_z,data.quepos_x,data.quepos_y,data.quepos_z,data.querot_x,data.querot_y,data.querot_z];

        connection.query(sql,params,function(err,rows,fields){
            if(!err){
                console.log(err);
            }else{
                console.log(rows.insertid);
            }

    });

    });

   socket.on('RequestReplayByUnity', function(){
           var msg = {};
           /*
           var msg = new Array();
           msg[0] = "test";
           msg[1]= "test2";
           //wqJSON.stringify(msg);
           JSONObject.CreateStringObject(msg);
           socket.emit('SetReplayBoard',msg);
           */
        var sql = 'select * from replayTable';
        connection.query(sql,function(err,rows, fields){
                if(err){
                        console.log(err);
                }else{
                        for(var i = 0; i < rows.length; i++){
                                console.log(rows[i].title+" : "+rows[i].save_time);
                                msg[rows[i].title] =[rows[i].save_time];

                        }
                        socket.emit('SetReplayBoard',msg);
                }




        });
   });

});




server.listen(app.get('port'),function(){
    connection.connect();
    console.log("------- SERVER IS RUNNING -------");
});

