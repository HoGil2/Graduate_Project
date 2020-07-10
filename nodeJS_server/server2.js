'use strict'
var PythonShell = require('python-shell')
var express = require('express');
var app = express();

var server = require('http').createServer(app);
var io = require('socket.io').listen(server);
var moment = require('moment');
require('moment-timezone');
moment.tz.setDefault("Asia/Seoul");

var date = moment().format('YYYY-MM-DD HH:mm:ss');


app.set('port', process.env.PORT || 8080);
var options = {
    mode: 'text',

 //   pythonPath: '~/Guide',

    args: ['point1', 'point2', 'point3']
}

var targettable= "";


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
    socket.on('disconnect', function() {
        console.log("[Server Log] Client Disconnected ... ");
    });
  socket.on('GetCommendedStroke', function(jdata){
        options.args[0] = jdata.point1;
        options.args[1] = jdata.point2;
        options.args[2] = jdata.point3;
	console.log("point" + options.args[0]);
        PythonShell.PythonShell.run("Stroke_Commend.py", options, function(err, results){
            if(err) console.log('err msg:', err);
            console.log('results:%j', results);
            socket.emit('RecieveStrokeCommend', { result: results });
        })
    })

   socket.on('RequestTableRow',function(data){
      var sql = 'SELECT COUNT(*) FROM '+data.title+';';
      connection.query(sql,function(err,rows, fields){
	if(err){
		console.log(err);
	}else{
		var msg={
 		tablerow: rows[0]

		}
		socket.emit('ReturnTableRow',msg);

	}

      });
   
   
   });



     socket.on('CreateTable',function(data){
	     console.log("createtable..."+data.user_id);
        var sql = 'SELECT replayTable.id FROM replayTable order by id desc;';  
        connection.query(sql,function(err,rows, fields){
                if(err){
			console.log(err);

                }else{
			if(rows[0]==undefined){
			date = moment().format('YYYY-MM-DD HH:mm:ss');
			var sql = 'insert into replayTable values(?,?,?,?)';
			var params = [1,"replay1",data.user_id,date];
			connection.query(sql,params,function(err,rows,fields){
				if(err){
					console.log(err);
				}else{
				 console.log("insert into replayTable: first rows");
				}
			});
			}else{
				//console.log("before: "+ rows[0].id);
				var title = "replay"+(++rows[0].id);
				targettable = title;
				//console.log("after: "+rows[0].id);
				var msg ={
					tablename: title
				}
		                socket.emit('SetTablename',msg);

				var sql = 'create table '+title+'(id INT not null auto_increment, ball1pos_x float not null, ball1pos_y float not null, ball1pos_z float not null, ball1rot_x float not null, ball1rot_y float not null, ball1rot_z float not null,ball2pos_x float not null,ball2pos_y float not null,ball2pos_z float not null,ball2rot_x float not null,ball2rot_y float not null,ball2rot_z float not null,ball3pos_x float not null,ball3pos_y float not null, ball3pos_z float not null,ball3rot_x float not null,ball3rot_y float not null,ball3rot_z float not null,quepos_x float not null,quepos_y float not null,quepos_z float not null,querot_x float not null,querot_y float not null,querot_z float not null,constraint '+title+'_PK primary key(id));'
				//var params = [title,title];
				connection.query(sql,function(err,row,fields){
					if(err){
						console.log(err);
					}else
					{
						date = moment().format('YYYY-MM-DD HH:mm:ss');
						sql = 'insert into replayTable(title,user_id,save_time) values(?,?,?)';
						params = [title,data.user_id,date];

						connection.query(sql,params,function(err,row,fields){
							if(err){
								console.log(err);
							}else{
								console.log("insert table :"+title)
							}
						});
					}
				});
						/*
				sql = 'insert into replayTable(id,title,user_id,save_time) values(?,?,?,?)';
				params =[1,title,data.user_id,date];

		CreateReplay		connection.query(sql,params,function(err,row,fields){
					if(err){
					}else
					{
						console.log("insert table :"+title);
					}
				});*/
			}
			
                }
    });
     });


    socket.on('SendDatabyUnity', function(data){
        var sql = 'insert into '+targettable+'(ball1pos_x, ball1pos_y, ball1pos_z,ball1rot_x, ball1rot_y, ball1rot_z,ball2pos_x, ball2pos_y, ball2pos_z,ball2rot_x, ball2rot_y, ball2rot_z,ball3pos_x, ball3pos_y, ball3pos_z,ball3rot_x, ball3rot_y, ball3rot_z,quepos_x, quepos_y, quepos_z,querot_x, querot_y, querot_z) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)';
        //console.log(sql);
	    var params = [data.ball1pos_x,data.ball1pos_y,data.ball1pos_z,data.ball1rot_x,data.ball1rot_y,data.ball1rot_z,data.ball2pos_x,data.ball2pos_y,data.ball2pos_z,data.ball2rot_x,data.ball2rot_y,data.ball2rot_z,data.ball3pos_x,data.ball3pos_y,data.ball3pos_z,data.ball3rot_x,data.ball3rot_y,data.ball3rot_z,data.quepos_x,data.quepos_y,data.quepos_z,data.querot_x,data.querot_y,data.querot_z];

        connection.query(sql,params,function(err,rows,fields){
            if(!err){
                console.log(err);
            }else{
                console.log("insert"+data.tablename);
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
   socket.on('RequestFrameByUnity',function(data){
	  var msg=[];
	   var sql = 'select * from '+data.title+';';
	  // console.log(sql);
	   connection.query(sql,function(err,rows, fields){
		   if(err){
			   console.log(err);
		   }else{
			   
			for(var i=0;i<100;i++){
		        msg.push({
			  ball1pos_x: rows[i].ball1pos_x,
			  ball1pos_y: rows[i].ball1pos_y,
			  ball1pos_z: rows[i].ball1pos_z,
			  ball1rot_x: rows[i].ball1rot_x,
			  ball1rot_y: rows[i].ball1rot_y,
			  ball1rot_z: rows[i].ball1rot_z,
			  ball2pos_x: rows[i].ball2pos_x,
			  ball2pos_y: rows[i].ball2pos_y,
			  ball2pos_z: rows[i].ball2pos_z,
			  ball2rot_x: rows[i].ball2rot_x,
			  ball2rot_y: rows[i].ball2rot_y,
			  ball2rot_z: rows[i].ball2rot_z,
			  ball3pos_x: rows[i].ball3pos_x,
			  ball3pos_y: rows[i].ball3pos_y,
			  ball3pos_z: rows[i].ball3pos_z,
			  ball3rot_x: rows[i].ball3rot_x,
			  ball3rot_y: rows[i].ball3rot_y,
			  ball3rot_z: rows[i].ball3rot_z,
			  quepos_x: rows[i].quepos_x,
			  quepos_y: rows[i].quepos_y,
			  quepos_z: rows[i].quepos_z,
			  querot_x: rows[i].querot_x,
			  querot_y: rows[i].querot_y,
			  querot_z: rows[i].querot_z 
			
			});
			}
		//	var parse = JSONObject.CreateStringObject(JSON.Stringify(msg));
		   
			console.log(msg);
			for(var i=0;i<100;i++){
			socket.emit('PlayReplay',msg[i]);
			}
		   }
		   });
	   });
/*
	socket.on('test',function(data){
		var zzz;
		var sql = 'select * from '+data.title+';';
    		connection.query(sql,function(err,rows, fields){
                   if(err){
                           console.log(err);
                   }else{
			 zzz = JSON.stringify(rows);
			 
			 console.log(zzz);

		   }
		});
		socket.emit('PlayReplay');
	});

*/

});




server.listen(app.get('port'),function(){
    connection.connect();
    console.log("------- SERVER IS RUNNING -------");
});

