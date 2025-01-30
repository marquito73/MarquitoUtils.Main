-- Script for create user track history table, store all sql files already executed
create table user_track_history (
	user_track_history_id bigint not null,
	dt_user_track_last_visit_dt datetime not null,
	visit_count int not null,
	ip_address nvarchar(20) not null
	constraint pk_user_track_history primary key clustered (user_track_history_id)
)
GO