/*----Sipro history----*/
CREATE TABLESPACE sipro_history
  DATAFILE 'sipro_analytic.dat' 
    SIZE 10M
    REUSE
    AUTOEXTEND ON NEXT 10M MAXSIZE 200M; 
	
CREATE TEMPORARY TABLESPACE sipro_history_temp
  TEMPFILE 'sipro_analytic_temp.dbf'
    SIZE 5M
    AUTOEXTEND ON;	
	
CREATE USER sipro_history
  IDENTIFIED BY m1nf1n
  DEFAULT TABLESPACE sipro_history
  TEMPORARY TABLESPACE sipro_history_temp
  QUOTA 20M on sipro_history;
	
GRANT create session TO sipro_history;
GRANT create table TO sipro_history;
GRANT create view TO sipro_history;
GRANT create any trigger TO sipro_history;
GRANT create any procedure TO sipro_history;
GRANT create sequence TO sipro_history;
GRANT create synonym TO sipro_history;

/*----Sipro analytic----*/
 CREATE TABLESPACE sipro_analytic
  DATAFILE 'sipro_analytic.dat' 
    SIZE 10M
    REUSE
    AUTOEXTEND ON NEXT 10M MAXSIZE 200M; 
	
CREATE TEMPORARY TABLESPACE sipro_analytic_temp
  TEMPFILE 'sipro_analytic_temp.dbf'
    SIZE 5M
    AUTOEXTEND ON;
	
CREATE USER sipro_analytic
  IDENTIFIED BY m1nf1n
  DEFAULT TABLESPACE sipro_analytic
  TEMPORARY TABLESPACE sipro_analytic_temp
  QUOTA 20M on sipro_analytic;
  
GRANT create session TO sipro_analytic;
GRANT create table TO sipro_analytic;
GRANT create view TO sipro_analytic;
GRANT create any trigger TO sipro_analytic;
GRANT create any procedure TO sipro_analytic;
GRANT create sequence TO sipro_analytic;
GRANT create synonym TO sipro_analytic;