-- Script for create script history table, store all sql files already executed
create table script_history (
	script_history_id bigint not null,
	script_name nvarchar(250) not null,
	dt_script_dt datetime not null
	constraint pk_script_history primary key clustered (script_history_id)
)
GO