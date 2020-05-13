#database 생성
CREATE DATABASE Billiard_DB default CHARACTER SET UTF8;

#database 사용
use Billiard_DB;

#리플레이 테이블 생성 1번만
create table replayTable(
    id INT not null auto_increment,
    title varchar(30) not null,
    user_id varchar(30) not null,
    save_time varchar(30) not null,
    constraint replayTable_PK primary key(id)
);


#프레임 table 생성 리플레이 별로 생성
create table frameTable(
	id INT not null auto_increment,
    ball1pos_x float not null,
    ball1pos_y float not null,
    ball1pos_z float not null,
	ball1rot_x float not null,
    ball1rot_y float not null,
    ball1rot_z float not null,
    ball2pos_x float not null,
    ball2pos_y float not null,
    ball2pos_z float not null,
    ball2rot_x float not null,
    ball2rot_y float not null,
    ball2rot_z float not null,
	ball3pos_x float not null,
    ball3pos_y float not null,
    ball3pos_z float not null,
    ball3rot_x float not null,
    ball3rot_y float not null,
    ball3rot_z float not null,
	quepos_x float not null,
    quepos_y float not null,
    quepos_z float not null,
    querot_x float not null,
    querot_y float not null,
    querot_z float not null,
    constraint frameTable_PK primary key(id)
);

select * from frameTable;
create table testTable(
	id INT auto_increment,
    ball1pos_xframeTable float not null,
    ball1pos_y float not null,
 constraint frameTable_PK primary key(id)
    );
    
drop table frameTable;

