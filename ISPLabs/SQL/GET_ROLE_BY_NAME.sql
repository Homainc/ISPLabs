﻿CREATE OR REPLACE PROCEDURE GET_ROLE_BY_NAME(
    PASS_NAME VARCHAR2,
    ROLE_ID OUT NUMBER,
    ROLE_NAME OUT VARCHAR2) 
IS
BEGIN
    SELECT
        USER_ROLE.ID, USER_ROLE.NAME
        INTO ROLE_ID, ROLE_NAME
    FROM USER_ROLE
    WHERE USER_ROLE.NAME = PASS_NAME;
END;