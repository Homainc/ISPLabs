CREATE OR REPLACE FUNCTION TABLE_EXIST RETURN NUMBER 
IS
    IS_EXIST NUMBER(1,0);
BEGIN
    SELECT COUNT(TABLE_NAME) INTO IS_EXIST FROM USER_TABLES
    WHERE TABLE_NAME = 'FORUM_PARTITION';
    RETURN IS_EXIST;
END TABLE_EXIST;