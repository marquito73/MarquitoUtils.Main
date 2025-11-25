-- Script for create translation tables, store all translations
create table translation_field (
	translation_field_id bigint not null,
	translation_entity_id bigint not null,
	translation_entity_class nvarchar(256) not null,
	translation_entity_property nvarchar(128) not null,
	constraint pk_translation_field primary key clustered (translation_field_id),
	constraint ix_translation_field unique(translation_entity_id, translation_entity_class, translation_entity_property)
)
GO

create table translation (
	translation_id bigint not null,
	language tinyint not null default 0,
	translation_field_id bigint not null,
	translation_content nvarchar(2000) not null,
	constraint pk_translation primary key clustered (translation_id),
	constraint fk_translation_translation_field foreign key (translation_field_id) references translation_field(translation_field_id),
	constraint ix_translation unique(language, translation_field_id, translation_content)
)
GO