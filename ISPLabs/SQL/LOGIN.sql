CREATE OR REPLACE FUNCTION LOGIN(PASS_LOGIN VARCHAR2,PASS_PASSWORD VARCHAR2, ER OUT VARCHAR2)
RETURN NUMBER IS
COUNT_USERS NUMBER;
BEGIN
    SELECT COUNT(*) INTO COUNT_USERS FROM FORUM_USER 
    WHERE LOGIN = PASS_LOGIN AND PASSWORD = PASS_PASSWORD;
    IF COUNT_USERS = 1 THEN
        RETURN 1;
    ELSE 
        ER:= 'WRONG LOGIN OR PASSWORD';
        RETURN 0;
    END IF;
END;
